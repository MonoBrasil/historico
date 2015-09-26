/*
MonoUML.DI - A library for handling Diagram Interchange elements
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

namespace MonoUML.DI
{
	public class Property
	{
		internal Property() {}
		
		public Property(string key, string @value)
		{
			_key = key;
			_value = @value;
		}
		
		public string Key
		{
			get { return _key; }
			set { _key = value; }
		}

		public string Value
		{
			get { return _value; }
			set { _value = value; }
		}
		
		public override string ToString()
		{
			return "(" + (_key==null?"<<null>>":_key) 
				+ "," + (_value==null?"<<null>>":_value) + ")";
		}
		
		private string _key;
		private string _value;
	}
}