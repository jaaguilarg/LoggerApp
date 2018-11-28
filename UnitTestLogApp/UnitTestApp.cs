using System;
using LoggerApp.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestLogApp
{
    [TestClass]
    public class UnitTestApp
    {
        private JobLogger jobLogger;

        #region Pruebas LogConsole

        [TestMethod]
        public void TestMethodLogConsoleAll()
        {
            jobLogger = new JobLogger(false, true, false, true, true, true);
            JobLogger.LogMessage("Log in console Message, Warning, Error");
        }

        [TestMethod]
        public void TestMethodLogConsoleMessage()
        {
            jobLogger = new JobLogger(false, true, false, true, false, false);
            JobLogger.LogMessage("Log in console Message");
        }

        [TestMethod]
        public void TestMethodLogConsoleWarning()
        {
            jobLogger = new JobLogger(false, true, false, false, true, false);
            JobLogger.LogMessage("Log in console Warning");
        }

        [TestMethod]
        public void TestMethodLogConsoleError()
        {
            jobLogger = new JobLogger(false, true, false, false, false, true);
            JobLogger.LogMessage("Log in console Warning");
        }

        #endregion Pruebas LogConsole

        #region Pruebas LogFile

        [TestMethod]
        public void TestMethodLogFileAll()
        {
            jobLogger = new JobLogger(true, false, false, true, true, true);
            JobLogger.LogMessage("Log in file Message, Warning, Error");
        }

        [TestMethod]
        public void TestMethodLogFileMessage()
        {
            jobLogger = new JobLogger(true, false, false, true, false, false);
            JobLogger.LogMessage("Log in file Message");
        }

        [TestMethod]
        public void TestMethodLogFileWarning()
        {
            jobLogger = new JobLogger(true, false, false, false, true, false);
            JobLogger.LogMessage("Log in file Warning");
        }

        [TestMethod]
        public void TestMethodLogFileError()
        {
            jobLogger = new JobLogger(true, false, false, false, false, true);
            JobLogger.LogMessage("Log in file Error");
        }

        #endregion Pruebas LogFile

        #region Pruebas LogDatabase

        [TestMethod]
        public void TestMethodLogDatabaseAll()
        {
            jobLogger = new JobLogger(false, false, true, true, true, true);
            JobLogger.LogMessage("Log in file Message, Warning, Error");
        }

        [TestMethod]
        public void TestMethodLogDatabaseMessage()
        {
            jobLogger = new JobLogger(false, false, true, true, false, false);
            JobLogger.LogMessage("Log in file Message");
        }

        [TestMethod]
        public void TestMethodLogDatabaseWarning()
        {
            jobLogger = new JobLogger(false, false, true, false, true, false);
            JobLogger.LogMessage("Log in file Warning");
        }

        [TestMethod]
        public void TestMethodLogDatabaseError()
        {
            jobLogger = new JobLogger(false, false, true, false, false, true);
            JobLogger.LogMessage("Log in file Error");
        }

        #endregion Pruebas LogDatabase
    }
}
