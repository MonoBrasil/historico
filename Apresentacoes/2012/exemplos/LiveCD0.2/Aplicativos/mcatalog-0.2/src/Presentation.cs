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
using System.Reflection;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

using Gecko;

public class Presentation: WebControl
{
	private string template;
	private string catalog;
	private Item item;

	public Presentation (): base ()
	{
		WebControl.SetProfilePath (Conf.HomeDir, "mCatalog");
		Visible = true;
	}

	public void Init ()
	{
		string mime_type = "text/html";
		base.OpenStream ("file:///", mime_type);
		base.AppendData ("<html><body><div style=\"text-align: center;\"><table style=\"width: 100%; height: 100%;\" border=\"0\"><tbody><tr align=\"center\"><td><img src=\""+Defines.IMAGE_DATADIR+"/logo.png\"></td></tr></tbody></table></div></body></html>");
		base.CloseStream ();
	}

	public void Load (string catalog, Item item)
	{
		this.catalog = catalog;
		this.template = "template_"+catalog+".html";
		this.item = item;

		string htmlString = CreateHtmlString();
		
		string mime_type = "text/html";
		base.OpenStream ("file:///", mime_type);
		base.AppendData (htmlString);
		base.CloseStream ();
	}

	private string CreateHtmlString ()
	{
		string htmlString = null;

		Assembly thisAssembly = Assembly.GetEntryAssembly ();
		Stream stream = thisAssembly.GetManifestResourceStream(template);
		if (stream != null) {
			StreamReader sr = new StreamReader (stream);
			template = sr.ReadToEnd();
			htmlString = FillTemplate (template);
		}
		else {
			htmlString = "<html><body><h1>ERROR:</h1><br><h3>No existe la plantilla</h3></body><html>";
		}

		return htmlString;
	}

	private string FillTemplate (string template)
	{
		string result = template;
		string pattern = @"##\w+##";

		foreach (Match m in Regex.Matches(template, pattern))
		{
			StringBuilder match = new StringBuilder (m.ToString());
			match.Replace ("##", "");
			string s = match.ToString();
			
			StringBuilder content;
			object aux;

			switch (s) {
				case "catalog_logo":
					content = new StringBuilder (Defines.IMAGE_DATADIR);
					content.Append ("/template_");
					content.Append (this.catalog);
					content.Append (".png");
				break;
				case "image": 
					content = new StringBuilder (item.ImagePath);
				break;
				case "rating":
					int rat = Int32.Parse (item.Columns[s].ToString());
					content = new StringBuilder ("");
					for (int i=0; i<5; i++) {
						if (i<rat) {
							content.Append ("<img src=\""+Defines.IMAGE_DATADIR+"/bigstar.png\">");
						}
						else {
							content.Append ("<img src=\""+Defines.IMAGE_DATADIR+"/littlestar.png\">");
						}
					}
					break;
				case "original_title":
					aux = item.Columns[s];
					if (aux != null && !aux.ToString().Equals("")) {
						content = new StringBuilder ("("+aux.ToString()+")");
					}
					else {
						content = new StringBuilder ("");
					}
					break;
				default:
					aux = item.Columns[s];
					if (aux != null) {
						content = new StringBuilder (aux.ToString());
					}
					else {
						content = new StringBuilder ("");
					}
					break;
			}
			content = content.Replace ("\n", "<br>");
			string escaped = Regex.Escape (content.ToString());
		
			result = Regex.Replace (result, m.ToString(), escaped);
		}
		
		return result;
	}
}
