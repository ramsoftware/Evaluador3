/* Evaluador de expresiones. Versión 3.0
 * Autor: Rafael Alberto Moreno Parra
 * Fecha: 25 de abril de 2021
 *
 * Pasos para la evaluación de expresiones algebraicas
 * I. Toma una expresión, por ejemplo: 3.14 + sen( 4 / x ) * ( 7.2 ^ 3 - 1 ) y la divide en partes así:
 *	[3.14] [+] [sen(] [4] [/] [x] [)] [*] [(] [7.2] [^] [3] [-] [1] [)]
 *
 * II. Toma las partes y las divide en piezas con la siguiente estructura:
 *		acumula = funcion  numero/variable/acumula  operador  numero/variable/acumula
 *		Siguiendo el ejemplo anterior sería:
 *		A = 7.2  ^  3
 *		B = A  -  1
 *		C = seno ( 4  /  x )
 *		D = C  *  B
 *		E = 3.14  +  D
 *
 *		Esas piezas se evalúan de arriba a abajo y así se interpreta la ecuación
 *
 * */
var Parte = /** @class */ (function () {
    function Parte(tipo, funcion, operador, numero, unaVariable) {
        this.Tipo = tipo; /* Acumulador, función, paréntesis que abre, paréntesis que cierra, operador, número, variable */
        this.Funcion = funcion; /* Código de la función 0:seno, 1:coseno, 2:tangente, 3: valor absoluto, 4: arcoseno, 5: arcocoseno, 6: arcotangente, 7: logaritmo natural, 8: valor tope, 9: exponencial, 10: raíz cuadrada, 11: raíz cúbica */
        this.Operador = operador; /* + suma - resta * multiplicación / división ^ potencia */
        this.Numero = numero; /* Número literal, por ejemplo: 3.141592 */
        this.UnaVariable = unaVariable; /* Variable algebraica */
        this.Acumulador = 0; /* Usado cuando la expresión se convierte en piezas. Por ejemplo:
                            3 + 2 / 5  se convierte así:
                            |3| |+| |2| |/| |5|
                            |3| |+| |A|  A es un identificador de acumulador */
    }
    return Parte;
}());
var Pieza = /** @class */ (function () {
    function Pieza(funcion, tipoA, numeroA, variableA, piezaA, operador, tipoB, numeroB, variableB, piezaB) {
        this.ValorPieza = 0;
        this.Funcion = funcion;
        this.TipoA = tipoA;
        this.NumeroA = numeroA;
        this.VariableA = variableA;
        this.PiezaA = piezaA;
        this.Operador = operador;
        this.TipoB = tipoB;
        this.NumeroB = numeroB;
        this.VariableB = variableB;
        this.PiezaB = piezaB;
    }
    return Pieza;
}());
var Evaluador3 = /** @class */ (function () {
    function Evaluador3() {
        /* Constantes de los diferentes tipos de datos que tendrán las piezas */
        this.ESFUNCION = 1;
        this.ESPARABRE = 2;
        this.ESPARCIERRA = 3;
        this.ESOPERADOR = 4;
        this.ESNUMERO = 5;
        this.ESVARIABLE = 6;
        this.ESACUMULA = 7;
        /* Inicializa la lista de partes */
        this.Partes = new Array();
        /* Inicializa la lista de piezas */
        this.Piezas = new Array();
        /* El arreglo unidimensional que lleva el valor de las variables */
        this.VariableAlgebra = new Array();
        /* Uso del chequeo de sintaxis */
        this.Sintaxis = new EvaluaSintaxis();
    }
    /* Analiza la expresión */
    Evaluador3.prototype.Analizar = function (expresionA) {
        var expresionB = this.Sintaxis.Transforma(expresionA);
        var chequeo = this.Sintaxis.SintaxisCorrecta(expresionB);
        if (chequeo === true) {
            this.Partes.length = 0;
            this.Piezas.length = 0;
            this.CrearPartes(expresionB);
            this.CrearPiezas();
        }
        return chequeo;
    };
    /* Divide la expresión en partes */
    Evaluador3.prototype.CrearPartes = function (expresion) {
        /* Debe analizarse con paréntesis */
        var NuevoA = "(" + expresion + ")";
        /* Reemplaza las funciones de tres letras por una letra mayúscula */
        var NuevoB = NuevoA.replace(/sen/gi, "A").replace(/cos/gi, "B").replace(/tan/gi, "C").replace(/abs/gi, "D").replace(/asn/gi, "E").replace(/acs/gi, "F").replace(/atn/gi, "G").replace(/log/gi, "H").replace(/cei/gi, "I").replace(/exp/gi, "J").replace(/sqr/gi, "K").replace(/rcb/gi, "L");
        /* Va de caracter en caracter */
        var Numero = "";
        for (var pos = 0; pos < NuevoB.length; pos++) {
            var car = NuevoB[pos];
            /* Si es un número lo va acumulando en una cadena */
            if ((car >= '0' && car <= '9') || car == '.') {
                Numero += car;
            }
            /* Si es un operador entonces agrega número (si existía) */
            else if (car == '+' || car == '-' || car == '*' || car == '/' || car == '^') {
                if (Numero.length > 0) {
                    this.Partes.push(new Parte(this.ESNUMERO, -1, '0', this.CadenaAReal(Numero), 0));
                    Numero = "";
                }
                this.Partes.push(new Parte(this.ESOPERADOR, -1, car, 0, 0));
            }
            /* Si es variable */
            else if (car >= 'a' && car <= 'z') {
                this.Partes.push(new Parte(this.ESVARIABLE, -1, '0', 0, car.charCodeAt(0) - 'a'.charCodeAt(0)));
            }
            /* Si es una función (seno, coseno, tangente, ...) */
            else if (car >= 'A' && car <= 'L') {
                this.Partes.push(new Parte(this.ESFUNCION, car.charCodeAt(0) - 'A'.charCodeAt(0), '0', 0, 0));
                pos++;
            }
            /* Si es un paréntesis que abre */
            else if (car == '(') {
                this.Partes.push(new Parte(this.ESPARABRE, -1, '0', 0, 0));
            }
            /* Si es un paréntesis que cierra */
            else {
                if (Numero.length > 0) {
                    this.Partes.push(new Parte(this.ESNUMERO, -1, '0', this.CadenaAReal(Numero), 0));
                    Numero = "";
                }
                /* Si sólo había un número o variable dentro del paréntesis le agrega + 0 (por ejemplo:  sen(x) o 3*(2) ) */
                if (this.Partes[this.Partes.length - 2].Tipo == this.ESPARABRE || this.Partes[this.Partes.length - 2].Tipo == this.ESFUNCION) {
                    this.Partes.push(new Parte(this.ESOPERADOR, -1, '+', 0, 0));
                    this.Partes.push(new Parte(this.ESNUMERO, -1, '0', 0, 0));
                }
                this.Partes.push(new Parte(this.ESPARCIERRA, -1, '0', 0, 0));
            }
        }
    };
    /* Convierte un número almacenado en una cadena a su valor real */
    Evaluador3.prototype.CadenaAReal = function (Numero) {
        //Parte entera
        var parteEntera = 0;
        var cont = 0;
        for (cont = 0; cont < Numero.length; cont++) {
            if (Numero[cont].charCodeAt(0) === '.'.charCodeAt(0))
                break;
            parteEntera = parteEntera * 10 + (Numero[cont].charCodeAt(0) - '0'.charCodeAt(0));
        }
        //Parte decimal
        var parteDecimal = 0;
        var multiplica = 1;
        for (var num = cont + 1; num < Numero.length; num++) {
            parteDecimal = parteDecimal * 10 + (Numero[num].charCodeAt(0) - '0'.charCodeAt(0));
            multiplica *= 10;
        }
        var numero = parteEntera + parteDecimal / multiplica;
        return numero;
    };
    /* Ahora convierte las partes en las piezas finales de ejecución */
    Evaluador3.prototype.CrearPiezas = function () {
        var cont = this.Partes.length - 1;
        do {
            var tmpParte = this.Partes[cont];
            if (tmpParte.Tipo === this.ESPARABRE || tmpParte.Tipo === this.ESFUNCION) {
                this.GenerarPiezasOperador('^', '^', cont); /* Evalúa las potencias */
                this.GenerarPiezasOperador('*', '/', cont); /* Luego evalúa multiplicar y dividir */
                this.GenerarPiezasOperador('+', '-', cont); /* Finalmente evalúa sumar y restar */
                if (tmpParte.Tipo === this.ESFUNCION) { /* Agrega la función a la última pieza */
                    this.Piezas[this.Piezas.length - 1].Funcion = tmpParte.Funcion;
                }
                /* Quita el paréntesis/función que abre y el que cierra, dejando el centro */
                this.Partes.splice(cont, 1);
                this.Partes.splice(cont + 1, 1);
            }
            cont--;
        } while (cont >= 0);
    };
    Evaluador3.prototype.GenerarPiezasOperador = function (operA, operB, ini) {
        var cont = ini + 1;
        do {
            var tmpParte = this.Partes[cont];
            if (tmpParte.Tipo === this.ESOPERADOR && (tmpParte.Operador === operA || tmpParte.Operador === operB)) {
                var tmpParteIzq = this.Partes[cont - 1];
                var tmpParteDer = this.Partes[cont + 1];
                /* Crea Pieza */
                this.Piezas.push(new Pieza(-1, tmpParteIzq.Tipo, tmpParteIzq.Numero, tmpParteIzq.UnaVariable, tmpParteIzq.Acumulador, tmpParte.Operador, tmpParteDer.Tipo, tmpParteDer.Numero, tmpParteDer.UnaVariable, tmpParteDer.Acumulador));
                /* Elimina la parte del operador y la siguiente */
                this.Partes.splice(cont, 1);
                this.Partes.splice(cont, 1);
                /* Retorna el contador en uno para tomar la siguiente operación */
                cont -= 1;
                /* Cambia la parte anterior por parte que acumula */
                tmpParteIzq.Tipo = this.ESACUMULA;
                tmpParteIzq.Acumulador = this.Piezas.length - 1;
            }
            cont++;
        } while (this.Partes[cont].Tipo !== this.ESPARCIERRA);
    };
    /* Evalúa la expresión convertida en piezas */
    Evaluador3.prototype.Evaluar = function () {
        var numA, numB, resultado = 0;
        for (var pos = 0; pos < this.Piezas.length; pos++) {
            var tmpPieza = this.Piezas[pos];
            switch (tmpPieza.TipoA) {
                case this.ESNUMERO:
                    numA = tmpPieza.NumeroA;
                    break;
                case this.ESVARIABLE:
                    numA = this.VariableAlgebra[tmpPieza.VariableA];
                    break;
                default:
                    numA = this.Piezas[tmpPieza.PiezaA].ValorPieza;
                    break;
            }
            switch (tmpPieza.TipoB) {
                case this.ESNUMERO:
                    numB = tmpPieza.NumeroB;
                    break;
                case this.ESVARIABLE:
                    numB = this.VariableAlgebra[tmpPieza.VariableB];
                    break;
                default:
                    numB = this.Piezas[tmpPieza.PiezaB].ValorPieza;
                    break;
            }
            switch (tmpPieza.Operador) {
                case '*':
                    resultado = numA * numB;
                    break;
                case '/':
                    resultado = numA / numB;
                    break;
                case '+':
                    resultado = numA + numB;
                    break;
                case '-':
                    resultado = numA - numB;
                    break;
                default:
                    resultado = Math.pow(numA, numB);
                    break;
            }
            if (isNaN(resultado) || !isFinite(resultado))
                return resultado;
            switch (tmpPieza.Funcion) {
                case 0:
                    resultado = Math.sin(resultado);
                    break;
                case 1:
                    resultado = Math.cos(resultado);
                    break;
                case 2:
                    resultado = Math.tan(resultado);
                    break;
                case 3:
                    resultado = Math.abs(resultado);
                    break;
                case 4:
                    resultado = Math.asin(resultado);
                    break;
                case 5:
                    resultado = Math.acos(resultado);
                    break;
                case 6:
                    resultado = Math.atan(resultado);
                    break;
                case 7:
                    resultado = Math.log(resultado);
                    break;
                case 8:
                    resultado = Math.ceil(resultado);
                    break;
                case 9:
                    resultado = Math.exp(resultado);
                    break;
                case 10:
                    resultado = Math.sqrt(resultado);
                    break;
                case 11:
                    resultado = Math.pow(resultado, 0.3333333333333333333333);
                    break;
            }
            if (isNaN(resultado) || !isFinite(resultado))
                return resultado;
            tmpPieza.ValorPieza = resultado;
        }
        return resultado;
    };
    /* Da valor a las variables que tendrá la expresión algebraica */
    Evaluador3.prototype.DarValorVariable = function (varAlgebra, valor) {
        this.VariableAlgebra[varAlgebra.charCodeAt(0) - 'a'.charCodeAt(0)] = valor;
    };
    return Evaluador3;
}());
var EvaluaSintaxis = /** @class */ (function () {
    function EvaluaSintaxis() {
        /* Mensajes de error de sintaxis */
        this._mensajeError = [
            '0. Caracteres no permitidos. Ejemplo: 3$5+2',
            '1. Un número seguido de una letra. Ejemplo: 2q-(*3)',
            '2. Un número seguido de un paréntesis que abre. Ejemplo: 7-2(5-6)',
            '3. Doble punto seguido. Ejemplo: 3..1',
            '4. Punto seguido de operador. Ejemplo: 3.*1',
            '5. Un punto y sigue una letra. Ejemplo: 3+5.w-8',
            '6. Punto seguido de paréntesis que abre. Ejemplo: 2-5.(4+1)*3',
            '7. Punto seguido de paréntesis que cierra. Ejemplo: 2-(5.)*3',
            '8. Un operador seguido de un punto. Ejemplo: 2-(4+.1)-7',
            '9. Dos operadores estén seguidos. Ejemplo: 2++4, 5-*3',
            '10. Un operador seguido de un paréntesis que cierra. Ejemplo: 2-(4+)-7',
            '11. Una letra seguida de número. Ejemplo: 7-2a-6',
            '12. Una letra seguida de punto. Ejemplo: 7-a.-6',
            '13. Un paréntesis que abre seguido de punto. Ejemplo: 7-(.4-6)',
            '14. Un paréntesis que abre seguido de un operador. Ejemplo: 2-(*3)',
            '15. Un paréntesis que abre seguido de un paréntesis que cierra. Ejemplo: 7-()-6',
            '16. Un paréntesis que cierra y sigue un número. Ejemplo: (3-5)7',
            '17. Un paréntesis que cierra y sigue un punto. Ejemplo: (3-5).',
            '18. Un paréntesis que cierra y sigue una letra. Ejemplo: (3-5)t',
            '19. Un paréntesis que cierra y sigue un paréntesis que abre. Ejemplo: (3-5)(4*5)',
            '20. Hay dos o más letras seguidas (obviando las funciones)',
            '21. Los paréntesis están desbalanceados. Ejemplo: 3-(2*4))',
            '22. Doble punto en un número de tipo real. Ejemplo: 7-6.46.1+2',
            '23. Paréntesis que abre no corresponde con el que cierra. Ejemplo: 2+3)-2*(4',
            '24. Inicia con operador. Ejemplo: +3*5',
            '25. Finaliza con operador. Ejemplo: 3*5*',
            '26. Letra seguida de paréntesis que abre (obviando las funciones). Ejemplo: 4*a(6-2)*',
        ];
        this.EsCorrecto = new Array();
    }
    /* Retorna si el caracter es un operador matemático */
    EvaluaSintaxis.prototype.EsUnOperador = function (car) {
        return car === '+' || car === '-' || car === '*' || car === '/' || car === '^';
    };
    /* Retorna si el caracter es un número */
    EvaluaSintaxis.prototype.EsUnNumero = function (car) {
        return car >= '0' && car <= '9';
    };
    /* Retorna si el caracter es una letra */
    EvaluaSintaxis.prototype.EsUnaLetra = function (car) {
        return car >= 'a' && car <= 'z';
    };
    /* 0. Detecta si hay un caracter no válido */
    EvaluaSintaxis.prototype.BuenaSintaxis00 = function (expresion) {
        var Resultado = true;
        var permitidos = 'abcdefghijklmnopqrstuvwxyz0123456789.+-*/^()';
        for (var pos = 0; pos < expresion.length && Resultado; pos++) {
            if (permitidos.indexOf(expresion[pos] || '') === -1)
                Resultado = false;
        }
        return Resultado;
    };
    /* 1. Un número seguido de una letra. Ejemplo: 2q-(*3) */
    EvaluaSintaxis.prototype.BuenaSintaxis01 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (this.EsUnNumero(carA) && this.EsUnaLetra(carB))
                Resultado = false;
        }
        return Resultado;
    };
    /* 2. Un número seguido de un paréntesis que abre. Ejemplo: 7-2(5-6) */
    EvaluaSintaxis.prototype.BuenaSintaxis02 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (this.EsUnNumero(carA) && carB === '(')
                Resultado = false;
        }
        return Resultado;
    };
    /* 3. Doble punto seguido. Ejemplo: 3..1 */
    EvaluaSintaxis.prototype.BuenaSintaxis03 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (carA === '.' && carB === '.')
                Resultado = false;
        }
        return Resultado;
    };
    /* 4. Punto seguido de operador. Ejemplo: 3.*1 */
    EvaluaSintaxis.prototype.BuenaSintaxis04 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (carA === '.' && this.EsUnOperador(carB))
                Resultado = false;
        }
        return Resultado;
    };
    /* 5. Un punto y sigue una letra. Ejemplo: 3+5.w-8 */
    EvaluaSintaxis.prototype.BuenaSintaxis05 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (carA === '.' && this.EsUnaLetra(carB))
                Resultado = false;
        }
        return Resultado;
    };
    /* 6. Punto seguido de paréntesis que abre. Ejemplo: 2-5.(4+1)*3 */
    EvaluaSintaxis.prototype.BuenaSintaxis06 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (carA === '.' && carB === '(')
                Resultado = false;
        }
        return Resultado;
    };
    /* 7. Punto seguido de paréntesis que cierra. Ejemplo: 2-(5.)*3 */
    EvaluaSintaxis.prototype.BuenaSintaxis07 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (carA === '.' && carB === ')')
                Resultado = false;
        }
        return Resultado;
    };
    /* 8. Un operador seguido de un punto. Ejemplo: 2-(4+.1)-7 */
    EvaluaSintaxis.prototype.BuenaSintaxis08 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (this.EsUnOperador(carA) && carB === '.')
                Resultado = false;
        }
        return Resultado;
    };
    /* 9. Dos operadores estén seguidos. Ejemplo: 2++4, 5-*3 */
    EvaluaSintaxis.prototype.BuenaSintaxis09 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (this.EsUnOperador(carA) && this.EsUnOperador(carB))
                Resultado = false;
        }
        return Resultado;
    };
    /* 10. Un operador seguido de un paréntesis que cierra. Ejemplo: 2-(4+)-7 */
    EvaluaSintaxis.prototype.BuenaSintaxis10 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (this.EsUnOperador(carA) && carB === ')')
                Resultado = false;
        }
        return Resultado;
    };
    /* 11. Una letra seguida de número. Ejemplo: 7-2a-6 */
    EvaluaSintaxis.prototype.BuenaSintaxis11 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (this.EsUnaLetra(carA) && this.EsUnNumero(carB))
                Resultado = false;
        }
        return Resultado;
    };
    /* 12. Una letra seguida de punto. Ejemplo: 7-a.-6 */
    EvaluaSintaxis.prototype.BuenaSintaxis12 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (this.EsUnaLetra(carA) && carB === '.')
                Resultado = false;
        }
        return Resultado;
    };
    /* 13. Un paréntesis que abre seguido de punto. Ejemplo: 7-(.4-6) */
    EvaluaSintaxis.prototype.BuenaSintaxis13 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (carA === '(' && carB === '.')
                Resultado = false;
        }
        return Resultado;
    };
    /* 14. Un paréntesis que abre seguido de un operador. Ejemplo: 2-(*3) */
    EvaluaSintaxis.prototype.BuenaSintaxis14 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (carA === '(' && this.EsUnOperador(carB))
                Resultado = false;
        }
        return Resultado;
    };
    /* 15. Un paréntesis que abre seguido de un paréntesis que cierra. Ejemplo: 7-()-6 */
    EvaluaSintaxis.prototype.BuenaSintaxis15 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (carA === '(' && carB === ')')
                Resultado = false;
        }
        return Resultado;
    };
    /* 16. Un paréntesis que cierra y sigue un número. Ejemplo: (3-5)7 */
    EvaluaSintaxis.prototype.BuenaSintaxis16 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (carA === ')' && this.EsUnNumero(carB))
                Resultado = false;
        }
        return Resultado;
    };
    /* 17. Un paréntesis que cierra y sigue un punto. Ejemplo: (3-5). */
    EvaluaSintaxis.prototype.BuenaSintaxis17 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (carA === ')' && carB === '.')
                Resultado = false;
        }
        return Resultado;
    };
    /* 18. Un paréntesis que cierra y sigue una letra. Ejemplo: (3-5)t */
    EvaluaSintaxis.prototype.BuenaSintaxis18 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (carA === ')' && this.EsUnaLetra(carB))
                Resultado = false;
        }
        return Resultado;
    };
    /* 19. Un paréntesis que cierra y sigue un paréntesis que abre. Ejemplo: (3-5)(4*5) */
    EvaluaSintaxis.prototype.BuenaSintaxis19 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (carA === ')' && carB === '(')
                Resultado = false;
        }
        return Resultado;
    };
    /* 20. Si hay dos letras seguidas (después de quitar las funciones), es un error */
    EvaluaSintaxis.prototype.BuenaSintaxis20 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (this.EsUnaLetra(carA) && this.EsUnaLetra(carB))
                Resultado = false;
        }
        return Resultado;
    };
    /* 21. Los paréntesis estén desbalanceados. Ejemplo: 3-(2*4)) */
    EvaluaSintaxis.prototype.BuenaSintaxis21 = function (expresion) {
        var parabre = 0; /* Contador de paréntesis que abre */
        var parcierra = 0; /* Contador de paréntesis que cierra */
        for (var pos = 0; pos < expresion.length; pos++) {
            var carA = expresion[pos];
            if (carA === '(')
                parabre++;
            if (carA === ')')
                parcierra++;
        }
        return parcierra == parabre;
    };
    /* 22. Doble punto en un número de tipo real. Ejemplo: 7-6.46.1+2 */
    EvaluaSintaxis.prototype.BuenaSintaxis22 = function (expresion) {
        var Resultado = true;
        var totalpuntos = 0; /* Validar los puntos decimales de un número real */
        for (var pos = 0; pos < expresion.length && Resultado; pos++) {
            var carA = expresion[pos];
            if (this.EsUnOperador(carA))
                totalpuntos = 0;
            if (carA === '.')
                totalpuntos++;
            if (totalpuntos > 1)
                Resultado = false;
        }
        return Resultado;
    };
    /* 23. Paréntesis que abre no corresponde con el que cierra. Ejemplo: 2+3)-2*(4'; */
    EvaluaSintaxis.prototype.BuenaSintaxis23 = function (expresion) {
        var Resultado = true;
        var parabre = 0; /* Contador de paréntesis que abre */
        var parcierra = 0; /* Contador de paréntesis que cierra */
        for (var pos = 0; pos < expresion.length && Resultado; pos++) {
            var carA = expresion[pos];
            if (carA === '(')
                parabre++;
            if (carA === ')')
                parcierra++;
            if (parcierra > parabre)
                Resultado = false;
        }
        return Resultado;
    };
    /* 24. Inicia con operador. Ejemplo: +3*5 */
    EvaluaSintaxis.prototype.BuenaSintaxis24 = function (expresion) {
        return !this.EsUnOperador(expresion[0]);
    };
    /* 25. Finaliza con operador. Ejemplo: 3*5* */
    EvaluaSintaxis.prototype.BuenaSintaxis25 = function (expresion) {
        return !this.EsUnOperador(expresion[expresion.length - 1]);
    };
    /* 26. Encuentra una letra seguida de paréntesis que abre. Ejemplo: 3-a(7)-5 */
    EvaluaSintaxis.prototype.BuenaSintaxis26 = function (expresion) {
        var Resultado = true;
        for (var pos = 0; pos < expresion.length - 1 && Resultado; pos++) {
            var carA = expresion[pos];
            var carB = expresion[pos + 1];
            if (this.EsUnaLetra(carA) && carB === '(')
                Resultado = false;
        }
        return Resultado;
    };
    EvaluaSintaxis.prototype.SintaxisCorrecta = function (ecuacion) {
        /* Reemplaza las funciones de tres letras por una letra */
        var expresion = ecuacion.replace(/sen\(/gi, "a+(").replace(/cos\(/gi, "a+(").replace(/tan\(/gi, "a+(").replace(/abs\(/gi, "a+(").replace(/asn\(/gi, "a+(").replace(/acs\(/gi, "a+(").replace(/atn\(/gi, "a+(").replace(/log\(/gi, "a+(").replace(/cei\(/gi, "a+(").replace(/exp\(/gi, "a+(").replace(/sqr\(/gi, "a+(").replace(/rcb\(/gi, "a+(");
        /* Hace las pruebas de sintaxis */
        this.EsCorrecto[0] = this.BuenaSintaxis00(expresion);
        this.EsCorrecto[1] = this.BuenaSintaxis01(expresion);
        this.EsCorrecto[2] = this.BuenaSintaxis02(expresion);
        this.EsCorrecto[3] = this.BuenaSintaxis03(expresion);
        this.EsCorrecto[4] = this.BuenaSintaxis04(expresion);
        this.EsCorrecto[5] = this.BuenaSintaxis05(expresion);
        this.EsCorrecto[6] = this.BuenaSintaxis06(expresion);
        this.EsCorrecto[7] = this.BuenaSintaxis07(expresion);
        this.EsCorrecto[8] = this.BuenaSintaxis08(expresion);
        this.EsCorrecto[9] = this.BuenaSintaxis09(expresion);
        this.EsCorrecto[10] = this.BuenaSintaxis10(expresion);
        this.EsCorrecto[11] = this.BuenaSintaxis11(expresion);
        this.EsCorrecto[12] = this.BuenaSintaxis12(expresion);
        this.EsCorrecto[13] = this.BuenaSintaxis13(expresion);
        this.EsCorrecto[14] = this.BuenaSintaxis14(expresion);
        this.EsCorrecto[15] = this.BuenaSintaxis15(expresion);
        this.EsCorrecto[16] = this.BuenaSintaxis16(expresion);
        this.EsCorrecto[17] = this.BuenaSintaxis17(expresion);
        this.EsCorrecto[18] = this.BuenaSintaxis18(expresion);
        this.EsCorrecto[19] = this.BuenaSintaxis19(expresion);
        this.EsCorrecto[20] = this.BuenaSintaxis20(expresion);
        this.EsCorrecto[21] = this.BuenaSintaxis21(expresion);
        this.EsCorrecto[22] = this.BuenaSintaxis22(expresion);
        this.EsCorrecto[23] = this.BuenaSintaxis23(expresion);
        this.EsCorrecto[24] = this.BuenaSintaxis24(expresion);
        this.EsCorrecto[25] = this.BuenaSintaxis25(expresion);
        this.EsCorrecto[26] = this.BuenaSintaxis26(expresion);
        var Resultado = true;
        for (var cont = 0; cont < this.EsCorrecto.length && Resultado; cont++)
            if (this.EsCorrecto[cont] === false)
                Resultado = false;
        return Resultado;
    };
    /* Transforma la expresión para ser chequeada y analizada */
    EvaluaSintaxis.prototype.Transforma = function (expresion) {
        /* Quita espacios, tabuladores y la vuelve a minúsculas */
        var nuevo = "";
        for (var num = 0; num < expresion.length; num++) {
            var letra = expresion[num];
            if (letra >= 'A' && letra <= 'Z')
                String.fromCharCode(letra.charCodeAt(0) + ' '.charCodeAt(0));
            if (letra != ' ' && letra != '	')
                nuevo += letra;
        }
        return nuevo;
    };
    /* Muestra mensaje de error sintáctico */
    EvaluaSintaxis.prototype.MensajesErrorSintaxis = function (CodigoError) {
        return this._mensajeError[CodigoError];
    };
    return EvaluaSintaxis;
}());
