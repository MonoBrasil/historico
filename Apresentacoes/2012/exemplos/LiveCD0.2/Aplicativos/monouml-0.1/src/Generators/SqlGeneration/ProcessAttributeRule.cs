/*
MonoUML.Widgets - A library for representing the Widget elements
Copyright (C) 2005  Miguel Huerta

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
using ExpertCoder.Templates;
using ES = ExpertCoder.ExpertSystem;
using TT =  MonoUML.Generators.SqlGeneration.TemplateTree;

namespace MonoUML.Generators.SqlGeneration
{
	public class ProcessAttributeRule: ES.TypeGuardedRule
	{
		public ProcessAttributeRule()
			:base(typeof(UML.Property), typeof(TT.Table))
		{
		}

		public override void Execute(ES.Environment env)
		{
			UML.Property attribute = (UML.Property)env.CurrentInputElement;
			TT.Table templTable = (TT.Table)env.CurrentOutputElement;
			bool isMultivalued = attribute.Upper > 1;

			TT.Column column = new TT.Column(attribute);
			templTable.ColumnsName.Add(column);
			column.ColumnName = attribute.Name;	
			if(isMultivalued)
			{
				column.Type = "array";
			}
			else
			{
				column.Type = attribute.Type.QualifiedName.Replace("::", ".").Split('.')[1];
			}
			column.MoreColumn = ",";
			
			if (attribute.Visibility == UML.VisibilityKind.@private )
			{									
				System.Console.WriteLine("nombre de la columna" + column.ColumnName);
				templTable.PrimaryKeys.Add(column.ColumnName);				
			}
		}
	}
}
