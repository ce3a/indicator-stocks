using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Timers;
using System.Runtime.InteropServices;
using System.IO;
using Gtk;
using Gdk;
using Pango;
using AppIndicator;
using ce3a.Yahoo.Finance;
using ce3a.Logging;

namespace indicatorstocks
{
	public class Indicator : IObserver<ConfigurationProvider>
	{
		private ApplicationIndicator indicator;
		private Menu menu;

		private Configuration config = Configuration.Instance;
		private System.Timers.Timer timer;
		private System.Object thisLock = new System.Object();
		private static ILogger logger;

		private static readonly char tabChar = '\t';
		private static readonly string quoteUnknown = "???";
		private static readonly char quotePadChar = '\u2007';

		private string[] symbols;
		private string[] Symbols
		{
			get { return symbols; }
			set 
			{ 
				lock (thisLock)
				{
					symbols = value;
					menu.Dispose();
					BuildMenu();
				}
			}
		}

		public Indicator(string name)
		{
			logger = LogManager.Logger;

			indicator = new ApplicationIndicator(name, name, Category.ApplicationStatus);

			symbols = config.Symbols;

			BuildMenu();

			config.Subscribe(this);
		}

		public void Start()
		{
			timer = new System.Timers.Timer(1000);
			timer.Elapsed += new ElapsedEventHandler(OnTimer);
			timer.Enabled = true;
		}

		public void Stop()
		{
			timer.Dispose();
		}

		public static string GetSymbolFromMenuItem(MenuItem menuItem)
		{
			string symbol = ((Label)menuItem.Child).Text;
			return symbol.Substring(0, symbol.IndexOf(tabChar));
		}

		private void BuildMenu()
		{
			indicator.Status = AppIndicator.Status.Passive;

			menu = new Menu();

			foreach (string symbol in Symbols)
			{
				MenuItem menuItem = new MenuItem(symbol + ": " + quoteUnknown);
				menuItem.Activated += OnQuoteSelected;
				menu.Append(menuItem);
			}

			AddDefaultMenus(menu);
			menu.ShowAll();

			indicator.Menu   = menu;
			indicator.Status = AppIndicator.Status.Active;
		}

		private Menu AddDefaultMenus(Menu menu)
		{
			AccelGroup agr = new AccelGroup();

			menu.Append(new SeparatorMenuItem());

			ImageMenuItem menuItemPrefs = new ImageMenuItem(Stock.Preferences, agr);
			menuItemPrefs.Activated += OnPrefs;
			menuItemPrefs.AddAccelerator("activate", agr,
				new AccelKey(Gdk.Key.q, Gdk.ModifierType.ControlMask, AccelFlags.Visible));

			menu.Append(menuItemPrefs);

			ImageMenuItem menuItemHelp = new ImageMenuItem(Stock.Help, agr);
			menuItemHelp.Activated += OnHelp;
			menuItemHelp.AddAccelerator("activate", agr,
				new AccelKey(Gdk.Key.q, Gdk.ModifierType.ControlMask, AccelFlags.Visible));

			menu.Append(menuItemHelp);

			ImageMenuItem menuItemInfo = new ImageMenuItem(Stock.About, agr);
			menuItemInfo.Activated += OnInfo;
			menuItemInfo.AddAccelerator("activate", agr,
				new AccelKey(Gdk.Key.q, Gdk.ModifierType.ControlMask, AccelFlags.Visible));

			menu.Append(menuItemInfo);

			ImageMenuItem menuItemQuit = new ImageMenuItem(Stock.Quit, agr);
			menuItemQuit.Activated += OnQuit;
			menuItemQuit.AddAccelerator("activate", agr,
				new AccelKey(Gdk.Key.q, Gdk.ModifierType.ControlMask, AccelFlags.Visible));

			menu.Append(menuItemQuit);

			return menu;
		}

		private string GetFontName()
		{
			string font = "Ubuntu 11";

			try 
			{
				using (Process p = new Process {
					   	StartInfo = new ProcessStartInfo {
					        FileName = "gsettings",
					        Arguments = "get org.gnome.desktop.interface font-name",
					        UseShellExecute = false,
					        RedirectStandardOutput = true,
					        CreateNoWindow = true
						}
					}
				)
				{
					p.Start();

					while (!p.StandardOutput.EndOfStream)
						font = p.StandardOutput.ReadLine();

					p.WaitForExit();
				}
			}
			catch
			{
				logger.LogError("Starting 'gsettings' to get the current font-name failed! ");
			}

			return font.Trim(new char[]{'\''});
		}

		private int GetTextPixelLength(string text, string font)
		{
			int width, height;

			Screen screen = Screen.Default;
			Pango.Layout layout = new Pango.Layout(PangoHelper.ContextGetForScreen(screen));

			layout.FontDescription = Pango.FontDescription.FromString(font);
		    layout.SetText(text);
		    layout.GetPixelSize(out width, out height);

			return width;
		}

		private void Update(float[] quotes)
		{
			logger.LogInfo("Updating MenuItems...");

			int maxNbrOfTabs = 0;
			int tabWidth;

			// TODO: add a "font has changed"-callback from dconf ???
			string curFont = GetFontName();

			logger.LogInfo("Label font name: " + curFont);
			logger.LogInfo(String.Format("Size of quotePadChar '\\u{0}': {1} pixel", ((int)quotePadChar).ToString("X"), GetTextPixelLength(quotePadChar.ToString(), curFont)));

			tabWidth = GetTextPixelLength(tabChar.ToString(), curFont);

			foreach (string symbol in Symbols)
				maxNbrOfTabs = Math.Max(maxNbrOfTabs,
					(int)Math.Ceiling((double)GetTextPixelLength(symbol, curFont) / tabWidth));

			Gtk.Application.Invoke(delegate {
				System.Collections.IEnumerator menuItemEnum = menu.AllChildren.GetEnumerator();
				System.Collections.IEnumerator symbolsEnum  = symbols.GetEnumerator();

				foreach (float quote in quotes)
				{
					if (menuItemEnum.MoveNext() && symbolsEnum.MoveNext())
					{
						// HACK:  symbol and quote aligment based on string width in pixels.
						// FIXME: Consider using Pango.Layout instead.
						int curWidth = GetTextPixelLength(symbolsEnum.Current.ToString() + tabChar, curFont);
						int nbrOfTabs = (int)Math.Ceiling(
							Math.Round((double)(maxNbrOfTabs * tabWidth - curWidth) / tabWidth, 1)
							+ 2);

						Label label = (Label)((MenuItem)menuItemEnum.Current).Child;
						label.Text = symbolsEnum.Current.ToString();
						for (int i = 0; i < nbrOfTabs; i++)
							label.Text += "\t";
						label.Text += (quote > 0 ? quote.ToString("0.00").PadLeft(8, quotePadChar) : quoteUnknown);
					}
				}
		    });
		}

		#region timer even handler
		protected void OnTimer(object sender, ElapsedEventArgs e)
		{
			timer.Enabled = false;

			float[] quotes = Quotes.GetQuotes(Symbols, Format.Bid);

			lock (thisLock)
			{
				Update(quotes);
			}

			timer.Interval = config.UpdateInterval * 1000;
			timer.Enabled = true;
		}
		#endregion

		#region gui event handler
		protected void OnPrefs(object sender, EventArgs args)
		{
			PreferencesDialog prefsDialog = new PreferencesDialog();
			prefsDialog.ShowAll();
		}

		[DllImport ("glib-2.0.dll")]
		static extern IntPtr g_get_language_names ();

		protected void OnHelp(object sender, EventArgs args)
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

		protected void OnInfo(object sender, EventArgs args)
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

		protected void OnQuit(object sender, EventArgs args)
		{
			timer.Dispose();
			Application.Quit();
		}

		protected void OnQuoteSelected(object sender, EventArgs args)
		{
			string symbol = Indicator.GetSymbolFromMenuItem((MenuItem)sender);
			string url = Quotes.GetChartUrl(symbol);

			Process.Start(url);
		}
		#endregion

		#region IObserver implementation
		public void OnCompleted()
		{
			throw new System.NotImplementedException ();
		}

		public void OnError(Exception error)
		{
			throw new System.NotImplementedException ();
		}

		public void OnNext(ConfigurationProvider configProvider)
		{
			logger.LogInfo("Notify Event from ConfigurationProvider received");

			Stop(); // TODO: not sure if it is reliable !!!

			Symbols = config.Symbols;

			Start();
		}
		#endregion
	}
}
