using System;
using AppIndicator;
using Gtk;
using System.Collections.Generic;
using System.Diagnostics;
using Pango;

namespace indicatorstocks
{
	public class Indicator
	{
		private ApplicationIndicator indicator;
		private Menu menu;

		private string[] symbols;
		public  string[] Symbols
		{
			get {return symbols;}
		}

		public Indicator(string[] symbols)
		{
			indicator = new ApplicationIndicator("indicator-stocks",
								                 "indicator-stocks",
			                                     Category.ApplicationStatus);

			menu = new Menu();

			this.symbols = symbols;

			foreach (string symbol in symbols)
				menu.Append(new MenuItem(symbol + ": ?"));

			AddQuitMenu(menu);
			menu.ShowAll();

			indicator.Menu   = menu;
			indicator.Status = Status.Active;
			indicator.Title  = "Stocks Indicator";
		}

		public void Update(float[] quotes)
		{
			System.Collections.IEnumerator menuItemEnum = menu.AllChildren.GetEnumerator();
			System.Collections.IEnumerator symbolsEnum  = symbols.GetEnumerator();

			foreach (float quote in quotes)
			{
				if (menuItemEnum.MoveNext() && symbolsEnum.MoveNext())
				{
					Label label = (Label)((MenuItem)menuItemEnum.Current).Child;
					label.Text = String.Format("{0}: {1:0.00}", symbolsEnum.Current, quote);
				}
			}
		}

		private Menu AddQuitMenu(Menu menu)
		{
			AccelGroup agr = new AccelGroup();

			menu.Append(new SeparatorMenuItem());

			ImageMenuItem menuItemQuit = new ImageMenuItem(Stock.Quit, agr);
			menuItemQuit.Activated += OnQuitActivated;
			menuItemQuit.AddAccelerator("activate", agr,
				new AccelKey(Gdk.Key.q, Gdk.ModifierType.ControlMask, AccelFlags.Visible));

			menu.Append(menuItemQuit);

			return menu;
		}

		private void OnQuitActivated(object sender, EventArgs args)
    	{
        	Application.Quit();
    	}
	}
}
