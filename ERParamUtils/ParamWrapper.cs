using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    class ParamWrapper
    {
        public string Name;
        private FSParam.Param Param;

        public FSParam.Param GetParam() {
            return Param;
        }
        public IReadOnlyList<FSParam.Param.Row> Rows
        {
            get => Param.Rows;
        }

        public ParamWrapper(string name, FSParam.Param p) {
            Name = name;
            Param = p;
        }
    }
}
