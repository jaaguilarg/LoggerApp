using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestLogApp;

namespace LoggerApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Empezando el Test");

            UnitTestApp test1 = new UnitTestApp();

            test1.TestMethodLogConsoleAll();
            test1.TestMethodLogConsoleMessage();
            test1.TestMethodLogConsoleWarning();
            test1.TestMethodLogConsoleError();

            test1.TestMethodLogFileAll();
            test1.TestMethodLogFileMessage();
            test1.TestMethodLogFileWarning();
            test1.TestMethodLogFileError();

            test1.TestMethodLogDatabaseAll();
            test1.TestMethodLogDatabaseMessage();
            test1.TestMethodLogDatabaseWarning();
            test1.TestMethodLogDatabaseError();

            Console.WriteLine("Presione una tecla para salir...");
            Console.ReadLine();
        }
    }
}
