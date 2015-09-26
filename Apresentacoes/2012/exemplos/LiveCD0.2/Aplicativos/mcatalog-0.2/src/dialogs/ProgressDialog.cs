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

using Gtk;
using GtkSharp;
using System;

using Mono.Posix;

public class ProgressDialog : Dialog
{
	private ProgressBar progress_bar;
	private Label message_label;

	private uint tag;

	public ProgressDialog (string searchStringName) 
	{
		this.Modal = true;
		this.Title = Mono.Posix.Catalog.GetString ("Wait, please");

		HasSeparator = false;
		BorderWidth = 6;
		SetDefaultSize (300, -1);

		message_label = new Label (String.Format (Mono.Posix.Catalog.GetString ("Searching {0}...."), searchStringName));
		VBox.PackStart (message_label, true, true, 12);

		progress_bar = new ProgressBar ();
		progress_bar.PulseStep = 0.1;
		VBox.PackStart (progress_bar, true, true, 6);

		AddButton (Stock.Cancel, ResponseType.Cancel);

		Response += HandleResponse;

		tag = GLib.Timeout.Add (100, new GLib.TimeoutHandler (Pulse));
	}

	private void HandleResponse (object obj, ResponseArgs args) {
		End ();
	}

	public void End ()
	{
		if (tag > 0)
			GLib.Source.Remove (tag);
		this.Destroy ();
	}

	public void CloseDialog ()
	{
		this.Respond (ResponseType.Close);
	}

	public bool Pulse () 
	{
		progress_bar.Pulse();
		return true;
	}
}
