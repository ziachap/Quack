function someNumber(a){
  console.log(a + 1);
}
function printSum(a, b){
  console.log(a + b);
}
function one(){
  console.log(1);
}
var apple = 3;
var orange;
var testBool;
while (apple + 1 < 6 - one()) {
  apple = apple + 1;
}
if (apple > 6) {
  orange = 5;
}
else {
  orange = 2;
}
printSum(apple, orange + 1);
