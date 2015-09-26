using System;
using System.Collections;

using Gtk;
using Gdk;

using Evolution;

using Mono.Posix;

public class LendItemDialog: Dialog
{
	private Database database;
	private Item item;
	
	private Gtk.Image image;
	private Label label;
	private Gtk.ComboBoxEntry borrowerComboBox;
	
	private ArrayList borrowers;
	
	public LendItemDialog (Item item, Database database)
	{
		this.database = database;
		this.item = item;
		this.Title = Mono.Posix.Catalog.GetString ("Lend");
		this.Modal = true;
		this.HasSeparator = false;
		
		VBox vBox = this.VBox;
		HBox hBox1 = new HBox ();
		
		string pixbuf = item.LargeCover;
		if (pixbuf != null) {
			image = new Gtk.Image (pixbuf);
		}
		
		label = new Label ();
		// Escape specials Markups characters
		string title;
		title = item.Title.Replace ("&", "&amp;");
		title = title.Replace ("<", "&lt;");
		title = title.Replace (">", "&gt;");
		label.Text = "<b>"+title+"</b>";
		label.UseMarkup = true;
		
		Label label1 = new Label (Mono.Posix.Catalog.GetString ("Borrower:"));
		borrowerComboBox = ComboBoxEntry.NewText ();
		PopulateEntryComboBox ();
		
		hBox1.PackStart (label1);
		hBox1.PackStart (borrowerComboBox);
		
		Gtk.Frame frame = new Frame ();
		frame.Shadow = ShadowType.Out;
		frame.Add (image);
		
		vBox.PackStart (frame);
		vBox.PackStart (label);
		vBox.PackStart (hBox1);
		vBox.Spacing = 5;
		
		Button cancelButton = (Button)this.AddButton (Gtk.Stock.Cancel, 0);
		Button okButton = (Button)this.AddButton (Gtk.Stock.Ok, 1);
		
		cancelButton.Clicked += OnCancelButtonClicked;
		okButton.Clicked     += OnOkButtonClicked;
		
		this.ShowAll();
	}

	private void PopulateEntryComboBox ()
	{
		borrowers = database.GetBorrowers ();
		foreach (Borrower borrower in borrowers) {
			borrowerComboBox.AppendText (borrower.Name);
		} 

		Book book = Book.NewSystemAddressbook ();
		book.Open (true);
		BookQuery query = BookQuery.AnyFieldContains ("");
		Contact[] list = book.GetContacts (query);
		foreach (Contact c in list) {
			borrowerComboBox.AppendText (c.FullName);
		}

		Entry entry = (Entry) borrowerComboBox.Child;
		entry.Completion = new EntryCompletion ();
		entry.Completion.Model = borrowerComboBox.Model;
		entry.Completion.TextColumn = 0;
	}

	public void OnOkButtonClicked (object o, EventArgs args)
	{
		int id = Int32.MaxValue;

		Entry entry = (Entry) borrowerComboBox.Child;
		if (entry.Text != null && !entry.Text.Equals ("")) {
			foreach (Borrower borrower in borrowers) {
				if (borrower.Name.Equals (entry.Text)) {
					id = borrower.Id;
					break;
				}
			}

			if (id == Int32.MaxValue) {
				id = database.AddBorrower (entry.Text);
			}

			Borrower newBorrower = new Borrower (id, entry.Text, null);
			database.LendItem (item, newBorrower);

			this.Destroy();
		}
		else {
			Gtk.Dialog dialog = new MessageDialog (this,
					DialogFlags.DestroyWithParent,
					MessageType.Error,
					ButtonsType.Close,
					Mono.Posix.Catalog.GetString ("You must write the borrower name"));
			dialog.Run ();
			dialog.Destroy();
		}
	}
	
	public void OnCancelButtonClicked (object o, EventArgs args)
	{
		this.Destroy();
	}
}
