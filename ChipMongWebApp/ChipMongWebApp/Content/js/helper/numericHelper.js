const precision = 2;

//-> toInt
const toInt = (number) => parseInt(number) ? parseInt(number) : 0;


//-> toFloat
const toFloat = (number) => parseFloat(number) ? parseFloat(number) : 0.0;
 

//-> toFloatWith
const toFloatWithTwoPrecision = (number) => {
    var num = parseFloat(number) ? parseFloat(number) : 0.0;
    return parseFloat(num).toFixed(precision);
}


//-> toFloatWithDollarCurrency
const toFloatWithDollarCurrency = (number) => toFloat(number) +  " $";
