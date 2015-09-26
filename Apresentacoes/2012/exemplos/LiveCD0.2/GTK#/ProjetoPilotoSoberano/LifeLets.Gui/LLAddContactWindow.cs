// created on 1/1/2000 at 01:08

using LifeLets.Lib;
using System;
using System.Collections;
using Gtk;
using Glade;

namespace LifeLets.GUI
{
	public class LLAddContactWindow
	{
		#region GladeSync
		[Glade.Widget] Window LLWindowContact;
		[Glade.Widget] VBox vbox1;
		[Glade.Widget] Label Type ;
		[Glade.Widget] HBox hbox1;
		[Glade.Widget] Label labelName;
		[Glade.Widget] Entry editName;
		[Glade.Widget] HBox hbox2;
		[Glade.Widget] Label labelIp;
		[Glade.Widget] Entry editIp;
		[Glade.Widget] HBox hbox3;
		[Glade.Widget] HButtonBox hbuttonbox1;
		[Glade.Widget] Button btCancel;
		[Glade.Widget] Button btOk;
		[Glade.Widget] Statusbar statusbar1;

		#endregion




		private Life myLife;
		private string remoteIP;
		private LifeLets.GUI.SovereChat chat;
		
		
		public LLAddContactWindow(Life myLife)
		{
			this.myLife = myLife;
						
			Glade.XML gxml = new Glade.XML (null, "llwindowaddcontact.glade", "LLWindowContact", null);
			gxml.Autoconnect(this);
			LLWindowContact.WindowPosition = WindowPosition.Center;	
	 
		}

		public void on_btOk_clicked (object sender, EventArgs a)
		{
			
			myLife.AddContact(editName.Text,editIp.Text);
			LLWindowContact.Destroy();
		
		}

		public void on_btCancel_clicked (object sender, EventArgs a)
		{
		//TODO: Add your code here.
		}
		
  	}
  	
  	
}

