using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPokeBot {
    public static class Tools {

        public static string CapFirst(string s) {
            if (s == null) return null;

            if (s.Length > 1) return char.ToUpper(s[0]) + s.Substring(1);

            return s.ToUpper();
        }

        public static SortedDictionary<int,int> StringToIntDict(string s) {
            Dictionary<int, int> retVal = s.Split(',')
                .Select(p => p.Trim().Split(':'))
                .ToDictionary(p => int.Parse(p[0]), p => int.Parse(p[1]));
            SortedDictionary<int, int> retValReal = new SortedDictionary<int, int>(retVal);
            return retValReal;
        }

        public static string IntDictToString(SortedDictionary<int,int> d) {
            string retVal = d.Aggregate(new StringBuilder(),
                (a, b) => a.Append(",").Append(b.Key.ToString()).Append(":").Append(b.Value.ToString()),
                (a) => a.Remove(0, 1).ToString());
            return retVal;
        }

    }
}
