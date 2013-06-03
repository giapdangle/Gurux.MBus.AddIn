using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.MBus.AddIn
{
	public class VariableDataDIF
	{
		public VariableDataDIF()
		{

		}

		public VariableDataDIF(byte messagePart)
		{
			Data = (VariableDataType)(messagePart & 0x0F);
			Function = (VariableDataFunction)((messagePart & 0x30) >> 4);
			StorageNumberLSB = (byte)((messagePart & 0x40) >> 6);
			ExtensionBit = (byte)((messagePart & 0x80) >> 7);
		}

		/// <summary>
		/// bits 0-3, VariableDataType
		/// </summary>
		public VariableDataType Data
		{
			get;
			set;
		}

		/// <summary>
		/// Bits 4-5, VariableDataFunction
		/// </summary>
		public VariableDataFunction Function
		{
			get;
			set;
		}

		/// <summary>
		/// bit 6
		/// </summary>
		public byte StorageNumberLSB
		{
			get;
			set;
		}

		/// <summary>
		/// bit 7
		/// </summary>
		public byte ExtensionBit
		{
			get;
			set;
		} 
      
		public int GetDataLength()
		{
			switch (Data)
			{
				case VariableDataType.NoData:
					return 0;
				case VariableDataType.Int8:
					return 1;
				case VariableDataType.Int16:
					return 2;
				case VariableDataType.Int24:
					return 3;
				case VariableDataType.Int32:
					return 4;
				case VariableDataType.Real32:
					return 4;
				case VariableDataType.Int48:
					return 6;
				case VariableDataType.Int64:
					return 8;
			    case VariableDataType.SelectionForReadout:
					return 0;
				case VariableDataType.BCD2:
					return 1;
				case VariableDataType.BCD4:
					return 2;
				case VariableDataType.BCD6:
					return 3;
				case VariableDataType.BCD8:
					return 4;
				case VariableDataType.VariableLength:
					//TODO Page 39
					throw new NotImplementedException("Variable data length not implented.");
				case VariableDataType.BCD12:
					return 6;
				case VariableDataType.SpecialFunctions:
					//TODO Page 39
					//throw new NotImplementedException("Special functions data length not implented.");
					return -1;
				default:
					return -1;
			}
		}

		public byte[] ToByteArray()
		{
			byte result = (byte)(Data + ((byte)Function << 4) + (StorageNumberLSB << 6) + (ExtensionBit << 7));
			return new byte[] { result };
		}
	}
	
}
