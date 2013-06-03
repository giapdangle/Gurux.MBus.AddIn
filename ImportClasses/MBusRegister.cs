using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.MBus.AddIn
{
	class MBusRegister
	{
		public MBusRegister()
		{
			Mask = new List<byte>();
		}

		public string MBusType
		{
			get;
			set;
		}

        public VariableDataType Type
		{
			get;
			set;
		}

		public string Unit
		{
			get;
			set;
		}

		public byte[] InformationBytes
		{
			get;
			set;
		}

		public VariableDataFunction Function
		{
			get;
			set;
		}

		public int StorageNumber
		{
			get;
			set;
		}

		public byte MultiplierBits
		{
			get;
			set;
		}

		public double Multiplier
		{
			get;
			set;
		}

		public int Tariff
		{
			get;
			set;
		}

		public int DeviceUnit
		{
			get;
			set;
		}

		public int DataLength
		{
			get;
			set;
		}

		public List<byte> Mask
		{
			get;
			set;
		}

		public bool IsVariableData
		{
			get;
			set;
		}

		public object Value
		{
			get;
			set;
		}
	}
}
