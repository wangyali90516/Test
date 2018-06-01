using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateRegularAssetToProduct
{
    public class Log
    {
        public static void WriteLog(string message, string logname)
        {
            //如果日志文件为空，则默认在Debug目录下新建 YYYY-mm-dd_Log.log文件
            //把异常信息输出到文件
            StreamWriter sw = new StreamWriter(logname, true);
            sw.WriteLine(message);
            sw.Close();
        }
    }
}
