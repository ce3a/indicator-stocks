using System;
using System.Diagnostics;

namespace ce3a.Logging
{
	public class Logger : ILogger
	{
		private bool verbose = false;

		public bool Verbose 
		{
			get {return verbose;}
			set {verbose = value;}
		}

		public Logger()
		{
		}

		public void LogInfo(string message)
		{
			if (Verbose)
				Console.WriteLine(String.Format("{0}: {1}", "[I]", message));
		}

 		public void LogError(string message)
		{
			if (Verbose)
				Console.WriteLine(String.Format("{0}: {1}", "[E]", message));
		}
	}
}
