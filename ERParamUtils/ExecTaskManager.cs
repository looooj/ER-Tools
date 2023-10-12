using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class ExecTask { 
    

    }

    public class ExecTaskManager
    {

        static ConcurrentDictionary<string,int> TaskDict = new();

        public static void Start(string taskName,int minWaitTime) {

            TaskDict.TryAdd(taskName, minWaitTime);

        }
        public static void End(string taskName) {

            TaskDict.Remove(taskName, out int v);

        }
        public void End() {

            TaskDict.Clear();
        }

        public static bool HaveTask() {

            return (TaskDict.Count > 0);
        }


    }
}
