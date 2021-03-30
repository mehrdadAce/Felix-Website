var result  = document.getElementById("result");
var slider1 = document.getElementById("voorschot");
var output1 = document.getElementById("value-voorschot");            
var slider2 = document.getElementById("looptijd");
var output2 = document.getElementById("value-looptijd");            
var slider3 = document.getElementById("garantie");
var output3 = document.getElementById("value-garantie");

output1.innerHTML = '€ ' + slider1.value;
output2.innerHTML = slider2.value + ' maanden';
output3.innerHTML = slider3.value + ' jaar';
result.innerHTML = '€ ' + Math.round(((28000 - 2382) / 23 + 25 * 3)/2) + ' / maand'

slider1.oninput = function() {
    output1.innerHTML = '€ ' + this.value;
    result.innerHTML = '€ ' + Math.round(((28000 - this.value) / slider2.value + (25 * slider3.value))/2) + ' / maand'
}

slider2.oninput = function() {
    output2.innerHTML = this.value + ' maanden';
    result.innerHTML = '€ ' + Math.round(((28000 - slider1.value) / this.value  + (25 * slider3.value))/2) + ' / maand'
}

slider3.oninput = function() {
    output3.innerHTML = this.value + ' jaar';
    result.innerHTML = '€ ' + Math.round(((28000 - slider1.value) / slider2.value + (25 * this.value))/2) + ' / maand'
}