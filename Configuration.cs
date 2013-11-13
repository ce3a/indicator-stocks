using System;
using System.IO;
using System.Collections.Generic;

namespace indicatorstocks
{
	public class Configuration
	{
		private String path;

		public Configuration (String name)
		{
			path = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData); 
			path += @"/" + name + @"/symbols.conf";
		
			if (!Directory.Exists (Path.GetDirectoryName (path)))
				Directory.CreateDirectory (Path.GetDirectoryName (path));

			if (!File.Exists (path))
				using (File.Create(path));
		}

		public string[] GetSymbols ()
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
	}
}
