using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.YGOSharp.Extensions
{
    public static class Int32Extensions
    {
        public static Boolean[] ToBooleanArray(this Int32 i)
        {
            return Convert.ToString(i, 2 /*for binary*/).Select(s => s.Equals('1')).ToArray();
        }
    }

}
