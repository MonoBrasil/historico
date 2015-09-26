/*
MonoUML.Widgets - A library for representing the Widget elements
Copyright (C) 2004  Mario Carrión
Copyright (C) 2004  Rodolfo Campero

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
using UML = ExpertCoder.Uml2;
using MonoUML.DI.Uml2Bridge;
using MonoUML.I18n;
using System;
using System.Reflection;

namespace MonoUML.Widgets
{
	public sealed class Helper
	{
		private Helper() {}
		
		private static void CloseNewElementNameModal(object sender, EventArgs args)
		{
			_dialog.Hide();
		}
		
		public static UML.Element GetSemanticElement(
			MonoUML.DI.GraphElement graphElement)
		{
			if(graphElement == null) return null;
			Uml2SemanticModelBridge bridge = 
				graphElement.SemanticModel as Uml2SemanticModelBridge;
			return bridge==null ? null : bridge.Element; 
		}
		
		// uses reflection to call the corresponding method in the "Create" class.
		public static UML.Element CreateUmlElement(string elementType)
		{
			Type create = typeof(UML.Create);
			object newElement = create.InvokeMember(
				elementType,
				BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,
				null,
				null,
				null);
			UML.NamedElement ne = newElement as UML.NamedElement;
			if(ne != null)
			{
				_dialog = new Gtk.MessageDialog(
					null, Gtk.DialogFlags.Modal, Gtk.MessageType.Question, 
					Gtk.ButtonsType.Ok, GettextCatalog.GetString ("New element name:"));
				Gtk.Entry elementName = new Gtk.Entry();
				elementName.Activated += new EventHandler(CloseNewElementNameModal);
				_dialog.VBox.Add(elementName);
				elementName.Show();
				_dialog.Run();
				ne.Name = String.Format(elementName.Text, elementType);
				_dialog.Destroy();
				_dialog = null;
			}
			return (UML.Element)newElement;
		}
	
		private static Gtk.MessageDialog _dialog;
	}
}
