# Quack
Quack is a language I made when I was bored and decided it would be fun to make a new language. I have used F# for the parser and C# for lexing, AST analysis and transpiling. I will slowly add new features to this.

At the moment Quack supports:
- Scoped variable declaration and assignment
- Numerical data
- Arithmetic expressions
- Boolean expressions
- Parentheses
- If/else (control flow)
- Print
- Functions (no return statement yet)

An example of Quack code:
```
func printSum(declare a, declare b) {
	print a + b;
}

declare apple = 3;
declare orange;

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
