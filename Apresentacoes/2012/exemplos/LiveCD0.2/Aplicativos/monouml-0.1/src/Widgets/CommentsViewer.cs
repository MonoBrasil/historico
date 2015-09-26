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
using MonoUML.I18n;

namespace MonoUML.Widgets
{
	public class CommentsViewer : MultipleObjectViewer
	{
		public CommentsViewer(IBroadcaster hub)
			: base (hub, GettextCatalog.GetString ("Owned comments:")) {}

		protected override void Add()
		{
			UML.Comment comment = UML.Create.Comment();
			comment.Body = GettextCatalog.GetString ("<<New comment>>");
			_umlElement.OwnedComment.Add(comment);
			_hub.BroadcastElementChange(_umlElement);
		}

		protected override void Delete(int index)
		{
			_umlElement.OwnedComment.RemoveAt(index);
			_hub.BroadcastElementChange(_umlElement);
		}

		protected override void Edit(int index)
		{
			_hub.BroadcastElementSelection(_umlElement.OwnedComment[index]);
		}

		public void ShowCommentsFor(UML.Element element)
		{
			_umlElement = element;
			string[] commentList = new string[element.OwnedComment.Count];
			for(int i = 0; i < element.OwnedComment.Count; i ++)
			{
				commentList[i] = ((UML.Comment)element.OwnedComment[i]).Body;
			}
			base.ShowList(commentList);
		}
		
		private UML.Element _umlElement;
	}
}
