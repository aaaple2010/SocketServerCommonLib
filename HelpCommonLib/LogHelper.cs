using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelpCommonLib
{
    public class LogHelper
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("RollingLogFileAppender");

        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("RollingLogFileAppender");

        public static void WriteLog(string info)
        {
            loginfo.Info(info);
        }

        public static void WriteLog(string info, Exception ex)
        {
            logerror.Error(info, ex);
        }
    }
}
