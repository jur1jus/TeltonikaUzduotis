using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
	public class Report
	{
		public string Band { get; set; }

		public int PCL { get; set; }

		public float TxPowerAvg1 { get; set; }

		public float TxPowerAvg2 { get; set; }

		public float TxPowerAgv3 { get; set; }

		public int CheckPassCount { get; set; }

		public int CheckFailCount { get; set; }

		public string FormatReportLine()
		{
			return $"{Band}, {PCL}, {TxPowerAvg1}, {TxPowerAvg2}, {TxPowerAgv3}, {CheckPassCount}, {CheckFailCount}{Environment.NewLine}";
		}

	}
}
