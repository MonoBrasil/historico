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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;

public class Exporter
{
	private Catalog catalog;
	private string template;

	public Exporter (Catalog catalog)
	{
		this.catalog = catalog;
	}

	public string Template
	{
		get {
			return template;
		}
		set {
			template = value;
		}
	}

	public bool Export (string fileName)
	{
		try {
			StreamReader sr = new StreamReader (template);
			string templateString = sr.ReadToEnd();
			string htmlString = FillTemplate (templateString);
			StreamWriter sw = new StreamWriter(fileName);
			sw.Write (htmlString);
			sw.Close();
			return true;
		}
		catch (Exception e) {
			Console.WriteLine (e);
			return false;
		}
	}

	private string FillTemplate (string templateString)
	{
		string result = "";

		string itemStringPattern = @"##ITEM##[\w\W]+##ITEM##";
		string itemString = "";

		foreach (Match m in Regex.Matches(templateString, itemStringPattern))
		{
			StringBuilder match = new StringBuilder (m.ToString());
			match.Replace ("##ITEM##", "");
			itemString = match.ToString();
		}

		string pattern = @"##\w+##";
		string aux;
		string content1 = "";

		foreach (Item item in catalog.ItemCollection) {
			string resultAux = itemString;
			foreach (Match m in Regex.Matches(itemString , pattern))
			{
				StringBuilder match = new StringBuilder (m.ToString());
				match.Replace ("##", "");
				aux = match.ToString();

				if (item.Columns[aux] != null) {
					switch (aux) {
						case "image":
							content1 = item.ImagePath;
						break;
						case "rating":
							int rat = Int32.Parse (item.Columns[aux].ToString());
							string ratingString = "";
							for (int i=0; i<5; i++) {
								if (i<rat) {
									ratingString += "<img src=\""+Defines.IMAGE_DATADIR+"/bigstar.png\">";
								}
								else {
									ratingString += "<img src=\""+Defines.IMAGE_DATADIR+"/littlestar.png\">";
								}
							}
							content1 = ratingString;
						break;
						default:
							content1 = item.Columns[aux].ToString();
						break;
					}
				}

				StringBuilder c = new StringBuilder (content1).Replace ("\n", "<br>");
				content1 = Regex.Escape (c.ToString());

				resultAux = Regex.Replace (resultAux, m.ToString(), content1);
			}
			result += resultAux;
		}

		foreach (Match m in Regex.Matches(templateString, itemStringPattern))
		{
			StringBuilder match = new StringBuilder (m.ToString());
			match.Replace ("##ITEM##", "");
			itemString = match.ToString();

			return Regex.Replace (templateString, m.ToString(), result);
		}

		return null;
	}

}
