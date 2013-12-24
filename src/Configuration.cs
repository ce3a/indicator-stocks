using System;
using System.IO;
using System.Collections.Generic;

namespace indicatorstocks
{
	public class Configuration
	{
		public static readonly Configuration Instance = new Configuration();

		private string path;

		private Configuration(){}

		public void Init(string name)
		{
			path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); 
			path += @"/" + name + @"/symbols.conf";
		
			if (!Directory.Exists (Path.GetDirectoryName (path)))
				Directory.CreateDirectory (Path.GetDirectoryName (path));

			if (!File.Exists (path))
				using (File.Create(path));
		}

		public string[] GetSymbols()
		{
			List<string> symbols = new List<string>();
			string line;

			using (StreamReader reader = new StreamReader(path)) 
			{
			    while ((line = reader.ReadLine()) != null) 
			        symbols.Add(line);
			}

			return symbols.ToArray();
		}

		public void SetSymbols(string[] symbols)
		{

		}


	}
}
