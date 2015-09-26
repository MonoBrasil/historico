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
using ExpertCoder.Xmi2;
using System.Collections;
using System.Reflection;

namespace MonoUML.ReverseEngineering
{

	public enum AssemblyImporterLevel
	{
		AssemblyImport, //Searchs ONLY within all the imported assemblies
		FullImport//Searchs within all the imported assemblies and its relations
	}

	public class AssemblyImporter
	{
		public AssemblyImporter (AssemblyImporterLevel level) : this ()
		{
			_level = level;
		}
	
		public AssemblyImporter ()
		{
			_level = AssemblyImporterLevel.AssemblyImport;
			 _assemblies = new ArrayList ();
			 _elements = new Hashtable ();
			 _packages = new Hashtable ();
			 _xmiElements = new ArrayList ();
			_importClasses = true;
			_importDelegates = true;
			_importEnumerations = true;
			_importEvents = true;
			_importInterfaces = true;
			_importPublicFields = true;
			_importPublicMethods = true;
			_importPrivateFields = true;
			_importPrivateMethods = true;
			_importProtectedFields = true;
			_importProtectedMethods = true;
			_importStructs = true;
		}
		
		public ArrayList Assemblies
		{
			get
			{
				return _assemblies;
			}
		}

		public Hashtable Elements
		{
			get
			{
				return _elements;
			}
		}
		
		public bool ImportClasses 
		{
			get
			{
				return _importClasses;
			}
			set
			{
				_importClasses = value;
			}
		}
		
		public AssemblyImporterLevel ImportingLevel
		{
			get
			{
				return _level;
			}
			set
			{
				_level = value;
			}
		}

		public bool ImportDelegates
		{
			get
			{
				return _importDelegates;
			}
			set
			{
				_importDelegates = value;
			}
		}

		public bool ImportEnumerations
		{
			get
			{
				return _importEnumerations;
			}
			set
			{
				_importEnumerations = value;
			}
		}

		public bool ImportEvents
		{
			get
			{
				return _importEvents;
			}
			set
			{
				_importEvents = value;
			}
		}

		public bool ImportInterfaces
		{
			get
			{
				return _importInterfaces;
			}
			set
			{
				_importInterfaces = value;
			}
		}
		
		public bool ImportPrivateFields
		{
			get
			{
				return _importPrivateFields;
			}
			set
			{
				_importPrivateFields = value;
			}
		}
		
		public bool ImportPrivateMethods
		{
			get
			{
				return _importPrivateMethods;
			}
			set
			{
				_importPrivateMethods = value;
			}
		}
		
		public bool ImportProtectedFields
		{
			get
			{
				return _importProtectedFields;
			}
			set
			{
				_importProtectedFields = value;
			}
		}
		
		public bool ImportProtectedMethods
		{
			get
			{
				return _importProtectedMethods;
			}
			set
			{
				_importProtectedMethods = value;
			}
		}
		
		public bool ImportPublicFields
		{
			get
			{
				return _importPublicFields;
			}
			set
			{
				_importPublicFields = value;
			}
		}
		
		public bool ImportPublicMethods
		{
			get
			{
				return _importPublicMethods;
			}
			set
			{
				_importPublicMethods = value;
			}
		}

		public bool ImportStructs
		{
			get
			{
				return _importStructs;
			}
			set
			{
				_importStructs = value;
			}
		}

		public Hashtable Packages
		{
			get
			{
				return _packages;
			}
		}
		
		public bool Reading 
		{
			get
			{
				return _reading;
			}
		}
		
		public ArrayList XmiElements
		{
			get
			{
				return _xmiElements;
			}
		}

		public bool AddAssembly (string filename)
		{
			bool loaded = true;
			try
			{
				Assembly asmb = Assembly.LoadFrom (filename);
				if (_assemblies.IndexOf (asmb) == -1)
				{
					_assemblies.Add (asmb);
				}
				else
				{
					System.Console.WriteLine ("ERROR! Already added! "+filename);
				}
			}
			catch (System.Exception ex)
			{
				loaded = false;
			}
			return loaded;
		}
		
		public bool DeleteAssembly (string filename)
		{
			bool deleted = true;
			try
			{
				Assembly asmb = Assembly.LoadFrom (filename);
				if (_assemblies.IndexOf (asmb) != -1)
				{
					_assemblies.Remove (asmb);
				}
				else
				{
					System.Console.WriteLine ("ERROR! Couldn't delete! "+filename);
				}
			}
			catch (System.Exception ex)
			{
				deleted = false;
			}
			return deleted;
		}
		
		public void ReadAssemblies ()
		{
			lock (this)
			{
				_reading = true;
				if (_assemblies.Count > 0)
				{
					foreach (System.Reflection.Assembly assmb in _assemblies)
					{
						System.Type []assmb_types = assmb.GetTypes ();		
						foreach (System.Type type in assmb_types)
						{ 
							BeginWithType (type);						
						}
					}
				}
				else
				{
					System.Console.WriteLine ("ERROR. Unable to read assemblies, not loaded.");
				}
				_reading = false;
			}
		}
		
		public void WriteXMI (string filename)
		{
			System.Console.WriteLine ("Serialization: STARTING");
			SerializationDriver ser = new SerializationDriver ();
			ser.AddSerializer (new ExpertCoder.Uml2.Serialization.Serializer ());
			ser.Serialize (_xmiElements, filename);
			System.Console.WriteLine ("Serialization: DONE");
		}
		
		private void BeginWithType (System.Type type)
		{
			if (type.IsEnum)
			{
				if (Elements [type.FullName] == null && ImportEnumerations)
				{
					AssemblyEnumeration enm = new AssemblyEnumeration (this, type);
					enm.Begin (); 
				}
			}
			else if (type.IsInterface)
			{
				if (Elements [type.FullName] == null && ImportInterfaces)
				{
					AssemblyInterface intrface = new AssemblyInterface (this, type);
					intrface.Begin ();
				}
			}
			else if (type.IsClass)
			{	
				if (Elements [type.FullName] == null && ImportClasses)
				{
					AssemblyClass cls = new AssemblyClass (this, type);
					cls.Begin ();
				}
			}
			else if (type.IsPrimitive)
			{
				if (Elements [type.FullName] == null)
				{
					AssemblyDataType dataType = new AssemblyDataType (this, type);
					dataType.Begin ();
				}
			}
			else if (type.IsValueType)
			{
				if (Elements [type.FullName] == null)
				{
					if (type.GetMembers (AssemblyHelper.BINDING_FLAGS).Length == 1)
					{
						AssemblyDataType dataType = new AssemblyDataType (this, type);
						dataType.Begin ();
					}
					else if (ImportStructs)
					{
						AssemblyStruct structure = new AssemblyStruct (this, type);
						structure.Begin ();
					}
				}
			}
		}
		
		private ArrayList _assemblies;
		private Hashtable _elements;
		private bool _importClasses;
		private bool _importDelegates;
		private bool _importEnumerations;
		private bool _importEvents;
		private bool _importInterfaces;
		private bool _importPublicFields;
		private bool _importPublicMethods;
		private bool _importPrivateFields;
		private bool _importPrivateMethods;
		private bool _importProtectedFields;
		private bool _importProtectedMethods;
		private bool _importStructs;
		private Hashtable _packages;
		private bool _reading = false;
		private AssemblyImporterLevel _level;
		private ArrayList _xmiElements;
	}
}
