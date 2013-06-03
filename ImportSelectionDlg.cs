using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gurux.Device.Editor;
using Gurux.Device;
using Gurux.Communication;
using Gurux.Common;

namespace Gurux.MBus.AddIn
{
    public partial class ImportSelectionDlg : Form, IGXWizardPage
	{
        GXMBusDevice Device;
		
        /// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="device"></param>
        public ImportSelectionDlg(GXMBusDevice device)
		{
			InitializeComponent();
            Device = device;
            TableAddressTb.Text = Device.DeviceAddress.ToString();
		}

        #region IGXWizardPage Members

        bool IGXWizardPage.IsShown()
        {
            return true;
        }

        void IGXWizardPage.Next()
        {
            byte value;
            if (!byte.TryParse(TableAddressTb.Text, out value))
            {
                throw new Exception("The device address must be a number.");
            }
            Device.DeviceAddress = value;
        }

        void IGXWizardPage.Back()
        {
        }

        void IGXWizardPage.Finish()
        {
        }

        void IGXWizardPage.Cancel()
        {
        }

        void IGXWizardPage.Initialize()
        {            
        }

        GXWizardButtons IGXWizardPage.EnabledButtons
        {
            get
            {
                return GXWizardButtons.All;
            }
        }

        string IGXWizardPage.Caption
        {
            get
            {
                return "";
            }
        }

        string IGXWizardPage.Description
        {
            get
            {
                return "";
            }
        }

        object IGXWizardPage.Target
        {
            get;
            set;
        }

        #endregion
    }
}
