/* Copyright (C) 2004 Cesar Garcia Tapia <tapia@mcatalog.net>
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public
 * License along with this program; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */

using System;
using System.Collections;

public class Borrower
{
	private int id;
	private string name;
	private ArrayList items;
	
	public Borrower (int id, string name, ArrayList items)
	{
		this.id = id;
		this.name = name;
		this.items = items;
	}
	
	public int Id
	{
		get {
			return this.id;
		}
	}
	
	public string Name {
		get {
			return this.name;
		}
	}
	
	public ArrayList Items
	{
		get {
			return items;
		}
	}
}
