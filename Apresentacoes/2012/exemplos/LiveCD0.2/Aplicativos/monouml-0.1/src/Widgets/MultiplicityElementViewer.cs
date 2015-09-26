/*
MonoUML.Widgets - A library for representing the Widget elements
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
using UML = ExpertCoder.Uml2;

namespace MonoUML.Widgets
{
	public class MultiplicityElementViewer : Gtk.HBox
	{
		public MultiplicityElementViewer(IBroadcaster hub)
		{
			_orderedViewer = new SingleBooleanViewer(hub, "Ordered:", "IsOrdered");
			_uniqueViewer = new SingleBooleanViewer(hub, "Unique:", "IsUnique");
			_lower = new Gtk.Entry();
			_lower.WidthChars = 4;
			_lower.Changed += new EventHandler(OnEntryChanged);
			_lower.Activated += new EventHandler(OnActivated);
			_lower.FocusOutEvent += new Gtk.FocusOutEventHandler(OnFocusOutEvent);
			_upper = new Gtk.Entry();
			_upper.WidthChars = 4;
			_upper.Changed += new EventHandler(OnEntryChanged);
			_upper.Activated += new EventHandler(OnActivated);
			_upper.FocusOutEvent += new Gtk.FocusOutEventHandler(OnFocusOutEvent);
			base.PackStart(_orderedViewer, false, false, 0);
			base.PackStart(_uniqueViewer, false, false, 2);
			base.PackStart(new Gtk.Label("Lower:"), false, false, 2);
			base.PackStart(_lower, false, false, 2);
			base.PackStart(new Gtk.Label("Upper:"), false, false, 2);
			base.PackStart(_upper, false, false, 2);
		}

		private void BroadcastChanges ()
		{
			if (_hasChanged)
			{
				Hub.Instance.Broadcaster.BroadcastElementChange (_multiplicityElement);
				_hasChanged = false;
			}
		}

		public new void Hide()
		{
			_multiplicityElement = null;
			base.Hide ();
		}
		
		private void OnActivated(object sender, EventArgs args)
		{
			BroadcastChanges ();
		}

		private void OnEntryChanged(object sender, EventArgs args)
		{
			string newText;
			UML.UnlimitedNatural newValue;
			if(_isChanging) return;
			_isChanging = true; 
			if(sender == _lower)
			{
				newText = _lower.Text;
				try
				{
					if(newText == String.Empty)
					{
						newValue = 1;
					}
					else
					{
						newValue = UInt32.Parse(newText);
					}
					_multiplicityElement.Lower = (uint)newValue;
					newText = newText=="" ? "" : newValue.ToString();
					_hasChanged = newText != _lastLower;
					_lastLower = newText;
					_lower.Text = _lastLower;
				}
				catch(Exception)
				{
					_lower.Text = _lastLower;
				}
			}
			else
			{
				newText = _upper.Text;
				try
				{
					if(newText == String.Empty)
					{
						newValue = 1;
					}
					else if(newText == "*")
					{
						newValue = UML.UnlimitedNatural.Infinity;
					}
					else
					{
						newValue = UInt32.Parse(newText);
					}
					_multiplicityElement.Upper = newValue;
					newText = newText=="" ? "" : newValue.ToString();
					_hasChanged = newText != _lastUpper;
					_lastUpper = newText;
					_upper.Text = _lastUpper;
				}
				catch(Exception)
				{
					_upper.Text = _lastUpper;
				}
			}
			_isChanging = false; 
		}
		
		private void OnFocusOutEvent(object sender, Gtk.FocusOutEventArgs args)
		{
			BroadcastChanges ();
		}

		public void ShowPropertiesFor(UML.MultiplicityElement element)
		{
			_multiplicityElement = element;
			_orderedViewer.ShowPropertyValueFor(element);
			_uniqueViewer.ShowPropertyValueFor(element);
			_lower.Text = element.Lower.ToString();
			_upper.Text = element.Upper.ToString();
		}
		
		private bool _hasChanged;
		private bool _isChanging;
		private SingleBooleanViewer _orderedViewer;
		private string _lastLower;
		private string _lastUpper;
		private Gtk.Entry _lower;
		private UML.MultiplicityElement _multiplicityElement;
		private SingleBooleanViewer _uniqueViewer;
		private Gtk.Entry _upper;
	}
}
