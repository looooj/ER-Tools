using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERParamEditor
{
    public class Tests
    {


        public static void TestName() {

            string[] names = { "a", "abc", "abc123","123" };

            var regx = new Regex("^[a-z]{3,}[0-9]?");
            foreach(string name in names)
            if (!regx.Match(name).Success)
            {
                MessageBox.Show("Not Match " + name);
            }

        }


        public static void Run() {

            TestName();
        }
    }
}
