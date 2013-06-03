using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.MBus.AddIn
{
	public class ShortFrame : BaseFrame
	{
		public ShortFrame()
		{

		}

		public ShortFrame(byte[] message)
		{
			ControlField = message[1];
			AddressField = message[2];
			CheckSum = message[3];
		}

		public override FrameType GetFrameType()
		{
			return FrameType.ShortFrame;
		}

		public override byte GetStartByte()
		{
			return 0x10;
		}

		/// <summary>
		/// C Field, Control Field, Function Field
		/// </summary>
		public byte ControlField
		{
			get;
			set;
		}

		/// <summary>
		/// A Field, Address Field
		/// </summary>
		/// <value>
		/// 1-250 are slaves
		/// 0 is unconfigured slave (factory default)
		/// 254 is broadcast with replies
		/// 255 is broadcast with no replies
		/// 253 addressing has been done in Network Layer instead of Data Link
		/// </value>
		public byte AddressField
		{
			get;
			set;
		}

		public byte CheckSum
		{
			get;
			set;
		}

		public override byte[] ToByteArray()
		{
			List<byte> bList = new List<byte>();
			bList.Add(GetStartByte());
			bList.Add(ControlField);
			bList.Add(AddressField);
			bList.Add(CheckSum);
			bList.Add(GetStopByte().Value);
			return bList.ToArray();
		}

		public void CountChecksum()
		{
			int tmp = 0;
			tmp = ControlField + AddressField;
			CheckSum = (byte)(tmp & 0xFF);
		}
	}
}
