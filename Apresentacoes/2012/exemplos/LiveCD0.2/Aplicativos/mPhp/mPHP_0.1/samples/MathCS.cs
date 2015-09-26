public class MathCS {

	public static void Main(string[] args) {
		System.Console.WriteLine("The following calculations are performed by the PHP class:");
		
		// define the number 5 as an object of type PHP.Integer
		PHP.Integer i = new PHP.Integer(5);
		
		// calculating faculty of 5
		PHP.Mixed fac5 = MathPHP.Fac(i);
		System.Console.WriteLine("Faculty of 5 = " + fac5.ToString());
		
		// calculating fibonacci of 5
		PHP.Mixed fib5 = MathPHP.Fib(i);
		System.Console.WriteLine("Fibonacci of 5 = " + fib5.ToString());
	}

}