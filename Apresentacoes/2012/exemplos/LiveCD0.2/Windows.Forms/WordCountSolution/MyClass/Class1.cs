
using System;
using System.Collections;public class MyClass : IComparable
{
	private int num;

	public MyClass(int num) { this.num = num; }

	public int Num
	{
		get
		{
			return num;
		}
	}

	int IComparable.CompareTo(object obj)
	{
		MyClass mc = (MyClass)obj;

		if (this.num > mc.num)
			return 1;
		if (this.num < mc.num)
			return -1;
		return 0;
	}

}

public class MainDriver
{
	public static void Main()
	{
		ArrayList arr = new ArrayList();

		arr.Add(new MyClass( 5 ));
		arr.Add(new MyClass( 3 ));
		arr.Add(new MyClass( 9 ));
		arr.Add(new MyClass(1 ));
		arr.Add(new MyClass( 6 ));

		arr.Sort();

		for (int i = 0; i < arr.Count; i++)
		{
			MyClass t = (MyClass) arr[i];
			Console.WriteLine( t.Num);
		}
	}
}