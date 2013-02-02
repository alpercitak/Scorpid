using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;
using AcLib.Helpers;
using AcLib.Net;

namespace Scorpid.Processing
{
    public class FileReceiver : Listener
    {
        #region "Constructors"

        public FileReceiver(enum_FileParseStyle prm_eStyle)
            : base()
        {
            _eStyle = prm_eStyle;

            base.SocketAccepted += new SocketAcceptedEvHandler(Base_SocketAccepted);
            base.DataReceived += new DataReceivedEvHandler(Base_DataReceived);
            base.TransferFinished += new TransferFinishedEvHandler(Base_TransferFinished);
        }//constructor

        #endregion

        #region "Delegators & Event Definitions"

        public delegate void StatusUpdateEvHandler(Socket prm_objSocket, Helper.StatusObject prm_objStatusObject);

        public event StatusUpdateEvHandler StatusUpdate;
        public new event SocketAcceptedEvHandler SocketAccepted;

        #endregion

        #region "Enum"

        public enum enum_FileParseStyle
        {
            ONRECEIVE,
            ONFINISH
        }//enum

        #endregion

        #region "Variables"

        private Dictionary<Socket, Helper.FileInfo> _dictFiles = new Dictionary<Socket, Helper.FileInfo>();
        private Dictionary<Socket, Helper.StatusObject> _dictStatusObjects = new Dictionary<Socket, Helper.StatusObject>();
        private enum_FileParseStyle _eStyle = enum_FileParseStyle.ONRECEIVE;

        #endregion

        #region "Methods"

        public void AcceptFile(Socket prm_objSocket)
        {
            if (prm_objSocket == null || !_dictFiles.ContainsKey(prm_objSocket)) return;

            string strExt = _dictFiles[prm_objSocket].FilenameRemote.Substring(_dictFiles[prm_objSocket].FilenameRemote.LastIndexOf(".") + 1);
            string strFilter = string.Format("{0} files (*.{0})|*.{0}|All files (*.*)|*.*", strExt);

            using (SaveFileDialog tmpFrm = new SaveFileDialog())
            {
                tmpFrm.DefaultExt = strExt;
                tmpFrm.Filter = strFilter;

                if (tmpFrm.ShowDialog() != DialogResult.OK) return;
                _dictFiles[prm_objSocket].FilenameLocal = tmpFrm.FileName;
            }//using

            Helper.Send(prm_objSocket, Helper.GetRequestAckMsg(_dictFiles[prm_objSocket].FilenameRemote, _dictFiles[prm_objSocket].Length));

            if (StatusUpdate != null) StatusUpdate(prm_objSocket, Helper.GetStatusObject(prm_objSocket, int.MinValue, 0, _dictFiles[prm_objSocket].FilenameLocal, Helper.enum_Status.RECEIVING, _dictStatusObjects));
        }//void

        public void RemoveStatusObject(Socket prm_objSocket)
        {
            if (!_dictStatusObjects.ContainsKey(prm_objSocket)) return;

            _dictStatusObjects.Remove(prm_objSocket);
        }//void

        #endregion

        #region "Events"

        private void Base_SocketAccepted(Socket prm_objSocket)
        {
            if (_dictFiles.ContainsKey(prm_objSocket)) return;

            _dictFiles.Add(prm_objSocket, new Helper.FileInfo());

            if (SocketAccepted != null) SocketAccepted(prm_objSocket);
        }//void

        private void Base_DataReceived(Socket prm_objSocket, byte[] bData)
        {
            if (!_dictFiles.ContainsKey(prm_objSocket)) return;

            string strData = Conversion.ToString(bData);

            if (strData.StartsWith(Helper.FILEREQUEST_SUFFIX))
            {
                Helper.ParseFileRequest(strData, _dictFiles[prm_objSocket]);
                if (StatusUpdate != null) StatusUpdate(prm_objSocket, Helper.GetStatusObject(prm_objSocket, _dictStatusObjects.Count + 1, 0, _dictFiles[prm_objSocket].FilenameRemote, Helper.enum_Status.PENDING, _dictStatusObjects));
            }//if
            else
            {
                _dictFiles[prm_objSocket].Transferred += bData.Length;

                if (_eStyle == enum_FileParseStyle.ONRECEIVE)
                {
                    using (FileStream objFS = new FileStream(_dictFiles[prm_objSocket].FilenameLocal, FileMode.Append, FileAccess.Write))
                    using (BinaryWriter objBinWriter = new BinaryWriter(objFS))
                        objBinWriter.Write(bData);
                }//if
                if (_eStyle == enum_FileParseStyle.ONFINISH)
                {
                    if (_dictFiles[prm_objSocket].Content == null)
                        _dictFiles[prm_objSocket].Content = new List<byte[]>();

                    _dictFiles[prm_objSocket].Content.Add(bData);
                }//if

                if (_dictFiles[prm_objSocket].Transferred % Listener.BUFFER_SIZE / 1000 == 0 && StatusUpdate != null)
                    StatusUpdate(prm_objSocket, Helper.GetStatusObject(prm_objSocket, Helper.GetPercentageDone(_dictFiles[prm_objSocket].Length - _dictFiles[prm_objSocket].Transferred, _dictFiles[prm_objSocket].Length), Helper.enum_Status.RECEIVING, _dictStatusObjects));
            }//else
        }//void

        private void Base_TransferFinished(Socket prm_objSocket)
        {
            if (!_dictFiles.ContainsKey(prm_objSocket)) return;

            if (_eStyle == enum_FileParseStyle.ONFINISH)
            {
                if (StatusUpdate != null) StatusUpdate(prm_objSocket, Helper.GetStatusObject(prm_objSocket, 100, Helper.enum_Status.PARSINGFILE, _dictStatusObjects));

                using (FileStream objFS = new FileStream(_dictFiles[prm_objSocket].FilenameLocal, FileMode.Append, FileAccess.Write))
                using (BinaryWriter objBinWriter = new BinaryWriter(objFS))
                {
                    foreach (byte[] bData in _dictFiles[prm_objSocket].Content)
                        objBinWriter.Write(bData);
                }//using

                _dictFiles[prm_objSocket].Dispose();
            }//if

            if (StatusUpdate != null) StatusUpdate(prm_objSocket, Helper.GetStatusObject(prm_objSocket, 100, Helper.enum_Status.FINISHED, _dictStatusObjects));
        }//void

        #endregion
    }//class
}//namespace
