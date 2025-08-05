using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ERParamUtils
{
    public class ObjectUtils
    {
        public static string GetPropertyValue(object obj, string propertyName, string def = "")
        {
            if (obj == null) { 
                return def;
            }

            Type type = obj.GetType();
            if (type == null)
            {
                return def;
            }

            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo == null)
            {
                return def;
            }

            object value = propertyInfo.GetValue(obj, null);

            if (value == null) 
            { return def; 
            }
            string str = value.ToString();
            if (str == null) { return def; }
            return str;
        }
    }
}
