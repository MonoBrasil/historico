/*
MonoUML.ReverseEngineering - Reverse Engineering from .NET assemblies
Copyright (C) 2005  Mario Carrión  <mario.carrion@gmail.com>

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
using Gnome;
using System;
using System.Threading;
using System.Collections;
using MonoUML.IconLibrary;
using MonoUML.I18n;

namespace MonoUML.ReverseEngineering
{
	public class Wizard : Gtk.Window
	{

		public Wizard () : base (GettextCatalog.GetString ("Reverse Engineering"))
		{
			_druid = new Gnome.Druid ();
			_start = new DruidPageEdge (Gnome.EdgePosition.Start);
			_finish = new DruidPageEdge (Gnome.EdgePosition.Finish);
			_pImportingLevel = new DruidPageStandard ();
			_pImportingLevelAccess = new DruidPageStandard ();
			_pImportingParameters = new DruidPageStandard ();
			_pAssemblies = new DruidPageStandard ();
			_assemblyImporter = new AssemblyImporter ();
			_importingLevel = AssemblyImporterLevel.AssemblyImport;
			
			_start.Title = GettextCatalog.GetString ("Reverse Engineering");
			_start.Text = GettextCatalog.GetString ("In the following pages you will be able to import .NET assemblies and then build its UML model.\n\nClick Forward to continue");
			
			_finish.Title = GettextCatalog.GetString ("Start reverse engineering");
			_finish.Text = GettextCatalog.GetString ("Click Apply to proceed the reverse engineering, when finished a dialog window will pop up.");
			_finish.FinishClicked += OnReverseEngineering;

			CreateAssemblies ();
			CreateImportingLevel ();
			CreateImportingParameters ();
			CreateImportingLevelAccess ();
			
			_druid.AppendPage (_start);
			_druid.AppendPage (_pAssemblies);
			_druid.AppendPage (_pImportingLevel);
			_druid.AppendPage (_pImportingParameters);
			_druid.AppendPage (_pImportingLevelAccess);
			_druid.AppendPage (_finish);
			_druid.Cancel += OnCancel;
			
			Add (_druid);

			SetDefaultSize (530, 295);
			Modal = true;
			WindowPosition = Gtk.WindowPosition.CenterAlways;
			Icon = PixbufLoader.GetIcon ("main_icon.png");
			ShowAll ();
			Resize (1, 1);
		}
		
		public ArrayList XmiElements 
		{
			get
			{
				return _xmiElements;
			}
		}
		
		private void CreateAssemblies ()
		{
			_pAssemblies.Title = GettextCatalog.GetString ("Add assemblies for importing");
			
			HBox hbox = new HBox (false, 2);

			_store = new ListStore (typeof (string));

			_pAssembliesView = new TreeView ();
			_pAssembliesView.HeadersVisible = false;
			_pAssembliesView.AppendColumn ("", new CellRendererText(), "text", 0);
			_pAssembliesView.Model = _store;
			_pAssembliesView.FocusInEvent += OnEnableButton; 
			ScrolledWindow scrolledWindow = new ScrolledWindow ();			
			scrolledWindow.Add (_pAssembliesView);
			scrolledWindow.HeightRequest = 120;
			
			VBox vbox = new VBox (false, 0);
			Button addAssembly = new Button ("");
			addAssembly.UseStock = true;
			addAssembly.Label = Gtk.Stock.Add;
			addAssembly.Clicked += OnAddAssembly;
			
			_delAssembly = new Button ("");
			_delAssembly.UseStock = true;
			_delAssembly.Label = Gtk.Stock.Delete;
			_delAssembly.Clicked += OnDeleteAssembly;
			_delAssembly.Sensitive = false;
			
			vbox.PackStart (addAssembly, false, false, 0);
			vbox.PackStart (_delAssembly, false, false, 0); 
			
			hbox.PackStart (scrolledWindow, true, true, 0);
			hbox.PackEnd (vbox, false, false, 0);
			
			_pAssemblies.AppendItem ("", hbox,"");
			_pAssemblies.NextClicked += VerifyAssemblies;
		}
		
		private void CreateImportingLevel ()
		{
			_pImportingLevel.Title = GettextCatalog.GetString ("Select importing level");

			RadioButton assemblyImport = new RadioButton (GettextCatalog.GetString ("Import _only elements within assembly."));
			assemblyImport.Clicked += RadioSelectedAssemblyImport;
			RadioButton fullImport = new RadioButton (GettextCatalog.GetString ("F_ull import from assembly."));
			fullImport.Clicked += RadioSelectedFullImport;
			fullImport.Group = assemblyImport.Group;

			this._importingLevel = AssemblyImporterLevel.AssemblyImport;
			 
			 VBox vbox = new VBox ();
			 vbox.PackStart (assemblyImport);
			 vbox.PackStart (fullImport);
			 
			_pImportingLevel.AppendItem ("", vbox,"");
		}
		
		private void CreateImportingLevelAccess ()
		{
			_pImportingLevelAccess.Title = GettextCatalog.GetString ("Set your importing parameters");
			
			VBox vbox = new VBox ();
			vbox.Spacing = 3;
			VBox vboxClasses = new VBox  ();
			VBox vboxIfaces = new VBox  ();
			
			CheckButton publicMethods = new CheckButton (GettextCatalog.GetString ("Import _public methods"));
			publicMethods.Active = true;
			publicMethods.Toggled += OnPublicMethods;
			CheckButton privateMethods = new CheckButton (GettextCatalog.GetString ("Import p_rivate methods"));
			privateMethods.Active = true;
			privateMethods.Toggled += OnPrivateMethods;
			CheckButton protectedMethods = new CheckButton (GettextCatalog.GetString ("Import pr_otected methods"));
			protectedMethods.Active = true;
			protectedMethods.Toggled += OnProtectedMethods;
			CheckButton publicFields = new CheckButton (GettextCatalog.GetString ("Import public fiel_ds"));
			publicFields.Active = true;
			publicFields.Toggled += OnPublicFields;
			CheckButton privateFields = new CheckButton (GettextCatalog.GetString ("Import private f_ields"));
			privateFields.Active = true;
			privateFields.Toggled += OnPrivateFields;
			CheckButton protectedFields = new CheckButton (GettextCatalog.GetString ("Import protected fi_elds"));
			protectedFields.Active = true;
			protectedFields.Toggled += OnProtectedFields;

			vboxClasses.PackStart (publicFields);
			vboxClasses.PackStart (protectedFields);
			vboxClasses.PackStart (privateFields);
			
			vboxIfaces.PackStart (publicMethods);
			vboxIfaces.PackStart (protectedMethods);
			vboxIfaces.PackStart (privateMethods);
			
			_frameClasses = new Frame ();
			_frameClasses.Label = GettextCatalog.GetString ("Classes");
			_frameClasses.Child = vboxClasses;
			
			_frameIfaces = new Frame ();
			_frameIfaces.Label = GettextCatalog.GetString ("Classes and Interfaces");
			_frameIfaces.Child = vboxIfaces;
			
			vbox.PackStart (_frameClasses);
			vbox.PackStart (_frameIfaces);
			
			_pImportingLevelAccess.AppendItem ("", vbox, "");
		}
		
		private void CreateImportingParameters ()
		{
			_pImportingParameters.Title = GettextCatalog.GetString ("Set your importing parameters");
			
			CheckButton classes = new CheckButton (GettextCatalog.GetString ("Import _Classes"));
			classes.Active = true;
			classes.Toggled += OnClassesToggled;
			CheckButton enumerations = new CheckButton (GettextCatalog.GetString ("Import _Enumerations"));
			enumerations.Active = true;
			enumerations.Toggled += OnEnumerationsToggled;
			CheckButton interfaces = new CheckButton (GettextCatalog.GetString ("Import _Interfaces"));
			interfaces.Active = true;
			interfaces.Toggled += OnInterfacesToggled;
			CheckButton structs = new CheckButton (GettextCatalog.GetString ("Import _Structs"));
			structs.Active = true;
			structs.Toggled += OnStructsToggled;
			CheckButton events = new CheckButton (GettextCatalog.GetString ("Import E_vents"));
			events.Sensitive = false;
			events.Toggled += OnEventsToggled;
			CheckButton delegates = new CheckButton (GettextCatalog.GetString ("Import _Delegates"));
			delegates.Sensitive = false;
			delegates.Toggled += OnDelegatesToggled;
			
			VBox vbox = new VBox ();
			vbox.PackStart (classes);
			vbox.PackStart (enumerations);
			vbox.PackStart (interfaces);
			vbox.PackStart (structs);
			vbox.PackStart (events);
			vbox.PackStart (delegates);
			
			_pImportingParameters.AppendItem ("", vbox, "");
		}
		
		private void OnAddAssembly (object sender, EventArgs e)
		{
			FileChooserDialog fc = new FileChooserDialog (
				GettextCatalog.GetString ("Select your .NET assembly"),
				null,
				FileChooserAction.Open);
			FileFilter filter;
			// dll files filter
			filter = new FileFilter ();
			filter.Name = GettextCatalog.GetString (".NET Assembly files");
			filter.AddPattern ("*.dll");
			fc.AddFilter (filter);
			// exe files filter
			filter = new FileFilter ();
			filter.Name = GettextCatalog.GetString (".NET Executable files");
			filter.AddPattern ("*.exe");
			fc.AddFilter (filter);
			// ALL files filter
			filter = new FileFilter ();
			filter.Name = GettextCatalog.GetString ("Other files");
			filter.AddPattern ("*.*");
			fc.AddFilter (filter);
			// configures the buttons
			fc.AddButton (Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
			fc.AddButton (Gtk.Stock.Open, Gtk.ResponseType.Ok);
			// runs the file chooser
			fc.SelectMultiple = true;
			Gtk.ResponseType response = (Gtk.ResponseType) fc.Run();
			fc.Hide();
			if (response == Gtk.ResponseType.Ok)
			{
				foreach (string filename in fc.Filenames)
				{
					try
					{
						_assemblyImporter.AddAssembly (filename);
						_store.AppendValues (filename); 
					} 
					catch (System.Exception ex)
					{
						ErrorMessage (
							MessageType.Error,
							GettextCatalog.GetString ("Filename: ")+
							filename+
							GettextCatalog.GetString (" is not a .NET assembly:\n\n")+
							ex.Message);
					}
				}  
			}
		}
		
		private void OnCancel (object sender, EventArgs e)
		{
			Hide ();
		}
		
		private void OnClassesToggled (object sender, EventArgs e)
		{
			_assemblyImporter.ImportClasses = _frameClasses.Sensitive = ((Gtk.CheckButton) sender).Active;
		}
		
		private void OnDelegatesToggled (object sender, EventArgs e)
		{
			_assemblyImporter.ImportDelegates = ((Gtk.CheckButton) sender).Active;
		}
		
		private void OnDeleteAssembly (object sender, EventArgs e)
		{
			TreeIter iter;
			TreeModel model;
			int index = -1;
			if (_pAssembliesView .Selection.GetSelected (out model, out iter))
			{
				TreePath path = model.GetPath (iter);
				index = path.Indices [0];
			}
			if (index > -1)
			{
				if (_pAssembliesView.Model.IterNthChild (out iter, index))
				{
					string selected = (string) _pAssembliesView.Model.GetValue(iter, 0);
					if (_assemblyImporter.DeleteAssembly (selected))
					{
						_delAssembly.Sensitive = false;
						_store.Remove (ref iter); 
					}
				}
			}
		}
		
		private void OnEnableButton (object o, Gtk.FocusInEventArgs args)
		{
			Gtk.TreeIter iter;
			if (((Gtk.TreeView) o).Model.GetIterFirst (out iter))
			{
				_delAssembly.Sensitive = true;
			}
		}
		
		private void OnEnumerationsToggled (object sender, EventArgs e)
		{
			_assemblyImporter.ImportEnumerations = ((Gtk.CheckButton) sender).Active;
		}

		private void OnEventsToggled (object sender, EventArgs e)
		{
			_assemblyImporter.ImportEvents = ((Gtk.CheckButton) sender).Active;
		}
		
		private void OnInterfacesToggled (object sender, EventArgs e)
		{
			_assemblyImporter.ImportInterfaces = _frameIfaces.Sensitive = ((Gtk.CheckButton) sender).Active;
		}

		private void OnPrivateFields (object sender, EventArgs e)
		{
			_assemblyImporter.ImportPrivateFields = ((Gtk.CheckButton) sender).Active;
		}

		private void OnPrivateMethods (object sender, EventArgs e)
		{
			_assemblyImporter.ImportPrivateMethods = ((Gtk.CheckButton) sender).Active;
		}

		private void OnProtectedFields (object sender, EventArgs e)
		{
			_assemblyImporter.ImportProtectedFields = ((Gtk.CheckButton) sender).Active;
		}
		
		private void OnProtectedMethods (object sender, EventArgs e)
		{
			_assemblyImporter.ImportProtectedMethods = ((Gtk.CheckButton) sender).Active;
		}

		private void OnPublicFields (object sender, EventArgs e)
		{
			_assemblyImporter.ImportPublicFields = ((Gtk.CheckButton) sender).Active;
		}		

		private void OnPublicMethods (object sender, EventArgs e)
		{
			_assemblyImporter.ImportPublicMethods = ((Gtk.CheckButton) sender).Active;
		}

		private void OnReverseEngineering (object sender, EventArgs e)
		{
			_assemblyImporter.ImportingLevel = _importingLevel;
			Thread thread = new Thread (new ThreadStart (_assemblyImporter.ReadAssemblies));
			thread.Start ();
			thread.Priority = System.Threading.ThreadPriority.Highest;
			ProgressWindow window = new ProgressWindow (_assemblyImporter, thread);
			window.Show ();
			while (_assemblyImporter.Reading || thread.ThreadState == System.Threading.ThreadState.Running )
			{
				while (Application.EventsPending ())
					Application.RunIteration (); 
			}
			thread.Abort ();
			window.Message = GettextCatalog.GetString ("Serializating...");
			_assemblyImporter.WriteXMI ("some-xmi.xmi"); //FIXME
			_xmiElements = _assemblyImporter.XmiElements;
			ErrorMessage (MessageType.Info , GettextCatalog.GetString ("The Reverse Engineering has finished.\n\nStarting to load."));
			Hide ();
		}

		private void OnStructsToggled (object sender, EventArgs e)
		{
			_assemblyImporter.ImportStructs = ((Gtk.CheckButton) sender).Active;
		}

		private void RadioSelectedAssemblyImport (object sender, EventArgs e)
		{
			_importingLevel = AssemblyImporterLevel.AssemblyImport;
		}
		
		private void RadioSelectedFullImport (object sender, EventArgs e)
		{
			_importingLevel = AssemblyImporterLevel.FullImport;
		}
		
		private void VerifyAssemblies (object sender, EventArgs e)
		{
			TreeIter iter; 
			if (!_store.GetIterFirst (out iter))
			{
				_druid.Page = _start;
				ErrorMessage (
					MessageType.Error,
					GettextCatalog.GetString ("Add at least one assembly before proceeding the next step.")
					);
			}
		}
		
		private void ErrorMessage (MessageType type, string message)
		{
			MessageDialog md = new MessageDialog (
				this,
				DialogFlags.DestroyWithParent,
				type,
				ButtonsType.Close,
				message);
			md.Run();
			md.Destroy();
		}

		private AssemblyImporter _assemblyImporter;
		private Gtk.Button _delAssembly;
		private Druid _druid;
		private DruidPageEdge _finish;
		private Frame _frameClasses;
		private Frame _frameIfaces;		
		private DruidPageStandard _pImportingLevel;
		private DruidPageStandard _pImportingLevelAccess;
		private DruidPageStandard _pAssemblies;
		private TreeView _pAssembliesView;
		private DruidPageStandard _pImportingParameters;
		private DruidPageEdge _start;
		private ListStore _store;
		private AssemblyImporterLevel _importingLevel;
		private ArrayList _xmiElements;
		
		private class ProgressWindow : Gtk.Window 
		{
			public ProgressWindow (AssemblyImporter  importer, Thread thread) : base (GettextCatalog.GetString ("Importing..."))
			{
				_thread = thread;
				_importer = importer;
				_progress = new ProgressBar ();
				_progress.Orientation = ProgressBarOrientation.LeftToRight;
				_progress.Pulse ();
				
				Button cancel = new Button ();
				cancel.UseStock = true;
				cancel.Label = Gtk.Stock.Cancel;
				cancel.Clicked += CancelImporting;

				HButtonBox buttonBox = new HButtonBox ();
				buttonBox.PackStart (cancel, false, false, 0);
				
				VBox vbox = new VBox ();
				_message = new Label (GettextCatalog.GetString ("Please wait while loading..."));
				vbox.PackStart (_message, false, false, 0);
				vbox.PackEnd (buttonBox, false, false, 0);
				vbox.PackEnd (_progress, false, false, 0);

				_timer = GLib.Timeout.Add (100, new GLib.TimeoutHandler (ProgressTimeout));
				
				Add (vbox);

				Modal = true;
				HeightRequest = 70;
				WidthRequest = 180;
				WindowPosition = Gtk.WindowPosition.CenterAlways;
				Resizable = false;
				Icon = PixbufLoader.GetIcon ("main_icon.png");
				ShowAll ();
			}
			
			public string Message
			{
				set
				{
					_message.Text = value; 
				}
			}
			
			private void CancelImporting (object sender, EventArgs e)
			{
				_thread.Abort ();
				GLib.Source.Remove (_timer);
				Hide ();
			}
			
			private bool ProgressTimeout ()
			{
				_progress.Pulse ();
				if (!_importer.Reading)
				{
					Hide ();
				}
				return _importer.Reading;
			}

			private AssemblyImporter _importer;
			private ProgressBar _progress;
			private Thread _thread;
			private Label _message;
			private uint _timer;
		}
	}

}
