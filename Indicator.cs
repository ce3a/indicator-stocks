using System;
using AppIndicator;
using Gtk;
using System.Collections.Generic;

namespace indicatorstockmarket
{
	public class Indicator
	{
		private ApplicationIndicator indicator;
		private Menu defaultMenu;

		public Indicator()
		{
			indicator = new ApplicationIndicator("example-simple-client",
								                 "go-top",
			                                     Category.ApplicationStatus);

			defaultMenu = AddQuitMenu(new Menu());
			defaultMenu.ShowAll();
		}

		public void Initialize()
		{
			indicator.Menu   = defaultMenu;
			indicator.Status = Status.Active;
		}

		public void Update(Dictionary<string, float> dict)
		{
			Menu menu = new Menu();

			foreach (KeyValuePair<string, float> pair in dict)
			{
				MenuItem menuItem;

				if (pair.Value <= 0)
					menuItem = new MenuItem(string.Format("{0}: ?", pair.Key));
				else
					menuItem = new MenuItem(string.Format("{0}: {1:0.00}", pair.Key, pair.Value));

				menu.Append(menuItem);
			}

			AddQuitMenu(menu);
			menu.ShowAll();
			indicator.Menu = menu;
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
