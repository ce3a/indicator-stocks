using System;
using System.Reflection;
using System.Diagnostics;
using Gtk;
using Yahoo.Finance;

namespace indicatorstocks
{
	public class UserEventHandler
	{
		public UserEventHandler()
		{
		}

		public void OnInfo(object sender, EventArgs args)
		{
			AboutDialog dialog = new AboutDialog();
			Assembly asm = Assembly.GetExecutingAssembly();
			
			dialog.ProgramName = (asm.GetCustomAttributes(
				typeof (AssemblyTitleAttribute), false)[0]
				as AssemblyTitleAttribute).Title;
			
			dialog.Version = asm.GetName().Version.ToString();
			
			dialog.Comments = (asm.GetCustomAttributes(
				typeof (AssemblyDescriptionAttribute), false)[0]
				as AssemblyDescriptionAttribute).Description;
			
			dialog.Copyright = (asm.GetCustomAttributes(
				typeof (AssemblyCopyrightAttribute), false)[0]
				as AssemblyCopyrightAttribute).Copyright;

			dialog.Response += delegate {
				dialog.Destroy();
			};
			
			dialog.Run();
		}

		public void OnQuit(object sender, EventArgs args)
		{
			Application.Quit();
		}

		public void OnQuoteSelected(object sender, EventArgs args)
		{
			string symbol = Indicator.GetSymbolFromMenuItem((MenuItem)sender);
			string url = Quotes.GetChartUrl(symbol);

			Process.Start(url);
		}
	}
}

