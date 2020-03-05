using System;
using System.IO;
using System.Text;

namespace UnhandledException
{
    class ErrorHandler
    {
        public static void Catch(string Func_Name, Exception exception, string additional_info = "")
        {
            string LocalStorage = Cons.MyDocuments + @"\_Maoci_Logs\";
            if (!Directory.Exists(LocalStorage))
                Directory.CreateDirectory(LocalStorage);
            using (StreamWriter sw = new StreamWriter(LocalStorage + Func_Name + ".log", false, Encoding.UTF8, 65536))
            {
                sw.WriteLine(
                    ">>>>>>>>>>>>>" + Environment.NewLine +
                    additional_info + Environment.NewLine +
                    exception.Message + Environment.NewLine +
                    exception.Source+ Environment.NewLine +
                    exception.StackTrace + Environment.NewLine +
                    "<<<<<<<<<<<<<"
                );
            }
        }
        public static void Dump(string Func_Name, string additional_info = "")
        {
            string LocalStorage = Cons.MyDocuments + "\\_Maoci_Logs\\";
            if (!Directory.Exists(LocalStorage))
                Directory.CreateDirectory(LocalStorage);
            using (StreamWriter sw = new StreamWriter(LocalStorage + Func_Name + ".log", false, Encoding.UTF8, 65536))
            {
                sw.WriteLine(
                    ">>>>>>>>>>>>>" + Environment.NewLine +
                    additional_info + Environment.NewLine +
                    "<<<<<<<<<<<<<"
                );
            }
        }
    }
}
