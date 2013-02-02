using System;
using System.Windows.Forms;

namespace Scorpid.Forms
{
    public partial class frmSendFile : Form
    {
        #region "Constructors"

        public frmSendFile()
        {
            InitializeComponent();
        }//constructor

        #endregion

        #region "Properties"

        internal string Filename
        {
            get { return tx_Filename.Text; }
        }//property

        internal string Recepient
        {
            get { return tx_Recepient.Text; }
        }//property

        #endregion

        #region "Methods"

        private void SetButtons()
        {
            bn_OK.Enabled = tx_Filename.Text.Trim().Length > 0 && tx_Recepient.Text.Trim().Length > 0;
        }//void

        #endregion

        #region "Events"

        private void frmSendFile_Load(object sender, EventArgs e)
        {
            SetButtons();
        }//void

        private void tx_Common_TextChanged(object sender, EventArgs e)
        {
            SetButtons();
        }//void

        private void tx_Filename_DoubleClick(object sender, EventArgs e)
        {
            tx_Filename.Text = string.Empty;
            try
            {
                using (OpenFileDialog tmpFrm = new OpenFileDialog())
                {
                    if (tmpFrm.ShowDialog(this) != DialogResult.OK) return;
                    tx_Filename.Text = tmpFrm.FileName;
                }//using
            }//try
            finally
            {
                SetButtons();
            }//finally
        }//void

        #endregion
    }//class
}//namespace
