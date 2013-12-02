using System;

namespace ce3a.Logging
{
	public class LogManager
	{
		private static ILogger logger = null;
		private static Object thisLock = new Object();

		public static ILogger Logger 
		{
			get 
			{
				lock (thisLock)
				{
					if (logger == null)
						logger = new Logger();
				}

				return logger;
			}
		}
	}
}

