using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Configuration;

namespace ConsoleApp3
{
	public class FileProcess
	{
		public string PathToFolder { get; set; }
		private string _dataStartReadingFlag = "Band, PCL, TX Power";
		private string _dataEndReadingPassFlag = ", PASS";
		private string _dataEndReadingFailFlag = ", FAIL";

		public List<Data> Data { get; set; }

		public FileProcess(string pathToFolder = null)
		{
			Data = new List<Data>();
			PathToFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["DataPath"]);
		}

		public void ReadFiles()
		{
			var directories = Directory.GetDirectories(PathToFolder);
			foreach (var directory in directories) {
				var files = Directory.GetFiles(directory);
				foreach (var file in files) {
					ReadFileLines(file);
				}
			}
		}

		private void ReadFileLines(string file)
		{
			var lines = File.ReadAllLines(file);

			var startIndex = Array.IndexOf(lines, lines.Where(l => l.StartsWith(_dataStartReadingFlag)).FirstOrDefault());
			var endIndex = Array.IndexOf(lines, lines.Where(l => l.EndsWith(_dataEndReadingPassFlag) || l.EndsWith(_dataEndReadingFailFlag)).LastOrDefault());

			Data dataFromFile;
			for (int i = startIndex; i <= endIndex; i++) {
				var line = lines[i];
				
				if (line.StartsWith(_dataStartReadingFlag)) {
					continue;
				}
				//File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "asd.txt", line + Environment.NewLine);
				var lineItems = line.Split(',');
				dataFromFile = new Data() {
					Band = lineItems[0],
					PCL = int.Parse(lineItems[1]),
					TxPower = float.Parse(lineItems[2], CultureInfo.InvariantCulture),
					TargetPower = float.Parse(lineItems[3], CultureInfo.InvariantCulture),
					MinPower = float.Parse(lineItems[4], CultureInfo.InvariantCulture),
					MaxPower = float.Parse(lineItems[5], CultureInfo.InvariantCulture),
					CheckResult = Helpers.ParseBool(lineItems[6].Trim())
				};
				Data.Add(dataFromFile);

			}
		}
	}
}
