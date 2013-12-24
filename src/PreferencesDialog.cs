using System;

namespace indicatorstocks
{

	[Gtk.TreeNode (ListOnly=true)]
	public class SymbolsNode : Gtk.TreeNode {

        private string symbol;

        public SymbolsNode (string symbol)
        {
			this.symbol = symbol;
        }

	    [Gtk.TreeNodeValue (Column=0)]
		public string Symbol
		{
			get {return symbol;}
		}

	}

	public partial class PreferencesDialog : Gtk.Dialog
	{

		public PreferencesDialog()
		{
			this.Build();

			// TODO: set Title
			// TODO: use Catalog for titles

			nodeviewSymbols.NodeStore = new Gtk.NodeStore(typeof (SymbolsNode));
			nodeviewSymbols.AppendColumn("Symbol", new Gtk.CellRendererText (), "text", 0);
			nodeviewSymbols.ShowAll();

			buttonOk.Clicked += OnClickedOk;
			buttonAddSymbol.Clicked += OnClickedAddSymbol;
			buttonDeleteSymbol.Clicked += OnClickedDeleteSymbol;
		}

		private void OnClickedOk(object sender, EventArgs args)
		{
			this.Destroy();
		}

		int i = 0;

		private void OnClickedAddSymbol(object sender, EventArgs args)
		{
			nodeviewSymbols.NodeStore.AddNode(new SymbolsNode("hihi" + i++));
		}

		private void OnClickedDeleteSymbol(object sender, EventArgs args)
		{
			SymbolsNode node = (SymbolsNode)nodeviewSymbols.NodeSelection.SelectedNode;
			nodeviewSymbols.NodeStore.RemoveNode(node);
		}
	}
}

