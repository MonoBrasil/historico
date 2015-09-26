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
	public class ExtendViewer : MultipleObjectViewer
	{
		public ExtendViewer(IBroadcaster hub)
			: base(hub, GettextCatalog.GetString ("Extends:")) {}

		protected override void Add()
		{
			UML.Extend extend = UML.Create.Extend();
			_useCase.Extend.Add(extend);
			_hub.BroadcastElementChange(_useCase);
		}

		protected override void Delete(int index)
		{
			_useCase.Extend.RemoveAt(index);
			_hub.BroadcastElementChange(_useCase);
		}

		protected override void Edit(int index)
		{
			_hub.BroadcastElementSelection(_useCase.Extend[index]);
		}

		public void ShowExtendFor(UML.UseCase element)
		{
			_useCase = element;
			UML.Extend extend;
			string[] extendList = new string[element.Extend.Count];
			for(int i = 0; i < element.Extend.Count; i ++)
			{
				extend = (UML.Extend)element.Extend[i];
				if(extend.ExtendedCase == null)
				{
					extendList[i] = GettextCatalog.GetString ("Extend #") + i;
				}
				else
				{
					extendList[i] = extend.ExtendedCase.QualifiedName;
				}
			}
			base.ShowList(extendList);
		}
		
		private UML.UseCase _useCase;
	}
}
