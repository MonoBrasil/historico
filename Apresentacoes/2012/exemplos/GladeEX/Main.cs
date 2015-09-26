// project created on 17/2/2006 at 23:36
using System;
using Gtk;
using Glade;

public class GladeApp
{


 #region GladeSync
	[Glade.Widget] Window window1;
	[Glade.Widget] VBox vbox1;
	[Glade.Widget] Label label2;
	[Glade.Widget] HBox hbox1;
	[Glade.Widget] Label label1;
	[Glade.Widget] Entry entry1;
	[Glade.Widget] Button button1;
	[Glade.Widget] HBox hbox2;
	[Glade.Widget] Label label3;
	[Glade.Widget] ComboBox combobox1;
	[Glade.Widget] Button button2;
	[Glade.Widget] Statusbar statusbar1;

 #endregion

	public void on_combobox1_changed (object sender, EventArgs a)
	{
	//TODO: Add your code here.
		Console.WriteLine("Combo!!");
		
	}



	public void on_button1_clicked (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	Console.WriteLine("TESTE");
	this.label2.Text = this.entry1.Text;
	}




	
	
	public static void Main (string[] args)
	{
		new GladeApp (args);
	}

	public GladeApp (string[] args) 
	{
		Application.Init ();

		Glade.XML gxml = new Glade.XML (null, "gui.glade", "window1", null);
		gxml.Autoconnect (this);
		Application.Run ();
	}

	// Connect the Signals defined in Glade
	private void OnWindowDeleteEvent (object sender, DeleteEventArgs a) 
	{
		Application.Quit ();
		a.RetVal = true;
	}
}





