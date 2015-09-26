/*
 * Copyright (C) 2004 Cesar Garcia Tapia <tapia@mcatalog.net>
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
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.IO;

using Gtk;
using Gdk;

public class Item
{
	private ListDictionary columns;
	private string cover;
	
	private string coverPixbuf;
	private string coverSmallPixbuf;
	private string coverMediumPixbuf;
	private string coverLargePixbuf;
	
	private string table;
	private bool selected;

	public Item (string table, ListDictionary columns): this (table, columns, true)
	{
	}
	
	public Item (string table, ListDictionary columns, bool isNew)
	{
		this.columns = columns;
		this.table = table;
		bool hasCover;
		
		if (columns["image"] != null) {
			cover = columns["image"].ToString();
			hasCover = true;
		}
		else {
			cover = Defines.IMAGE_DATADIR + "/shelfbgnp.png";
			hasCover = false;
		}

		if (hasCover) {
			try {
				coverPixbuf  = cover;
			}
			catch {
				coverPixbuf = null;
			}
		}
		else {
			coverPixbuf = null;
		}
		
		if (isNew) {
			cover = SetCache (ImageSize.Large);
			columns["image"] = cover;
			SetCache (ImageSize.Medium);
			SetCache (ImageSize.Small);
		}
		
		coverLargePixbuf = GetLargePixbuf ();
		coverMediumPixbuf = GetMediumPixbuf ();
		coverSmallPixbuf = GetSmallPixbuf ();
	}

	public bool IsSelected {
		get {
			return selected;
		}
		set {
			selected = value;
		}
	}
	
	public ListDictionary Columns {
		get {
			return columns;
		}
	}

	public int Id {
		get {
			if (columns["id"] != null) {
				return Int32.Parse (columns["id"].ToString());
			}
			else {
				return -1;
			}
		}
	}

	public string Title {
		get {
			if (columns["title"] != null) {
				return columns["title"].ToString();
			}
			else {
				return "";
			}
		}
	}
	
	public string Table {
		get {
			return this.table;
		}
	}

	public string ImagePath {
		get {
			return cover;
		}
	}

	public string CoverPixbuf {
		get {
			return coverPixbuf;
		}
	}

	private string GetSmallPixbuf () {
		string pixbuf;
		if (coverPixbuf != null) {
			pixbuf = GetCache (ImageSize.Small);
			if (pixbuf == null) {
				SetCache (ImageSize.Small);
			}
			return pixbuf;
		}
		else {
			return null;
		}
	}

	private string GetMediumPixbuf () {
		string  pixbuf;
		if (coverPixbuf != null) {
			pixbuf = GetCache (ImageSize.Medium);
			if (pixbuf == null) {
				SetCache (ImageSize.Medium);
			}
			return pixbuf;
		}
		else {
			return null;
		}
	}

	private string GetLargePixbuf () {
		if (coverPixbuf != null) {
			return coverPixbuf;
		}
		else {
			return null;
		}
	}

	public string SmallCover {
		get {
			return coverSmallPixbuf; 
		}
	}

	public string MediumCover {
		get {
			return coverMediumPixbuf;
		}
	}

	public string LargeCover {
		get {
			return coverLargePixbuf;
		}
	}

	private string GetCache (ImageSize size)
	{
		string file;

		StringBuilder cacheDir = new StringBuilder (Conf.HomeDir);
		cacheDir = cacheDir.Append ("/cache");
		switch (size) {
			case ImageSize.Small:
				cacheDir = cacheDir.Append ("/small/");
				cacheDir = cacheDir.Append (table);
				cacheDir = cacheDir.Append (Id);
				cacheDir = cacheDir.Append (".png");
				break;
			case ImageSize.Medium:
				cacheDir = cacheDir.Append ("/medium/");
				cacheDir = cacheDir.Append (table);
				cacheDir = cacheDir.Append (Id);
				cacheDir = cacheDir.Append (".png");
				break;
			case ImageSize.Large:
				cacheDir = cacheDir.Append ("/large/");
				cacheDir = cacheDir.Append (table);
				cacheDir = cacheDir.Append (Id);
				cacheDir = cacheDir.Append (".png");
				break;
			default:
				return null;
		}
		file = cacheDir.ToString ();
		if (File.Exists (file)) {
			return file;
		}
		else {
			return null;
		}
	}

	private string SetCache (ImageSize size)
	{
		string fileAux = null;
		StringBuilder cacheDir;
		Gdk.Pixbuf pixbuf;

		if (this.coverPixbuf != null) {
			pixbuf = new Pixbuf (coverPixbuf);
			switch (size) {
				case ImageSize.Small:
					if (pixbuf.Height > pixbuf.Width) {
						int x = 50*pixbuf.Width/pixbuf.Height;
						pixbuf = pixbuf.ScaleSimple (x, 50, InterpType.Hyper);
					}
					else {
						int x = 50*pixbuf.Height/pixbuf.Width;
						pixbuf = pixbuf.ScaleSimple (50, x, InterpType.Hyper);
					}
					cacheDir = new StringBuilder (Conf.HomeDir);
					cacheDir = cacheDir.Append ("/cache");
					cacheDir = cacheDir.Append ("/small/");
					cacheDir = cacheDir.Append (System.IO.Path.GetFileName(cover));
					cacheDir = cacheDir.Append (".png");
					fileAux = cacheDir.ToString();
					pixbuf.Save (fileAux, "png");
					break;
				case ImageSize.Medium:
					if (pixbuf.Height > pixbuf.Width) {
						int x = 100*pixbuf.Width/pixbuf.Height;
						pixbuf = pixbuf.ScaleSimple (x, 100, InterpType.Hyper);
					}
					else {
						int x = 100*pixbuf.Height/pixbuf.Width;
						pixbuf = pixbuf.ScaleSimple (100, x, InterpType.Hyper);
					}
					cacheDir = new StringBuilder (Conf.HomeDir);
					cacheDir = cacheDir.Append ("/cache");
					cacheDir = cacheDir.Append ("/medium/");
					cacheDir = cacheDir.Append (System.IO.Path.GetFileName(cover));
					cacheDir = cacheDir.Append (".png");
					fileAux = cacheDir.ToString();
					pixbuf.Save (fileAux, "png");

					break;
				case ImageSize.Large:
					cacheDir = new StringBuilder (Conf.HomeDir);
					cacheDir = cacheDir.Append ("/cache");
					cacheDir = cacheDir.Append ("/large/");
					cacheDir = cacheDir.Append (System.IO.Path.GetFileName(cover));
					cacheDir = cacheDir.Append (".png");
					fileAux = cacheDir.ToString();
					pixbuf.Save (fileAux, "png");
					break;
			}		
		}
		else {
			fileAux = null;
		}

		return fileAux;
	}

	public static void RemoveCache (string table, int id)
	{
		try {
			StringBuilder cacheDir = new StringBuilder (300);
			cacheDir = cacheDir.Append (Conf.HomeDir);
			
			StringBuilder file = new StringBuilder (100);
			file = file.Append (table);
			file = file.Append (id);
			file = file.Append (".png");

			StringBuilder aux;
			
			aux = cacheDir.Append ("/cache/small/");
			aux = aux.Append (file.ToString());
			File.Delete (aux.ToString());

			aux = cacheDir.Append ("/cache/medium/");
			aux = aux.Append (file.ToString());
			File.Delete (aux.ToString());

			aux = cacheDir.Append ("/cache/large/");
			aux = aux.Append (file.ToString());
			File.Delete (aux.ToString());
		}
		catch {}
	}
}
