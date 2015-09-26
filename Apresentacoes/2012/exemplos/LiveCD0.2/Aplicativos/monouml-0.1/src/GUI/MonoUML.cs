/*
MonoUML.GUI - The core classes for running the MonoUML's GUI
Copyright (C) 2004  Mario Carrión

This file is part of MonoUML.

MonoUML is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

MonoUML is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MonoUML; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
using Gtk;
using Glade;
using Gnome;
using System;
using Code = MonoUML.CodeGeneration;
using IconLibrary = MonoUML.IconLibrary;
using RevEng = MonoUML.ReverseEngineering;
using Widgets = MonoUML.Widgets;
using UML2 = MonoUML.Widgets.UML2;
using Preferences = MonoUML.Preferences;
using MonoUML.XmiImporter;
using MonoUML.I18n;

namespace MonoUML.GUI
{
	public class MonoUML : Gnome.Program
	{
		// Constructor
		public MonoUML(string[] args) : base(ID+VERSION, VERSION, Gnome.Modules.UI, args)
		{
			Application.Init();
			_glade = new XML (null, GLADE_FILE, "_mainWindow", ID);
			_glade.Autoconnect (this);
            _mainWindow.Icon = IconLibrary.PixbufLoader.GetIcon ("main_icon.png");
			BuildProperties ();
			//Preferences.Core instance = Preferences.Core.Instance;
			//instance.Run ();
			if (RELEASE_TYPE.Equals ("SVN") && args.Length <= 0)
			{
				//Gtk.Window win = (Gtk.Window) _glade.GetWidget ("_mainWindow");
				MessageDialog md = new MessageDialog (_mainWindow,
										DialogFlags.DestroyWithParent,
										MessageType.Warning,
										ButtonsType.Close, 
										GettextCatalog.GetString (
										"You are running a MonoUML development version. This version\n"+
										"MIGHT HAVE bugs, if have found any please report it to\n"+
										"our bugzilla site:\nhttp://bugzilla.monouml.org\n"+
										"Any help is very appreciated.\n"+
										"\nMore information in the mailing list."+
										"If you want to test, there are some samples in the 'samples/' "+
										"directory.\n\nRegards\n----\nMonoUML Team."));
				md.Run ();
				//win.Show ();
				md.Hide ();				
			}
         
			if ((args.Length == 1) && System.IO.File.Exists (args[0]))
			{
				// first show all widgets, then open the project, because when 
				// the project is open the UMLPropertiesTab hides all its inner widgets.
				ShowAllWidgets ();
				_hub.OpenProject (args[0]);
			}
			else
			{
				BuildSelectProjectDialog();
			}
			Application.Run();
		}

		// Shows all the contained widgets
		public void ShowAllWidgets()
		{
			_mainHPaned.Visible = true;
			_mainVPaned.Visible = true;
			_mainWindow.ShowAll();
		}

        // Builds the handlers for the dettachable widgets
        private void BuildProperties()
        {
        	System.Console.WriteLine ("BuildProperties");
			_canvasArea = new Widgets.CanvasArea ();
			_mainVPaned.Add1 (_canvasArea); 
        	_propertiesNotebook = new Notebook ();
        	_mainVPaned.Add2 (_propertiesNotebook);        	
			_tree = new Widgets.Tree();
			_scrolledwindowTree.Add(_tree);
			_umlPropertiesTab = new Widgets.UMLPropertiesTab();
			// Push tab into a scrolledwindow
			Viewport propertiesViewport = new Viewport ();
			propertiesViewport.ShadowType = ShadowType.None;
			propertiesViewport.BorderWidth = 3;
			propertiesViewport.Add (_umlPropertiesTab);
			_propertiesScroll = new ScrolledWindow ();
			_propertiesScroll.HscrollbarPolicy = PolicyType.Automatic;
			_propertiesScroll.VscrollbarPolicy = PolicyType.Automatic;
			_propertiesScroll.ShadowType = ShadowType.In;
			_propertiesScroll.BorderWidth = 3;
			_propertiesScroll.Add (propertiesViewport);
			
			_propertiesNotebook.AppendPage(
				_propertiesScroll,
				new Gtk.Label (GettextCatalog.GetString ("UML Properties"))
			);
			_hub = Widgets.Hub.Instance;
			_hub.AddView (_tree);
			_hub.AddView (_canvasArea.NoteBook);
			_hub.AddView (_umlPropertiesTab);
		}

		// Builds the "Select your project" Dialog
		private void BuildSelectProjectDialog()
		{
			SelectProjectDialog dialog = new SelectProjectDialog (_mainWindow);
			ProjectDialogAction action = ProjectDialogAction.New;
			dialog.Run ();
			action = dialog.Action;
			dialog.Hide ();
			// ShowAllWidgets was moved to here, so it's done before creating
			// or opening a project. The reason is that ShowAllWidgets shows
			// (surprise) ALL the widgets in the window, including those inside
			// the UMLPropertiesTab. If the user interacts with those widgets
			// (which have no associated model element) an exception is thrown.
			// The cure: show all widgets before, then let the hub do its job
			// and eventually the UMLPropertiesTab hides the widgets.
			ShowAllWidgets();
			switch (action)
			{
				case ProjectDialogAction.Open:
					if (_hub.OpenProject() != Gtk.ResponseType.Ok)
					{
						_hub.CreateNewProject();
					}
					break;
				case ProjectDialogAction.ImportXmi:
					ImportXmi ();
					break;
				case ProjectDialogAction.ReverseEngineering:
					ReverseEngineering ();
					break;
				default:
					_hub.CreateNewProject();
					break;
			}
		}
		
		private void ImportXmi ()
		{
			FileChooserDialog fc = new FileChooserDialog (
				GettextCatalog.GetString ("Select a model or diagram file"),
				null,
				FileChooserAction.Open);
			FileFilter filter;
			// xmi files filter
			filter = new FileFilter ();
			filter.Name = GettextCatalog.GetString ("XMI files");
			filter.AddPattern ("*.xmi");
			fc.AddFilter (filter);
			// argo files filter
			filter = new FileFilter ();
			filter.Name = GettextCatalog.GetString ("Argo files");
			filter.AddPattern ("*.zargo");
			fc.AddFilter (filter);
			// poseidon files filter
			filter = new FileFilter ();
			filter.Name = GettextCatalog.GetString ("Poseidon files");
			filter.AddPattern ("*.zuml");
			fc.AddFilter (filter);
			// configures the buttons
			fc.AddButton (Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
			fc.AddButton (Gtk.Stock.Open, Gtk.ResponseType.Ok);
			// runs the file chooser
			fc.SelectMultiple = false;
			Gtk.ResponseType response = (Gtk.ResponseType)fc.Run();
			string filename = fc.Filename;
			fc.Hide();
			if (response == Gtk.ResponseType.Ok)
			{
				Importer importer = new Importer ();
				System.IO.Stream inputStream = importer.ImportXmiStream (filename);
				_hub.OpenProject (inputStream);
				inputStream.Close ();
				ShowAllWidgets();
			}
		}

		// Opens a new "About Window"
        private void OnAboutEvent(object o, EventArgs args) 
        {
			Gdk.Pixbuf pixbuf = IconLibrary.PixbufLoader.GetIcon ("about.png");
			
	        Gnome.About about_window = new Gnome.About(
	        	"MonoUML", 
	        	VERSION,
	        	"Copyright (C) 2004-2005, MonoUML Team",
	        	GettextCatalog.GetString ("The CASE tool for Mono\n(SVN pre-alpha version)"),
	        	_authors,
	        	_documenters,
	        	_translators,
        		pixbuf);
        	about_window.Icon = IconLibrary.PixbufLoader.GetIcon ("main_icon.png");
        	about_window.Show();
        }

        private void OnImportXMI (object o, EventArgs args)
        {
			ImportXmi ();
        }

		private void OnOpenProject(object o, EventArgs args)
		{
			_hub.OpenProject();
			ShowAllWidgets();
		}
        
		private void OnNewProject(object o, EventArgs args)
		{
			_hub.CreateNewProject();
		}
		
		private void OnCloseEvent(object o, EventArgs args)
		{
			_hub.CloseProject();
		}

		private void OnNewUmlDiagram(object o, EventArgs args)
		{
			_hub.NewUMLDiagram();
		}

		private void OnNewUmlElement(object o, EventArgs args)
		{
			_hub.NewUMLElement();
		}

		private void OnSaveProject(object o, EventArgs args) 
		{
			_hub.Save();
        }

		private void OnSaveProjectAs(object o, EventArgs args) 
		{
			_hub.SaveAs();
        }

        private void OnWindowDeleteEvent(object o, DeleteEventArgs args) 
        {
	       	QuitMe();
			args.RetVal = true;
        }
        
        private void OnWindowQuitEvent(object o, EventArgs args) 
        {
        	QuitMe();
        }
        
        private void OnCodeGeneration (object o, EventArgs args)
        {
        	Code.Wizard wizard = new Code.Wizard (_hub.ElementsList);
       		wizard.Show ();
        }
        
        private void OnReverseEngineering (object o, EventArgs args)
        {
        	ReverseEngineering ();
        }
        
        private void LoadFromReverseEngineering (object sender, EventArgs e)
        {
        	try
        	{
				System.Collections.ArrayList arr = ((RevEng.Wizard) sender).XmiElements;
				if (arr != null)
				{
					// ShowAllWidgets must be called first. See comment on BuildSelectProjectDialog.
					ShowAllWidgets();
//					_hub.OpenProject (arr); //FIXME:
					_hub.OpenProject ("some-xmi.xmi");
				}
			}
			catch (Exception ex)
			{
				System.Console.WriteLine ("Error! "+ex.Message);
			}
        }

		// Closes the window
        private void QuitMe()
        {
        	Application.Quit();
        }
        
        private void ReverseEngineering ()
        {
        	RevEng.Wizard wizard = new RevEng.Wizard ();
       		wizard.Show ();
       		wizard.Hidden += LoadFromReverseEngineering;
        }

		private XML _glade;
		public static readonly string GLADE_FILE = "gui.glade";
		private const string ID = "monouml";
		private const string VERSION = "0.1";
		private const string RELEASE_TYPE = "SVN";
		//Glade Widgets
		[Widget] private Gtk.Window _mainWindow;
		[Widget] private ScrolledWindow _scrolledwindowTree;
		[Widget] private VPaned _mainVPaned;
		[Widget] private HPaned _mainHPaned;
        //
		private ScrolledWindow _propertiesScroll;
		private Notebook _propertiesNotebook;
		//
        private Widgets.Hub _hub;
        private Widgets.Tree _tree;
        private Widgets.CanvasArea _canvasArea;
        private Widgets.UMLPropertiesTab _umlPropertiesTab;
		//
        private string []_authors = new string[] 
        	{
				"Mario Carrión <mario.carrion@gmail.com>",
				"Rodolfo Campero <rodolfo.campero@gmail.com>",
				"Manuel Cerón <ceronman@gmail.com>",
				"Mario Fuentes <mario@gnome.cl>",
			};
		private string []_documenters = new string[]
			{
				"Mario Carrión <mario.carrion@gmail.com>",
				"Rodolfo Campero <rodolfo.campero@gmail.com>",
				"Miguel Huerta <hgmiguel@gmail.com>"
			};
		private string _translators = ""+
			"Eduardo García <enzo@enzolutions.com> (Spanish)\n" +
			"Everaldo Canuto <everaldo@simios.org> (Portuguese-Brasil)" +
			GettextCatalog.GetString ("\n\nHelp us by translating to your native language.");
	}
}
