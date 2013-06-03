using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.MBus.AddIn
{
	public class LongFrame : BaseFrame
	{
		public LongFrame()
		{
			UserData = new byte[0];
		}

		public LongFrame(byte[] message)
		{
			LField1 = message[1];
			LField2 = message[2];
			ControlField = message[4];
			AddressField = message[5];
			ControlInformationField = message[6];
			UserData = new byte[message.Length - 9];
			Array.Copy(message, 7, UserData, 0, message.Length - 9);
			CheckSum = message[message.Length - 2];
		}

		public override FrameType GetFrameType()
		{
			return FrameType.LongFrame;
		}

		public override byte GetStartByte()
		{
			return 0x68;
		}

		//Data length excluding first 4 and last 2 bytes.
		//Should have same value as LField2
		public byte LField1
		{
			get;
			set;
		}

		//Data length excluding first 4 and last 2 bytes.
		//Should have same value as LField1
		public byte LField2
		{
			get;
			set;
		}

		public byte ControlField
		{
			get;
			set;
		}

		public byte AddressField
		{
			get;
			set;
		}

		public byte ControlInformationField
		{
			get;
			set;
		}

		public byte[] UserData
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
			bList.Add(LField1);
			bList.Add(LField2);
			bList.Add(GetStartByte());
			bList.Add(ControlField);
			bList.Add(AddressField);
			bList.Add(ControlInformationField);
			bList.AddRange(UserData);
			bList.Add(CheckSum);
			bList.Add(GetStopByte().Value);
			return bList.ToArray();
		}

		public void CountChecksumAndLength()
		{
			int tmp = 0;
			tmp = ControlField + AddressField + ControlInformationField;
			if (UserData != null)
			{
				foreach (byte b in UserData)
				{
					tmp += b;
				}
			}
			CheckSum = (byte)(tmp & 0xFF);
			LField1 = LField2 = (byte)(UserData.Length + 3);
		}
	}
	
}
