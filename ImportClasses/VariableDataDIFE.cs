using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.MBus.AddIn
{
	public class VariableDataDIFE
	{
		public VariableDataDIFE()
		{

		}

		public VariableDataDIFE(byte messagePart)
		{
			StorageNumber = (byte)(messagePart & 0x0F);
			Tariff = (byte)((messagePart & 0x30) >> 4);
			DeviceUnit = (byte)((messagePart & 0x40) >> 6);
			ExtensionBit = (byte)((messagePart & 0x80) >> 7);
		}

		/// <summary>
		/// 4 bit
		/// </summary>
		public byte StorageNumber
		{
			get;
			set;
		}

		public byte Tariff
		{
			get;
			set;
		}

		public byte DeviceUnit
		{
			get;
			set;
		}

		public byte ExtensionBit
		{
			get;
			set;
		}

		public byte[] ToByteArray()
		{
			byte result = (byte)(StorageNumber + (Tariff << 4) + (DeviceUnit << 6) + (ExtensionBit << 7));
			return new byte[] { result };
		}
	}
}
