using System;
using System.Reflection;
using System.Diagnostics;
using Gtk;
using Yahoo.Finance;
using System.Runtime.InteropServices;
using System.IO;

namespace indicatorstocks
{
	public class UserEventHandler
	{
		public UserEventHandler()
		{
		}

		[DllImport ("glib-2.0.dll")]
		static extern IntPtr g_get_language_names ();

		public void OnHelp(object sender, EventArgs args)
		{
			Assembly asm = Assembly.GetExecutingAssembly();

			foreach (var lang in GLib.Marshaller.NullTermPtrToStringArray (g_get_language_names (), false)) {
                string path = String.Format ("{0}/gnome/help/{1}/{2}",
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
				    (asm.GetCustomAttributes(
						typeof (AssemblyTitleAttribute), false)[0]
						as AssemblyTitleAttribute).Title,
				    lang);

                if (System.IO.Directory.Exists (path)) {
					Process.Start(String.Format("ghelp://{0}", path));
                    break;
                }
            }
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

