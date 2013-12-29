using Gtk;
using ce3a.Logging;

namespace indicatorstocks
{
	class IndicatorStocks
	{
		private static Indicator indicator;
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

			indicator = new Indicator(name);
			indicator.Start();

			logger.LogInfo("Calling Application.Run()");
			Application.Run();
			logger.LogInfo("Exiting...");
		}
	}
}
