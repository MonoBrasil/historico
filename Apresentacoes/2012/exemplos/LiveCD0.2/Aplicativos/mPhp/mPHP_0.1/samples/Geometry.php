<?php

class Shape {
	const pi = 3.14159265;
}

class Circle extends Shape {
	public $radius;
	public function __construct($radius) {
		$this->radius = $radius;
	}
	public function GetArea() {
		return 0.5 * Shape::pi * $this->radius * $this->radius;
	}
	public function GetRound() {
		return 2 * Shape::pi * $this->radius;
	}
}

class Rectangle extends Shape {
	public $x;
	public $y;
	public function __construct($x, $y) {
		$this->x = $x;
		$this->y = $y;
	}
	public function GetArea() {
		return $this->x * $this->y;
	}
	public function GetRound() {
		return 2 * $this->x + 2 * $this->y;
	}
}

$c = new Circle(2);
echo '$c is a circle with radius 2\n';
echo 'area of $c = ' . $c->GetArea() . '\n';
echo 'round of $c = ' . $c->GetRound() . '\n\n';

$r = new Rectangle(2, 4);
echo '$r is a rectangle with lateral leghts ' . $r->x . ' and ' . $r->y . '\n';
echo 'area of $r = ' . $r->GetArea() . '\n';
echo 'round of $r = ' . $r->GetRound() . '\n\n';

echo '$c is ' . (!($c instanceof Shape) ? 'not ' : '') . 'a Shape' . '\n';
echo '$c is ' . (!($c instanceof Circle) ? 'not ' : '') . 'a Circle' . '\n';
echo '$c is ' . (!($c instanceof Rectangle) ? 'not ' : '') . 'a Rectangle' . '\n';
echo '$r is ' . (!($r instanceof Shape) ? 'not ' : '') . 'a Shape' . '\n';
echo '$r is ' . (!($r instanceof Circle) ? 'not ' : '') . 'a Circle' . '\n';
echo '$r is ' . (!($r instanceof Rectangle) ? 'not ' : '') . 'a Rectangle' . '\n';

?>