using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gurux.Device.Editor;
using Gurux.Device;
using Gurux.Communication;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Gurux.MBus.AddIn
{
	[DataContract(Namespace = "http://www.gurux.org")]
	class GXMBusProperty : GXProperty
	{
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXMBusProperty()
        {
            this.AccessMode = AccessMode.Read;
        }

        [DefaultValue(Gurux.Device.AccessMode.Read)]
        public override AccessMode AccessMode
        {
            get
            {
                return base.AccessMode;
            }
            set
            {
                base.AccessMode = value;
            }
        }

        /// <summary>
        /// Data and variable information.
        /// </summary>
        [Browsable(false), System.ComponentModel.Category("Data"), System.ComponentModel.Description("Data and variable information.")]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]        
        public byte[] InfoBytes
        {
            get;
            set;
        }

        /// <summary>
        /// Data and variable information mask.
        /// </summary>
        [Browsable(false), System.ComponentModel.Category("Data"), System.ComponentModel.Description("Data and variable information mask.")]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public byte[] InfoMask
		{
			get;
            set;			
		}

		/// <summary>
        /// Data length.
		/// </summary>
        [Browsable(false), ReadOnly(false), System.ComponentModel.Category("Data"), System.ComponentModel.Description("Data length.")]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]        
        public int DataLength
		{
			get;
			set;
		}

        /// <summary>
        /// Value multiplier.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [Browsable(true), ReadOnly(false), System.ComponentModel.Category("Data"), System.ComponentModel.Description("Value multiplier.")]
		public int Multiplier
		{
			get;
			set;
		}

        /// <summary>
        /// 1 based ordinal index.
        /// </summary>
        [Browsable(false)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public int Ordinal
        {
            get;
            set;
        }

        /// <summary>
        /// MBus property type.
        /// </summary>
        [Browsable(false)]
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public VariableDataType Type
        {
            get;
            set;
        }
	}
}
