using System;
using Gtk;
using System.Threading;
using Yahoo;
using System.Collections.Generic;

namespace indicatorstockmarket
{
	class IndicatorStockMarket
	{
		public static void Main (string[] args)
		{
			Application.Init();

			Indicator indicator = new Indicator();

			indicator.Initialize();

			// Create the thread object. This does not start the thread.
	        Worker workerObject = new Worker(indicator);
	        Thread workerThread = new Thread(workerObject.DoWork);

	        // Start the worker thread.
	        workerThread.Start();
	        Console.WriteLine("main thread: Starting worker thread...");

			Application.Run();

			// Request that the worker thread stop itself:
	        workerObject.RequestStop();

	        // Use the Join method to block the current thread 
	        // until the object's thread terminates.
	        workerThread.Join();
	        Console.WriteLine("main thread: Worker thread has terminated.");
		}
	}

	public class Worker
	{
		// Volatile is used as hint to the compiler that this data
	    // member will be accessed by multiple threads.
	    private volatile bool _shouldStop;
		Indicator indicator;

		public Worker(Indicator indicator)
		{
			this.indicator = indicator;
		}

	    // This method will be called when the thread is started.
	    public void DoWork()
	    {
			int i = 0;

	        while (!_shouldStop)
	        {
				Thread.Sleep(3000);
				Console.WriteLine("worker thread: working... " + ++i);

				string[] symbols = {"AMD.DE", "TL0.DE", "SAP.DE", "xyi"};
				float[]  quotes  = Finance.GetQuote(symbols, Format.Bid, 5000);

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

	        Console.WriteLine("worker thread: terminating gracefully.");
	    }
	    public void RequestStop()
	    {
	        _shouldStop = true;
	    }
	}

}
