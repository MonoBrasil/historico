// created on 10/12/2004 at 02:29

using LifeLets.Lib;
using System;
using System.Collections;
using Gtk;
using Glade;
using System.Threading;

namespace LifeLets.GUI
{
	public class LLCreditsWindow
	{
	
		private Life myLife;
		
		#region GladeSync
		[Glade.Widget] Window windowCredits;
		[Glade.Widget] VBox vbox1;
		[Glade.Widget] Label label2;
		[Glade.Widget] HSeparator hseparator1;
		[Glade.Widget] ScrolledWindow scrolledwindow1;
		[Glade.Widget] TextView textview1;
		[Glade.Widget] HBox hbox1;
		[Glade.Widget] Button button1;
		[Glade.Widget] Statusbar statusbar1;

		#endregion

	public void on_button1_clicked (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}




	

		public LLCreditsWindow (Life life)
		{
		
			
			this.myLife = life;
			Glade.XML gxml = new Glade.XML (null, "LLCreditsWindow.glade", "windowCredits", null);
			

			gxml.Autoconnect(this);
			this.windowCredits.WindowPosition = WindowPosition.Center;
		     
		     System.Threading.Thread thrCredits = new System.Threading.Thread(new ThreadStart(this.moveText));
			thrCredits.IsBackground = true;
			thrCredits.Start();
				    	
		}			
		
		private void moveText()
		{
			while(true)
			{
				//this.scrolledwindow1.
						    
			}
			//	System.Threading.Thread.Sleep(3000);
		}		
		
						
	}
}


