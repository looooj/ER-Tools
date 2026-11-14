using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml.XPath;

namespace Utils
{



    public class ReplaceCommand {

        public string Id="none";
        public string TargetFunction="";
        public string TargetLine = "";

        public bool Active = false;
        public bool Finish = false;
        public int LeftCount = 0;
        public List<string> Lines=new();

    }

    public class ReplaceContext { 
    
        public int LineNo;
        public int Indent;
        public int SkipCount = 0;

        public bool IsConsole = false;
    }

    /*
    @@insert@@func@@line@@

    

     */

    public  class ReplaceSource
    {

        public static bool HasConsole()
        {
            try
            {
                // Accessing WindowHeight throws an IOException if no console is attached
                return Console.WindowHeight > 0;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public static void Proc(string source, string replaceCmd, string result) { 
        
            var sourceLines1 = File.ReadAllLines(source);
            var replaceLines1 = File.ReadAllLines(replaceCmd);
            List<string> sourceLines = new();
            sourceLines.AddRange(sourceLines1);
            List<string> replaceLines = new();
            replaceLines.AddRange(replaceLines1);
            List<string> resultLines = new();
            Proc(sourceLines, replaceLines, resultLines);

            File.WriteAllLines(result, resultLines);
        }
        public static void Proc(List<string> pyLines, List<string> replaceLines,List<string> result) {


            //var pylines = File.ReadAllLines(pyFile);

            // var replaceLines = File.ReadAllLines(replaceFile);

            //var cmd = "";
            //PythonSourceInfo currentSourceInfo = new();
            Dictionary<string, List<ReplaceCommand>> funcDict = new Dictionary<string, List<ReplaceCommand>>();

            //List<ReplaceCommand> cmds = new List<ReplaceCommand>();
            ReplaceCommand? currentCmd = null;
            foreach (var line in replaceLines) { 
            
                var line1=line.Trim();

                if (string.IsNullOrEmpty(line)) {
                    continue;
                }
                //if (line1.StartsWith("#")) {
                //    continue;
                //}

                if (line1.StartsWith("@@")) {

                    var items = line1.Substring(2,line1.Length-4).Split("@@");

                    currentCmd = new ReplaceCommand();

                    currentCmd.Id = items[0];
                    currentCmd.TargetFunction = items[1];
                    currentCmd.TargetLine = items[2];

                    //cmds.Add(currentCmd);

                    if (!funcDict.ContainsKey(currentCmd.TargetFunction))
                    {
                        funcDict.Add(currentCmd.TargetFunction, new List<ReplaceCommand>());
                    }
                    funcDict[currentCmd.TargetFunction].Add(currentCmd);
                    continue;
                }
                if (currentCmd != null) { 
                    currentCmd.Lines.Add(line);
                }
            }

            Proc(pyLines, funcDict, result);
        }

        public static void Proc(List<string> pyLines, Dictionary<string, List<ReplaceCommand>> funcDict, List<string> result)
        {

            //Dictionary<string,List<ReplaceCommand>> funcDict = new Dictionary<string,List<ReplaceCommand>>();
            //List<ReplaceCommand> currentCmds = null;
            ReplaceContext context = new();
            context.IsConsole = HasConsole();
            for (var i = 0; i < pyLines.Count; i++ ) 
            {
                var line = pyLines[i];
                result.Add(line);

                var line1 = line.Trim();

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                if (line1.StartsWith("#"))
                {
                    continue;
                }
                //def t000001000_x31
                if (line1.StartsWith("def ") && line1.EndsWith(":") ) {
                    int indent = line.IndexOf("def");
                    if (funcDict.ContainsKey(line1)) { 
                    
                        var currentCmds = funcDict[line1];
                        context.LineNo = i + 1;
                        context.Indent = indent;

                        i = ProcCmd(pyLines, context, currentCmds, result);
                    }
                }

            }
        }

        static int ProcCmd(List<string> pyLines, ReplaceContext context, List<ReplaceCommand> cmds,List<string> result) {

            var i = context.LineNo;
            for (; i < pyLines.Count; i++) {

                var line = pyLines[i];
                if (context.SkipCount < 1) { 
                   result.Add(line);            
                }
                if (context.SkipCount > 0) {
                    context.SkipCount--;
                }

                var line1 = line.Trim();

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                if (line1.StartsWith("#"))
                {
                    continue;
                }

                if (line1.StartsWith("def ") && line1.EndsWith(":")) {
                    if (line.IndexOf("def") == context.Indent) {
                        break;
                    }
                }
                ProcLines(line,i, context, cmds, result);
            }
            return i;             
        }

        static void ProcLines(string line, int lineNo, ReplaceContext context, List<ReplaceCommand> cmds, List<string> result)
        {

            foreach (var cmd in cmds)
            {

                if (cmd.Finish)
                {
                    continue;
                }                

                if (!line.Contains(cmd.TargetLine))
                {
                    continue;
                }
                if (context.IsConsole)
                {
                    Console.WriteLine(cmd.Id);
                    Console.WriteLine(cmd.TargetLine);
                    foreach (var tmp in cmd.Lines) {
                        Console.WriteLine(tmp);
                    }
                }

                if (cmd.Id == "insert")
                {
                    result.InsertRange(result.Count - 1, cmd.Lines);
                    cmd.Finish = true;
                    cmd.Active = true;
                }
                if (cmd.Id == "append")
                {
                    result.AddRange(cmd.Lines);
                    cmd.Finish = true;
                    cmd.Active = true;
                }
                if (cmd.Id == "replace")
                {
                    cmd.Active = true;
                    cmd.Finish = true;
                    result.RemoveAt(result.Count - 1);
                    result.AddRange(cmd.Lines);
                    context.SkipCount = cmd.Lines.Count - 1;
                }
            }
        }
    }

}
