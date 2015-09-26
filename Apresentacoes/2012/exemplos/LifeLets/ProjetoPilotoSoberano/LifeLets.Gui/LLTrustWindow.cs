// created on 1/1/2000 at 04:18


using LifeLets.Lib;
using System;
using System.Collections;
using Gtk;
using Glade;

namespace LifeLets.GUI
{
	public class LLTrustWindow
	{
	
		private Life myLife;
		
		#region GladeSync
		[Glade.Widget] Window WindowTrust;
		[Glade.Widget] VBox vbox1;
		[Glade.Widget] HBox hbox2;
		[Glade.Widget] Label label1;
		[Glade.Widget] Label labelFriend;
		[Glade.Widget] HBox hbox1;
		[Glade.Widget] HScale hscale1;
		[Glade.Widget] HBox hbox3;
		[Glade.Widget] HButtonBox hbuttonbox2;
		[Glade.Widget] Button btCancel;
		[Glade.Widget] Button btOk;
		[Glade.Widget] Statusbar statusbar1;

		#endregion


	

		public LLTrustWindow (Life life)
		{
			this.myLife = life;
			Glade.XML gxml = new Glade.XML (null, "llTrustWindow.glade", "WindowTrust", null);
			gxml.Autoconnect(this);	    	
		}			
	}
}

