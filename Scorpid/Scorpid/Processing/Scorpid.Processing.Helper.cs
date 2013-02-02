using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using AcLib.Helpers;
using AcLib.Net;

namespace Scorpid.Processing
{
    public class Helper
    {
        #region "Enum"

        public enum enum_Status
        {
            PENDING,
            RECEIVING,
            SENDING,
            PARSINGFILE,
            FINISHED
        }//enum

        #endregion

        #region "Constants"

        //public const int PORT = 8998;
        public const string FILEREQUEST_SUFFIX = "FileRequest";
        private const string OK = "OK";
        private const char FILEREQUEST_SEPERATOR = (char)1;
        private const char NULLCHAR = '\0';

        #endregion

        #region "Methods"

        public static void Send(Socket prm_objSocket, string prm_strData)
        {
            prm_objSocket.Send(Conversion.ToByteArray(prm_strData));
        }//void

        public static void Send(Socket prm_objSocket, byte[] prm_arrData)
        {
            prm_objSocket.Send(prm_arrData);
        }//void

        public static string Receive(Socket prm_objSocket)
        {
            byte[] bData = new byte[Listener.BUFFER_SIZE];
            prm_objSocket.Receive(bData);

            return Conversion.ToString(bData).Trim(NULLCHAR);
        }//function

        public static string GetRequestMsg(string prm_strFilename, long prm_lngLength)
        {
            return string.Format("{0}{1}{2}{3}{4}", FILEREQUEST_SUFFIX, FILEREQUEST_SEPERATOR, GetTrimmedFileName(prm_strFilename), FILEREQUEST_SEPERATOR, prm_lngLength.ToString());
        }//function

        public static string GetRequestAckMsg(string prm_strFilename, long prm_lngLength)
        {
            return string.Format("{0}{1}{2}", GetRequestMsg(prm_strFilename, prm_lngLength), FILEREQUEST_SEPERATOR, OK);
        }//function

        public static void ParseFileRequest(string prm_strData, FileInfo prm_objFileInfo)
        {
            string[] arrSplit = prm_strData.Split(FILEREQUEST_SEPERATOR);
            if (arrSplit.Length != 3) return;

            prm_objFileInfo.FilenameRemote = arrSplit[1];
            prm_objFileInfo.Length = Conversion.ToInt64(arrSplit[2]);
        }//void

        public static StatusObject GetStatusObject(Socket prm_objSocket, double prm_dblPercentage, Helper.enum_Status prm_eStatus, Dictionary<Socket, StatusObject> prm_dictStatusObjects)
        {
            return GetStatusObject(prm_objSocket, int.MinValue, prm_dblPercentage, string.Empty, prm_eStatus, prm_dictStatusObjects);
        }//void

        public static StatusObject GetStatusObject(Socket prm_objSocket, int prm_intNumber, double prm_dblPercentage, string prm_strFilename, Helper.enum_Status prm_eStatus, Dictionary<Socket, StatusObject> prm_dictStatusObjects)
        {
            if (!prm_dictStatusObjects.ContainsKey(prm_objSocket))
                prm_dictStatusObjects.Add(prm_objSocket, new Helper.StatusObject());

            if (prm_intNumber != int.MinValue) prm_dictStatusObjects[prm_objSocket].Number = prm_intNumber;
            if (prm_strFilename != string.Empty) prm_dictStatusObjects[prm_objSocket].Filename = prm_strFilename;
            prm_dictStatusObjects[prm_objSocket].Percentage = prm_dblPercentage;
            prm_dictStatusObjects[prm_objSocket].Status = prm_eStatus;

            return prm_dictStatusObjects[prm_objSocket];
        }//void

        public static double GetPercentageDone(long prm_lngLeft, long prm_lngTotalLength)
        {
            if (prm_lngTotalLength == 0) return 0;

            return Math.Round(((double)(prm_lngTotalLength - prm_lngLeft) / (double)prm_lngTotalLength) * 100, 2);
        }//function

        public static string GetTrimmedFileName(string prm_strFullPath)
        {
            return Path.GetFileName(prm_strFullPath);
        }//function

        #endregion

        #region "Subclasses"

        public class FileInfo : IDisposable
        {
            public string FilenameRemote = string.Empty;
            public string FilenameLocal = string.Empty;
            public long Length = 0;
            public long Transferred = 0;
            public List<byte[]> Content = null;

            public void Dispose()
            {
                foreach (System.Reflection.PropertyInfo objPI in this.GetType().GetProperties())
                    objPI.SetValue(null, null, null);

                GC.Collect();
            }//void
        }//class

        public class StatusObject
        {
            public int Number = 0;
            public double Percentage = 0;
            public enum_Status Status = enum_Status.PENDING;
            public string Filename = string.Empty;
        }//class

        #endregion
    }//class
}//namespace
