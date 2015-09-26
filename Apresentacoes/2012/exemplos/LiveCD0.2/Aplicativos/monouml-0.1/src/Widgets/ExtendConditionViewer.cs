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
	public class ExtendConditionViewer : SingleObjectViewer
	{
		public ExtendConditionViewer(IBroadcaster hub)
			: base(hub, GettextCatalog.GetString ("Extend condition:")) {}
		
		protected override object ElementValue
		{
			get { return _extend.Condition; }
		}

		protected override void Clear()
		{
			_extend.Condition = null;
			_hub.BroadcastElementChange(_extend);
		}

		protected override void Edit()
		{
			if(_extend.Condition == null)
			{
				_extend.Condition = UML.Create.Constraint();
				UML.OpaqueExpression spec = UML.Create.OpaqueExpression();
				spec.Language = "OCL";
				spec.Body = "true";
				_extend.Condition.Specification = spec;
			}
			_hub.BroadcastElementChange(_extend);
			_hub.BroadcastElementSelection(_extend.Condition);
		}

		public void ShowConditionFor(UML.Extend extend)
		{
			_extend = extend;
			string representation = null;
			if(extend.Condition != null)
			{
				if(extend.Condition.Specification != null)
				{
					representation = extend.Condition.Specification.ToString();
				}
				else
				{
					representation = GettextCatalog.GetString ("<<A constraint>>");
				}
			}
			base.SetValue(representation);
		}
		
		private UML.Extend _extend;
	}
}
