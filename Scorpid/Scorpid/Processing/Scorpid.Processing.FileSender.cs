using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using AcLib.Helpers;
using AcLib.Net;

namespace Scorpid.Processing
{
    public class FileSender
    {
        #region "Delegators & Event Definitions"

        public delegate void SendStartedEvHandler(Socket prm_objSocket);
        public delegate void StatusUpdateEvHandler(Socket prm_objSocket, Helper.StatusObject prm_objStatusObject);

        public event SendStartedEvHandler SendStarted;
        public event StatusUpdateEvHandler StatusUpdate;

        #endregion

        #region "Variables"

        private Dictionary<Socket, Helper.StatusObject> _dictStatusObjects = new Dictionary<Socket, Helper.StatusObject>();

        #endregion

        #region "Methods"

        public void SendFile(string prm_strFilename, string prm_strRecepient)
        {
            Thread objThread = new Thread(new ParameterizedThreadStart(SendFile));
            objThread.Start(string.Format("{0}|{1}", prm_strFilename, prm_strRecepient));
        }//void

        private void SendFile(object prm_objParams)
        {
            string strFilename = string.Empty;
            string strRecepient = string.Empty;

            if (!CheckParams(prm_objParams, ref strFilename, ref strRecepient)) return;

            FileStream objFileStream = null;

            try
            {
                objFileStream = File.OpenRead(strFilename);
                if (objFileStream == null) return;

                Socket objSocket = new Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.IP);
                objSocket.Connect(strRecepient, AcLib.Helpers.Local.GetPort());

                if (SendStarted != null) SendStarted(objSocket);
                if (StatusUpdate != null) StatusUpdate(objSocket, Helper.GetStatusObject(objSocket, _dictStatusObjects.Count + 1, 0, strFilename, Helper.enum_Status.PENDING, _dictStatusObjects));

                if (!WaitForAck(objSocket, objFileStream)) return;

                SendFile(objSocket, objFileStream);

                if (StatusUpdate != null) StatusUpdate(objSocket, Helper.GetStatusObject(objSocket, 100, Helper.enum_Status.FINISHED, _dictStatusObjects));

                objSocket.Close();
                objSocket.Dispose();
            }//try
            catch (Exception Ex)
            {
                Logging.DoLog(Ex.Message);
            }//catch
            finally
            {
                if (objFileStream != null)
                {
                    objFileStream.Close();
                    objFileStream.Dispose();
                    objFileStream = null;
                }//if
            }//finally
        }//void

        private void SendFile(Socket prm_objSocket, FileStream prm_objFileStream)
        {
            byte[] bData = new byte[Listener.BUFFER_SIZE];

            int intRead = prm_objFileStream.Read(bData, 0, Listener.BUFFER_SIZE);
            int intTotalLength = (int)prm_objFileStream.Length;
            int intLeft = intTotalLength;

            if (intRead < Listener.BUFFER_SIZE)
            {
                byte[] bDataTmp = new byte[Listener.BUFFER_SIZE];
                Array.Copy(bData, bDataTmp, bData.Length);

                bData = new byte[intRead];
                Array.Copy(bDataTmp, bData, intRead);
            }//if

            while (intRead > 0)
            {
                Helper.Send(prm_objSocket, bData);

                if (StatusUpdate != null) StatusUpdate(prm_objSocket, Helper.GetStatusObject(prm_objSocket, Helper.GetPercentageDone(intLeft, intTotalLength), Helper.enum_Status.SENDING, _dictStatusObjects));
                intLeft -= intRead;
                intRead = prm_objFileStream.Read(bData, 0, intLeft >= Listener.BUFFER_SIZE ? Listener.BUFFER_SIZE : intLeft);
            }//while
        }//void

        private bool CheckParams(object prm_objParams, ref string prm_strFilename, ref string prm_strRecepient)
        {
            if (prm_objParams == null) return false;

            string strParams = prm_objParams.ToString();
            if (strParams == string.Empty) return false;

            string[] arrSplit = strParams.Split('|');
            if (arrSplit.Length != 2) return false;

            prm_strFilename = arrSplit[0];
            prm_strRecepient = arrSplit[1];

            if (prm_strFilename.Trim().Length == 0 || prm_strRecepient.Trim().Length == 0) return false;

            return true;
        }//function

        private bool WaitForAck(Socket prm_objSocket, FileStream prm_objFileStream)
        {
            Helper.Send(prm_objSocket, Helper.GetRequestMsg(Helper.GetTrimmedFileName(prm_objFileStream.Name), prm_objFileStream.Length));

            if (Helper.Receive(prm_objSocket) != Helper.GetRequestAckMsg(prm_objFileStream.Name, prm_objFileStream.Length)) return false;

            return true;
        }//function

        public void RemoveStatusObject(Socket prm_objSocket)
        {
            if (!_dictStatusObjects.ContainsKey(prm_objSocket)) return;

            _dictStatusObjects.Remove(prm_objSocket);
        }//void

        #endregion
    }//class
}//namespace
