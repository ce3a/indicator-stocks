using System;

namespace ce3a.Logging
{
	public interface ILogger
	{
		bool Verbose {get; set;}
		void LogInfo(string message);
 		void LogError(string message);
	}
}

