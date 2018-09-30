# Quack
Quack is a language I made when I was bored and decided it would be fun to make a new language. I will slowly add new features to this.

At the moment Quack supports:
- Variable declaration and assignment
- Numerical data
- Arithmetic operations
- Open/close parentheses
- If/else (control flow)
- Print

An example of Quack code:
```
declare a;
declare b = 8;
a = (2 + (b / 2)) - (1 * 3);
if ((1 + 3) < b) {
	b = 2;
}
else {
	b = 10;
}
print a;
print b;
```

Quack code is transpiled into JavaScript code. I plan on adding more transpilation targets in the future and perhaps even compilation!
