using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gurux.Device.Editor;
using Gurux.Device;
using Gurux.Communication;
using System.Runtime.Serialization;

namespace Gurux.MBus.AddIn
{
	[GXReadMessage("Readout", "ReadoutReply", "IsReadoutComplete", "ReadMoreData")]
	[DataContract(Namespace = "http://www.gurux.org")]
	class GXMBusCategory : GXCategory
	{
		public GXMBusCategory()
		{
		}
	}
}
