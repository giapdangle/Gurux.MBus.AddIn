using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gurux.Device.Editor;
using Gurux.Device;
using Gurux.Communication;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Gurux.MBus.AddIn
{
	[DataContract(Namespace = "http://www.gurux.org")]
	public class GXMBusDevice : GXDevice
	{
		public GXMBusDevice()
		{
			this.GXClient.Bop = (byte)0x68;
			this.GXClient.Eop = (byte)0x16;
            this.GXClient.ChecksumSettings.Type = ChecksumType.Sum8Bit;
			this.GXClient.ChecksumSettings.Count = -2;
			this.GXClient.ChecksumSettings.Position = -2;
			this.GXClient.ChecksumSettings.Start = 4;
			this.WaitTime = 5000;
			this.ResendCount = 0;
			this.GXClient.ByteOrder = ByteOrder.BigEndian;
			DeviceAddress = 254;
			DeviceID = "";
			SetAllowedMediaTypes();
		}

		void SetAllowedMediaTypes()
		{
			this.AllowedMediaTypes.Clear();
			this.AllowedMediaTypes.Add(new GXMediaType("Net", null));
			this.AllowedMediaTypes.Add(new GXMediaType("Serial", "<Bps>2400</Bps><Parity>Even</Parity>"));
		}

		[DefaultValue(0), Browsable(true), ReadOnly(false), System.ComponentModel.Category("Data"), System.ComponentModel.Description("Address of the device. Range from 1 to 250 or 254/255 for broadcast.")]
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		[ValueAccess(ValueAccessType.Edit, ValueAccessType.Edit)]
		public byte DeviceAddress
		{
			get;
			set;
		}

		[DefaultValue("-"), Browsable(true), ReadOnly(false), System.ComponentModel.Category("Data"), System.ComponentModel.Description("The identification number of the device. If provided other devices are ignored.")]
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		[ValueAccess(ValueAccessType.Edit, ValueAccessType.Edit)]
		public string DeviceID
		{
			get;
			set;
		}
	}
}
