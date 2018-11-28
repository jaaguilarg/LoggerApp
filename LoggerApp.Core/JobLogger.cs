using LoggerApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerApp.Core
{
    public class JobLogger
    {
        private static bool _logToFile;
        private static bool _logToConsole;
        private static bool _logMessage;
        private static bool _logWarning;
        private static bool _logError;
        private static bool _logToDatabase;
        private static bool _initialized;

        public JobLogger(bool logToFile, bool logToConsole, bool logToDatabase, bool logMessage, bool logWarning, bool logError)
        {
            _logToFile = logToFile;
            _logToConsole = logToConsole;
            _logMessage = logMessage;
            _logWarning = logWarning;
            _logError = logError;
            _logToDatabase = logToDatabase;
        }

        public static void LogMessage(string strMessage)
        {
            strMessage = strMessage.Trim();
            if (strMessage == null || strMessage.Length == 0)
            {
                return;
            }

            if (!_logToConsole && !_logToFile && !_logToDatabase)
            {
                //throw new Exception("Invalid configuration");
                Console.WriteLine("Invalid configuration");
                return;
            }

            if ((!_logError && !_logMessage && !_logWarning))
            {
                //throw new Exception("Error or Warning or Message must be specified");
                Console.WriteLine("Invalid configuration");
                return;
            }

            Guid logId = Guid.NewGuid();
            if (_logToConsole) LogMessageConsole(strMessage, _logMessage, _logWarning, _logError, logId);
            if (_logToFile) LogMessageFile(strMessage, _logMessage, _logWarning, _logError, logId);
            if (_logToDatabase) LogMessageDB(strMessage, _logMessage, _logWarning, _logError, logId);
        }

        private static void LogMessageDB(string strMessage, bool message, bool warning, bool error, Guid logId)
        {
            LogDAO _LogDAO = new LogDAO();

            if(message) _LogDAO.InsertLogMessage(strMessage, "Message", logId);

            if (warning) _LogDAO.InsertLogMessage(strMessage, "Warning", logId);

            if (error) _LogDAO.InsertLogMessage(strMessage, "Error", logId);
        }

        private static void LogMessageFile(string strMessage, bool message, bool warning, bool error, Guid logId)
        {
            string pathDirectory = ConfigurationManager.AppSettings["DirectoryPath"];
            string fileLog = "LogApp_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            if (pathDirectory == null)
            {
                Console.WriteLine("Invalid configuration");
                return;
            }

            if (!Directory.Exists(pathDirectory))
                Directory.CreateDirectory(pathDirectory);

            using (StreamWriter w = new StreamWriter(Path.Combine(pathDirectory, fileLog)))
            {
                if (message)
                {
                    w.WriteLine("*******************************MESSAGE*********************************************");
                    w.WriteLine("Id: " + logId);
                    w.WriteLine("Date: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                    w.WriteLine("Description: " + strMessage);
                    w.WriteLine("*******************************END*************************************************");
                }

                if (warning)
                {
                    w.WriteLine("*******************************WARNING*********************************************");
                    w.WriteLine("Id: " + logId);
                    w.WriteLine("Date: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                    w.WriteLine("Description: " + strMessage);
                    w.WriteLine("*******************************END*************************************************");
                }

                if (error)
                {
                    w.WriteLine("*******************************ERROR***********************************************");
                    w.WriteLine("Id: " + logId);
                    w.WriteLine("Date: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                    w.WriteLine("Description: " + strMessage);
                    w.WriteLine("*******************************END*************************************************");
                }
            }
        }

        private static void LogMessageConsole(string strMessage, bool message, bool warning, bool error, Guid logId)
        {
            if (message)
            {
                Console.WriteLine("*******************************MESSAGE*********************************************");
                Console.WriteLine("Id: " + logId);
                Console.WriteLine("Date: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                Console.WriteLine("Description: " + strMessage);
                Console.WriteLine("*******************************END*************************************************");
            }

            if (warning)
            {
                Console.WriteLine("*******************************WARNING*********************************************");
                Console.WriteLine("Id: " + logId);
                Console.WriteLine("Date: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                Console.WriteLine("Description: " + strMessage);
                Console.WriteLine("*******************************END*************************************************");
            }

            if (error)
            {
                Console.WriteLine("*******************************ERROR***********************************************");
                Console.WriteLine("Id: " + logId);
                Console.WriteLine("Date: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                Console.WriteLine("Description: " + strMessage);
                Console.WriteLine("*******************************END*************************************************");
            }
        }
    }
}