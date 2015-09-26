/*
MonoUML.Widgets - A library for representing the Widget elements
Copyright (C) 2005  Rodolfo Campero

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
using System.Collections;

namespace MonoUML.Widgets
{
	// An event queue acts like a decorator: externally it behaves like an
	// IBroadcaster, but it isn't one. An event queue has a reference to an 
	// actual broadcaster, and its job is to keep an event queue, enqueuing
	// events while some event is processed, and then dequeuing and calling
	// the appropriate method on the actual broadcaster.
	internal class EventQueue : IBroadcaster
	{
		private enum EventKind
		{
			Change,
			Selection
		}
		
		private class EventPair
		{
			public EventPair(EventKind kind, object subject)
			{
				Kind = kind;
				Subject = subject;
			}
			
			public EventKind Kind;
			public object Subject;
			
			public override string ToString()
			{
				return Kind.ToString() + " " + Subject.ToString();
			}
		}
		
		public EventQueue(IBroadcaster actualBroadcaster)
		{
			_broadcaster = actualBroadcaster;
			_pendingNotifications = new Queue();
		}
		
		public object LastSelectedElement
		{
			get { return _lastSelected; }
		}

		public void BroadcastElementChange(object modifiedElement)
		{
			if(!_broadcasting)
			{
				try
				{
					_broadcasting = true;
					//Console.WriteLine("Broadcasting Change " + modifiedElement.ToString());
					_broadcaster.BroadcastElementChange(modifiedElement);
				}
				finally
				{
					_broadcasting = false;
					DequeueEvent();
				}
			}
			else
			{
				EnqueueEvent(EventKind.Change, modifiedElement);
			}
		}
		
		public void BroadcastElementSelection(object element)
		{
			_lastSelected = element;
			if(!_broadcasting)
			{
				try
				{
					_broadcasting = true;
					//Console.WriteLine("Broadcasting Selection " + element.ToString());
					_broadcaster.BroadcastElementSelection(element);
				}
				finally
				{
					_broadcasting = false;
					DequeueEvent();
				}
			}
			else
			{
				EnqueueEvent(EventKind.Selection, element);
			}
		}
		
		private void DequeueEvent()
		{
			if(_pendingNotifications.Count > 0)
			{
				EventPair pair = (EventPair)_pendingNotifications.Dequeue();
				_lastDequeued = pair.Subject;
				//Console.WriteLine ("Dequeued: " + pair);
				switch(pair.Kind)
				{
					case EventKind.Change: BroadcastElementChange(pair.Subject); break;
					case EventKind.Selection: BroadcastElementSelection(pair.Subject); break;
				} 
				// this fixes some desynchronization problems between the views.
				if( _pendingNotifications.Count == 0
					&& !object.ReferenceEquals(_lastSelected, _lastDequeued))
				{
					BroadcastElementSelection(_lastSelected);
				}
			}
		}

		private void EnqueueEvent(EventKind kind, object element)
		{
			_pendingNotifications.Enqueue(new EventPair(kind, element));
			//Console.WriteLine ("Enqueued: " + _pendingNotifications.Peek());
		}
		
		private IBroadcaster _broadcaster;
		private bool _broadcasting;
		private Queue _pendingNotifications;
		private object _lastDequeued;
		private object _lastSelected;
	}
}
