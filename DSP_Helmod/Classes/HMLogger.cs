using System;
using System.Activities.Debugger;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Classes
{
    public class HMLogger
    {
        public static LoggerLevel Level = LoggerLevel.Off;
        public static string Path;
        public static void Error(string message, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            if (Level >= LoggerLevel.Error) Write(LoggerLevel.Error, message, file, member, line);
        }
        public static void Warn(string message, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            if (Level >= LoggerLevel.Warn) Write(LoggerLevel.Warn, message, file, member, line);
        }
        public static void Info(string message, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            if (Level >= LoggerLevel.Info) Write(LoggerLevel.Info, message, file, member, line);
        }
        public static void Debug(string message, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            if (Level >= LoggerLevel.Debug) Write(LoggerLevel.Debug, message, file, member, line);
        }

        public static void Trace(string message, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            if (Level >= LoggerLevel.Trace) Write(LoggerLevel.Trace, message, file, member, line);
        }

        internal static void Write(LoggerLevel level, string message, string file, string member, int line)
        {
            string content = $"{level.ToString().ToUpper(),5}|{System.IO.Path.GetFileName(file)}:{line}|{message}";
            UnityEngine.Debug.Log(content);

            using (StreamWriter w = File.AppendText("C:/Temp/helmod.log"))
            {
                WriterLog(w, content);
            }
        }
        internal static void WriterLog(TextWriter w, string message)
        {
            w.WriteLine(message);
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
