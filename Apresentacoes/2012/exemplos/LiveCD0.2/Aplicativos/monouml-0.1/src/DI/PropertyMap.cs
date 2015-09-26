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
using System.Collections;

namespace MonoUML.DI
{
	public class PropertyMap : IEnumerable
	{
		public PropertyMap()
		{
			_properties = new Hashtable();
		}
		
		internal Property Add(Property property)
		{
			_properties.Add(property.Key, property);
			return property;			
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return _properties.Values.GetEnumerator();
		}
		
		internal bool Contains(Property property)
		{
			return _properties.ContainsValue(property);
		}
		
		public string this[string key]
		{
			get
			{
				Property property = (Property)_properties[key];
				return property==null ? null : property.Value;
			}
			set
			{
				// a null value removes the property
				if(value==null)
				{
					_properties.Remove(key);
				}
				else
				{
					if(_properties.ContainsKey(key))
					{
						((Property)_properties[key]).Value = value; 
					}
					else
					{
						_properties.Add(key, new Property(key, value));
					}
				}
			}
		}

		private Hashtable _properties; 
	}
}