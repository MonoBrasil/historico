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
using System.Net;
using System.Collections;

using System.Web.Services;
using Amazon;

using Mono.Posix;

[SearchEngineAttribute("films")]
[SearchEngineAttribute("books")]
[SearchEngineAttribute("albums")]
public class SearchEngineAmazon: SearchEngine 
{
	public SearchEngineAmazon ()
	{
	}

	public override string Name
	{
		get {
			return "Amazon";
		}
	}

	public override ArrayList Query (string category, string query)
	{
		KeywordRequest kr = new KeywordRequest ();
		kr.keyword = query;
		kr.devtag = "0AERXJ86ENN0GXCN5402";
		kr.page = "1";
		kr.mode = category;
		switch (category) {
			case "films":
				kr.mode = "dvd";
				break;
			case "books":
				kr.mode = "books";
				break;
			case "albums":
				kr.mode = "music";
				break;
		}
		kr.type = "heavy";

		ProductInfo productsInfo = null;
		try {
			AmazonSearchService ass = new AmazonSearchService ();
			productsInfo = ass.KeywordSearchRequest(kr);
		}
		catch (Exception e) {
			return null;
		}

		ArrayList list = new ArrayList ();
		foreach (Details details in  productsInfo.Details) {
			SearchResults searchResults = DetailsToSearchResults (category, details);
			list.Add (searchResults);
		}

		return list;
	}

	private SearchResults DetailsToSearchResults (string category, Details details)
	{
		SearchResults results = null;
		
		switch (category) {
			case "films":
				SearchResultsFilm film_results = new SearchResultsFilm ();
				film_results.Name = details.ProductName;
				film_results.OriginalTitle = "";
				film_results.Rating = 1;
				film_results.Image = details.ImageUrlMedium; 
				film_results.Directors = details.Directors;
				film_results.Starring = details.Starring;
				film_results.Date = details.TheatricalReleaseDate;
				film_results.Genre = "";
				film_results.RunningTime = details.RunningTime;
				film_results.Country = "";
				film_results.Language = "";
				film_results.Manufacturer = details.Manufacturer;
				film_results.Medium = "DVD";
				film_results.Comments = details.ProductDescription;
				
				results = film_results;
				break;
			case "albums":
				SearchResultsAlbum album_results = new SearchResultsAlbum ();
				album_results.Name = details.ProductName;
				album_results.Rating = 1;
				album_results.Image = details.ImageUrlMedium; 
				album_results.Artists = details.Artists; 
				album_results.Label = details.Manufacturer;
				album_results.Date = details.ReleaseDate;
				album_results.Style = "";
				album_results.ASIN = details.Asin;
				if (details.Tracks != null) {
					for (int i = 0; i < details.Tracks.Length; i++) {
						album_results.Tracks[i] = details.Tracks[i].TrackName;
					}
				}
				album_results.Medium = "CDRom"; 
				album_results.Runtime = details.RunningTime; 
				album_results.Comments = details.ProductDescription;

				results = album_results;
				break;
			case "books":
				SearchResultsBook books_results = new SearchResultsBook ();
				books_results.Name = details.ProductName;
				books_results.Rating = 1;
				books_results.Image = details.ImageUrlMedium; 
				books_results.Authors = details.Authors; 
				books_results.Date = details.ReleaseDate;
				books_results.OriginalTitle = "";
				books_results.Genre = "";
				books_results.Pages = details.NumberOfPages;
				books_results.Publisher = details.Manufacturer;
				books_results.ISBN = details.Isbn;
				books_results.Country = "";
				books_results.Language = "";
				books_results.Comments = details.ProductDescription;

				results = books_results;
				break;
		}
		
		return results;
	}
	
	private void PrintDetails (Details details)
	{
		System.Console.WriteLine("url: "+details.Url);
		System.Console.WriteLine("Asin; "+details.Asin);
		System.Console.WriteLine("ProductName; "+details.ProductName);
		System.Console.WriteLine("Catalog; "+details.Catalog);
		System.Console.WriteLine("image: "+details.ImageUrlLarge);

		System.Console.WriteLine("BrowseList: ");
		if (details.BrowseList != null) {
			foreach (BrowseNode bn in details.BrowseList) {
				System.Console.WriteLine("\t"+bn.BrowseId+" | "+bn.BrowseName);
			}
		}

		System.Console.WriteLine("KeyPhrases: ");
		if (details.KeyPhrases != null) {
			foreach (KeyPhrase k in details.KeyPhrases) {
				System.Console.WriteLine("\t"+k.KeyPhrase1+" | "+k.Type);
			}
		}

		System.Console.Write ("Artists: ");
		if (details.Artists != null) {
			foreach (string s in details.Artists) {
				System.Console.Write(s+",");
			}
		}
		System.Console.WriteLine();

		System.Console.Write("Authors: ");
		if (details.Authors != null) {
			foreach (string s in details.Authors) {
				System.Console.Write(s+", ");
			}
		}
		System.Console.WriteLine();

		System.Console.WriteLine("Mpn; "+details.Mpn);

		System.Console.Write("Starring: ");
		if (details.Starring != null) {
			foreach (string s in details.Starring) {
				System.Console.Write(s+", ");
			}
		}
		System.Console.WriteLine();

		System.Console.Write("Directors: ");
		if (details.Directors != null) {
			foreach (string s in details.Directors) {
				System.Console.Write(s+", ");
			}
		}
		System.Console.WriteLine();

		System.Console.WriteLine("TheatricalReleaseDate; "+details.TheatricalReleaseDate);
		System.Console.WriteLine("ReleaseDate; "+details.ReleaseDate);
		System.Console.WriteLine("Manufacturer; "+details.Manufacturer);
		System.Console.WriteLine("Distributor; "+details.Distributor);
		System.Console.WriteLine("MerchantId; "+details.MerchantId);
		System.Console.WriteLine("MultiMerchant; "+details.MultiMerchant);
		System.Console.WriteLine("MerchantSku; "+details.MerchantSku);
		System.Console.WriteLine("NumberOfOfferings; "+details.NumberOfOfferings);
		System.Console.WriteLine("ThirdPartyNewCount; "+details.ThirdPartyNewCount);
		System.Console.WriteLine("UsedCount; "+details.UsedCount);
		System.Console.WriteLine("CollectibleCount; "+details.CollectibleCount);
		System.Console.WriteLine("RefurbishedCount; "+details.RefurbishedCount);
		//              ThirdPartyProductInfo ThirdPartyProductInfo;
		System.Console.WriteLine("SalesRank; "+details.SalesRank);
		System.Console.WriteLine("Media; "+details.Media);
		System.Console.WriteLine("ReadingLevel; "+details.ReadingLevel);
		System.Console.WriteLine("NumberOfPages; "+details.NumberOfPages);
		System.Console.WriteLine("NumberOfIssues; "+details.NumberOfIssues);
		System.Console.WriteLine("IssuesPerYear; "+details.IssuesPerYear);
		System.Console.WriteLine("SubscriptionLength; "+details.SubscriptionLength);
		System.Console.WriteLine("DeweyNumber; "+details.DeweyNumber);
		System.Console.WriteLine("RunningTime; "+details.RunningTime);
		System.Console.WriteLine("Publisher; "+details.Publisher);
		System.Console.WriteLine("NumMedia; "+details.NumMedia);
		System.Console.WriteLine("Isbn; "+details.Isbn);

		System.Console.Write("Features: ");
		if (details.Features != null) {
			foreach (string s in details.Features) {
				System.Console.Write(s+", ");
			}
		}
		System.Console.WriteLine();

		System.Console.WriteLine("MpaaRating; "+details.MpaaRating);
		System.Console.WriteLine("EsrbRating; "+details.EsrbRating);
		System.Console.WriteLine("AgeGroup; "+details.AgeGroup);
		System.Console.WriteLine("Availability; "+details.Availability);
		System.Console.WriteLine("Upc; "+details.Upc);

		System.Console.Write("Accessories: ");
		if (details.Accessories != null) {
			foreach (string s in details.Accessories) {
				System.Console.Write(s+", ");
			}
		}
		System.Console.WriteLine();

		System.Console.Write("Platforms: ");
		if (details.Platforms != null) {
			foreach (string s in details.Platforms) {
				System.Console.Write(s+", ");
			}
		}
		System.Console.WriteLine();

		System.Console.WriteLine("Encoding; "+details.Encoding);
		System.Console.WriteLine("ProductDescription; "+details.ProductDescription);
		//              Reviews Reviews;

		System.Console.Write("Lists: ");
		if (details.Lists != null) {
			foreach (string s in details.Lists) {
				System.Console.Write(s+", ");
			}
		}
		System.Console.WriteLine();

		System.Console.WriteLine("Status; "+details.Status);

	}
}
