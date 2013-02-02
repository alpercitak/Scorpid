using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;
using Scorpid.Helpers;
using Scorpid.Processing;

namespace Scorpid.Forms
{
    public partial class frmMain : Form
    {
        #region "Constructors"

        public frmMain()
        {
            InitializeComponent();
            _objGridHandlerRcv = new GridHandler(objGVRcv);
            _objGridHandlerSnd = new GridHandler(objGVSnd);
        }//constructor

        #endregion

        #region "Constants"

        private const int COLUMN_NO = 0;
        private const int COLUMN_NAME = 1;
        private const int COLUMN_STATUS = 2;
        private const int COLUMN_PERCENTAGE = 3;

        #endregion

        #region "Variables"

        private GridHandler _objGridHandlerRcv = null;
        private GridHandler _objGridHandlerSnd = null;

        private DataTable _objDataTableRcv = null;
        private DataTable _objDataTableSnd = null;

        private FileSender _objFileSender = null;
        private FileReceiver _objFileReceiver = null;

        private Dictionary<Socket, DataRow> _dictSndValues = new Dictionary<Socket, DataRow>();
        private Dictionary<Socket, DataRow> _dictRcvValues = new Dictionary<Socket, DataRow>();

        #endregion

        #region "Methods"

        private void PrepareGridRcv()
        {
            _objDataTableRcv = new DataTable();

            _objGridHandlerRcv.PrepareDG();
            _objGridHandlerRcv.AddHeader("No #", "No #", typeof(int), 100, true, _objDataTableRcv);
            _objGridHandlerRcv.AddHeader("Name", "Name", typeof(string), 100, true, _objDataTableRcv);
            _objGridHandlerRcv.AddHeader("Status", "Status", typeof(string), 100, true, _objDataTableRcv);
            _objGridHandlerRcv.AddHeader("Done %", "Done %", typeof(string), 100, true, _objDataTableRcv);

            dgRcv.DataSource = _objDataTableRcv;
        }//void

        private void PrepareGridSnd()
        {
            _objDataTableSnd = new DataTable();

            _objGridHandlerSnd.PrepareDG();
            _objGridHandlerSnd.AddHeader("No #", "No #", typeof(int), 100, true, _objDataTableSnd);
            _objGridHandlerSnd.AddHeader("Name", "Name", typeof(string), 100, true, _objDataTableSnd);
            _objGridHandlerSnd.AddHeader("Status", "Status", typeof(string), 100, true, _objDataTableSnd);
            _objGridHandlerSnd.AddHeader("Done %", "Done %", typeof(string), 100, true, _objDataTableSnd);

            dgSnd.DataSource = _objDataTableSnd;
        }//void

        private void InitMenu()
        {
            Menu = new MainMenu();

            MenuItem objMenuItemFile = Menu.MenuItems.Add("File");
            objMenuItemFile.MenuItems.Add("Exit", objMenuItemExit_Click);

            MenuItem objMenuItemOperations = Menu.MenuItems.Add("Operations");
            objMenuItemOperations.MenuItems.Add("Send File", objMenuItemSendFile_Click);
            objMenuItemOperations.MenuItems.Add("Clear Completed", objMenuItemClearCompleted_Click);
        }//void

        private void InitReceiver()
        {
            _objFileReceiver = new FileReceiver(FileReceiver.enum_FileParseStyle.ONFINISH);
            _objFileReceiver.SocketAccepted += new AcLib.Net.Listener.SocketAcceptedEvHandler(_objFileReceiver_SocketAccepted);
            _objFileReceiver.StatusUpdate += new FileReceiver.StatusUpdateEvHandler(_objFileReceiver_StatusUpdate);

            _objFileReceiver.Start();
        }//void

        private void InitSender()
        {
            _objFileSender = new FileSender();
            _objFileSender.SendStarted += new FileSender.SendStartedEvHandler(_objFileSender_SendStarted);
            _objFileSender.StatusUpdate += new FileSender.StatusUpdateEvHandler(_objFileSender_StatusUpdate);
        }//void

        #endregion

        #region "Events"

        private void frmMain_Load(object sender, System.EventArgs e)
        {
            PrepareGridRcv();
            PrepareGridSnd();

            InitMenu();
            InitReceiver();
            InitSender();
        }//void

        private void objMenuItemExit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }//void

        private void objMenuItemSendFile_Click(object sender, System.EventArgs e)
        {
            using (frmSendFile tmpFrm = new frmSendFile())
            {
                if (tmpFrm.ShowDialog(this) != DialogResult.OK) return;

                _objFileSender.SendFile(tmpFrm.Filename, tmpFrm.Recepient);
            }//using 
        }//void

        private void objMenuItemClearCompleted_Click(object sender, System.EventArgs e)
        {
            foreach (KeyValuePair<Socket, DataRow> objKVPair in _dictRcvValues)
            {
                if (objKVPair.Value[COLUMN_STATUS].ToString() != Helper.enum_Status.FINISHED.ToString()) continue;

                _objDataTableRcv.Rows.Remove(objKVPair.Value);
            }//foreach

            foreach (KeyValuePair<Socket, DataRow> objKVPair in _dictSndValues)
            {
                if (objKVPair.Value[COLUMN_STATUS].ToString() != Helper.enum_Status.FINISHED.ToString()) continue;

                _objDataTableSnd.Rows.Remove(objKVPair.Value);
            }//foreach
        }//void

        private void dgRcv_DoubleClick(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo objHitInfo = objGVRcv.CalcHitInfo(objGVRcv.GridControl.PointToClient(Control.MousePosition));
            if (!objHitInfo.InRowCell) return;

            DataRow objDataRow = _objGridHandlerRcv.GV.GetDataRow(objHitInfo.RowHandle);
            if (objDataRow == null) return;

            if (objDataRow[COLUMN_STATUS].ToString() == Helper.enum_Status.PENDING.ToString())
            {
                Socket objSocket = null;

                foreach (KeyValuePair<Socket, DataRow> objKVPair in _dictRcvValues)
                    if (objKVPair.Value == objDataRow)
                        objSocket = objKVPair.Key;

                _objFileReceiver.AcceptFile(objSocket);
            }//if
            else if (objDataRow[COLUMN_STATUS].ToString() == Helper.enum_Status.FINISHED.ToString())
                if (File.Exists(objDataRow[COLUMN_NAME].ToString()))
                    System.Diagnostics.Process.Start(objDataRow[COLUMN_NAME].ToString());
        }//void

        private void _objFileSender_SendStarted(Socket prm_objSocket)
        {
            if (_dictSndValues.ContainsKey(prm_objSocket)) return;

            DataRow objDataRow = _objDataTableSnd.NewRow();
            _objDataTableSnd.Rows.Add(objDataRow);

            _dictSndValues.Add(prm_objSocket, objDataRow);
        }//void

        private void _objFileSender_StatusUpdate(Socket prm_objSocket, Helper.StatusObject prm_objStatusObject)
        {
            if (!_dictSndValues.ContainsKey(prm_objSocket)) return;

            System.Action act = () =>
            {
                _dictSndValues[prm_objSocket][COLUMN_NAME] = prm_objStatusObject.Filename;
                _dictSndValues[prm_objSocket][COLUMN_NO] = prm_objStatusObject.Number;
                _dictSndValues[prm_objSocket][COLUMN_PERCENTAGE] = prm_objStatusObject.Percentage;
                _dictSndValues[prm_objSocket][COLUMN_STATUS] = prm_objStatusObject.Status;
            };
            this.dgSnd.Invoke(act);

            if (prm_objStatusObject.Status == Helper.enum_Status.FINISHED)
            {
                prm_objSocket.Close();
                prm_objSocket.Dispose();
                prm_objSocket = null;
                _objFileSender.RemoveStatusObject(prm_objSocket);
                GC.Collect();
            }//if
        }//void

        private void _objFileReceiver_SocketAccepted(Socket prm_objSocket)
        {
            if (_dictRcvValues.ContainsKey(prm_objSocket)) return;

            DataRow objDataRow = _objDataTableRcv.NewRow();
            _objDataTableRcv.Rows.Add(objDataRow);

            _dictRcvValues.Add(prm_objSocket, objDataRow);
        }//void

        private void _objFileReceiver_StatusUpdate(Socket prm_objSocket, Helper.StatusObject prm_objStatusObject)
        {
            if (!_dictRcvValues.ContainsKey(prm_objSocket)) return;

            System.Action act = () =>
            {
                _dictRcvValues[prm_objSocket][COLUMN_NAME] = prm_objStatusObject.Filename;
                _dictRcvValues[prm_objSocket][COLUMN_NO] = prm_objStatusObject.Number;
                _dictRcvValues[prm_objSocket][COLUMN_PERCENTAGE] = prm_objStatusObject.Percentage;
                _dictRcvValues[prm_objSocket][COLUMN_STATUS] = prm_objStatusObject.Status;
            };
            this.dgRcv.Invoke(act);

            if (prm_objStatusObject.Status == Helper.enum_Status.FINISHED)
            {
                prm_objSocket.Close();
                prm_objSocket.Dispose();

                _objFileReceiver.RemoveStatusObject(prm_objSocket);
                prm_objSocket = null;

                GC.Collect();
            }//if
        }//void

        #endregion
    }//class
}//namespace
