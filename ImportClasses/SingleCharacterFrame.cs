using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.MBus.AddIn
{
	public class SingleCharacterFrame : BaseFrame
	{
		public override FrameType GetFrameType()
		{
			return FrameType.SingleCharacterFrame;
		}

		public override byte GetStartByte()
		{
			return 0xE5;
		}

		public override byte? GetStopByte()
		{
			return null;
		}

		public override byte[] ToByteArray()
		{
			return new byte[] { GetStartByte() };
		}
	}
}
