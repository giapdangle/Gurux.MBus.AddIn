using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.MBus.AddIn
{
	public static class GXMBusCommon
	{

		public static byte[] UIntToBCD(uint num, int size)
		{
			byte[] bytes = new byte[size];
			for (int i = size - 1; i >= 0; --i)
			{
				uint loDigit = num % 10;
				num /= 10;
				uint hiDigit = num % 10;
				num /= 10;
				bytes[i] = (byte)((hiDigit << 4) | loDigit);
			}
			return bytes;
		}

	}
}
