
using LifeLets.Lib;
using LifeLets.Core;
using System;
using System.Collections;
using Gtk;
using Glade;
using GLib;

namespace LifeLets.GUI
{
	public class LLNavigateWindow
	{
	
		private Hashtable contacts;
		
		private TreeStore store;
		
		#region GladeSync
		[Glade.Widget] Window NaviationWindow;
		[Glade.Widget] VBox vbox1;
		[Glade.Widget] HBox hbox1;
		[Glade.Widget] VBox vbox2;
		[Glade.Widget] HBox hbox2;
		[Glade.Widget] Label labelPath;
		[Glade.Widget] Label labelCurrentPath;
		[Glade.Widget] HBox hbox3;
		[Glade.Widget] VBox vbox3;
		[Glade.Widget] Image image1;
		[Glade.Widget] Label lbAbout;
		[Glade.Widget] Notebook notebook1;
		[Glade.Widget] VBox vbox4;
		[Glade.Widget] HBox hbox4;
		[Glade.Widget] Label label8;
		[Glade.Widget] Label label9;
		[Glade.Widget] ScrolledWindow scrolledwindow1;
		[Glade.Widget] TreeView treeview1;
		[Glade.Widget] HBox hbox5;
		[Glade.Widget] Label label10;
		[Glade.Widget] Entry entry1;
		[Glade.Widget] Button btGo;
		[Glade.Widget] Button btCancel;
		[Glade.Widget] Label label5;
		[Glade.Widget] Label label6;
		[Glade.Widget] Label label7;
		[Glade.Widget] Statusbar statusbar1;

		#endregion


		public LLNavigateWindow (Hashtable contacts,string nickName,double currentTrust)
		{
			this.contacts = contacts;
			Glade.XML gxml = new Glade.XML (null, "LLNavigation.glade", "NaviationWindow", null);
			gxml.Autoconnect(this);					
			
			labelCurrentPath.Text = "";
			label7.Text = "Text Navigation";
			
			TreeViewMount();
			
			MakeTreeView(nickName,currentTrust);
			
			this.btGo.Clicked += new EventHandler(on_btGo_clicked);
		}	
		
		private void MakeTreeView(string friendNickname,double currentTrust)
		{
				double trusting = 0;
				
				store.Clear();		
				
				try				
				{			    
					trusting = (currentTrust*((Contact)contacts[friendNickname]).Trust)/100;
					
					lbAbout.Text = ((Contact)contacts[friendNickname]).Name;
					label9.Text = friendNickname;
					
					string currentPath = labelCurrentPath.Text;										
																				
					this.contacts = PeerNetwork.GetRemoteContacts(((Contact)contacts[friendNickname]).IP);
					
					currentPath += ">" + friendNickname;
					labelCurrentPath.Text = currentPath;
				}
				catch
				{
					Console.WriteLine("ERROR :  The contact is offline!");
					
				}
					            
				foreach(Contact contact in contacts.Values)
				{
				    double MyTrustOnCurrent = (currentTrust*contact.Trust)/100;
				    
				    contact.Status = PeerNetwork.RetrieveStatus(contact);
				    
				    TreeIter iter = new TreeIter (); 
					store.Append (out iter);
					store.SetValue (iter, 0, contact.Nick);
					store.SetValue (iter, 1, contact.Name);
					store.SetValue (iter, 2, MyTrustOnCurrent.ToString());
					store.SetValue (iter, 3, contact.Status);					
				}		
		}
		
		private void TreeViewMount()
		{
			// Definição do modelo da treeview com 4 colunas:
			// Nick; Nome; Trust; Status
			this.store = new TreeStore (GType.String,GType.String,GType.String,GType.String);
			
			 
			this.treeview1.HeadersVisible=true;		    
		    this.treeview1.Model = store;
		 
		    TreeViewColumn NickCol = new TreeViewColumn ();
		    CellRenderer NickRender = new CellRendererText();
		    NickCol.Title = "Nick";
		    NickCol.PackStart (NickRender, true);
		    NickCol.AddAttribute (NickRender, "text", 0);
		    this.treeview1.AppendColumn (NickCol);
		 
		 	TreeViewColumn NameCol = new TreeViewColumn ();
		    CellRenderer NameRenderer = new CellRendererText ();
		    NameCol.Title = "Name";
		    NameCol.PackStart (NameRenderer, false);
		    NameCol.AddAttribute (NameRenderer, "text", 1);
		    this.treeview1.AppendColumn (NameCol);
		    
		    TreeViewColumn TrustCol = new TreeViewColumn ();
		    CellRenderer TrustRenderer = new CellRendererText ();
		    TrustCol.Title = "Trust(%)";
		    TrustCol.PackStart (TrustRenderer, false);
		    TrustCol.AddAttribute (TrustRenderer, "text", 2);
		    this.treeview1.AppendColumn (TrustCol);
		 
		    TreeViewColumn StatusCol = new TreeViewColumn ();
		    CellRenderer StatusRender = new CellRendererText ();
		    StatusCol.Title = "Status";
		    StatusCol.PackStart (StatusRender, false);
		    StatusCol.AddAttribute (StatusRender, "text", 3);
		    this.treeview1.AppendColumn (StatusCol);
		 			 	
		    this.scrolledwindow1.Show();
		    this.NaviationWindow.ShowAll();				
		}
		
		public void on_btGo_clicked(object sender, EventArgs e)
		{
			TreeModel tm;
			TreeIter ti;
 			string nickName ="";
 			string nickTrust = "";
		
			if (treeview1.Selection.GetSelected(out tm, out ti))
            {
                        nickName = (string) tm.GetValue (ti, 0);
                        nickTrust = (string) tm.GetValue (ti, 2);

                       	MakeTreeView(nickName,Double.Parse(nickTrust));                      											
            }
						
		}
	}
}

