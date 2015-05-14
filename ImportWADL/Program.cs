using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImportWADL
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			Models.WADL wadl = new Models.WADL("C:\\Users\\Nathan\\Documents\\GitHub\\RestClient.NET\\SolutionItems\\YahooSearch.wadl");

			string code = wadl.CreateRestClientDotNetClass(SkaCahToa.Rest.RestClientBase.DataTypes.JSON, "testClassName", "SkaCahToa.Rest.CustomApis");

			using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"WriteLines2.txt"))
			{
				file.Write(code);
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}