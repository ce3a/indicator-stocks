using System;
using Gtk;
using System.Threading;
using System.Collections.Generic;
using System.Timers;
using Yahoo;

namespace indicatorstockmarket
{
	class IndicatorStockMarket
	{
		private static Indicator indicator;
		private static System.Timers.Timer timer;

		public static void Main (string[] args)
		{
			Application.Init();

			indicator = new Indicator();

			timer = new System.Timers.Timer(5000);
			timer.Elapsed += new ElapsedEventHandler(OnTimer);
			timer.Enabled = true;
			timer.AutoReset = true;

			Application.Run();

			timer.Dispose();
		}

		private static void OnTimer (object sender, ElapsedEventArgs e)
		{
			Console.WriteLine("OnTimer");

			string[] symbols = {"AMD.DE", "TL0.DE", "SAP.DE", "SIE.DE"};
			float[]  quotes  = Finance.GetQuote(symbols, Format.Bid);

			System.Collections.IEnumerator symbolsEnum = symbols.GetEnumerator();
			System.Collections.IEnumerator quotesEnum  = quotes.GetEnumerator();

			Dictionary<string, float> dict = new Dictionary<string, float>();

			while (symbolsEnum.MoveNext())
				if (quotesEnum.MoveNext())
					dict.Add((string)symbolsEnum.Current, (float)quotesEnum.Current);

			indicator.Update(dict);

			// Debug
			int j = 0;
			foreach (string symbol in symbols)
				Console.WriteLine(symbol + ": " + quotes[j++]);
		}
	}
}
