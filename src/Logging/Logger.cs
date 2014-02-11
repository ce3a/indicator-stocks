using System;
using System.Diagnostics;
using System.IO;

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
			if (!Verbose)
				return;

			StringReader stringReader = new StringReader(message); 
			String line;

			while ((line = stringReader.ReadLine()) != null)
				Console.WriteLine(String.Format("{0}: {1}", "[I]", line));
		}

 		public void LogError(string message)
		{
			if (!Verbose)
				return;

			StringReader stringReader = new StringReader(message); 
			String line;

			while ((line = stringReader.ReadLine()) != null)
				Console.WriteLine(String.Format("{0}: {1}", "[E]", line));
		}
	}
}
