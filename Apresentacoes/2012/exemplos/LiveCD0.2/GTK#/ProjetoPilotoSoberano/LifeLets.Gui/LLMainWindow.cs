#region license
// LifeLets  - a first Mono Sovereign Computing Aplication
// Copyright (C) 2004 Mono Basic Brazilian Team
// see a team members on AUTHORS.TXT
//
// This software is licensed unde CC-GPL (Creative Commons GPL)
//
// GNU General Public License, Free Software Foundation
// The GNU General Public License is a Free Software license. 
// Like any Free Software license, it grants to you the four following freedoms:
//
//   0. The freedom to run the program for any purpose.
//   1. The freedom to study how the program works and adapt it to your needs.
//   2. The freedom to redistribute copies so you can help your neighbor.
//   3. The freedom to improve the program and release your improvements to the public,
//   so that the whole community benefits.
//
// You may exercise the freedoms specified here provided that you comply with the
// express conditions of this license. The principal conditions are:
//   1- You must conspicuously and appropriately publish on each copy distributed an 
//      appropriate copyright notice and disclaimer of warranty and keep intact all 
//      the notices that refer to this License and to the absence of any warranty; 
//      and give any other recipients of the Program a copy of the GNU General Public
//      License along with the Program. Any translation of the GNU General Public 
//      License must be accompanied by the GNU General Public License.
//   2- If you modify your copy or copies of the program or any portion of it,
//      or develop a program based upon it, you may distribute the resulting work 
//      provided you do so under the GNU General Public License. Any translation 
//      of the GNU General Public License must be accompanied by the GNU General 
//      Public License.
//   3- If you copy or distribute the program, you must accompany it with the complete 
//      corresponding machine-readable source code or with a written offer, valid for 
//      at least three years, to furnish the complete corresponding machine-readable source code.
//      Any of these conditions can be waived if you get permission from the copyright holder.
//      Your fair use and other rights are in no way affected by the above.
//      This is a human-readable summary of the Legal Code (the full GNU General Public License).
//      here  http://creativecommons.org/licenses/GPL/current/
//
// Contact Information
//
// http://monobasic.sl.org.br
// mailto:letslife@psl-pr.softwarelivre.org
//

#endregion

#region information
///
/// Class Console.cs
///
/// Sumary:
///		Is a main window aplication
///
/// Authors:
/// Alessandro Binhara
///
/// ChangeLog: 
/// created on 29/11/2004 at 22:07
///  
///Added a ChatWindow container by Maverson Eduaro 06/12/2004
///added remove contacts = Fábio / Alexandre El Kadri 11/12/2004
///
///
#endregion

using System;
using System.Threading;
using System.Collections;
using Gtk;
using GLib;
using Glade;
using LifeLets.Lib;


namespace LifeLets.GUI
{

	public class LLMainWindow
	{
	    // Aqui se encontra a declaração de todos os objetos contidos no projeto do Glade: 
	    #region GladeSync
	[Glade.Widget] Window MainWindow;
	[Glade.Widget] VBox vbox1;
	[Glade.Widget] MenuBar menubar1;
	[Glade.Widget] MenuItem menuitem1;
	[Glade.Widget] Menu menuitem1_menu;
	[Glade.Widget] ImageMenuItem my_nname;
	[Glade.Widget] Image image59;
	[Glade.Widget] ImageMenuItem my_nick;
	[Glade.Widget] Image image60;
	[Glade.Widget] ImageMenuItem new_friend;
	[Glade.Widget] Image image61;
	[Glade.Widget] ImageMenuItem chat1;
	[Glade.Widget] Image image62;
	[Glade.Widget] ImageMenuItem change_picture;
	[Glade.Widget] Image image63;
	[Glade.Widget] ImageMenuItem file_share;
	[Glade.Widget] Image image64;
	[Glade.Widget] SeparatorMenuItem separatormenuitem1;
	[Glade.Widget] ImageMenuItem come_online;
	[Glade.Widget] Image image65;
	[Glade.Widget] ImageMenuItem config;
	[Glade.Widget] Image image66;
	[Glade.Widget] ImageMenuItem quit;
	[Glade.Widget] Image image67;
	[Glade.Widget] MenuItem menuitem4;
	[Glade.Widget] Menu menuitem4_menu;
	[Glade.Widget] ImageMenuItem about;
	[Glade.Widget] Image image69;
	[Glade.Widget] ImageMenuItem what_is_sovereigncomputing;
	[Glade.Widget] Image image70;
	[Glade.Widget] Label label1;
	[Glade.Widget] HBox hbox1;
	[Glade.Widget] Image smallPhoto;
	[Glade.Widget] Button btAdd;
	[Glade.Widget] Image image71;
	[Glade.Widget] Button btRemove;
	[Glade.Widget] Image image72;
	[Glade.Widget] Button btNickName;
	[Glade.Widget] Image image73;
	[Glade.Widget] Button btChat;
	[Glade.Widget] Image image74;
	[Glade.Widget] Button btNavegatior;
	[Glade.Widget] Image image75;
	[Glade.Widget] Button btMessage;
	[Glade.Widget] Image image76;
	[Glade.Widget] Button btPicture;
	[Glade.Widget] Image image77;
	[Glade.Widget] Button btTrust;
	[Glade.Widget] Image image78;
	[Glade.Widget] Button btIpFind;
	[Glade.Widget] Image image79;
	[Glade.Widget] ScrolledWindow scrolledMainWindow;
	[Glade.Widget] TreeView treeviewContacts;

	    #endregion 



		private Hashtable chats;
		private Life myLife;
		private TreeStore store;
		private Hashtable contactTree;
		
		public Hashtable Chats
		{
			get
			{
				return this.chats;
			}
		}

		public void RefreshFromLife()
		{
			MainWindow.Title = "LifeLets: "+ myLife.Name;
			if (myLife.PathToPhoto != null) {
				Console.WriteLine("PathToPhoto = '{0}'", myLife.PathToPhoto);
				try {
//					Gdk.Pixbuf photo = new Gdk.Pixbuf(myLife.PathToPhoto);
//					smallPhoto.FromPixbuf  = photo;
					smallPhoto.File = myLife.PathToPhoto;
					Console.WriteLine("set smallPhoto.File");
				} catch (Exception e) {
					smallPhoto.FromFile = "";
					Console.WriteLine("smallPhoto couldn't be Pixbuffed!!!/n{0}", e);
				}
			} else {
				Console.WriteLine("No PathToPhoto");
			}			
		}
		
  		public LLMainWindow (Life myLife, Hashtable chats) 
		{
			this.myLife = myLife;
			this.chats = chats;
			this.contactTree = new Hashtable();
			
        	Application.Init();

        	Glade.XML gxml = new Glade.XML ("gui.glade", "MainWindow");
        	gxml.Autoconnect (this);
        	
			TreeViewMount();
			
		    this.scrolledMainWindow.Show();
		    this.MainWindow.ShowAll();				
		    this.MainWindow.DeleteEvent += new DeleteEventHandler(delete);


			// thread para ficar fazendo atualização da lista de contatos
			System.Threading.Thread thrServidor4 = new System.Threading.Thread(new ThreadStart(this.refreshContactList));
			thrServidor4.IsBackground = true;
			thrServidor4.Start();
						
      		this.RefreshFromLife();
        	Application.Run();
		}

       static void delete(object o, DeleteEventArgs args)
       {
           Application.Quit();
       }

		private void refreshContactList()
		{
			while(true)
			{
				Refreshing();
				System.Threading.Thread.Sleep(3000);
			}
		}				
		
		private void Refreshing()
		{
		IDictionaryEnumerator enumerator = myLife.Contacts.GetEnumerator();
			
				
		while(enumerator.MoveNext())
		{
			Contact contact = (Contact)enumerator.Value;
				if( !this.contactTree.Contains(contact.Nick))
				{						
				    TreeIter iter = new TreeIter (); 
				    store.Append (out iter);
				    store.SetValue (iter, 0, contact.Nick);
				    store.SetValue (iter, 1, contact.Status);
				    this.contactTree.Add(contact.Nick, store.GetPath(iter));
				}
				else
				{					
					TreeIter iter = new TreeIter();
					
					store.GetIter(out iter,(TreePath)(this.contactTree[contact.Nick]));
				
					string status = (String) store.GetValue(iter, 1);
															
					if(status != contact.Status && contact.Status != "")
					{
						store.SetValue(iter, 1, contact.Status); 
					}
				}	
			}		
		}
		
		
		private void TreeViewMount()
		{
			this.store = new TreeStore (GType.String,GType.String);
			 
		    this.treeviewContacts.HeadersVisible = true;
		    this.treeviewContacts.Model = store;
		 
		    TreeViewColumn DemoCol = new TreeViewColumn ();
		    CellRenderer DemoRenderer = new CellRendererText();
		    DemoCol.Title = "Nick";
		    DemoCol.PackStart (DemoRenderer, true);
		    DemoCol.AddAttribute (DemoRenderer, "text", 0);
		    this.treeviewContacts.AppendColumn (DemoCol);
		 
		 
		    TreeViewColumn DataCol = new TreeViewColumn ();
		    CellRenderer DataRenderer = new CellRendererText ();
		    DataCol.Title = "Status";
		    DataCol.PackStart (DataRenderer, false);
		    DataCol.AddAttribute (DataRenderer, "text", 1);
		    this.treeviewContacts.AppendColumn (DataCol);
		 		 
		}
	
		
	   	public void on_chat1_activate (object sender, EventArgs a)
		{
			System.Console.Write("\ntype the remote ip: ");
			
			LLChatWindow chat = new LLChatWindow(myLife, System.Console.ReadLine());
			this.chats.Add(chat.RemoteIP, chat);
		}


	public void on_what_is_sovereigncomputing_activate (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_btPicture_clicked (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_btTrust_clicked (object sender, EventArgs a)
	{
	LLTrustWindow  trustWindow = new LLTrustWindow(myLife);
	//TODO: Add your code here.
	}

	public void on_btRemove_clicked (object sender, EventArgs a)
	{
		TreeModel tm;
		TreeIter ti;
 		string nickname ="";
		
		if (treeviewContacts.Selection.GetSelected(out tm, out ti))
                {
                        nickname = (string) tm.GetValue (ti, 0);
						myLife.RemoveContact(nickname);
						store.Clear();
						this.contactTree.Clear();
						this.Refreshing();
                }
	}

	public void on_config_activate (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_btAdd_clicked (object sender, EventArgs a)
	{
		LLAddContactWindow addContact  = new LLAddContactWindow(myLife);
				
	}

	

	public void on_btIpFind_clicked (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_btChat_clicked (object sender, EventArgs a)
	{
	
	TreeModel tm;
		TreeIter ti;
 		string nickname ="";
 		
		
		if (treeviewContacts.Selection.GetSelected(out tm, out ti))
                {
                        nickname = (string) tm.GetValue (ti, 0);
						LLChatWindow chat = new LLChatWindow(myLife,myLife.FindIpAddress(nickname));
						this.chats.Add(chat.RemoteIP, chat);
                }
	
	//TODO: Add your code here.
	}

	public void on_my_nick_activate (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void OnWindowDeleteEvent (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_come_online_activate (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_my_name_activate (object sender, EventArgs a)
	{
		
		new LLChangeName(this.myLife, this);
		//System.Console.WriteLine("teste");
		
	}

	public void on_credits_activate (object sender, EventArgs a)
	{
	
	LLCreditsWindow creditsWindow = new LLCreditsWindow(myLife);
	//TODO: Add your code here.
	}

	public void on_btNavegatior_clicked (object sender, EventArgs a)
	{
				TreeModel tm;
				TreeIter ti;
			   	string nickName ="";
  	
			  	if (treeviewContacts.Selection.GetSelected(out tm, out ti))
                {
                		nickName = (string) tm.GetValue (ti, 0);
                        LLNavigateWindow navigateWindow = new LLNavigateWindow(myLife.Contacts,nickName,100);
                } 
	}

	public void on_btMessage_clicked (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_btNickName_clicked (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_file_share_activate (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_new_friend_activate (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	private static System.Reflection.Assembly entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
	private static Gdk.Pixbuf cachedLogo = null;

	private static Gdk.Pixbuf logo
	{
		get { 
			if (cachedLogo == null) {
				cachedLogo = new Gdk.Pixbuf(entryAssembly.GetManifestResourceStream("logoLifeLets.jpg"));
			}
			return cachedLogo;
		}
	}
	
	public void on_about_activate (object sender, EventArgs a)
	{
		Gnome.About about = new Gnome.About(
			"LifeLets", 
			"0.1", 
			"(C) 2004 Mono Basic Brazilian Team", 
			"Everyday's use 'sovereign computing' utilities", 
			new System.String [] {
				"Klaus Wuestefled", 
				"Rafael Teixeira",
				"Alessandro de Oliveira Binhara",
				"Helio Y. Mine",
				"Marcelo Davila de Pauli",
				"Marco Antonio Konopaki",
				"Jacson",
				"Maverson" }, 
			new System.String [] {}, // documenters
			"", // string translator_credits, 
			logo);
		about.Run();
	}

	public void on_quit_activate (object sender, EventArgs a)
	{
		Application.Quit();
	
	}

	public void on_treeviewContacts_row_activated (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

	public void on_treeviewContacts_select_cursor_row (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}

/*	public void on_chat1_activate (object sender, EventArgs a)
	{
	//TODO: Add your code here.
	}
*/

	public void on_change_picture_activate (object sender, EventArgs a)
	{
		using (FileChooserDialog chooser = 
			new FileChooserDialog ("Choose new picture file", this.MainWindow, FileChooserAction.Open, 
				Stock.Cancel, ResponseType.Cancel, Stock.Open, ResponseType.Ok)) {
			chooser.SelectMultiple = false;
			ResponseType response = (ResponseType) chooser.Run();
			if (response == ResponseType.Ok) {
				myLife.PathToPhoto = chooser.Filename;
				Console.WriteLine("FileChooserDialog filename = '{0}'", myLife.PathToPhoto);
				RefreshFromLife();
			} else {
				Console.WriteLine("FileChooserDialog response = '{0}'", response);
			}
			chooser.Destroy();
		}
	}

	}

}










