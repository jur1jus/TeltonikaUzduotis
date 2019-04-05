using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace ConsoleApp3
{
	public class ReportMaking
	{
		public List<Report> Report { get; set; }
		public List<Data> Data { get; set; }
		public ReportMaking(List<Data> data)
		{
			if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ReportsPath"]))) {
				Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ReportsPath"]));
			}
			Data = data;
			Report = new List<Report>();
		}

		public void MakeReport()
		{
			var data = Data.GroupBy(x => new { x.Band, x.PCL }, (key, group) => new {
				Band = key.Band,
				PCL = key.PCL,
				Result = group.ToList()
			});

			foreach (var dataItem in data) {
				var reportData = new Report();
				var dataResult = dataItem.Result;
				var x1 = dataResult.Where(x => x.TxPower < x.MinPower).Any() ? dataResult.Where(x => x.TxPower < x.MinPower).Average(x => x.TxPower) : 0;
				var x2 = dataResult.Where(x => x.TxPower >= x.MinPower && x.TxPower <= x.MaxPower).Any() ? dataResult.Where(x => x.TxPower >= x.MinPower && x.TxPower <= x.MaxPower).Average(x => x.TxPower) : 0;
				var x3 = dataResult.Where(x => x.TxPower > x.MaxPower).Any() ? dataResult.Where(x => x.TxPower > x.MaxPower).Average(x => x.TxPower) : 0;
				var passCount = dataResult.Where(x => x.CheckResult == true).Count();
				var failCount = dataResult.Where(x => x.CheckResult == false).Count();

				reportData.Band = dataItem.Band;
				reportData.PCL = dataItem.PCL;
				reportData.TxPowerAvg1 = x1;
				reportData.TxPowerAvg2 = x2;
				reportData.TxPowerAgv3 = x3;
				reportData.CheckPassCount = passCount;
				reportData.CheckFailCount = failCount;

				Report.Add(reportData);
			}
		}

		public void SaveReportToFile()
		{
			var datetime = DateTime.Now;

			var filename = $"report-{datetime.ToShortDateString()}_{datetime.LongTime()}.csv";
			var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ReportsPath"], filename);
			File.Create(filePath).Dispose();

			StringBuilder reportText = new StringBuilder();
			reportText.Append($"BAND, PCL, TxPower < Min (avg.), TxPower in range (avg.), TxPower > Max (avg.), PASS Count, FAIL Count{Environment.NewLine}");

			foreach (var item in Report) {
				reportText.Append(item.FormatReportLine());
			}

			File.AppendAllText(filePath, reportText.ToString());
		}
	}
}
