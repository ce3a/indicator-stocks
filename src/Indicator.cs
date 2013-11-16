using System;
using System.Collections.Generic;
using System.Diagnostics;
using Gtk;
using AppIndicator;

namespace indicatorstocks
{
	public class Indicator
	{
		private UserEventHandler userEventHandler;
		private ApplicationIndicator indicator;
		private Menu menu;

		private static readonly string symbolQuoteSeparator = "  \t";

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

			foreach (string symbol in symbols)
			{
				MenuItem menuItem = new MenuItem(symbol + ": ???");
				menuItem.Activated += userEventHandler.OnQuoteSelected;
				menu.Append(menuItem);
			}

			AddDefaultMenus(menu);
			menu.ShowAll();

			indicator.Menu   = menu;
			indicator.Status = Status.Active;
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
						Label label = (Label)((MenuItem)menuItemEnum.Current).Child;
						label.Text = symbolsEnum.Current + symbolQuoteSeparator + 
							(quote > 0 ? quote.ToString("0.00").PadLeft(6,'\x2007') : "???");
					}
				}
		    });
		}

		public static string GetSymbolFromMenuItem(MenuItem menuItem)
		{
			string symbol = ((Label)menuItem.Child).Text;
			return symbol.Substring(0, symbol.IndexOf(symbolQuoteSeparator));
		}

		private Menu AddDefaultMenus(Menu menu)
		{
			AccelGroup agr = new AccelGroup();

			menu.Append(new SeparatorMenuItem());

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
	}
}
