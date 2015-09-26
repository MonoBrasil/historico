using System;
using Gtk;

public class MyWindow : Window
{	
	public MyWindow () : base ("MyWindow")
	{
		this.SetDefaultSize (400, 300);
		this.DeleteEvent += new DeleteEventHandler (OnMyWindowDelete);
		this.ShowAll ();
	}
	
	void OnMyWindowDelete (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}