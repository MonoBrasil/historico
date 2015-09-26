namespace MonoUML.Widgets.UML2
{
	public delegate void MovedHandler (object o);

	public delegate void UMLElementMovedHandler (object obj, double dx, double dy);
	public delegate void UMLElementMotionedHandler (object obj, double dx, double dy);
	public delegate void UMLElementButtonEventHandler(object obj, Gdk.EventButton eb);
	public delegate void UMLElementResizedHandler (object obj, double w, double h);
	public delegate void UMLElementEnterNotifyHandler (object obj);
	public delegate void UMLElementLeaveNotifyHandler (object obj);
	
	public delegate void UMLElementSelectedHandler (object obj);
	
	//
	public delegate void UMLElementNameChangedHandler (object obj, string new_name); 
	
	//The entry is being moved
	public delegate void UMLEntryMovedHandler (UMLEntry obj, double dx, double dy);
	
	//UMLNodeEntry's UMLEntry is being moved
	public delegate void UMLNodeEntryMovedHandler (object obj, UMLEntry entry, double dx, double dy);

	// Widht: new width
	// Height: new height
	// dx,dy: TopLeft moved	
	public delegate void ResizedHandler (object o, double width, double height,
			double dx, double dy);
	
}
