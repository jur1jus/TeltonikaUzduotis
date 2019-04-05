using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp3
{
	public class Program
	{
		static void Main(string[] args)
		{
			try {
				FileProcess fileProcess = new FileProcess();
				Console.WriteLine("Start reading data...");
				fileProcess.ReadFiles();
				Console.WriteLine("Complete reading data...");
				ReportMaking reportMaking = new ReportMaking(fileProcess.Data);
				Console.WriteLine("Start making report...");
				reportMaking.MakeReport();
				Console.WriteLine("Complete making report...");
				Console.WriteLine("Start saving report...");
				reportMaking.SaveReportToFile();
				Console.WriteLine("Complete saving report...");
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
			
			Console.WriteLine("Press any key to close program...");
			Console.ReadKey();
		}
	}
}
