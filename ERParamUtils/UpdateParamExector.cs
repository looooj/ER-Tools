using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class UpdateParamOptions {

        public bool Restore = true;
        public bool Publish = true;
        public string Target = "*";
    }

    class UpdateParamExector
    {
        public void Exec(ParamProject project, UpdateParamOptions options) {


            if (options.Restore) {
                project.Restore();
            }

        }
    }
}
