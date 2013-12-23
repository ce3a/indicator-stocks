using System;
using System.Reflection;
using System.Diagnostics;
using Gtk;
using System.Runtime.InteropServices;
using System.IO;

using ce3a.Yahoo.Finance;

namespace indicatorstocks
{
	public class UserEventHandler
	{
		public UserEventHandler()
		{
		}

		public void OnPrefs(object sender, EventArgs args)
		{
			PreferencesDialog preferencesDialog = new PreferencesDialog();

			preferencesDialog.Run();
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
			
			dialog.Version = String.Format("{0}.{1}.{2}",
			                               asm.GetName().Version.Major,
			                               asm.GetName().Version.Minor,
			                               asm.GetName().Version.Build);

			dialog.Comments = (asm.GetCustomAttributes(
				typeof (AssemblyDescriptionAttribute), false)[0]
				as AssemblyDescriptionAttribute).Description;

			dialog.Comments += String.Format("\n\nRevision: {0}", asm.GetName().Version.Revision);
			
			dialog.Copyright = (asm.GetCustomAttributes(
				typeof (AssemblyCopyrightAttribute), false)[0]
				as AssemblyCopyrightAttribute).Copyright;

			dialog.LogoIconName = About.LogoIconName;

			dialog.License = About.License;

			dialog.Authors = About.Authors;

			dialog.Artists = About.Artists;

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

