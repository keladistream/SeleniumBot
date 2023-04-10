using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumBot
{
    public static class Tools
    {
        public static void PrintOutput(string input)
        {
            Console.WriteLine("[" + DateTime.Now + "] " + input);
        }

        public static int StandardTimeout = 1500;
    }
}
