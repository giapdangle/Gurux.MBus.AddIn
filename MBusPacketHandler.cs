using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Gurux.Device;
using Gurux.Communication;
using System.Windows.Forms;
using Gurux.Device.Editor;
using Gurux.Common;

namespace Gurux.MBus.AddIn
{
	public class MBusPacketHandler : Gurux.Device.IGXPacketHandler
	{

		private const byte MASK_FCV = 0x10;
		private const byte MASK_FCB = 0x20;

		private int m_PreviousFCB = 0;

        public object Parent
        {
            get;
            set;
        }

        /// <inheritdoc cref="IGXPacketHandler.Connect"/>
        public void Connect(object sender)
        {
        }

        /// <inheritdoc cref="IGXPacketHandler.Disconnect"/>
        public void Disconnect(object sender)
        {

        }

		public bool IsTransactionComplete(object sender, string command, GXPacket packet)
		{
			try
			{
				switch (command)
				{
					case "IsReadoutComplete":
						return IsReadoutComplete(sender, packet);
					default:
						throw new Exception("IsTransactionComplete failed. Unknown command: " + command);
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("IsTransactionComplete Failed: " + ex.Message);
				System.Diagnostics.Debug.Write(ex.StackTrace);
				System.Diagnostics.Debug.WriteLine("------------------------------------------");
				throw;
			}
		}

		public void ExecuteParseCommand(object sender, string command, GXPacket[] packets)
		{
			try
			{
				switch (command)
				{
					case "ReadoutReply":
						ReadoutReply(sender, packets);
						break;
					default:
						throw new Exception("ExecuteCommand failed. Unknown command: " + command);
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("ExecuteParseCommand Failed: " + ex.Message);
				System.Diagnostics.Debug.Write(ex.StackTrace);
				System.Diagnostics.Debug.WriteLine("------------------------------------------");
				throw;
			}
		}

		public void ExecuteSendCommand(object sender, string command, GXPacket packet)
		{
			try
			{
				switch (command)
				{
					case "Readout":
						Readout(sender, packet);
						break;
					case "ReadMoreData":
						ReadMoreData(sender, packet);
						break;
					default:
						throw new Exception("ExecuteCommand failed. Unknown command: " + command);
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("ExecuteSendCommand Failed: " + ex.Message);
				System.Diagnostics.Debug.Write(ex.StackTrace);
				System.Diagnostics.Debug.WriteLine("------------------------------------------");
				throw;
			}
		}

		public object DeviceValueToUIValue(GXProperty sender, object value)
		{
            GXMBusProperty prop = sender as GXMBusProperty;
            byte[] data = (byte[]) value;

			if (value is byte[])
			{
				//TODO: implement BCD and DateTime handling
				//BCD methods are in #region BCD converters
				byte[] val = value as byte[];
				switch (val.Length)
				{
					case 1:
						return val[0];
					case 2:
						return BitConverter.ToUInt16(val, 0);
					case 4:
						return BitConverter.ToUInt32(val, 0);
					case 8:
						return BitConverter.ToUInt64(val, 0);
				}
			}
			return value;
		}

		public object UIValueToDeviceValue(GXProperty sender, object value)
		{
			//TODO: implement
			return value;
		}



		private bool IsReadoutComplete(object sender, GXPacket packet)
		{
			byte cField = (byte)packet.ExtractData(typeof(System.Byte), 3, 1);
			byte fcv = (byte) (cField & MASK_FCV);
			byte fcb = (byte) (cField & MASK_FCB);

			if (sender is GXMBusCategory)
			{
				ParseReadoutReply(packet, sender as GXMBusCategory);
			}
			else
			{
				//This should never happen
				throw new Exception("Unknown parameter \"sender\" in IsReadoutComplete.");
			}
			packet.Clear();
			if (fcv == 0)
			{
				return true;
			}
			else
			{
				m_PreviousFCB = fcb;
				return false;
			}
		}

		private void ReadoutReply(object sender, GXPacket[] packets)
		{
			//Do nothing, parsing is done in IsReadoutComplete
		}

		private void ParseReadoutReply(GXPacket packet, GXMBusCategory category)
		{
			List<byte> dataBuff = new List<byte>();
			dataBuff.Add((byte)packet.Bop);
			dataBuff.AddRange((byte[])packet.ExtractData(typeof(byte[]), 0, -1));
			dataBuff.Add((byte)packet.Checksum);
			dataBuff.Add((byte)packet.Eop);
			List<MBusRegister> registers = MBus.ParseReadReply(dataBuff.ToArray());
			for (int pos = 0; pos < registers.Count; ++pos)
			{
                foreach (GXMBusProperty it in category.Properties)
                {
                    if (it.Ordinal == pos)
                    {
                        it.SetValue(registers[pos].Value, false, PropertyStates.ValueChangedByDevice);
                        break;
                    }
                }				
			}
		}

		private void ReadMoreData(object sender, GXPacket packet)
		{
			GXMBusDevice dev = (GXMBusDevice)GetDeviceFromSender(sender);
			packet.AppendData((byte)0x5B);
			packet.AppendData(dev.DeviceAddress);
		}

		private void Readout(object sender, GXPacket packet)
		{			
			GXMBusDevice device = (GXMBusDevice)GetDeviceFromSender(sender);
			IGXMedia media = (IGXMedia)device.GXClient.Media;

			bool isSerial = device.GXClient.MediaType == "Serial";
			if (isSerial)
			{
				((Gurux.Serial.GXSerial)media).BaudRate = 2400;
				((Gurux.Serial.GXSerial)media).DataBits = 8;
				((Gurux.Serial.GXSerial)media).Parity = System.IO.Ports.Parity.Even;
				((Gurux.Serial.GXSerial)media).StopBits = System.IO.Ports.StopBits.One;
			}


			List<byte> buff = new List<byte>(new byte[] {0x10, 0x40, device.DeviceAddress});
			buff.Add(CountShortFrameChecksum(buff.ToArray()));
			buff.Add(0x16);

			ReceiveParameters<byte[]> recParams = new ReceiveParameters<byte[]>();			
			lock (media.Synchronous)
			{
				recParams.AllData = true;
				recParams.Count = 1;
				recParams.Peek = false;
				recParams.Eop = null;
				recParams.Reply = null;
				recParams.WaitTime = device.WaitTime;

				media.Send(buff.ToArray(), null);
				if (!media.Receive(recParams))
				{
					throw new Exception("Handshake timeout.");
				}
			}
			System.Threading.Thread.Sleep(2000);

			if (!(recParams.Reply.Length > 0 && recParams.Reply[0] == 0xE5))
			{
				throw new Exception("Invalid handshake response.");
			}

			packet.Bop = (byte)0x10;
			packet.Eop = (byte)0x16;
			packet.ChecksumSettings.Position = -2;
            packet.ChecksumSettings.Type = ChecksumType.Sum8Bit;
			packet.ChecksumSettings.Start = 1;
			packet.ChecksumSettings.Count = -2;
			packet.AppendData((byte)0x5B);
			packet.AppendData(device.DeviceAddress);

			m_PreviousFCB = -1;
		}

		internal byte CountShortFrameChecksum(byte[] buff)
		{
			return (byte)((buff[1] + buff[2]) & 0xFF);
		}

		internal GXDevice GetDeviceFromSender(object sender)
		{
			if (sender is GXDevice)
			{
				return sender as GXDevice;
			}
			if (sender is GXProperty)
			{
				return (sender as GXProperty).Device;
			}
			if (sender is GXCategory)
			{
				return (sender as GXCategory).Device;
			}
			if (sender is GXTable)
			{
				return (sender as GXTable).Device;
			}
			throw new Exception("Unknown sender: " + sender.GetType().FullName);
		}

		#region BCD converters
		public static string BCDToString(bool isLittleEndian, byte[] bytes)
		{
			StringBuilder bcd = new StringBuilder(bytes.Length * 2);
			if (isLittleEndian)
			{
				for (int i = bytes.Length - 1; i >= 0; i--)
				{
					byte bcdByte = bytes[i];
					int idHigh = bcdByte >> 4;
					int idLow = bcdByte & 0x0F;
					if (idHigh > 9 || idLow > 9)
					{
						throw new ArgumentException(
						String.Format("One of the argument bytes was not in binary-coded decimal format: byte[{0}] = 0x{1:X2}.",
						i, bcdByte));
					}
					bcd.Append(string.Format("{0}{1}", idHigh, idLow));
				}
			}
			else
			{
				for (int i = 0; i < bytes.Length; i++)
				{
					byte bcdByte = bytes[i];
					int idHigh = bcdByte >> 4;
					int idLow = bcdByte & 0x0F;
					if (idHigh > 9 || idLow > 9)
					{
						throw new ArgumentException(
						String.Format("One of the argument bytes was not in binary-coded decimal format: byte[{0}] = 0x{1:X2}.",
						i, bcdByte));
					}
					bcd.Append(string.Format("{0}{1}", idHigh, idLow));
				}
			}
			return bcd.ToString();
		}

		public static byte[] DoubleToBcd(double input, int arrLength, int digitPos)
		{
			//The double must be converted to long, see dploce and dplocd.
			long value = 0;
			byte[] ret = new byte[arrLength];
			for (int i = 0; i < arrLength; i++)
			{
				ret[i] = (byte)(value % 10);
				value /= 10;
				ret[i] |= (byte)((value % 10) << 4);
				value /= 10;
			}
			return ret;
		}
		#endregion

    }
}