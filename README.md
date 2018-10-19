# Quack
Quack is a language I made when I was bored and decided it would be fun to make a new language. I have used F# for the parser and C# for lexing, AST analysis and transpiling. I will slowly add new features to this.

At the moment Quack supports:
- Scoped variable declaration and assignment
- Variable types - any, int, bool (only limited type checking for now)
- Functions and lambdas (no return statement yet)
- If/else (branching)
- While (looping)
- Arithmetic expressions
- Boolean expressions
- Parentheses
- Print

An example of Quack code:
```
func printSum(int a, int b) {
	print a + b;
}

func printOne() => print 1;

int apple = 3;
any orange;

while (apple < 6) {
	apple = apple + 1;
}

if (apple > 6) {
	orange = apple;
}
else {
	orange = 2;
}

printSum(apple, orange + 1);
```

Quack code is transpiled into JavaScript code. I plan on adding more transpilation targets in the future and perhaps even compilation!

![alt tag](http://i64.tinypic.com/2mrumnc.jpg)
