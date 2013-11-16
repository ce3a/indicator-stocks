using System;
using Gtk;
using System.Threading;
using System.Collections.Generic;
using System.Timers;
using Yahoo.Finance;

namespace indicatorstocks
{
	class IndicatorStocks
	{
		private static Indicator indicator;
		private static System.Timers.Timer timer;
		private static Configuration configuration;

		private static readonly int time = 30000;
		private static readonly string name = "indicator-stocks";

		public static void Main (string[] args)
		{
			Application.Init();

			configuration = new Configuration(name);
			indicator = new Indicator(name, new UserEventHandler(), configuration.GetSymbols());

			DoWork();

			timer = new System.Timers.Timer(time);
			timer.Elapsed += new ElapsedEventHandler(OnTimer);
			timer.Enabled = true;
			timer.AutoReset = true;

			Application.Run();

			timer.Dispose();
		}

		private static void DoWork()
		{
			float[] quotes = Quotes.GetQuotes(indicator.Symbols, Format.Bid);

			indicator.Update(quotes);
		}

		private static void OnTimer (object sender, ElapsedEventArgs e)
		{
			DoWork();		
		}
	}
}
