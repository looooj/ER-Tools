using Utils;

namespace ReplaceTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string src;
            string cfg;
            if (args.Length > 0 && args[0] == "t2")
            {
                //src = @"E:\games\er\ESDStudio_v1.0.2\net6.0-windows\esdtool\t602006000.py";
                src = @"E:\games\er\ESDStudio_v1.0.2\net6.0-windows\esdtool\t800001100.py";
                cfg = @"D:\myprojects\game-tools\ER-Tools\templates\commons\rp04.txt";
            }
            else
            {
                if (args.Length < 2)
                {

                    printUsage();
                    return;
                }

                src = args[0];
                cfg = args[1];
            }

            string result = src + ".py";
            ReplaceSource.Proc(src, cfg, result);
        }

        static void printUsage() {

            Console.WriteLine("ReplaceTool source replace-config ");
        }
    }
}
