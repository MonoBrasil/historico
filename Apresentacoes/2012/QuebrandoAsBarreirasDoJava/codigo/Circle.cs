public class Circle : Ellipse
{
   public Circle(int x, int y, int r ) : base (x, y, r, r)
   {
   }

   public override void print()
   {
	Console.Write("Circle (r: " + w + ") ");
	base.print();
   }
}
