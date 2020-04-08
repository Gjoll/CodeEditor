using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeEditor
{
    public static class StringExtensions
    {
        public static String[] ToLines(this String value)
        {
            if (value == null)
                return Array.Empty<string>();
            value = value.Replace("\r", "");
            if (value.EndsWith("\n"))
                value = value.Substring(0, value.Length - 1);
            return value.Split('\n');
        }
    }
}
