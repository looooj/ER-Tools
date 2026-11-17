using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml.XPath;
using static System.Net.Mime.MediaTypeNames;

namespace Utils
{



    public class ReplaceCommand
    {

        public string Id = "none";
        public string TargetFunction = "";
        public string TargetLine = "";

        public bool Active = false;
        public bool Finish = false;
        public int LeftCount = 0;
        public List<string> Lines = new();

    }

    public class ReplaceContext
    {

        public string? TargetFunction = "";
        public int LineNo;
        public int Indent;
        public int FinishCount = 0;
        public int SkipCount = 0;
        public int SkipIndent = 0;
        public bool IsConsole = false;
        public List<ReplaceCommand> Commands = new();
    }

    /*
    @@insert@@func@@line@@

    

     */

    public class ReplaceSource
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

        public static void Proc(string source, string replaceCmd, string result)
        {

            var sourceLines1 = File.ReadAllLines(source);
            var replaceLines1 = File.ReadAllLines(replaceCmd);
            List<string> sourceLines = new();
            sourceLines.AddRange(sourceLines1);
            List<string> replaceLines = new();
            replaceLines.AddRange(replaceLines1);
            List<string> resultLines = new();
            Proc(sourceLines, CheckInclude(replaceCmd, replaceLines), resultLines);

            File.WriteAllLines(result, resultLines);
        }

        static Dictionary<string, string> varDict = new();
        static List<string> CheckInclude(string sourcePath, List<string> replaceLines)
        {

            var ret = new List<string>();

            foreach (var line in replaceLines)
            {

                if (line.StartsWith("@@include@@"))
                {

                    var items = line.Split("@@",StringSplitOptions.RemoveEmptyEntries);
                    var fn = Directory.GetParent(sourcePath) + "\\" + items[1];
                    var tmp = File.ReadAllLines(fn);
                    ret.AddRange(tmp);
                    continue;
                }
                ret.Add(line);
            }
            return ret;

        }
        public static void Proc(List<string> pyLines, List<string> replaceLines, List<string> result)
        {


            //var pylines = File.ReadAllLines(pyFile);

            // var replaceLines = File.ReadAllLines(replaceFile);

            //var cmd = "";
            //PythonSourceInfo currentSourceInfo = new();
            Dictionary<string, List<ReplaceCommand>> funcDict = new Dictionary<string, List<ReplaceCommand>>();

            //List<ReplaceCommand> cmds = new List<ReplaceCommand>();
            ReplaceCommand? currentCmd = null;
            foreach (var line in replaceLines)
            {

                var line1 = line.Trim();

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                if (line.StartsWith("#"))
                {
                    continue;
                }
                if (line1.StartsWith("@@var@@"))
                {
                    var items = line1.Split("@@",StringSplitOptions.RemoveEmptyEntries);
                    if (items.Length < 2)
                        continue;
                    varDict.Add(items[1], items[2]);
                    continue;
                }
                if (line1.StartsWith("@@"))
                {

                    var items = line1.Split("@@", StringSplitOptions.RemoveEmptyEntries);
                    //line1.Substring(2, line1.Length - 4).Split("@@");
                    if (items.Length < 2)
                        continue;
                    currentCmd = new ReplaceCommand();

                    currentCmd.Id = items[0];
                    currentCmd.TargetFunction = items[1];
                    currentCmd.TargetLine = items[2];

                    if (!funcDict.ContainsKey(currentCmd.TargetFunction))
                    {
                        funcDict.Add(currentCmd.TargetFunction, new List<ReplaceCommand>());
                    }
                    funcDict[currentCmd.TargetFunction].Add(currentCmd);
                    continue;
                }
                if (currentCmd != null)
                {
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
            for (var i = 0; i < pyLines.Count; i++)
            {
                var line = pyLines[i];
                result.Add(line);

                var line1 = line.Trim();

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                //if (line.StartsWith("#"))
                //{
                //    continue;
                //}

                //if (line1.StartsWith("def ") && line1.EndsWith(":") ) {
                //int indent = line.IndexOf("def");
                if (funcDict.ContainsKey(line1))
                {

                    //var currentCmds = funcDict[line1];
                    context.TargetFunction = line;
                    context.Commands = funcDict[line1];
                    context.LineNo = i + 1;
                    context.FinishCount = 0;
                    context.SkipCount = 0;
                    context.SkipIndent = 0;
                    context.Indent = GetIndent(line);
                    if (context.IsConsole)
                    {
                        Console.WriteLine(line);
                    }
                    ProcVar(context);
                    i = ProcCmd(pyLines, context, result);
                }
                //}

            }
        }

        static int ProcCmd(List<string> pyLines, ReplaceContext context, List<string> result)
        {

            var cmds = context.Commands;
            var i = context.LineNo;
            for (; i < pyLines.Count; i++)
            {

                var line = pyLines[i];

                if (context.SkipIndent > 0)
                {
                    if (context.SkipIndent == GetIndent(line))
                    {
                        context.SkipCount = 1;
                    }
                    else
                    {
                        context.SkipIndent = 0;
                    }
                }
                if (context.SkipCount < 1)
                {
                    result.Add(line);
                }
                if (context.SkipCount > 0)
                {
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

                //if (line1.StartsWith("def ") && line1.EndsWith(":")) {
                //    if (line.IndexOf("def") == context.Indent) {
                //        break;
                //    }
                //}
                if (GetIndent(line) <= context.Indent)
                {
                    if (context.IsConsole)
                    {
                        Console.WriteLine("cmd count={0} finish={1}", context.Commands.Count, context.FinishCount);
                    }
                    break;
                }
                
                ProcLine(line, i, context, result);
            }
            return i;
        }

        static string ApplyVar(string line)
        {
            var ret = line;
            foreach (var key in varDict.Keys)
            {

                var val = varDict[key];
                ret = ret.Replace(key, val);
            }
            return ret;
        }
        static int GetIndent(string text)
        {

            int indent = text.Length - text.TrimStart(' ').Length;
            return indent;
        }

        static void ProcVar(ReplaceContext context) {

            foreach (var cmd in context.Commands) {

                for (int i = 0; i < cmd.Lines.Count; i++) {

                    cmd.Lines[i]=ApplyVar(cmd.Lines[i]);
                }
            }
        } 
        static int ProcLine(string line, int lineNo, ReplaceContext context, List<string> result)
        {
            var cmds = context.Commands;
            var index = 0;

            foreach (var cmd in cmds)
            {
                index++;
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
                    foreach (var tmp in cmd.Lines)
                    {
                        Console.WriteLine(tmp);
                    }
                }
                if (cmd.Id == "cancel")
                {
                    cmd.Finish = true;
                    cmd.Active = true;
                    if (index <= cmds.Count)
                        cmds[index].Finish = true;
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
                if (cmd.Id == "append-block")
                {
                    cmd.Active = true;
                    cmd.Finish = true;
                    context.SkipIndent = GetIndent(cmd.Lines[0]);
                    result.AddRange(cmd.Lines);
                }
                if (cmd.Id == "replace-block")
                {
                    cmd.Active = true;
                    cmd.Finish = true;
                    context.SkipIndent = GetIndent(cmd.Lines[0]);
                    result.RemoveAt(result.Count - 1);
                    result.AddRange(cmd.Lines);
                }
                if (cmd.Id == "replace")
                {
                    cmd.Active = true;
                    cmd.Finish = true;
                    result.RemoveAt(result.Count - 1);
                    result.AddRange(cmd.Lines);
                    //if (cmd.Lines.Count > 1)
                    //    lineNo = lineNo + (cmd.Lines.Count - 1);
                    context.SkipCount = (cmd.Lines.Count - 1);
                }
                if (cmd.Finish)
                {
                    context.FinishCount++;
                }
            }
            return lineNo;
        }
    }

}
