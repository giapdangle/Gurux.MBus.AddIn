using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.MBus.AddIn
{
	class FixedData
	{
		public FixedData()
		{

		}

		public FixedData(byte[] message)
		{
			byte[] tmpArray = new byte[4];
			Array.Copy(message, 0, tmpArray, 0, 4);

			ID = BitConverter.ToUInt32(tmpArray, 0);

			AccessNumber = message[4];

			Status = (FixedDataStatus) message[5];
			tmpArray = new byte[2];

			Array.Copy(message, 6, tmpArray, 0, 2);
			MediumAndStatus = new FixedDataMediumAndUnit(tmpArray);

			string val = string.Empty;
			tmpArray = new byte[4];
			Array.Copy(message, 8, tmpArray, 0, 4);
			for (int i = tmpArray.Length -1; i > -1; --i)
			{
				byte b = tmpArray[i];
				val += (char)(((b & 0xF0) >> 4) + 0x30);
				val += (char)((b & 0x0F) + 0x30);
			}
			Counter1 = uint.Parse(val);

			val = string.Empty;
			Array.Copy(message, 12, tmpArray, 0, 4);
			for (int i = tmpArray.Length - 1; i > -1; --i)
			{
				byte b = tmpArray[i];
				val += (char)(((b & 0xF0) >> 4) + 0x30);
				val += (char)((b & 0x0F) + 0x30);
			}
			Counter2 = uint.Parse(val);
		}

		/// <summary>
		/// Range from 00000000 to 99999999
		/// </summary>
		public uint ID
		{
			get;
			set;
		}

		public byte AccessNumber
		{
			get;
			set;
		}

		public FixedDataStatus Status
		{
			get;
			set;
		}

		public FixedDataMediumAndUnit MediumAndStatus
		{
			get;
			set;
		}

		public uint Counter1
		{
			get;
			set;
		}

		public uint Counter2
		{
			get;
			set;
		}

		public byte[] ToByteArray()
		{
			List<byte> result = new List<byte>();
			result.AddRange(BitConverter.GetBytes(ID));
			result.Add(AccessNumber);
			result.Add((byte)Status);
			result.AddRange(MediumAndStatus.ToByteArray());
			result.AddRange(BitConverter.GetBytes(Counter1));
			result.AddRange(BitConverter.GetBytes(Counter2));
			return result.ToArray();
			
		}
	}

	/// <summary>
	/// Always transmitted with LSB first.
	/// </summary>
	public class FixedDataMediumAndUnit
	{
		public FixedDataMediumAndUnit()
		{

		}

		public FixedDataMediumAndUnit(byte[] messagePart)
		{
			byte medium = (byte)(messagePart[0] >> 6 & 0x3);
			medium += (byte)((messagePart[1] >> 6 & 0x3) << 2);
			Counter1Medium = Counter2Medium = (FixedDataMedium)medium;
			Counter1Unit = (FixedDataPhysicalUnit)((messagePart[0]) & 0x3F);
			Counter2Unit = (FixedDataPhysicalUnit)((messagePart[1]) & 0x3F);
		}

		/// <summary>
		/// 2 bits
		/// </summary>
		public FixedDataMedium Counter2Medium
		{
			get;
			set;
		}

		/// <summary>
		/// 6 bits
		/// </summary>
		public FixedDataPhysicalUnit Counter2Unit
		{
			get;
			set;
		}

		/// <summary>
		/// 2 bits
		/// </summary>
		public FixedDataMedium Counter1Medium
		{
			get;
			set;
		}

		/// <summary>
		/// 6 bits
		/// </summary>
		public FixedDataPhysicalUnit Counter1Unit
		{
			get;
			set;
		}

		public byte[] ToByteArray()
		{
			List<byte> result = new List<byte>();
			result.Add((byte)(Counter2Medium + (((byte)Counter2Unit) << 2)));
			result.Add((byte)(Counter1Medium + (((byte)Counter1Unit) << 2)));
			return result.ToArray();
		}
	}
}
