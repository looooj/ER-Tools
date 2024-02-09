using ERParamCmd;
using NLog;

namespace ERParamCmd2
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

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length <= 1) {
                ERParamCommand.PrintHelp("");
                return;
            }

            int argsLen = args.Length - 1;
            string[] args2 = new string[argsLen];

            Array.Copy(args, 1, args2, 0, argsLen);

            ERParamCommand.Run(args2);
            

        }

        static void InitNLogger()
        {

            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = ".\\logs\\ERParamCmd-log.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logconsole);
            // Apply config           
            NLog.LogManager.Configuration = config;
        }
    }
}