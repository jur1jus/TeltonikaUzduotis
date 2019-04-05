using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
	public static class Helpers
	{
		public static bool ParseBool(string value)
		{
			return value == "1" || value == "True" || value == "true" || value == "PASS" || value == "pass" ? true : false;
		}

		public static string LongTime(this DateTime value)
		{
			return $"{value.Hour}{value.Minute}{value.Second}";
		}
	}
}
