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

    }
}
