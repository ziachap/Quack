function someNumber(a){
  console.log(a + 1);
}
function printSum(a, b){
  console.log(a + b);
  return;
}
function getSum(a, b){
  return a + b;
}
function one(){
  console.log(1);
}
var apple = 3;
var orange;
var testBool;
while (apple + 1 < 6) {
  apple = apple + 1;
}
if (!true) {
  orange = 5;
}
else {
  orange = 2;
}
console.log(getSum(apple, orange + 1));
