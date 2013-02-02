using System;
using System.Windows.Forms;

namespace Scorpid
{
    public static class Program
    {
        #region "Methods"

        [STAThread]
        public static void Main()
        {
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.frmMain());
        }//void

        #endregion
    }//class
}//namespace
