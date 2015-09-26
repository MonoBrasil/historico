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
using System;
using UML = ExpertCoder.Uml2;

namespace MonoUML.Widgets
{
	public class AnnotatedElementViewer : MultipleObjectViewer
	{
		public AnnotatedElementViewer(IBroadcaster hub)
			: base(hub, "Annotated elements:") {}

		protected override void Add()
		{
			ElementChooserDialog chooser = new ElementChooserDialog(typeof(UML.Element));
			if(chooser.Run() == Gtk.ResponseType.Accept.value__)
			{
				if(!_comment.AnnotatedElement.Contains(chooser.SelectedObject))
				{
					_comment.AnnotatedElement.Add(chooser.SelectedObject);
					_hub.BroadcastElementChange(_comment);
				}
			}
		}

		protected override void Delete(int index)
		{
			_comment.AnnotatedElement.RemoveAt(index);
			_hub.BroadcastElementChange(_comment);
		}

		protected override void Edit(int index)
		{
			_hub.BroadcastElementSelection(_comment.AnnotatedElement[index]);
		}

		public void ShowAnnotatedElementsFor(UML.Comment comment)
		{
			_comment = comment;
			UML.NamedElement named;
			string[] annotatedList = new string[comment.AnnotatedElement.Count];
			object current;
			for(int i = 0; i < comment.AnnotatedElement.Count; i ++)
			{
				current = comment.AnnotatedElement[i];
				named = current as UML.NamedElement;
				annotatedList[i] = (named == null ? 
					current.GetType().Name.Substring(6) : named.QualifiedName);
			}
			base.ShowList(annotatedList);
		}
		
		private UML.Comment _comment;
	}
}
