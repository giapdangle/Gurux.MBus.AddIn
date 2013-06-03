using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.MBus.AddIn
{
	public class VariableData
	{
		public VariableData()
		{
			ManufacturerData = new byte[0];
			Header = new FixedDataHeader();
			DataBlocks = new List<VariableDataBlock>();
		}

		public FixedDataHeader Header
		{
			get;
			set;
		}

		public List<VariableDataBlock> DataBlocks
		{
			get;
			set;
		}

		public byte MDH
		{
			get;
			set;
		}

		public byte[] ManufacturerData
		{
			get;
			set;
		}

		public byte[] ToByteArray(bool SelectMode)
		{
			List<byte> result = new List<byte>();
			if (SelectMode)
			{
				result.Add(0x80);
			}
			else
			{
				result.AddRange(Header.ToByteArray());
			}
			foreach (VariableDataBlock vdb in DataBlocks)
			{
				result.AddRange(vdb.ToByteArray(SelectMode));
			}
			result.Add(MDH);
			result.AddRange(ManufacturerData);
			return result.ToArray();
		}
	}

	public class FixedDataHeader
	{
		public FixedDataHeader()
		{

		}

		public FixedDataHeader(byte[] messagePart)
		{
			byte[] tmpArray = new byte[4];

			Array.Copy(messagePart, 0, tmpArray, 0, 4);
			ID = BitConverter.ToUInt32(tmpArray, 0);

			tmpArray = new byte[2];
			Array.Copy(messagePart, 4, tmpArray, 0, 2);
			Manufacturer = BitConverter.ToUInt16(tmpArray, 0);

			Version = messagePart[6];
			Medium = messagePart[7];
			AccessNumber = messagePart[8];
			Status = messagePart[9];

			Array.Copy(messagePart, 10, tmpArray, 0, 2);
			Signature = BitConverter.ToUInt16(tmpArray, 0);
		}

		/// <summary>
		/// BCD range from 00000000 to 99999999
		/// </summary>
		public uint ID
		{
			get;
			set;
		}

		/// <summary>
		/// Calculated from the ASCII code of EN 61107 manufacturer ID
		/// </summary>
		public ushort Manufacturer
		{
			get;
			set;
		}

		/// <summary>
		/// Generation or version of this counter.
		/// </summary>
		public byte Version
		{
			get;
			set;
		}

		public byte Medium
		{
			get;
			set;
		}

		/// <summary>
		/// Chapter 6.2 in MBDOC48.PDF
		/// </summary>
		public byte AccessNumber
		{
			get;
			set;
		}

		/// <summary>
		/// Same as FixedDataStatus, but lowest two bits are application errors.
		/// </summary>
		public byte Status
		{
			get;
			set;
		}

		/// <summary>
		/// Reserved for future
		/// </summary>
		public ushort Signature
		{
			get;
			set;
		}

		public byte[] ToByteArray()
		{
			List<byte> result = new List<byte>();
			result.AddRange(GXMBusCommon.UIntToBCD(ID, 4));
			result.AddRange(BitConverter.GetBytes(Manufacturer));
			result.Add(Version);
			result.Add(Medium);
			result.Add(AccessNumber);
			result.Add(Status);
			result.AddRange(BitConverter.GetBytes(Signature));
			return result.ToArray();
		}
	}

	public class VariableDataBlock
	{
		public VariableDataBlock()
		{
			DataInformationFieldExtensions = new List<VariableDataDIFE>();
			VariableInformationFieldExtensions = new List<VariableDataVIFE>();
		}

		public VariableDataBlock(ref byte[] MessagePart)
		{
			DataInformationField = new VariableDataDIF(MessagePart[0]);

			DataInformationFieldExtensions = new List<VariableDataDIFE>();
			if (DataInformationField.ExtensionBit != 0)
			{
				for (int i = 1; i < MessagePart.Length; i++)
				{
					DataInformationFieldExtensions.Add(new VariableDataDIFE(MessagePart[i]));
					if ((MessagePart[i] & 0x80) == 0)
					{
						break;
					}
				}
			}

			VariableInformationField = new VariableDataVIF(MessagePart[DataInformationFieldExtensions.Count + 1]);

			VariableInformationFieldExtensions = new List<VariableDataVIFE>();
			if (VariableInformationField.ExtensionBit != 0)
			{
				for (int i = DataInformationFieldExtensions.Count + 2; i < MessagePart.Length; i++)
				{
					VariableInformationFieldExtensions.Add(new VariableDataVIFE(MessagePart[i]));
					if ((MessagePart[i] & 0x80) == 0)
					{
						break;
					}
				}
			}

			int startPos = 2 + DataInformationFieldExtensions.Count + VariableInformationFieldExtensions.Count;
			int len = DataInformationField.GetDataLength();
			if (len == -1)
			{
				len = MessagePart.Length - startPos;
			}
			DataPayload = new byte[len];
			Array.Copy(MessagePart, startPos, DataPayload, 0, len);

			byte[] tmpArray = new byte[MessagePart.Length - len - startPos];
			Array.Copy(MessagePart, startPos + len, tmpArray, 0, tmpArray.Length);
			MessagePart = tmpArray;
		}

		public VariableDataDIF DataInformationField
		{
			get;
			set;
		}

		public List<VariableDataDIFE> DataInformationFieldExtensions
		{
			get;
			set;
		}

		public VariableDataVIF VariableInformationField
		{
			get;
			set;
		}

		public List<VariableDataVIFE> VariableInformationFieldExtensions
		{
			get;
			set;
		}

		public byte[] DataPayload
		{
			get;
			set;
		}

		public byte[] ToByteArray(bool SelectMode)
		{
			List<byte> result = new List<byte>();
			result.AddRange(DataInformationField.ToByteArray());
			foreach (VariableDataDIFE dife in DataInformationFieldExtensions)
			{
				result.AddRange(dife.ToByteArray());
			}
			result.AddRange(VariableInformationField.ToByteArray());
			foreach (VariableDataVIFE vife in VariableInformationFieldExtensions)
			{
				result.AddRange(vife.ToByteArray());
			}
			if (SelectMode)
			{
				result.AddRange(DataPayload);
			}
			return result.ToArray();
		}
	}
}
