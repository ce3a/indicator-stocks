using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections;

namespace Yahoo
{
	public class Finance
	{
		public Finance()
		{
		}

		public static float[] GetQuote(string[] symbols, string format)
		{
			float[] quotes = new float[symbols.Length];

			string url = @"http://finance.yahoo.com/d/quotes.csv?s=";
			foreach (string symbol in symbols)
				url += (symbol + '+');
			url = url.TrimEnd (new char[] {'+'}) + "&f=" + format;

			HttpWebRequest  req;
			HttpWebResponse rsp;
			StreamReader    strm;

			try
			{
				req = (HttpWebRequest)WebRequest.Create(url);

				using (rsp = (HttpWebResponse)req.GetResponse())
				{
					strm = new StreamReader(rsp.GetResponseStream (), Encoding.ASCII);

					for (int i = 0; i < quotes.Length; i++)
						float.TryParse(strm.ReadLine().Replace(".",","), out quotes[i]);
				}
			}
			catch (WebException ex)
			{
				Console.WriteLine(ex.Message);
			}

			return quotes;
		}
	}

	public sealed class Format
	{
		public static readonly string Ask = "a";
		public static readonly string Bid = "b";
	}
}

