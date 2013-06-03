using System;
using System.Collections.Generic;
using System.Text;
using Gurux.Device;
using Gurux.Communication;
using System.Windows.Forms;

namespace Gurux.MBus.AddIn
{
	public class MBusPacketParser : Gurux.Communication.IGXPacketParser
	{
		public void Load(object sender)
		{			
		}

        /// <inheritdoc cref="IGXPacketParser.Connect"/>
        public void Connect(object sender)
        {
        }

        /// <inheritdoc cref="IGXPacketParser.Disconnect"/>
        public void Disconnect(object sender)
        {

        }

		public void ParsePacketFromData(object sender, GXParsePacketEventArgs e)
		{		
		}

		public void BeforeSend(object sender, GXPacket packet)
		{		
		}

		public void IsReplyPacket(object sender, GXReplyPacketEventArgs e)
		{		
		}

		public void AcceptNotify(object sender, GXReplyPacketEventArgs e)
		{	
		}

		public void CountChecksum(object sender, GXChecksumEventArgs e)
		{	
		}

		public void ReceiveData(object sender, GXReceiveDataEventArgs e)
		{		
		}

		public void Received(object sender, GXReceivedPacketEventArgs e)
		{		
		}

        public void VerifyPacket(object sender, GXVerifyPacketEventArgs e)
        {

        }

		public void Unload(object sender)
		{		
		}

		public void InitializeMedia(object sender, Gurux.Common.IGXMedia media)
		{		
		}
	}
}
