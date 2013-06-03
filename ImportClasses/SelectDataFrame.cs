using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.MBus.AddIn
{
	public class SelectDataFrame : LongFrame
	{
		public SelectDataFrame(byte address)
		{
			ControlField = (byte)CFieldFunctions.SendUserData;
			AddressField = address;
			ControlInformationField = (byte)CIField.DataSend;
		}
	}
}
