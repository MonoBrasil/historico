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

// THANKS TO FELIX ORTEGA FOR HELPING WITH THE REGULAR EXPRESIONS

using System;
using System.Collections;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

[SearchEngineAttribute("films")]
public class SearchEngineIMDB: SearchEngine
{
	public SearchEngineIMDB ()
	{
	}

	public override string Name
	{
		get {
			return "IMDB";
		}
	}

	public override ArrayList Query (string category, string query)
	{
		WebRequest req;
		WebResponse res;
		StreamReader sr;

		string html;

		req = WebRequest.Create("http://imdb.com/find?q=" + query + ";s=tt");
		res = req.GetResponse();

		sr = new StreamReader(res.GetResponseStream());
		html = sr.ReadToEnd();
		StringBuilder match = new StringBuilder (html);
		match.Replace ("</a>", "</a>\n");
		match.Replace ("&#34;", "\"");
		match.Replace ("&#38;", "&");
		html = match.ToString();

		sr.Close();
		res.Close ();

		Regex r = new Regex("<.*title/tt([^/]*)/\"[^>]*>([^<]*).*>");
		MatchCollection mc = r.Matches(html);

		if( mc.Count > 0)
		{
			int i = 0;
			string code;
			ArrayList list = new ArrayList ();
			foreach(Match m in mc)
			{
				code = m.Result("$1");
				SearchResults searchResults = GetInfo (code);
				list.Add (searchResults);
				i++;

			}
			return list;
		}
		else
			return null;
	}

	public SearchResults GetInfo (string filmCode)
	{
		WebRequest req;
		WebResponse res;
		StreamReader sr;

		Regex regex;
		MatchCollection matchCollection;

		string html;

		SearchResultsFilm searchResults = new SearchResultsFilm ();

		if (ValidCommand(filmCode)) {
			req = WebRequest.Create("http://imdb.com/title/tt" + filmCode + "/");
			res = req.GetResponse();

			sr = new StreamReader(res.GetResponseStream());

			html = sr.ReadToEnd();
			html = html.Replace ("\n", "");
			sr.Close();
			res.Close();

			// **** IMAGE ****
			regex = new Regex ("<img border=\"0\" alt=\"cover\" src=\"(?<image>.*?)\"");
			matchCollection = regex.Matches(html);

			foreach(Match m in matchCollection)
			{
				searchResults.Image = m.Result("${image}");
			}

			// **** TITLE AND YEAR ****
			regex = new Regex(@"<title>(?<title>.*?) \((?<year>[^\)]*)\)</title>");
			matchCollection = regex.Matches(html);

			foreach(Match m in matchCollection)
			{
				searchResults.Name = m.Result("${title}");
				searchResults.Date = m.Result("${year}");
			}

			// **** DIRECTOR ****
			regex = new Regex (@"Directed by</b><br>(?<directors>.*?)<br><br>", RegexOptions.ExplicitCapture);
			matchCollection = regex.Matches(html);

			foreach(Match m in matchCollection)
			{
				string aux = m.Result("${directors}");
				regex = new Regex ("<a href=(.*?)>(?<director>.*?)</a>");
				MatchCollection mc = regex.Matches (aux);

				searchResults.Directors = new string [mc.Count];

				int i = 0;
				foreach (Match m1 in mc) {
					string director = m1.Result("${director}");
					searchResults.Directors[i++] = director;
				}

			}

			// **** COMMENTS **** 
			regex = new Regex(@"Plot Outline:</b> (?<comment>.*?)<a");
			matchCollection = regex.Matches(html);

			foreach(Match m in matchCollection)
			{
				searchResults.Comments = m.Result("${comment}");
			}

			// **** RATING **** 
			regex = new Regex(@"User Rating:(.*)<b>(?<rating>.*)/10</b> ((.*) votes)");
			matchCollection = regex.Matches(html);

			foreach(Match m in matchCollection)
			{
				string rat = m.Result ("${rating}");
				double rating = Double.Parse (rat, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
				int rating1 = (int)Math.Round (rating/2);
				searchResults.Rating = rating1;
			}

			// **** CAST ****
			regex = new Regex (@"Complete credited cast:(?<cast>.*?)(more)");
			matchCollection = regex.Matches(html);

			if (matchCollection.Count == 0) {
				regex = new Regex (@"Cast overview, first billed only:(?<cast>.*?)(more)");
				matchCollection = regex.Matches(html);
			}

			foreach(Match m in matchCollection)
			{
				string aux = m.Result("${cast}");
				regex = new Regex ("<a href=(.*?)>(?<actor>.*?)</a>");
				MatchCollection mc = regex.Matches (aux);

				searchResults.Starring = new string [mc.Count];

				int i = 0;
				foreach (Match m1 in mc) {
					string actor = m1.Result("${actor}");
					searchResults.Starring [i++] = actor;
				}
			}

			// **** RUNTIME ****
			regex = new Regex (@"Runtime:</b>(?<runtime>.*?) min");
			matchCollection = regex.Matches (html);

			foreach(Match m in matchCollection)
			{
				searchResults.RunningTime = m.Result("${runtime}");
			}	

			// **** COUNTRY ****
			regex = new Regex (@"Country:</b>(?<countries>.*?)<br>");
			matchCollection = regex.Matches(html);

			foreach(Match m in matchCollection)
			{
				string aux = m.Result("${countries}");
				regex = new Regex ("<a href=(.*?)>(?<country>.*?)</a>");
				MatchCollection mc = regex.Matches (aux);

				int i = 0;
				string country = "";
				foreach (Match m1 in mc) {
					country += m1.Result("${country}");
					i++;
					if (i != mc.Count) {
						country +=" / ";
					}
				}
				searchResults.Country = country;
			}

			// **** LANGUAGE ****
			regex = new Regex (@"Language:</b>(?<languages>.*?)<br>");
			matchCollection = regex.Matches(html);

			foreach(Match m in matchCollection)
			{
				string aux = m.Result("${languages}");
				regex = new Regex ("<a href=(.*?)>(?<language>.*?)</a>");
				MatchCollection mc = regex.Matches (aux);

				int i = 0;
				string language ="";
				foreach (Match m1 in mc) {
					language += m1.Result("${language}");
					i++;
					if (i != mc.Count) {
						language +=" / ";
					}
				}
				searchResults.Language = language;
			}
		}

		return searchResults;
	}

	private bool ValidCommand(string cmd)
	{
		try {
			Int32.Parse (cmd);
			return (cmd.Length == 7);
		}
		catch {
			return false;
		}
	}
}
