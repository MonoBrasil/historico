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
	public class StandardProperty
	{
		private StandardProperty() {}

		public static readonly string BackgroundColor = "BackgroundColor";
		public static readonly string FontColor = "FontColor";
		public static readonly string FontFamily = "FontFamily";
		public static readonly string FontSize = "FontSize";
		public static readonly string ForegroundColor = "ForegroundColor";
		public static readonly string LineStyle = "LineStyle";
		public static readonly string LineThickness = "LineThickness";
		public static readonly string Translucent = "Translucent";
	}
}