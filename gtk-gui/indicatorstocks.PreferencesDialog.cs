
// This file has been generated by the GUI designer. Do not modify.
namespace indicatorstocks
{
	public partial class PreferencesDialog
	{
		private global::Gtk.Notebook notebook1;
		private global::Gtk.VBox vbox3;
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		private global::Gtk.NodeView nodeviewSymbols;
		private global::Gtk.Entry entryNewSymbol;
		private global::Gtk.HButtonBox hbuttonbox3;
		private global::Gtk.Button buttonAddSymbol;
		private global::Gtk.Button buttonDeleteSymbol;
		private global::Gtk.Label label1;
		private global::Gtk.VBox vbox4;
		private global::Gtk.HBox hbox1;
		private global::Gtk.Label label4;
		private global::Gtk.SpinButton spinbutton2;
		private global::Gtk.Label label3;
		private global::Gtk.Label label2;
		private global::Gtk.Button buttonOk;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget indicatorstocks.PreferencesDialog
			this.Name = "indicatorstocks.PreferencesDialog";
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child indicatorstocks.PreferencesDialog.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.notebook1 = new global::Gtk.Notebook ();
			this.notebook1.CanFocus = true;
			this.notebook1.Name = "notebook1";
			this.notebook1.CurrentPage = 0;
			// Container child notebook1.Gtk.Notebook+NotebookChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			this.vbox3.BorderWidth = ((uint)(6));
			// Container child vbox3.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.nodeviewSymbols = new global::Gtk.NodeView ();
			this.nodeviewSymbols.CanFocus = true;
			this.nodeviewSymbols.Name = "nodeviewSymbols";
			this.GtkScrolledWindow.Add (this.nodeviewSymbols);
			this.vbox3.Add (this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.GtkScrolledWindow]));
			w3.Position = 0;
			// Container child vbox3.Gtk.Box+BoxChild
			this.entryNewSymbol = new global::Gtk.Entry ();
			this.entryNewSymbol.CanFocus = true;
			this.entryNewSymbol.Name = "entryNewSymbol";
			this.entryNewSymbol.Text = global::Mono.Unix.Catalog.GetString ("enter new symbol here...");
			this.entryNewSymbol.IsEditable = true;
			this.entryNewSymbol.InvisibleChar = '•';
			this.vbox3.Add (this.entryNewSymbol);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.entryNewSymbol]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hbuttonbox3 = new global::Gtk.HButtonBox ();
			this.hbuttonbox3.Name = "hbuttonbox3";
			this.hbuttonbox3.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(2));
			// Container child hbuttonbox3.Gtk.ButtonBox+ButtonBoxChild
			this.buttonAddSymbol = new global::Gtk.Button ();
			this.buttonAddSymbol.CanFocus = true;
			this.buttonAddSymbol.Name = "buttonAddSymbol";
			this.buttonAddSymbol.UseUnderline = true;
			// Container child buttonAddSymbol.Gtk.Container+ContainerChild
			global::Gtk.Alignment w5 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w6 = new global::Gtk.HBox ();
			w6.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w7 = new global::Gtk.Image ();
			w7.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-add", global::Gtk.IconSize.Menu);
			w6.Add (w7);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w9 = new global::Gtk.Label ();
			w9.LabelProp = global::Mono.Unix.Catalog.GetString ("_Hinzufügen");
			w9.UseUnderline = true;
			w6.Add (w9);
			w5.Add (w6);
			this.buttonAddSymbol.Add (w5);
			this.hbuttonbox3.Add (this.buttonAddSymbol);
			global::Gtk.ButtonBox.ButtonBoxChild w13 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox3 [this.buttonAddSymbol]));
			w13.Expand = false;
			w13.Fill = false;
			// Container child hbuttonbox3.Gtk.ButtonBox+ButtonBoxChild
			this.buttonDeleteSymbol = new global::Gtk.Button ();
			this.buttonDeleteSymbol.CanFocus = true;
			this.buttonDeleteSymbol.Name = "buttonDeleteSymbol";
			this.buttonDeleteSymbol.UseUnderline = true;
			// Container child buttonDeleteSymbol.Gtk.Container+ContainerChild
			global::Gtk.Alignment w14 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w15 = new global::Gtk.HBox ();
			w15.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w16 = new global::Gtk.Image ();
			w16.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-remove", global::Gtk.IconSize.Menu);
			w15.Add (w16);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w18 = new global::Gtk.Label ();
			w18.LabelProp = global::Mono.Unix.Catalog.GetString ("_Entfernen");
			w18.UseUnderline = true;
			w15.Add (w18);
			w14.Add (w15);
			this.buttonDeleteSymbol.Add (w14);
			this.hbuttonbox3.Add (this.buttonDeleteSymbol);
			global::Gtk.ButtonBox.ButtonBoxChild w22 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox3 [this.buttonDeleteSymbol]));
			w22.Position = 1;
			w22.Expand = false;
			w22.Fill = false;
			this.vbox3.Add (this.hbuttonbox3);
			global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hbuttonbox3]));
			w23.Position = 2;
			w23.Expand = false;
			w23.Fill = false;
			this.notebook1.Add (this.vbox3);
			// Notebook tab
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Symbols");
			this.notebook1.SetTabLabel (this.vbox3, this.label1);
			this.label1.ShowAll ();
			// Container child notebook1.Gtk.Notebook+NotebookChild
			this.vbox4 = new global::Gtk.VBox ();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			this.vbox4.BorderWidth = ((uint)(6));
			// Container child vbox4.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.label4 = new global::Gtk.Label ();
			this.label4.Name = "label4";
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("Update interval:");
			this.hbox1.Add (this.label4);
			global::Gtk.Box.BoxChild w25 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.label4]));
			w25.Position = 0;
			w25.Expand = false;
			w25.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.spinbutton2 = new global::Gtk.SpinButton (1, 10000, 10);
			this.spinbutton2.CanFocus = true;
			this.spinbutton2.Name = "spinbutton2";
			this.spinbutton2.Adjustment.PageIncrement = 10;
			this.spinbutton2.ClimbRate = 1;
			this.spinbutton2.Numeric = true;
			this.spinbutton2.Value = 30;
			this.hbox1.Add (this.spinbutton2);
			global::Gtk.Box.BoxChild w26 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.spinbutton2]));
			w26.Position = 1;
			w26.Expand = false;
			w26.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("seconds");
			this.hbox1.Add (this.label3);
			global::Gtk.Box.BoxChild w27 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.label3]));
			w27.Position = 2;
			w27.Expand = false;
			w27.Fill = false;
			this.vbox4.Add (this.hbox1);
			global::Gtk.Box.BoxChild w28 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.hbox1]));
			w28.Position = 0;
			w28.Expand = false;
			w28.Fill = false;
			this.notebook1.Add (this.vbox4);
			global::Gtk.Notebook.NotebookChild w29 = ((global::Gtk.Notebook.NotebookChild)(this.notebook1 [this.vbox4]));
			w29.Position = 1;
			// Notebook tab
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Settings");
			this.notebook1.SetTabLabel (this.vbox4, this.label2);
			this.label2.ShowAll ();
			w1.Add (this.notebook1);
			global::Gtk.Box.BoxChild w30 = ((global::Gtk.Box.BoxChild)(w1 [this.notebook1]));
			w30.Position = 0;
			// Internal child indicatorstocks.PreferencesDialog.ActionArea
			global::Gtk.HButtonBox w31 = this.ActionArea;
			w31.Name = "dialog1_ActionArea";
			w31.Spacing = 10;
			w31.BorderWidth = ((uint)(5));
			w31.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-close";
			this.AddActionWidget (this.buttonOk, -7);
			global::Gtk.ButtonBox.ButtonBoxChild w32 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w31 [this.buttonOk]));
			w32.Expand = false;
			w32.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 265;
			this.DefaultHeight = 375;
			this.Show ();
		}
	}
}
