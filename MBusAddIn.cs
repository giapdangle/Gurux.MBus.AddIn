using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using Gurux.Device.Editor;
using Gurux.Device;
using Gurux.Communication;
using Gurux.Common;

namespace Gurux.MBus.AddIn
{
	[GXCommunicationAttribute(typeof(MBusPacketHandler), typeof(MBusPacketParser))]
	public class MBus : GXProtocolAddIn
	{
		public MBus() : base("MBus", false, true, true)
		{
			base.WizardAvailable = VisibilityItems.None;
		}

		public override VisibilityItems ItemVisibility
		{
			get
			{
				return VisibilityItems.Categories | VisibilityItems.Tables;
			}
		}

		public override Functionalities GetFunctionalities(object target)
		{
			if (target is GXCategoryCollection)
			{
				return Functionalities.None;
			}
			else if (target is GXTableCollection)
			{
				return Functionalities.None;
			}
			return Functionalities.Remove | Functionalities.Edit;
		}

        public override void ImportFromDevice(Control[] addinPages, GXDevice device, IGXMedia media)
        {
            media.Open();
            GXMBusDevice dev = device as GXMBusDevice;
            object data;
            //Handshake
            ShortFrame sf = new ShortFrame();
            sf.AddressField = dev.DeviceAddress;
            sf.ControlField = (byte)CFieldFunctions.SendSlaveInit;
            sf.CountChecksum();
            data = sf.ToByteArray();
            ReceiveParameters<byte[]> recParams = new ReceiveParameters<byte[]>()
            {                
                Count = 1,
                Peek = false,
                WaitTime = device.WaitTime
            };

            lock (media.Synchronous)
            {
                media.Send(data, null);
                if (!media.Receive(recParams))
                {
                    throw new Exception("Failed to receive reply from the device in given time.");
                }
                if (recParams.Reply.Length < 1 || recParams.Reply[0] != 0xE5)
                {
                    throw new Exception("Handshake failed.");
                }

                bool morePacketsAvailable = false;
                byte fcb = 1;

                List<MBusRegister> items = new List<MBusRegister>();
                do
                {
                    sf = new ShortFrame();
                    sf.AddressField = dev.DeviceAddress;
                    if (fcb == 1)
                    {
                        sf.ControlField = (byte)CFieldFunctions.RequestClass2Data;
                    }
                    else
                    {
                        sf.ControlField = (byte)CFieldFunctions.RequestClass2Data | (byte)CFieldRequest.FrameCountBit;
                    }
                    sf.CountChecksum();

                    recParams.AllData = true;
                    recParams.Count = 1;
                    recParams.Eop = (byte)0x16;
                    recParams.Reply = null;    
                    int cnt = 0;
                    //Some meters can't answer right a way.
                    do
                    {
                        media.Send(sf.ToByteArray(), null);
                        if (++cnt == 6)
                        {
                            throw new Exception("Failed to receive reply from the device in given time.");
                        }                        
                    }
                    while (!media.Receive(recParams));
                    while (!VerifyReadReplyCheckSum(recParams.Reply))
                    {
                        recParams.Count = 0;
                        recParams.Reply = null;
                        bool gotData = media.Receive(recParams);
                        if (!gotData)
                        {
                            break;
                        }
                    }
                    morePacketsAvailable = recParams.Reply.Length > 5 && (recParams.Reply[4] & 0x10) != 0;
                    items.AddRange(ParseReadReply(recParams.Reply));
                }
                while (morePacketsAvailable);

                GXMBusCategory defaultCategory = null;
                if ((defaultCategory = (GXMBusCategory)device.Categories.Find("Default")) == null)
                {
                    defaultCategory = new GXMBusCategory();
                    defaultCategory.Name = "Default";
                    device.Categories.Add(defaultCategory);
                }
                for (int pos = 0; pos < items.Count; ++pos)
                {
                    MBusRegister item = items[pos];
                    GXMBusProperty prop = new GXMBusProperty();                    
                    prop.Ordinal = pos;
                    if (item.IsVariableData)
                    {
                        string name = item.MBusType;
                        int len = name.IndexOf('_');
                        if (len != -1)
                        {
                            name = name.Substring(0, len);
                        }
                        name += " " + item.Function.ToString();
                        if (item.Tariff != 0)
                        {
                            name += " Tariff " + item.Tariff.ToString();
                        }
                        prop.Name = name;
                        prop.Unit = item.Unit;
                        prop.DataLength = item.DataLength;
                        item.Mask.Reverse();
                        prop.InfoMask = item.Mask.ToArray();
                        prop.Type = item.Type;                                                
                        if (item.MBusType.ToLower().Contains("date") || item.MBusType.ToLower().Contains("timepoint"))
                        {
                            prop.ValueType = typeof(DateTime);
                        }
                        else
                        {                            
                            prop.ValueType = typeof(string);
                        }                         
                    }
                    else
                    {
                        prop.Name = item.MBusType;
                        prop.Unit = item.Unit;
                        prop.ValueType = typeof(string);
                        prop.DataLength = 4;
                    }                    
                    prop.InfoBytes = item.InformationBytes.Reverse().ToArray();
                    defaultCategory.Properties.Add(prop);
                }
            }
        }

		private bool VerifyReadReplyCheckSum(byte[] p)
		{
			if (p.Length < 2)
			{
				return false;
			}
			if (p.Length < p[1] + 6)
			{
				return false;
			}
			int checkSum = 0;
			for (int i = 4; i <= p.Length - 3; ++i)
			{
				checkSum += p[i];
			}
			checkSum &= 0xFF;

			return checkSum == p[p.Length-2];
		}

		internal static List<MBusRegister> ParseReadReply(byte[] reply)
		{
			List<MBusRegister> results = new List<MBusRegister>();

			LongFrame lf = new LongFrame(reply);

			if ((lf.ControlInformationField & 0x73) == 0x72) //Variable data
			{
				VariableData vd = new VariableData();
				vd.Header = new FixedDataHeader(lf.UserData);

				byte[] tmpArray = new byte[lf.UserData.Length - 12];
				Array.Copy(lf.UserData, 12, tmpArray, 0, lf.UserData.Length - 12);

				while (tmpArray.Length > 0)
				{
					VariableDataBlock vdb = new VariableDataBlock(ref tmpArray);
					vd.DataBlocks.Add(vdb);
				}
				foreach (VariableDataBlock vdb in vd.DataBlocks)
				{
					MBusRegister reg = new MBusRegister();
					reg.IsVariableData = true;
                    if (vdb.DataInformationField.Data != VariableDataType.SpecialFunctions)
                    {
                        reg.Unit = vdb.VariableInformationField.Unit;
                    }
					reg.Type = vdb.DataInformationField.Data;
					reg.DataLength = vdb.DataInformationField.GetDataLength();
					string unitStr = vdb.VariableInformationField.UnitInfo.ToString();
					if (unitStr.Contains(' '))
					{
						reg.MBusType = unitStr.Substring(0, unitStr.IndexOf(' '));
					}
					else
					{
						reg.MBusType = unitStr;
					}
					reg.Function = vdb.DataInformationField.Function;
					reg.StorageNumber = vdb.DataInformationField.StorageNumberLSB;
					reg.Mask.Add(0xBF); //DIF storage number may change 

					List<byte> infoBytes = new List<byte>();
					infoBytes.AddRange(vdb.DataInformationField.ToByteArray());

					for (int i = 0; i < vdb.DataInformationFieldExtensions.Count; ++i)
					{
						var dife = vdb.DataInformationFieldExtensions[i];
						infoBytes.AddRange(dife.ToByteArray());
						reg.StorageNumber += dife.StorageNumber << (i * 4 + 1);
						reg.Tariff += dife.Tariff << (i * 2);
						reg.DeviceUnit += dife.DeviceUnit << i;
						reg.Mask.Add(0xC0); //DIFE tariff and storage number may change
					}

					infoBytes.AddRange(vdb.VariableInformationField.ToByteArray());
					reg.Mask.Add(vdb.VariableInformationField.Mask);
					reg.Multiplier = vdb.VariableInformationField.Multiplier; 

					if (vdb.VariableInformationField.ToByteArray()[0] == 0xFD)
					{
						if (vdb.VariableInformationFieldExtensions[0].ExtensionBit == 1)
						{
							VariableValueAlternateInformation0xFD vvai;
							byte multiBits, mask;
							string unit;
							double multi;
							vdb.VariableInformationFieldExtensions[1].GetVariableInfo0xFD(out vvai, out mask, out multiBits, out unit, out multi);
							reg.Unit = unit;
							reg.MultiplierBits = multiBits;
							reg.Multiplier = multi; 
							reg.Mask.Add(mask);
							unitStr = vvai.ToString();
							if (unitStr.Contains(' '))
							{
								reg.MBusType = unitStr.Substring(0, unitStr.IndexOf(' '));
							}
							else
							{
								reg.MBusType = unitStr;
							}
						}
					}
					else if (vdb.VariableInformationField.ToByteArray()[0] == 0xFB)
					{
						if (vdb.VariableInformationFieldExtensions[0].ExtensionBit == 1)
						{
							if (vdb.VariableInformationFieldExtensions[0].ExtensionBit == 1)
							{
								VariableValueAlternateInformation0xFB vvai;
								byte multiBits, mask;
								string unit;
								double multi;
								vdb.VariableInformationFieldExtensions[1].GetVariableInfo0xFB(out vvai, out mask, out multiBits, out unit, out multi);
								reg.Unit = unit;
								reg.MultiplierBits = multiBits;
								reg.Multiplier = multi;
								reg.Mask.Add(mask);
								unitStr = vvai.ToString();
								if (unitStr.Contains(' '))
								{
									reg.MBusType = unitStr.Substring(0, unitStr.IndexOf(' '));
								}
								else
								{
									reg.MBusType = unitStr;
								}
							}
						}
					}
					else if (vdb.VariableInformationField.ExtensionBit == 1)
					{
						if (vdb.VariableInformationFieldExtensions[0].ExtensionBit == 1)
						{
							VariableValueInformationExtensionType vviet;
							byte mask;
							string unitExt;
							double multi;
							vdb.VariableInformationFieldExtensions[1].GetExtendedVariableInfo(out vviet, out mask, out unitExt, out multi);
							reg.Multiplier = multi; 
							reg.Unit += unitExt;
							reg.Mask.Add(mask);
						}
					}
					for (int i = 0; i < vdb.VariableInformationFieldExtensions.Count; ++i)
					{
						var vife = vdb.VariableInformationFieldExtensions[i];
						infoBytes.AddRange(vife.ToByteArray());
					}

					reg.InformationBytes = infoBytes.ToArray();
					reg.Value = vdb.DataPayload;

					results.Add(reg);
				}
			}
			else //Fixed data
			{
				FixedData fd = new FixedData(lf.UserData);

				MBusRegister reg1 = new MBusRegister();
				reg1.IsVariableData = false;
				reg1.MBusType = fd.MediumAndStatus.Counter1Medium.ToString();
				reg1.Unit = MathEnumToString(fd.MediumAndStatus.Counter1Unit.ToString());
				reg1.InformationBytes = fd.MediumAndStatus.ToByteArray();

				MBusRegister reg2 = new MBusRegister();
				reg1.IsVariableData = false;
				reg2.MBusType = fd.MediumAndStatus.Counter2Medium.ToString();
				if (fd.MediumAndStatus.Counter2Unit == FixedDataPhysicalUnit.SameButHistoric)
				{
					reg2.Unit = reg1.Unit + " (historic)";
				}
				else
				{
					reg2.Unit = MathEnumToString(fd.MediumAndStatus.Counter2Unit.ToString());
				}
				reg2.InformationBytes = fd.MediumAndStatus.ToByteArray();

				results.Add(reg1);
				results.Add(reg2);
			}
			
			return results;
		}

		internal static string MathEnumToString(string enumToString)
		{
			enumToString = enumToString.Replace("Times", " * ");
			enumToString = enumToString.Replace("Per", "/");
			enumToString = enumToString.Replace("m3", "m^3");
			enumToString = enumToString.Replace("To", "^");
			enumToString = enumToString.Replace("Minus", "-");
			enumToString = enumToString.Replace("Degrees", "°");
			enumToString = enumToString.Replace("Celcius", "C");
			return enumToString;
		}		

		public override void ModifyWizardPages(object source, GXPropertyPageType type, List<Control> pages)
		{
            if (type == GXPropertyPageType.Import)
            {
                pages.Insert(1, new ImportSelectionDlg(source as GXMBusDevice));
            }
		}

		public override Type GetDeviceType()
		{
			return typeof(GXMBusDevice);
		}

		public override Type[] GetPropertyTypes(object parent)
		{
			return new Type[] { typeof(GXMBusProperty) };
		}

		public override Type[] GetCategoryTypes(object parent)
		{
			return new Type[] { typeof(GXMBusCategory) };
		}

		public override Type[] GetTableTypes(object parent)
		{
			return new Type[] { typeof(GXMBusTable) };
		}
	}
}
