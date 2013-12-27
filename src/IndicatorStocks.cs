using System;
using Gtk;
using System.Threading;
using System.Collections.Generic;
using System.Timers;
using ce3a.Yahoo.Finance;
using ce3a.Logging;

namespace indicatorstocks
{
	class IndicatorStocks
	{
		private static Indicator indicator;
		private static System.Timers.Timer timer;
		private static Configuration config = Configuration.Instance;

		private static readonly string name = "indicator-stocks";

		private static ILogger logger;

		public static void Main(string[] args)
		{
			logger = LogManager.Logger;

			// TODO: implement command line arguments parser!
			if (args.Length > 0) 
				if (args.Length > 0 && args[0] == "-v")
					logger.Verbose = true;

			logger.LogInfo("Calling Application.Init()");
			Application.Init();

			config.Init(name);
			indicator = new Indicator(name, new UserEventHandler(), config.GetSymbols());

			DoWork();

			timer = new System.Timers.Timer(config.UpdateInterval * 1000);
			timer.Elapsed += new ElapsedEventHandler(OnTimer);
			timer.Enabled = true;
			timer.AutoReset = true;

			logger.LogInfo("Calling Application.Run()");
			Application.Run();

			timer.Dispose();
			logger.LogInfo("Exiting...");
		}

		private static void DoWork()
		{
			float[] quotes = Quotes.GetQuotes(indicator.Symbols, Format.Bid);

			indicator.Update(quotes);
		}

		private static void OnTimer(object sender, ElapsedEventArgs e)
		{
			DoWork();

			timer.Interval = config.UpdateInterval * 1000;
		}
	}
}
