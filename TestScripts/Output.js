function printSum(a, b){
  console.log(a + b);
}
var a;
var apple = 3;
var orange;
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
