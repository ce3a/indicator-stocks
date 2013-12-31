using System;

namespace indicatorstocks
{
	[Gtk.TreeNode(ListOnly=true)]
	internal class SymbolsNode : Gtk.TreeNode 
	{
        private string symbol;

        public SymbolsNode(string symbol)
        {
			this.symbol = symbol;
        }

	    [Gtk.TreeNodeValue(Column=0)]
		public string Symbol
		{
			get { return symbol; }
		}
	}

	public partial class PreferencesDialog : Gtk.Dialog
	{
		private Configuration config = Configuration.Instance;

		public PreferencesDialog()
		{
			this.Build();

			// TODO: set Title
			// TODO: use Catalog for titles

			notebook1.CurrentPage = 0;

			updateIntervalSpinButton.Value = config.UpdateInterval;
			updateIntervalSpinButton.UpdatePolicy = Gtk.SpinButtonUpdatePolicy.IfValid;

			nodeviewSymbols.NodeStore = new Gtk.NodeStore(typeof (SymbolsNode));
			nodeviewSymbols.AppendColumn("Symbol", new Gtk.CellRendererText (), "text", 0);
			nodeviewSymbols.ShowAll();

			foreach (string s in config.Symbols)
				nodeviewSymbols.NodeStore.AddNode(new SymbolsNode(s));

			buttonAddSymbol.Sensitive = false;
			buttonDeleteSymbol.Sensitive = false;

			entryNewSymbol.FocusInEvent += OnEntrySelected; 
			entryNewSymbol.TextInserted += OnEntryTextChanged;
			entryNewSymbol.TextDeleted += OnEntryTextChanged;

			nodeviewSymbols.CursorChanged += OnSymbolSelected;

			buttonOk.Clicked += OnClickedOk;
			buttonAddSymbol.Clicked += OnClickedAddSymbol;
			buttonDeleteSymbol.Clicked += OnClickedDeleteSymbol;

			updateIntervalSpinButton.ValueChanged += 
				new global::System.EventHandler(OnUpdateIntervalSpinButtonValueChanged);
		}

		~PreferencesDialog()
		{
			config.Save();
		}

		private void OnClickedOk(object sender, EventArgs args)
		{
			this.Destroy();
		}

		private void OnClickedAddSymbol(object sender, EventArgs args)
		{
			config.AddSymbols(new string[]{entryNewSymbol.Text});
			nodeviewSymbols.NodeStore.AddNode(new SymbolsNode(entryNewSymbol.Text));
			entryNewSymbol.Text = "";
		}

		private void OnClickedDeleteSymbol(object sender, EventArgs args)
		{
			SymbolsNode node = (SymbolsNode)nodeviewSymbols.NodeSelection.SelectedNode;

			config.RemoveSymbols(new string[]{node.Symbol});
			nodeviewSymbols.NodeStore.RemoveNode(node);
			buttonDeleteSymbol.Sensitive = false;
		}

		private void OnEntrySelected(object sender, EventArgs args)
		{
			entryNewSymbol.Text = "";
		}

		private void OnEntryTextChanged(object sender, EventArgs args)
		{
			if (entryNewSymbol.Text.Length > 0)
				buttonAddSymbol.Sensitive = true;
			else
				buttonAddSymbol.Sensitive = false;
		}

		private void OnSymbolSelected(object sender, EventArgs args)
		{
			buttonDeleteSymbol.Sensitive = true;
		}

		protected void OnUpdateIntervalSpinButtonValueChanged(object sender, EventArgs e)
		{
			config.UpdateInterval = ((Gtk.SpinButton)sender).ValueAsInt;
		}
	}
}
