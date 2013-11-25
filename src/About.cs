using System;
using System.Reflection;
using System.IO;

namespace indicatorstocks
{
	public static class About
	{
		private static readonly string[] authors = 
		{
			"Sergej Sawazki",
		};

		private static readonly string[] artists = 
		{
			"Sergej Sawazki",
			"Agmenor"
		};

		private static readonly string logoIconName = "indicator-stocks";

		public static string[] Authors
		{
			get {return authors;}
		}

		public static string[] Artists {
			get {return artists;}
		}
		public static string LogoIconName
		{
			get {return logoIconName;}
		}

		public static string License
		{
			get
			{
				Assembly asm = Assembly.GetExecutingAssembly();
				Stream objStream = asm.GetManifestResourceStream(typeof(About).Namespace + ".LICENSE.md");
				StreamReader objReader = new StreamReader(objStream);
				return objReader.ReadToEnd();
			}
		}
	}
}
