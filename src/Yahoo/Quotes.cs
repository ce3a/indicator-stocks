using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections;
using System.Globalization;

namespace Yahoo.Finance
{
	public class Quotes
	{
		public Quotes()
		{
		}

		public static float[] GetQuotes(string[] symbols, string format)
		{
			float[] quotes = new float[symbols.Length];

			string url = @"http://finance.yahoo.com/d/quotes.csv?s=";
			foreach (string symbol in symbols)
				url += (symbol + '+');
			url = url.TrimEnd (new char[] {'+'}) + "&f=" + format;

			try
			{
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

				using (HttpWebResponse rsp = (HttpWebResponse)req.GetResponse())
				{
					StreamReader strm = new StreamReader(rsp.GetResponseStream (), Encoding.ASCII);

					for (int i = 0; i < quotes.Length; i++)
						float.TryParse(strm.ReadLine(), 
						               NumberStyles.AllowDecimalPoint, 
						               CultureInfo.InvariantCulture, 
						               out quotes[i]);
				}
			}
			catch (WebException ex)
			{
				Console.WriteLine(ex.Message);
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
