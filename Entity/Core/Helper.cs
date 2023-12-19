using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Core
{
    public static class Helper
    {
        public static List<string>? ToStringArray(string? value, char splitter = ',')
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return value.Trim(splitter).Split(splitter).ToList();
        }
    }
}
