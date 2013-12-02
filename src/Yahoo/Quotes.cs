using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections;
using System.Globalization;
using ce3a.Logging;

namespace ce3a.Yahoo.Finance
{
	public class Quotes
	{
		public Quotes()
		{
		}

		public static float[] GetQuotes(string[] symbols, string format)
		{
			ILogger logger = LogManager.Logger;

			float[] quotes = new float[symbols.Length];

			string url = @"http://finance.yahoo.com/d/quotes.csv?s=";
			foreach (string symbol in symbols)
				url += (symbol + '+');
			url = url.TrimEnd (new char[] {'+'}) + "&f=" + format;

			try
			{
				logger.LogInfo(String.Format("{0}: \"{1}\"", "Sending HTTP request", url));
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

				using (HttpWebResponse rsp = (HttpWebResponse)req.GetResponse())
				{
					String csv = new StreamReader(rsp.GetResponseStream (), Encoding.ASCII).ReadToEnd();
					logger.LogInfo(String.Format("{0}:\n{1}","HTTP response", csv));

					StringReader stringReader = new StringReader(csv);

					for (int i = 0; i < quotes.Length; i++)
						float.TryParse(stringReader.ReadLine(), 
						               NumberStyles.AllowDecimalPoint, 
						               CultureInfo.InvariantCulture, 
						               out quotes[i]);
				}
			}
			catch (WebException ex)
			{
				logger.LogError(ex.Message);
			}

			return quotes;
		}

		public static string GetChartUrl(string symbol)
		{
			return "http://finance.yahoo.com/echarts?s=" + symbol + "+Interactive#symbol=" + symbol + ";range=1d";
		}
	}

	public sealed class Format
	{
		public static readonly string Ask = "a";
		public static readonly string Bid = "b";
	}
}
