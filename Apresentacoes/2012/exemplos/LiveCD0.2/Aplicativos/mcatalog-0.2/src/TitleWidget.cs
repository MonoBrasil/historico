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
using System.Collections;
using System.Collections.Specialized;

using Gtk;

using Mono.Posix;

public delegate void TitleWidgetOrderChangedHandler(Order order);

public class TitleWidget: HBox 
{
	private Label labelTable;
	private ComboBox comboOrderBy;
	private Button buttonSorting;
	private bool ascending;

	Label label0;
	Label label1;
	Label label2;

	public event TitleWidgetOrderChangedHandler OnOrderChanged;
	
	private ListDictionary fields;
	private int fieldsNumber;
	
	public TitleWidget (): base ()
	{
		label0 = new Label ("");
		this.PackStart (label0, true, true, 0);
		
		labelTable = new Label ();
		this.PackStart (labelTable, false, true, 0);

		label1 = new Label (Mono.Posix.Catalog.GetString ("by"));
		this.PackStart (label1, false, true, 0);
		
		comboOrderBy = ComboBox.NewText ();
		comboOrderBy.Changed += OnComboChanged;
		this.PackStart (comboOrderBy, false, true, 0);

		label2 = new Label (",");
		this.PackStart (label2, false, true, 0);

		buttonSorting = new Button ();
		buttonSorting.Relief = ReliefStyle.None;
		buttonSorting.CanFocus = false;
		buttonSorting.Clicked += OnButtonSortingClicked; 
		this.PackStart (buttonSorting, false, true, 0);

		this.Spacing = 5;
	}

	public void SetView (View view)
	{
		if (view == View.Shelf) {
			label0.Visible = true;
			label1.Visible = true;
			comboOrderBy.Visible = true;
			label2.Visible = true;
			buttonSorting.Visible = true;
		}
		else if (view == View.List) {
			label0.Visible = false;
			label1.Visible = false;
			comboOrderBy.Visible = false;
			label2.Visible = false;
			buttonSorting.Visible = false;
		}
	}

	public string Table
	{
		set {
			labelTable.Text = "<b>"+value+"</b>";
			labelTable.UseMarkup = true;
		}
	}

	public ListDictionary Fields
	{
		set {
			fields = value;

			while (fieldsNumber > 0) {
				comboOrderBy.RemoveText (fieldsNumber);
				fieldsNumber--;
			}

			fieldsNumber = 0;
			foreach (string s in value.Values) {
				comboOrderBy.InsertText (fieldsNumber++, s);
			}
		}
	}

	public Order Order
	{
		set {
			if (value.Ascending) {
				this.ascending = true;
				if (buttonSorting.Child != null) {
					buttonSorting.Remove (buttonSorting.Child);
				}
				buttonSorting.Add (new Gtk.Image (Stock.SortAscending, IconSize.Button));
				buttonSorting.Child.Visible = true;
			}
			else {
				this.ascending = false;
				if (buttonSorting.Child != null) {
					buttonSorting.Remove (buttonSorting.Child);
				}
				buttonSorting.Add (new Gtk.Image (Stock.SortDescending, IconSize.Button));
				buttonSorting.Child.Visible = true;
			}

			int i = 0;
			foreach (string field in fields.Keys) {
				if (field.Equals(value.Field)) {
					comboOrderBy.Active = i;
					break;
				}
				i++;
			}
		}
	}

	private void OnComboChanged (object o, EventArgs args)
	{
		ThrowEvent ();
	}

	private void OnButtonSortingClicked (object o, EventArgs args)
	{
		this.ascending = !this.ascending;
		ThrowEvent ();
	}
	
	private void ThrowEvent ()
	{
		TreeIter iter;
		if (comboOrderBy.GetActiveIter (out iter)) {
			Order order = new Order ();
			order.Ascending = this.ascending;
			foreach (string s in fields.Keys) {
				if (fields[s].Equals ((string) comboOrderBy.Model.GetValue (iter, 0))) {
					order.Field = s;
					break;
				}
			}

			this.Order = order;
			if (this.OnOrderChanged != null) {
				this.OnOrderChanged (order);
			}
		}
		
	}
}
