// created on 27/11/2004 at 17:37
using System;
using Gtk;
using GtkSharp;
using Glade;
using LifeLets.Lib;

namespace LifeLets.GUI
{
	public class ChatWindow
	{
 		#region GladeSync
		[Glade.Widget] Window windowSovereChat;
		[Glade.Widget] VBox vbox1;
		[Glade.Widget] Label label1;
		[Glade.Widget] HBox hbox1;
		[Glade.Widget] ScrolledWindow scrolledwindow1;
		[Glade.Widget] TextView textview1;
		[Glade.Widget] HSeparator hseparator1;
		[Glade.Widget] HBox hbox4;
		[Glade.Widget] Label label3;
		[Glade.Widget] HBox hbox2;
		[Glade.Widget] Entry entry1;
		[Glade.Widget] EventBox eventbox1;
		[Glade.Widget] Button button4;
		[Glade.Widget] Button button5;
		[Glade.Widget] Alignment alignment1;
		[Glade.Widget] HBox hbox3;
		[Glade.Widget] Image image1;
		[Glade.Widget] Label label2;
		[Glade.Widget] Statusbar statusbar1;
 		#endregion GladeSync
		
		private Life life;
		
		public ChatWindow(Life life)
		{
			this.life = life;

           Application.Init();
           Glade.XML gxml = new Glade.XML (null, "soverechatwindow.glade", "windowSovereChat", null);
           gxml.Autoconnect (this);
           button5.Clicked +=  new EventHandler (quit);
           Application.Run();

		}
		
//		public send()
//		{
//			SovereChat message = new SovereChat(ip,life.Nome);
//			
//		} 
		
		
        public static void Factory(Life life) 
        {
                new ChatWindow(life);
        }

        /* Connect the Signals defined in Glade */
        public void quit (object o, EventArgs args) 
        {
                Application.Quit ();
        }
        
public void gtk_widget_show (object sender, EventArgs a)
	{
	//TODO: Add your code here.
		Application.Quit();
	}

	public void gtk_true (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}          
	}
}
