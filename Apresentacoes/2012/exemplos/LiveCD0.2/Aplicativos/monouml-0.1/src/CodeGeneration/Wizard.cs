/*
MonoUML.CodeGeneration - Wizard-like class for code generating
Copyright (C) 2005  Rodolfo Campero
Copyright (C) 2005  Mario Carrión

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
using ES = ExpertCoder.ExpertSystem;
using Gtk;
using Gnome;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using MonoUML.I18n;

namespace MonoUML.CodeGeneration
{
	public class Wizard : Gtk.Window
	{
		public Wizard (IList inputModel) :
			base (GettextCatalog.GetString ("Code generation"))
		{
			_inputModel = inputModel;
			_generators = new ArrayList ();
			
			_druid = new Gnome.Druid ();
			_start = new DruidPageEdge (Gnome.EdgePosition.Start);
			_finish = new DruidPageEdge (Gnome.EdgePosition.Finish);
			_pLanguages = new DruidPageStandard ();
			_parametersDruid = new DruidPageStandard ();

			_druid.AppendPage (_start);
			_druid.AppendPage (_pLanguages);
			_druid.AppendPage (_parametersDruid);
			_druid.AppendPage (_finish);
			_druid.Cancel += DruidCancel;
			
			_start.Title = GettextCatalog.GetString ("Code Generation");
			_start.Text = GettextCatalog.GetString ("In the following pages you will be able to create source code from the UML model.\n\nClick Forward to continue");
			
			_finish.Title = GettextCatalog.GetString ("Start code generation");
			_finish.Text = GettextCatalog.GetString ("Click Apply to proceed the code generation, when finished a dialog window will pop up.");
			_finish.FinishClicked += GenerateCode;
			
			CreatePLanguagesPage ();
			CreateParametersPage ();
			
			Add (_druid);

			SetDefaultSize (530, 250);
			Modal = true;
			WindowPosition = Gtk.WindowPosition.CenterAlways;
			LookForCodeGenerators ();
			Resize (1, 1) ;
		}

		public new void Show ()
		{
			if (_dllsFound == true)
			{
				ShowAll ();
				base.Show ();
			}
		}

		private void CreateParametersPage ()
		{
			_viewport = new Viewport ();
			_viewportDocumentation = new Label ("here!");
			ScrolledWindow scrolledWindow = new ScrolledWindow ();			
			scrolledWindow.Add (_viewport);
			VBox vbox = new VBox();
			vbox.PackStart (scrolledWindow, true, true, 2);
			vbox.PackEnd (_viewportDocumentation, false, false, 0);
			
			_parametersDruid.Title = GettextCatalog.GetString ("Required values");
			_parametersDruid.AppendItem ("", vbox,"");
			_parametersDruid.NextClicked += VerifyWrittenValues;
		}
		
		private void CreatePLanguagesPage ()
		{
			_store = new TreeStore (typeof (string));
			
			_pLangView = new TreeView ();
			ScrolledWindow scrolledWindow = new ScrolledWindow ();			
			scrolledWindow.Add (_pLangView);
			_pLangView.HeadersVisible = false;
			_pLangView.AppendColumn ("", new CellRendererText(), "text", 0);
			_pLangView.Model = _store;
		
			Frame pLangframe = new Frame ();
			pLangframe.Label = GettextCatalog.GetString ("Available code generators");
			pLangframe.Child = scrolledWindow;

			_pLanguages.Title = GettextCatalog.GetString ("Select a model processor");
			_pLanguages.NextClicked += VerifyLanguage;
			_pLanguages.AppendItem ("", pLangframe, "");
		}
		
		private void CreateRequiredValues ()
		{
			_parameters = new StringDictionary ();
			if (_table != null)
			{
				_viewport.Remove (_table);
			}
			// creates the expert system
			ES.Environment environment = new ES.Environment (_parameters);
			environment.InputModel = _inputModel;
			ConstructorInfo ci = _generatorType.GetConstructor (
				new System.Type[] { typeof (ES.Environment) } );
			_expert = (ES.Expert) ci.Invoke (new object[] { environment });
			
			// gets the parameter descriptors and asks for their values.
			_descriptors = _expert.Parameters;
			_entries = new Gtk.Entry [_descriptors.Length];
			_table = new Table ( (uint)_descriptors.Length, 3, false);
			
			ES.ParameterDescriptor pd;
			string required;
			Gtk.Entry entry;
			for(uint i = 0; i < _descriptors.Length; i ++)
			{
				pd = _descriptors [i];
				_table.Attach (new Label (pd.Name), 0, 1, i, i + 1);
				entry = new Gtk.Entry ();
				entry.Changed += new EventHandler (EntryChangedHandler);
				entry.FocusInEvent += new FocusInEventHandler (EntryFocusHandler);
				_entries[i] = entry;
				_table.Attach (entry, 1, 2, i, i + 1);
				required = (pd.IsRequired ? "required" : "");
				_table.Attach (new Label (required), 2, 3, i, i + 1);
				_viewportDocumentation.Text = pd.Documentation;
			}
			_viewport.Add (_table);		
			ShowAll ();
		}
		
		private void DruidCancel (object sender, EventArgs e)
		{
			Hide ();
		}
		
		private void EntryChangedHandler (object sender, EventArgs args)
		{
			Gtk.Entry entry = (Gtk.Entry) sender;
			_parameters [_descriptors [IndexOf (sender)].Name] = entry.Text;
		}
		
		private void EntryFocusHandler (object sender, FocusInEventArgs args)
		{
			_viewportDocumentation.Text = _descriptors [IndexOf (sender)].Documentation;
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

		private void GenerateCode (object o, FinishClickedArgs args)
		{
			string msg = null;
			try
			{
				_expert.AssignParameters ();
				_expert.Process ();
			}
			catch (System.Reflection.TargetInvocationException tie)
			{
				msg = tie.InnerException.Message;
				Console.WriteLine (msg);
				Console.WriteLine (tie.InnerException.StackTrace);
			}
			catch (Exception e)
			{
				msg = e.Message;
				Console.WriteLine (e.GetType().FullName);
				Console.WriteLine (msg);
				Console.WriteLine (e.StackTrace);
			}
			if (msg != null)
			{
				ErrorMessage (MessageType.Error,
					GettextCatalog.GetString ("Your code couldn't be generated:\n\n")+
					msg+
					GettextCatalog.GetString ("\n\nTry again."));
			}
			else
			{
				ErrorMessage (MessageType.Info, GettextCatalog.GetString ("Code generation done."));
			}
			Hide ();
		}
		
		private int IndexOf (object entry)
		{
			return Array.IndexOf (_entries, entry);
		}
		
		private bool LookForCodeGenerators()
		{
			_dllsFound = true;
			string codeBase = Assembly.GetExecutingAssembly ().CodeBase;
			if (codeBase.StartsWith ("file://"))
			{
				codeBase = codeBase.Substring (7);
			}
			codeBase = System.IO.Path.GetDirectoryName (codeBase);
			try
			{
				string generatorsDir = System.IO.Path.Combine (codeBase, "generators/");
				Assembly pluginDll;
				object[] attribs;
				_generators = new ArrayList();
				_store.Clear ();
				if (Directory.GetFiles (generatorsDir, "*.dll").Length == 0)
				{
					throw new Exception (GettextCatalog.GetString ("No dlls found"));
				}
				foreach (string file in Directory.GetFiles (generatorsDir, "*.dll"))
				{
					pluginDll = Assembly.LoadFile (file);
					foreach (Type type in pluginDll.GetTypes ())
					{
						if (typeof (ES.Expert).IsAssignableFrom (type))
						{
							attribs = type.GetCustomAttributes (typeof (ExpertCoder.ExpertSystem.ExpertSystemInformationAttribute), false);
							if (attribs.Length > 0)
							{
								ES.ExpertSystemInformationAttribute info = (ES.ExpertSystemInformationAttribute)attribs[0];
								if (info.IsIntendedForFinalUser)
								{
									_generators.Add (type);
									_store.AppendValues (info.Name + " - " + info.Purpose);
								}
							}
							else
							{
								// expert systems without documentation are treated
								// as intended for final users (yeah, that's ironic!)
								_generators.Add (type);
								_store.AppendValues (type.FullName);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				_dllsFound = false;
				ErrorMessage (MessageType.Error, 
				GettextCatalog.GetString (""+
				"You don't have any code-generation assembly.\n\n"+
				"Remember that you must create a folder called 'generators/' where all "+
				"the assemblies must exist, try again."));
			}
			return _dllsFound;
		}
		
		private void VerifyLanguage (object o, NextClickedArgs args)
		{
			TreeIter iter;
			TreeModel model;
			int index = -1;
			
			if (_pLangView.Selection.GetSelected (out model, out iter))
			{
				TreePath path = model.GetPath (iter);
				index = path.Indices [0];
			}
			if (index <= -1)
			{
				ErrorMessage (
					MessageType.Error,
					GettextCatalog.GetString  ("Please select the programming language for generating your code")
					);
				_druid.Page = _start;
			}
			else
			{
				_generatorType = (Type) _generators [index];
				CreateRequiredValues ();
			}
		}
		
		private void VerifyWrittenValues (object o, NextClickedArgs args)
		{
			foreach (Gtk.Entry entry in _entries)
			{
				if (entry.Text.Equals (""))
				{
					ErrorMessage (
						MessageType.Error,
						GettextCatalog.GetString  ("Please fill the required value")
						);
					entry.GrabFocus ();
					_druid.Page = _pLanguages;
					break;
				}
			}
		}
		
		private ES.ParameterDescriptor[] _descriptors;
		private Druid _druid;
		private bool _dllsFound = true;
		private Gtk.Entry[] _entries;
		private ES.Expert _expert;
		private DruidPageEdge _finish;
		private Type _generatorType;
		private ArrayList _generators;
		private IList _inputModel;
		private StringDictionary _parameters;
		private DruidPageStandard _parametersDruid;
		private DruidPageStandard _pLanguages;
		private TreeView _pLangView;  
		private DruidPageEdge _start;
		private TreeStore _store;
		private Table _table = null;
		private Viewport _viewport;
		private Label _viewportDocumentation;
	}
}
