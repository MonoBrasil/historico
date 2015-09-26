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
using Uml2 = ExpertCoder.Uml2;

namespace MonoUML.ReverseEngineering
{
	internal class AssemblyPackage
	{
		public AssemblyPackage (AssemblyImporter importer, System.Type systemType)
		{
			_importer = importer;
			_systemType = systemType;
			_namespace = systemType.Namespace; 
		}
		
		public void Begin ()
		{
			if (!_namespace.Equals (""))
			{
				char[] sep = {'.'};
				string[] subNs = _namespace.Split (sep);
				string before = "";
				_lastPkg = null;
				
				if (subNs.Length > 0)
				{
					foreach (string str in subNs)
					{
						if (!before.Equals (""))
						{
							before += "."+str;
						}
						else
						{
							before = str;
						}
						Uml2.Package pkg = CreatePackage (before, str);
						if (_lastPkg != null)
						{
							try
							{ 
								_lastPkg.NestedPackage.Add (pkg);
							}
							catch (System.Exception ex) { }
						}
						_lastPkg = pkg;
					}
				}
				else
				{
					_lastPkg = CreatePackage (_namespace, _namespace);
				}
			}
		}
		
		public Uml2.Package LastPackage
		{
			get
			{
				return _lastPkg;
			}
		}

		private Uml2.Package CreatePackage (string ns, string name)
		{
			Uml2.Package pkg = (Uml2.Package) _importer.Packages [ns];
			if (pkg == null)
			{
				//System.Console.WriteLine ("Package added: "+ns);
				pkg = Uml2.Create.Package ();
				pkg.Name = name;
				try
				{
					_importer.Packages.Add (ns, pkg);
					_importer.XmiElements.Add (pkg);
				}
				catch (System.Exception ex) {}
			}
			return pkg;
		}
		
		private AssemblyImporter _importer;
		private Uml2.Package _lastPkg;
		private string _namespace = "";
		private System.Type _systemType;
	}
}