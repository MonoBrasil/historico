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

using GConf;

public class Conf {
	public static string GConfBase = "/apps/mcatalog";
	public static string home = Environment.GetEnvironmentVariable("HOME")+"/.gnome2/mcatalog";
	public static GConf.Client GConfClient;
	private static bool debug = false;

	public static string HomeDir {
		get {
			if (!Directory.Exists(home)) {
				Directory.CreateDirectory (home);
			}
			if (!Directory.Exists(home+"/cache")) {
				Directory.CreateDirectory (home+"/cache");
			}
			if (!Directory.Exists(home+"/cache/small")) {
				Directory.CreateDirectory (home+"/cache/small");
			}
			if (!Directory.Exists(home+"/cache/medium")) {
				Directory.CreateDirectory (home+"/cache/medium");
			}
			if (!Directory.Exists(home+"/cache/large")) {
				Directory.CreateDirectory (home+"/cache/large");
			}
			return home;
		}
	}

	public static string DownloadedImagesDir {
		get {
			string dir = HomeDir+"/downloaded";
			if (!Directory.Exists (dir)) {
				Directory.CreateDirectory (dir);
			}
			return dir;
		}
	}

	public static void EmptyCache ()
	{
		Directory.Delete (home+"/cache", true);
	}

	public static int Get (string key, int defaultVal)
	{
		try {
			EnsureClient ();

			if (debug)
				Console.WriteLine ("Getting config key {0} with default value {1}", key, defaultVal);
			return (int) GConfClient.Get (GetFullKey (key));
		}
		catch {
			return defaultVal;
		}
	}

	public static string Get (string key, string defaultVal)
	{
		try {
			EnsureClient ();

			if (debug)
				Console.WriteLine ("Getting config key {0} with default value {1}", key, defaultVal);
			return (string) GConfClient.Get (GetFullKey (key));
		}
		catch {
			return defaultVal;
		}
	}

	public static bool Get (string key, bool defaultVal)
	{
		try {
			EnsureClient ();

			if (debug)
				Console.WriteLine ("Getting config key {0} with default value {1}", key, defaultVal);
			return (bool) GConfClient.Get (GetFullKey (key));
		} catch {
			return defaultVal;
		}
	}

	public static void Set (string key, object value)
	{
		EnsureClient ();

		if (debug)
			Console.WriteLine ("Saving config key {0} with value {1}", key, value);
		GConfClient.Set(GetFullKey (key), value);
	}

	public static View ItemView
	{
		get {
			try {
				string confValue = (string)Conf.Get ("ui/active_item_view", Enum.GetName (typeof (View), View.Shelf));
				return (View)Enum.Parse (typeof (View), confValue);
			}
			catch {
				return (View) 0;
			}
		}
		set {
			Conf.Set ("ui/active_item_view", Enum.GetName (typeof (View), value));
		}
	}

	public static void AddNotify (NotifyEventHandler handler)
	{
		EnsureClient ();

		GConfClient.AddNotify (GConfBase, handler);
	}

	public static void Sync ()
	{
		EnsureClient ();

		GConfClient.SuggestSync ();
	}

	public static string GetFullKey (string key)
	{
		if (key.StartsWith ("/")) {
			return key;
		}

		return GConfBase + "/" + key;
	}

	private static void EnsureClient ()
	{
		if (GConfClient == null) {
			GConfClient = new GConf.Client();
		}
	}
}
