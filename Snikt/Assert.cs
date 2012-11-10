using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snikt
{
    internal static class Assert
    {
        public static void ThrowIfNull(string value, string name)
        {
            ThrowIfNull(value, name, string.Empty);
        }

        public static void ThrowIfNull(string value, string name, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(name, message);
            }
        }
    }
}
