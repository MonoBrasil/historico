// project created on 18/2/2006 at 01:59
using System;
using Gtk;
using Glade;

public class GladeApp
{

	#region GladeSync
	[Glade.Widget] Window window1;
	[Glade.Widget] VBox vbox1;
	[Glade.Widget] Label larrbel2;
	[Glade.Widget] HBox hbox1;
	[Glade.Widget] Label label1;
	[Glade.Widget] ComboBoxEntry comboboxentry1;
	[Glade.Widget] Button button1;

	#endregion


	public void on_comboboxentry1_changed (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	Console.WriteLine("oaslaosdfasdf ");
	this.label2.Text = this.comboboxentry1.Entry.Text;
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




