/*
MonoUML.Widgets.Toolbar - Canvas' Toolbars
Copyright (C) 2005  Mario Carrión

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
using Gtk;
using Gdk;
using MonoUML.Widgets;
using MonoUML.Widgets.UML2;
using MonoUML.I18n;
using System;

namespace MonoUML.Widgets.Toolbar
{

	public class ToolbarUseCase : ToolbarBase
	{
	
		public ToolbarUseCase (UMLDiagram diagram) : base (diagram)
		{
			_actionActor = new CreateActorAction (diagram);
			_actionUseCase = new CreateUseCaseAction (diagram);
		}
		
		protected override void DrawIcons ()
		{ 
        	_tbuttonActor = CreateToolButton ("actor_tree.png", GettextCatalog.GetString ("Add an Actor"));
        	_tbuttonUseCase = CreateToolButton ("usecase_tree.png", GettextCatalog.GetString ("Add an Use Case"));  
			Insert (_tbuttonActor, -1);
			Insert (_tbuttonUseCase, -1);
			_tbuttonActor.Clicked += QueueActor;
			_tbuttonUseCase.Clicked += QueueUseCase;
		}

		private void QueueActor (object sender, EventArgs e)
		{
			_diagram.UMLCanvas.QueueAction = _actionActor;
			//curso.Unref ();
		}

		private void QueueUseCase (object sender, EventArgs e)
		{
			 //curso = new Gdk.Cursor (Gdk.CursorType.Hand1);
			//_tbuttonActor.GdkWindow.Cursor = curso;
			
			_diagram.UMLCanvas.QueueAction = _actionUseCase;
		}

		private CreateActorAction _actionActor;
		private CreateUseCaseAction _actionUseCase;
		private Gtk.ToolButton _tbuttonActor;
		private Gtk.ToolButton _tbuttonUseCase;
	//	private Gdk.Cursor curso;
	}
}