using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlSentenceBuilder.Tests.Extensions
{
    static class StringExtensions
    {
        internal static string TrimEveryWhere(this string self)
        {
            self = self.Trim();
            while (self.Contains("  "))
            {
                self = self.Replace("  ", " ");
            }
            self = self.Replace(", ", ",");
            return self;

        }
    }
}
