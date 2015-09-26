// created on 1/1/2000 at 02:26

using LifeLets.Lib;
using System;
using System.Collections;
using Gtk;
using Glade;

namespace LifeLets.GUI
{
	public class LLViewPictureWindow
	{
		#region GladeSync
	[Glade.Widget] Window ViewPictureWindow;
	[Glade.Widget] VBox vbox1;
	[Glade.Widget] Image imgViewPicture;
	[Glade.Widget] Button btOK;

		#endregion

		private Life myLife;
		private string remoteIP;
		private LifeLets.GUI.SovereChat chat;
		
		public LLViewPictureWindow(Life myLife)
		{
			this.myLife = myLife;
						
			Glade.XML gxml = new Glade.XML (null, "llviewpicturewindow.glade", "ViewPictureWindow", null);
			gxml.Autoconnect(this);
		}
		public void on_btOK_clicked (object sender, EventArgs a)
		{
			ViewPictureWindow.Destroy();
			//TODO: Add your code here.
		}
		public void on_ViewPictureWindow_activate_focus (object sender, EventArgs a)
		{
			//Photo.ShowPicture(myLife);
		//TODO: Add your code here.
		}		
		public void on_imgViewPicture_focus (object sender, EventArgs a)
		{
			//Photo.ShowPicture(myLife);
			//System.Console.WriteLine("show picture");
			this.imgViewPicture;
			//TODO: Add your code here.
		}		
	}
}