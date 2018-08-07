const precision = 2;

//-> toInt
let toInt = (number) => parseInt(number) ? parseInt(number) : 0;


//-> toFloat
let toFloat = (number) => parseFloat(number) ? parseFloat(number) : 0.0;
 

//-> toFloatWith
let toFloatWithTwoPrecision = (number) => {
    var num = parseFloat(number) ? parseFloat(number) : 0.0;
    return parseFloat(num).toFixed(precision);
}


//-> toFloatWithDollarCurrency
let toFloatWithDollarCurrency = (number) => toFloat(number) +  " $";
