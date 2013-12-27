using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;

namespace indicatorstocks
{
	public class Configuration
	{
		public static readonly Configuration Instance = new Configuration();

		private readonly string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		private readonly string fileName = "conf.xml";

		private string filePath;

		private XDocument confDoc;

		private Configuration(){}

		public void Init(string appName)
		{
			oldPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); 
			oldPath += @"/" + appName + @"/symbols.conf";
		
			// new concept..
			filePath += Path.Combine(new string[]{folderPath, appName, fileName});

			if (!Directory.Exists(Path.GetDirectoryName(filePath)))
				Directory.CreateDirectory(Path.GetDirectoryName(filePath));

			if (!File.Exists(filePath))
			{
				using (FileStream fs = File.Create(filePath))
				{
					GetDefaultConfDoc().Save(fs);

					using (StreamWriter sw = new StreamWriter(fs))
						sw.WriteLine(); // HACK: add an empty line at the end of the file
				}
			}

			confDoc = XDocument.Load(filePath);
		}

		#region LEGACY
		private string oldPath;

		public string[] GetSymbols()
		{
			List<string> symbols = new List<string>();
			string line;

			using (StreamReader reader = new StreamReader(oldPath)) 
			{
			    while ((line = reader.ReadLine()) != null) 
			        symbols.Add(line);
			}

			return symbols.ToArray();
		}
		#endregion

//		public void SetSymbols(string[] symbols)
//		{
//
//		}

		public int UpdateInterval
		{
			get {return Int32.Parse(confDoc.Root.Element("Settings").Element("UpdateInterval").Value);}

			set {confDoc.Root.Element("Settings").Element("UpdateInterval").Value = String.Format("{0}", value);}
		}

		private XDocument GetDefaultConfDoc()
		{
			return new XDocument(
				new XElement("Configuration",
					new XElement("Stocks"),
					new XElement("Settings",
			             new XElement("UpdateInterval", 30))
			    )
			);
		}

	}
}
