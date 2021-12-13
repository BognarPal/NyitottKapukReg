using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NyitottKapukReg.WebAPI.Controllers
{
    public static class ControllerExtension
    {
        private static object lockLogFile = new object();
        private static readonly Random globalRandom = new Random();
        private static object lockRandom = new object();

        private static string LogFileName()
        {
            try
            {
                string runningDirectory = Assembly.GetExecutingAssembly().Location;
                string logPath = Path.Combine(runningDirectory, "log");
                if (!Directory.Exists(logPath))
                    Directory.CreateDirectory(logPath);
                logPath = Path.Combine(logPath, DateTime.Today.Year.ToString());
                if (!Directory.Exists(logPath))
                    Directory.CreateDirectory(logPath);
                logPath = Path.Combine(logPath, DateTime.Today.Month.ToString("D2"));
                if (!Directory.Exists(logPath))
                    Directory.CreateDirectory(logPath);

                return Path.Combine(logPath, $"{DateTime.Today:yyyy.MM.dd}.log");
            }
            catch
            {
                return null;
            }
        }

        private static void WriteLog(string message)
        {
            try
            {
                lock (lockLogFile)
                {
                    string logFileName = LogFileName();
                    if (logFileName != null)
                    {
                        using (var sw = new StreamWriter(logFileName, true, Encoding.UTF8))
                        {
                            sw.WriteLine($"{DateTime.Now:HH:mm:ss}\t{message}");
                            sw.Close();
                        }
                    }
                }
            }
            catch { }
        }

        private static void WriteLog(Exception ex)
        {
            var newLine = "\n        \t";
            var message = $"{ex.Message}{newLine}{ex.StackTrace.Replace("\n", newLine)}";
            WriteLog(message);
        }

        public static ActionResult Run(this ControllerBase controller, Func<ActionResult> function)
        {
            try
            {
                return function();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return controller.BadRequest(new
                {
#if DEBUG
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace
#else
                    ErrorMessage = "Váratlan hiba"
#endif
                });
            }
        }

        public static int NewRandom(this ControllerBase controller, int n)
        {
            lock (lockRandom)
            {
                return globalRandom.Next(n);
            }
        }
    }
}
