// project created on 18/2/2006 at 00:31
using System;
using Gtk;
using Glade;

public class GladeApp
{

#region GladeSync
	[Glade.Widget] Window window1;
	[Glade.Widget] VBox vbox1;
	[Glade.Widget] MenuBar menubar1;
	[Glade.Widget] MenuItem menuitem1;
	[Glade.Widget] Menu menu1;
	[Glade.Widget] ImageMenuItem novo1;
	[Glade.Widget] ImageMenuItem abrir1;
	[Glade.Widget] ImageMenuItem salvar1;
	[Glade.Widget] ImageMenuItem salvar_como1;
	[Glade.Widget] SeparatorMenuItem separatormenuitem1;
	[Glade.Widget] ImageMenuItem sair1;
	[Glade.Widget] MenuItem menuitem2;
	[Glade.Widget] Menu menu2;
	[Glade.Widget] ImageMenuItem recortar1;
	[Glade.Widget] ImageMenuItem copiar1;
	[Glade.Widget] ImageMenuItem colar1;
	[Glade.Widget] ImageMenuItem excluir1;
	[Glade.Widget] MenuItem menuitem3;
	[Glade.Widget] Menu menu3;
	[Glade.Widget] MenuItem menuitem4;
	[Glade.Widget] Menu menu4;
	[Glade.Widget] MenuItem sobre1;
	[Glade.Widget] Statusbar statusbar1;

#endregion  

	public void on_salvar_como1_activate (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_abrir1_activate (object sender, EventArgs a)
	{
	//TODO: Add your code here.
		Console.WriteLine("Abrir!!!");
		WAbrir wa = new WAbrir();
		wa.Show();
	}

	public void on_salvar1_activate (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_recortar1_activate (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_copiar1_activate (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_novo1_activate (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_colar1_activate (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_sair1_activate (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_sobre1_activate (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_excluir1_activate (object sender, EventArgs a)
	{
	//TODO: Add your code here.
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


