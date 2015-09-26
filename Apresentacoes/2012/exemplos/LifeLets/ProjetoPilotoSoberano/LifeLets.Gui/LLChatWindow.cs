// created on 2/12/2004 at 12:36
//
// Contact Information
//
// http://monobasic.sl.org.br
// mailto:letslife@psl-pr.softwarelivre.org
//

#region information
///
/// Class LLChatWindow.cs
///
/// Sumary:
///		Is a chat window aplication
///
/// Authors:
/// Maverson Eduardo S. R.
///
/// ChangeLog: 
/// created on 02/12/2004 
/// modified on 03/12/2004 
/// Added new methods to handle messages on 6/12/2004
///
///
#endregion

using LifeLets.Lib;
using System;
using System.Collections;
using Gtk;
using Glade;

namespace LifeLets.GUI
{
	public class LLChatWindow
	{
		#region GladeSync
		[Glade.Widget] Window ChatWindow;
		[Glade.Widget] VBox vbox1;
		[Glade.Widget] Label label1;
		[Glade.Widget] HBox hbox1;
		[Glade.Widget] ScrolledWindow scrolledwindow1;
		[Glade.Widget] TextView txtMessages;
		[Glade.Widget] HSeparator hseparator1;
		[Glade.Widget] HBox hbox4;
		[Glade.Widget] Label label3;
		[Glade.Widget] HBox hbox2;
		[Glade.Widget] Entry etrMessage;
		[Glade.Widget] EventBox eventbox1;
		[Glade.Widget] Button btnOk;
		[Glade.Widget] Button btnExit;
		[Glade.Widget] Alignment alignment1;
		[Glade.Widget] HBox hbox3;
		[Glade.Widget] Image image1;
		[Glade.Widget] Label label2;
		[Glade.Widget] Statusbar statusbar1;

		#endregion

		private Life myLife;
		private string remoteIP;
		private LifeLets.GUI.SovereChat chat;
		
		public string RemoteIP
		{
			get
			{
				return this.remoteIP;
			}
		}
		
		public void gtk_widget_destroy (object sender, EventArgs a)
		{
		//TODO: Add your code here.
		}

		public LLChatWindow(Life myLife, string remoteIP)
		{
			this.myLife = myLife;
			this.remoteIP = remoteIP;
			this.chat = new LifeLets.GUI.SovereChat(remoteIP);
						
			Glade.XML gxml = new Glade.XML (null, "LLChatWindow.glade", "ChatWindow", null);
			//this.ChatWindow.DefaultWidth = 500;
			gxml.Autoconnect(this);
	    	btnExit.Clicked += new EventHandler (on_btnExit_clicked);
	    	btnOk.Clicked += new EventHandler (on_btnOk_clicked);
		}

		public void on_btnExit_clicked (object sender, EventArgs a)
		{
			this.ChatWindow.Destroy();
		}

		public void on_btnOk_clicked (object sender, EventArgs a)
		{
			this.txtMessages.Buffer.Text +=this.myLife.Name + ": " + this.etrMessage.Text.ToString() +  "\n";
			
			SovereChat message = new SovereChat(this.remoteIP, this.etrMessage.Text.ToString() );

			this.etrMessage.Text = "";
		}

		public void gtk_widget_show (object sender, EventArgs a)
		{
			//TODO: Add your code here.
		}

		public void gtk_true (object sender, EventArgs a)
		{
		//TODO: Add your code here.
		}
		
		public void showmessage(string message)
		{
			this.txtMessages.Buffer.Text += message + "\n";		
		}
	}
}



