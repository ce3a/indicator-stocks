using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;

namespace indicatorstocks
{
	public interface ConfigurationProvider
	{
		int UpdateInterval { get; }

		string[] Symbols { get; }
	}

	internal class Unsubscriber<BaggageInfo> : IDisposable
	{
		private List<IObserver<BaggageInfo>> _observers;
		private IObserver<BaggageInfo> _observer;

		internal Unsubscriber(List<IObserver<BaggageInfo>> observers, IObserver<BaggageInfo> observer)
		{
			this._observers = observers;
			this._observer = observer;
		}

		public void Dispose() 
		{
			if (_observers.Contains(_observer))
				_observers.Remove(_observer);
		}
	}

	public class Configuration : ConfigurationProvider, IObservable<ConfigurationProvider>
	{
		public static readonly Configuration Instance = new Configuration();

		private List<IObserver<ConfigurationProvider>> observers;

		private readonly string folderPath;
		private readonly string fileName;

		private string filePath;

		private XDocument confDoc;

		private Configuration()
		{
			observers = new List<IObserver<ConfigurationProvider>>();
			folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			fileName = "conf.xml";
		}

		public void Init(string appName)
		{
			filePath += Path.Combine(new string[]{folderPath, appName, fileName});

			if (!Directory.Exists (Path.GetDirectoryName (filePath)))
				Directory.CreateDirectory (Path.GetDirectoryName (filePath));

			if (!File.Exists (filePath)) {
				using (FileStream fs = File.Create(filePath)) {
					GetDefaultConfDoc ().Save (fs);

					using (StreamWriter sw = new StreamWriter(fs))
						sw.WriteLine (); // HACK: add an empty line at the end of the file
				}
			}

			confDoc = XDocument.Load (filePath);

			#region read symbols from symbols.conf (LEGACY)
			// TODO: remove this region in one of the following releases
			string oldPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); 
			oldPath += @"/" + appName + @"/symbols.conf";

			if (File.Exists(oldPath))
			{
				List<string> symbols = new List<string>();
				string line;

				using (StreamReader reader = new StreamReader(oldPath)) 
				{
				    while ((line = reader.ReadLine()) != null) 
				        symbols.Add(line);
				}

				AddSymbols(symbols.ToArray());
				Save();

				try { File.Move(oldPath, oldPath + ".back"); } catch {}
			}
			#endregion
		}

		#region IObservable implementation
		public IDisposable Subscribe (IObserver<ConfigurationProvider> observer)
		{
			if (!observers.Contains(observer))
				observers.Add(observer);

			return new Unsubscriber<ConfigurationProvider>(observers, observer);
		}
		#endregion

		public void Save()
		{
			using (FileStream fs = File.Create(filePath))
				confDoc.Save(fs);
		}

		public void AddSymbols(string[] symbols)
		{
			foreach (string s in symbols)
				confDoc.Root.Element("Stocks").Add(new XElement("Stock", new XElement("Symbol", s)));

			NotifyObservers();
		}

		public void RemoveSymbols(string[] symbols)
		{
			foreach (string s in symbols)
				foreach (XElement element in confDoc.Root.Element("Stocks").Descendants("Stock"))
					if (element.Element("Symbol").Value == s)
						element.Remove();

			NotifyObservers();
		}

		public string[] Symbols
		{
			get
			{
				List<string> symbols = new List<string>();

				foreach (XElement element in confDoc.Root.Element("Stocks").Descendants("Stock"))
					symbols.Add(element.Element("Symbol").Value);

				return symbols.ToArray();
			}
		}

		public int UpdateInterval
		{
			get
			{ 
				return Int32.Parse(confDoc.Root.Element("Settings").Element("UpdateInterval").Value); 
			}

			set	
			{ 
				confDoc.Root.Element("Settings").Element("UpdateInterval").Value = String.Format("{0}", value); 
				NotifyObservers();
			}
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

		private void NotifyObservers()
		{
			foreach (var observer in observers)
				observer.OnNext(this);
		}
	}
}
