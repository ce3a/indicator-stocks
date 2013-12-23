using System;

namespace indicatorstocks
{
	public partial class PreferencesDialog : Gtk.Dialog
	{

		public PreferencesDialog()
		{
			this.Build();

			// TODO: set Title

			buttonCancel.Clicked += OnClickedCancel;
			buttonOk.Clicked += OnClickedOk;

									
			nodeviewSymbols.AppendColumn("Symbol", new Gtk.CellRendererText (), "text", 0);
			nodeviewSymbols.AppendColumn("Song Title", new Gtk.CellRendererText (), "text", 1);
			nodeviewSymbols.ShowAll();


		}

		private void OnClickedCancel(object sender, EventArgs args)
		{
			this.Destroy();
		}

		private void OnClickedOk(object sender, EventArgs args)
		{
			this.Destroy();
		}
	}
}

