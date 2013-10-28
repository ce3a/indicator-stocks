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

		public static float[] GetQuote(string[] symbols, string format, int timeout = 3000)
		{
			float[] quotes = new float[symbols.Length];

			string url = @"http://finance.yahoo.com/d/quotes.csv?s=";
			foreach (string symbol in symbols)
				url += (symbol + '+');
			url = url.TrimEnd (new char[] {'+'}) + "&f=" + format;

			HttpWebRequest  req;
			HttpWebResponse rsp;

			try
			{
				req = (HttpWebRequest)WebRequest.Create(url);
				req.Timeout = timeout;
				rsp = (HttpWebResponse)req.GetResponse();
			}
			catch (WebException ex)
			{
				Console.WriteLine(ex.Message);
				return quotes;
			}

			StreamReader strm = 
	          new StreamReader(rsp.GetResponseStream (), Encoding.ASCII);

			for (int i = 0; i < quotes.Length; i++)
				float.TryParse (strm.ReadLine().Replace(".",","), out quotes[i]);
				
			return quotes;
		}
	}

	public sealed class Format
	{
		public static readonly string Ask = "a";
		public static readonly string Bid = "b";
	}
}

