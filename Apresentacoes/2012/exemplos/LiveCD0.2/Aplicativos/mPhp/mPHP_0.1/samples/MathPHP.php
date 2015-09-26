<?php

class MathPHP {

	public static function Fib($a) {
		if ($a == 0)
			return 0;
		else if ($a == 1)
			return 1;
		else
			return fib($a - 1) + fib($a - 2);
	}
	
	public static function Fac($a) {
		if ($a == 0 || $a == 1)
			return 1;
		else
			return $a * fac($a - 1);
	}
	
}

?>