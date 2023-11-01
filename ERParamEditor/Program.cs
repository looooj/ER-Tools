using Microsoft.VisualBasic.Logging;
using NLog;

namespace ERParamEditor
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.




            
            ApplicationConfiguration.Initialize();
            InitNLogger();
            Application.Run(new MainForm());
        }
        static void InitNLogger() {

            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = ".\\logs\\ERParam-log.txt" };
            
            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
                        // Apply config           
            NLog.LogManager.Configuration = config;
        }
    }
}