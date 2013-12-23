using System;
using System.Collections.Generic;
using System.Diagnostics;
using Gtk;
using AppIndicator;
using Gdk;
using Pango;

namespace indicatorstocks
{
	public class Indicator
	{
		private UserEventHandler userEventHandler;
		private ApplicationIndicator indicator;
		private Menu menu;

		private Screen screen = Screen.Default;
		private Pango.Layout layout;

		private readonly int spaceWidth;
		private static readonly string quoteUnknown = "???";
		private static readonly char symbolPadChar = '\x2007';
		private static readonly string symbolQuoteSeparator = "\t";

		private int maxWidth = 0;

		private string[] symbols;
		public  string[] Symbols
		{
			get {return symbols;}
		}

		public Indicator(string name, UserEventHandler userEventHandler, string[] symbols)
		{
			indicator = new ApplicationIndicator(name, name, Category.ApplicationStatus);

			this.userEventHandler = userEventHandler;
			this.symbols = symbols;

			menu = new Menu();

			layout = new Pango.Layout(PangoHelper.ContextGetForScreen(screen));
			layout.FontDescription = new FontDescription();

			spaceWidth = GetTextPixelLength(symbolPadChar.ToString());

			foreach (string symbol in symbols)
			{
				MenuItem menuItem = new MenuItem(symbol + ":" + quoteUnknown);
				menuItem.Activated += userEventHandler.OnQuoteSelected;
				menu.Append(menuItem);

				maxWidth = Math.Max(maxWidth, GetTextPixelLength(symbol));
			}

			AddDefaultMenus(menu);
			menu.ShowAll();

			indicator.Menu   = menu;
			indicator.Status = AppIndicator.Status.Active;
		}

		public void Update(float[] quotes)
		{
			Gtk.Application.Invoke(delegate {
				System.Collections.IEnumerator menuItemEnum = menu.AllChildren.GetEnumerator();
				System.Collections.IEnumerator symbolsEnum  = symbols.GetEnumerator();

				foreach (float quote in quotes)
				{
					if (menuItemEnum.MoveNext() && symbolsEnum.MoveNext())
					{
						// HACK:  symbol and quote aligmant based on string width in pixels.
						// FIXME: Consider using Pango.Layout instead.
						int curWidth = GetTextPixelLength(symbolsEnum.Current.ToString());
						int nbrOfPadChars = (maxWidth - curWidth) / spaceWidth + symbolsEnum.Current.ToString().Length + 1;

						Label label = (Label)((MenuItem)menuItemEnum.Current).Child;
						label.Text = 
							symbolsEnum.Current.ToString().PadRight(nbrOfPadChars, symbolPadChar) + 
								symbolQuoteSeparator + 
							(quote > 0 ? quote.ToString("0.00").PadLeft(8, symbolPadChar) : quoteUnknown);
					}
				}
		    });
		}

		public static string GetSymbolFromMenuItem(MenuItem menuItem)
		{
			string symbol = ((Label)menuItem.Child).Text;
			return symbol.Substring(0, symbol.IndexOf(symbolPadChar));
		}

		private Menu AddDefaultMenus(Menu menu)
		{
			AccelGroup agr = new AccelGroup();

			menu.Append(new SeparatorMenuItem());

			ImageMenuItem menuItemPrefs = new ImageMenuItem(Stock.Preferences, agr);
			menuItemPrefs.Activated += userEventHandler.OnPrefs;
			menuItemPrefs.AddAccelerator("activate", agr,
				new AccelKey(Gdk.Key.q, Gdk.ModifierType.ControlMask, AccelFlags.Visible));

			menu.Append(menuItemPrefs);

			ImageMenuItem menuItemHelp = new ImageMenuItem(Stock.Help, agr);
			menuItemHelp.Activated += userEventHandler.OnHelp;
			menuItemHelp.AddAccelerator("activate", agr,
				new AccelKey(Gdk.Key.q, Gdk.ModifierType.ControlMask, AccelFlags.Visible));

			menu.Append(menuItemHelp);

			ImageMenuItem menuItemInfo = new ImageMenuItem(Stock.About, agr);
			menuItemInfo.Activated += userEventHandler.OnInfo;
			menuItemInfo.AddAccelerator("activate", agr,
				new AccelKey(Gdk.Key.q, Gdk.ModifierType.ControlMask, AccelFlags.Visible));

			menu.Append(menuItemInfo);

			ImageMenuItem menuItemQuit = new ImageMenuItem(Stock.Quit, agr);
			menuItemQuit.Activated += userEventHandler.OnQuit;
			menuItemQuit.AddAccelerator("activate", agr,
				new AccelKey(Gdk.Key.q, Gdk.ModifierType.ControlMask, AccelFlags.Visible));

			menu.Append(menuItemQuit);

			return menu;
		}

		private int GetTextPixelLength(string text)
		{
			int width, height;

		    layout.SetText(text);
		    layout.GetPixelSize(out width, out height);

			return width;
		}
	}
}
