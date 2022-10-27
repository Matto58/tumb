namespace TheUltimateMicroBeast
{
    public class Logger
    {
        public struct LogType
        {
            public static int Info = 0;
            public static int Warning = 1;
            public static int Error = 2;

            public LogType()
            {
            }
        }
        public static void Log(string text, int logType = 0, bool addLn = true)
        {
            if (logType == 0) Console.ForegroundColor = ConsoleColor.DarkGray;
            if (logType == 1) Console.ForegroundColor = ConsoleColor.Yellow;
            if (logType == 2) Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(
                "<" +
                (logType == 1 ? "WARN" : logType == 2 ? "ERROR" : "INFO") +
                "> " + text +
                (addLn ? "\n" : "")
            );
            Console.ResetColor();
        }
    }
}