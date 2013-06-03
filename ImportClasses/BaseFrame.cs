using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.MBus.AddIn
{
	public abstract class BaseFrame
	{
		public abstract FrameType GetFrameType();

		public virtual byte? GetStopByte()
		{
			return 0x16;
		}

		public abstract byte GetStartByte();

		public abstract byte[] ToByteArray();
	}
}
