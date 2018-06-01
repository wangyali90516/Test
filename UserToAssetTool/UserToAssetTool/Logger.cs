using System;
using System.IO;
using System.Net.Mime;
using System.Windows.Forms;

namespace WindowsFormsMigrationData
{
    public static class Logger
    {
        public static readonly object objLock = new object();

        public static void LogError(string message, int type)
        {
            lock (objLock)
            {
                string typeName = type == 0 ? "CommonError" : "ErrorAssert";
                string pathR = Application.StartupPath + $"\\log{typeName}.txt";
                if (!File.Exists(pathR))
                {
                    //创建
                    using (FileStream fs = new FileStream(pathR, FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.WriteLine(message + "----------" + DateTime.Now);
                        }
                    }
                }
                else
                {
                    FileInfo file = new FileInfo(pathR);
                    using (StreamWriter sww = file.AppendText())
                    {
                        sww.WriteLine(message + "----------" + DateTime.Now);
                    }
                }
            }
        }

        public static void LogRecord(string message, int type)
        {
            string typeName = type == 0 ? "RecordUser" : "RecordAssert";
            lock (objLock)
            {
                string pathR = Application.StartupPath + $"\\log{typeName}.txt";
                if (!File.Exists(pathR))
                {
                    //创建
                    using (FileStream fs = new FileStream(pathR, FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.WriteLine(message);
                        }
                    }
                }
                else
                {
                    FileInfo file = new FileInfo(pathR);
                    using (StreamWriter sww = file.AppendText())
                    {
                        sww.WriteLine(message);
                    }
                }
            }
        }

        public static void Logw(string typeName,string message)
        {
            lock (objLock)
            {
                string pathR = Application.StartupPath + $"\\log{typeName}.txt";
                if (!File.Exists(pathR))
                {
                    //创建
                    using (FileStream fs = new FileStream(pathR, FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.WriteLine(message);
                        }
                    }
                }
                else
                {
                    FileInfo file = new FileInfo(pathR);
                    using (StreamWriter sww = file.AppendText())
                    {
                        sww.WriteLine(message);
                    }
                }
            }
        }
    }
}