using System;
using System.Collections.Generic;
using System.Text;

namespace Factory {
    public static class Extensions {
        public static bool CaseInsensitiveContains(this string text, string value, StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase) {
            return text.IndexOf(value, stringComparison) >= 0;
        }
    }
}
