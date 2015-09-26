/*
MonoUML.Widgets - A library for representing the Widget elements
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
	public class IncludeViewer : MultipleObjectViewer
	{
		public IncludeViewer(IBroadcaster hub)
			: base(hub, GettextCatalog.GetString ("Includes:")) {}

		protected override void Add()
		{
			UML.Include include = UML.Create.Include();
			_useCase.Include.Add(include);
			_hub.BroadcastElementChange(_useCase);
		}

		protected override void Delete(int index)
		{
			_useCase.Include.RemoveAt(index);
			_hub.BroadcastElementChange(_useCase);
		}

		protected override void Edit(int index)
		{
			_hub.BroadcastElementSelection(_useCase.Include[index]);
		}

		public void ShowIncludeFor(UML.UseCase element)
		{
			_useCase = element;
			UML.Include include;
			string[] includeList = new string[element.Include.Count];
			for(int i = 0; i < element.Include.Count; i ++)
			{
				include = (UML.Include)element.Include[i];
				if(include.Addition == null)
				{
					includeList[i] = GettextCatalog.GetString ("Include #") + i;
				}
				else
				{
					includeList[i] = include.Addition.QualifiedName;
				}
			}
			base.ShowList(includeList);
		}
		
		private UML.UseCase _useCase;
	}
}
