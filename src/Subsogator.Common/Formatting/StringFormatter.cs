using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Infrastructure.Formatting
{
    public class StringFormatter
    {
        public static string FormatCheckString(string checkStringToFormat) 
        {
            return checkStringToFormat.Trim().ToLower();
        }
    }
}
