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

		public static void Main (string[] args)
		{
			Application.Init();

			indicator = new Indicator(new UserEventHandler(), 
			                          new string[]{"AMD.DE", "SAP.DE", "SIE.DE", "TL0.DE"});

			DoWork();

			timer = new System.Timers.Timer(1000);
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

			// Debug
			int j = 0;
			Console.WriteLine("----");
			foreach (string symbol in indicator.Symbols)
				Console.WriteLine(symbol + ": " + quotes[j++]);
		}

		private static void OnTimer (object sender, ElapsedEventArgs e)
		{
			DoWork();		
		}
	}
}
