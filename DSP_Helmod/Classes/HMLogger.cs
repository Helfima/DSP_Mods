using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Classes
{
    public class HMLogger
    {
        public static LoggerLevel Level = LoggerLevel.Off;
        public static string Path;
        private static StreamWriter writer;
        public static void Error(string message)
        {
            if (Level >= LoggerLevel.Error) Write(LoggerLevel.Error, message);
        }
        public static void Warn(string message)
        {
            if (Level >= LoggerLevel.Warn) Write(LoggerLevel.Warn, message);
        }
        public static void Info(string message)
        {
            if (Level >= LoggerLevel.Info) Write(LoggerLevel.Info, message);
        }
        public static void Debug(string message)
        {
            if (Level >= LoggerLevel.Debug) Write(LoggerLevel.Debug, message);
        }

        public static void Trace(string message)
        {
            if (Level >= LoggerLevel.Trace) Write(LoggerLevel.Trace, message);
        }

        internal static void Write(LoggerLevel level, string message)
        {
            string date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string content = $"{date}|{level.ToString().ToUpper(),5}|{message}";
            UnityEngine.Debug.Log(content);
        }

    }
    public enum LoggerLevel
    {
        Off = 0,
        Error = 1,
        Warn = 2,
        Info = 3,
        Debug = 4,
        Trace = 5,
        All = 9,
    }
}
