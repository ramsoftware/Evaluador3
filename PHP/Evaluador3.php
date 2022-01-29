<?php
/* Evaluador de expresiones. Versión 3.0
 * Autor: Rafael Alberto Moreno Parra
 * Fecha: 25 de marzo de 2021
 * 
 * Pasos para la evaluación de expresiones algebraicas
 * I. Toma una expresión, por ejemplo: 3.14 + sen( 4 / x ) * ( 7.2 ^ 3 - 1 ) y la divide en partes así:
 *	[3.14] [+] [sen(] [4] [/] [x] [)] [*] [(] [7.2] [^] [3] [-] [1] [)]
 *
 * II. Toma las partes y las divide en piezas con la siguiente estructura:
 *	acumula = funcion  numero/variable/acumula  operador  numero/variable/acumula
 *	Siguiendo el ejemplo anterior sería:
 *	A = 7.2  ^  3
 *	B = A  -  1
 *	C = seno ( 4  /  x )
 *	D = C  *  B
 *	E = 3.14  +  D
 *
 *	Esas piezas se evalúan de arriba a abajo y así se interpreta la ecuación
 *
 * */
require_once("EvaluaSintaxis.php");

class Evaluador3 {
	/* Constantes de los diferentes tipos de datos que tendrán las piezas */
	var $ESFUNCION = 1;
	var $ESPARABRE = 2;
	var $ESPARCIERRA = 3;
	var $ESOPERADOR = 4;
	var $ESNUMERO = 5;
	var $ESVARIABLE = 6;
	var $ESACUMULA = 7;

	/* Listado de partes en que se divide la expresión
	   Toma una expresión, por ejemplo: 3.14 + sen( 4 / x ) * ( 7.2 ^ 3 - 1 ) y la divide en partes así:
	   [3.14] [+] [sen(] [4] [/] [x] [)] [*] [(] [7.2] [^] [3] [-] [1] [)]
	   Cada parte puede tener un número, un operador, una función, un paréntesis que abre o un paréntesis que cierra */
	var $Partes = array();

	/* Listado de piezas que ejecutan
		Toma las partes y las divide en piezas con la siguiente estructura:
		acumula = funcion  numero/variable/acumula  operador  numero/variable/acumula
		Siguiendo el ejemplo anterior sería:
		A =  7.2  ^  3
		B =  A  -  1
		C =  seno ( 4  /  x )
		D =  C  *  B
		E =  3.14 + D

	   Esas piezas se evalúan de arriba a abajo y así se interpreta la ecuación */
	var $Piezas = array();

	/* El arreglo unidimensional que lleva el valor de las variables */
	var $VariableAlgebra = array();
	
	/* Uso del chequeo de sintaxis */
	var $Sintaxis;

	/* Analiza la expresión */
	public function Analizar($expresionA) {
		$this->Sintaxis = new EvaluaSintaxis();
		$expresionB = $this->Sintaxis->Transforma($expresionA);
		$chequeo = $this->Sintaxis->SintaxisCorrecta($expresionB);
		if ($chequeo) {
			unset($this->Partes);
			unset($this->Piezas);
			$this->CrearPartes($expresionB);
			$this->CrearPiezas();
		}
		return $chequeo;
	}

	/* Divide la expresión en partes */
	function CrearPartes($expresion) {
		/* Debe analizarse con paréntesis */
		$NuevoA = "(" . $expresion . ")";

		/* Reemplaza las funciones de tres letras por una letra mayúscula */
		$NuevoB = str_replace("sen", "A", $NuevoA);
		$NuevoB = str_replace("cos", "B", $NuevoB);
		$NuevoB = str_replace("tan", "C", $NuevoB);
		$NuevoB = str_replace("abs", "D", $NuevoB);
		$NuevoB = str_replace("asn", "E", $NuevoB);
		$NuevoB = str_replace("acs", "F", $NuevoB);
		$NuevoB = str_replace("atn", "G", $NuevoB);
		$NuevoB = str_replace("log", "H", $NuevoB);
		$NuevoB = str_replace("cei", "I", $NuevoB);
		$NuevoB = str_replace("exp", "J", $NuevoB);
		$NuevoB = str_replace("sqr", "K", $NuevoB);
		$NuevoB = str_replace("rcb", "L", $NuevoB);

		/* Va de caracter en caracter */
		$Numero = "";
		for ($pos = 0; $pos < strlen($NuevoB); $pos++) {
			$car = $NuevoB[$pos];
			/* Si es un número lo va acumulando en una cadena */
			if (($car >= '0' && $car <= '9') || $car === '.') {
				$Numero = $Numero . $car;
			}
			/* Si es un operador entonces agrega número (si existía) */
			else if ($car === '+' || $car === '-' || $car === '*' || $car === '/' || $car === '^') {
				if (strlen($Numero) > 0) {
					$objeto = new Parte($this->ESNUMERO, -1, '0', $this->CadenaAReal($Numero), 0);
					$this->Partes[] = $objeto;
					$Numero = "";
				}
				$objeto = new Parte($this->ESOPERADOR, -1, $car, 0, 0);
				$this->Partes[] = $objeto;
			}
			/* Si es variable */
			else if ($car >= 'a' && $car <= 'z') {
				$objeto = new Parte($this->ESVARIABLE, -1, '0', 0, ord($car) - ord('a'));
				$this->Partes[] = $objeto;
			}
			/* Si es una función (seno, coseno, tangente, ...) */
			else if ($car >= 'A' && $car <= 'L') {
				$objeto = new Parte($this->ESFUNCION, ord($car) - ord('A'), '0', 0, 0);
				$this->Partes[] = $objeto;
				$pos++;
			}
			/* Si es un paréntesis que abre */
			else if ($car === '(') {
				$objeto = new Parte($this->ESPARABRE, -1, '0', 0, 0);
				$this->Partes[] = $objeto;
			}
			/* Si es un paréntesis que cierra */
			else {
				if (strlen($Numero) > 0) {
					$objeto = new Parte($this->ESNUMERO, -1, '0', $this->CadenaAReal($Numero), 0);
					$this->Partes[] = $objeto;
					$Numero = "";
				}
				/* Si sólo había un número o variable dentro del paréntesis le agrega + 0 (por ejemplo:  sen(x) o 3*(2) ) */
				if ($this->Partes[sizeof($this->Partes) - 2]->Tipo == $this->ESPARABRE || $this->Partes[sizeof($this->Partes) - 2]->Tipo == $this->ESFUNCION) {
					$objeto = new Parte($this->ESOPERADOR, -1, '+', 0, 0);
					$this->Partes[] = $objeto;
					$objeto = new Parte($this->ESNUMERO, -1, '0', 0, 0);
					$this->Partes[] = $objeto;
				}

				$objeto = new Parte($this->ESPARCIERRA, -1, '0', 0, 0);
				$this->Partes[] = $objeto;
			}
		}
	}
	
	/* Convierte un número almacenado en una cadena a su valor real */
	function CadenaAReal($Numero) {
		//Parte entera
		$parteEntera = 0;
		$cont = 0;
		for ($cont = 0; $cont < strlen($Numero); $cont++) {
			if ($Numero[$cont] === '.') break;
			$parteEntera = $parteEntera * 10 + (ord($Numero[$cont]) - ord('0'));
		}

		//Parte decimal
		$parteDecimal = 0;
		$multiplica = 1;
		for ($num = $cont + 1; $num < strlen($Numero); $num++) {
			$parteDecimal = $parteDecimal * 10 + (ord($Numero[$num]) - ord('0'));
			$multiplica *= 10;
		}

		$numero = $parteEntera + $parteDecimal / $multiplica;
		return $numero;
	}

	/* Ahora convierte las partes en las piezas finales de ejecución */
	function CrearPiezas() {
		$cont = sizeof($this->Partes) - 1;
		do {
			$tmpParte = $this->Partes[$cont];
			if ($tmpParte->Tipo == $this->ESPARABRE || $tmpParte->Tipo == $this->ESFUNCION) {
				$this->GenerarPiezasOperador('^', '^', $cont);  /* Evalúa las potencias */
				$this->GenerarPiezasOperador('*', '/', $cont);  /* Luego evalúa multiplicar y dividir */
				$this->GenerarPiezasOperador('+', '-', $cont);  /* Finalmente evalúa sumar y restar */

				if ($tmpParte->Tipo == $this->ESFUNCION) { /* Agrega la función a la última pieza */
					$this->Piezas[sizeof($this->Piezas) - 1]->Funcion = $tmpParte->Funcion;
				}

				/* Quita el paréntesis/función que abre y el que cierra, dejando el centro */
				unset($this->Partes[$cont]);
				$this->Partes = array_values($this->Partes);
				unset($this->Partes[$cont+1]);
				$this->Partes = array_values($this->Partes);
			}
			$cont--;
		} while ($cont >= 0);
	}

	/* Genera las piezas buscando determinado operador */
	function GenerarPiezasOperador($operA, $operB, $inicia) {
		$cont = $inicia + 1;
		do {
			$tmpParte = $this->Partes[$cont];
			if ($tmpParte->Tipo == $this->ESOPERADOR && ($tmpParte->Operador == $operA || $tmpParte->Operador == $operB)) {
				$tmpParteIzq = $this->Partes[$cont - 1];
				$tmpParteDer = $this->Partes[$cont + 1];
				
				/* Crea Pieza */
				$objeto = new Pieza(-1,
					$tmpParteIzq->Tipo, $tmpParteIzq->Numero,
					$tmpParteIzq->UnaVariable, $tmpParteIzq->Acumulador,
					$tmpParte->Operador,
					$tmpParteDer->Tipo, $tmpParteDer->Numero,
					$tmpParteDer->UnaVariable, $tmpParteDer->Acumulador);
				$this->Piezas[] = $objeto;

				/* Elimina la parte del operador y la siguiente */
				unset($this->Partes[$cont]);
				$this->Partes = array_values($this->Partes);
				unset($this->Partes[$cont]);
				$this->Partes = array_values($this->Partes);

				/* Cambia la parte anterior por parte que acumula */
				$tmpParteIzq->Tipo = $this->ESACUMULA;
				$tmpParteIzq->Acumulador = sizeof($this->Piezas) - 1;
				
				/* Retorna el contador en uno para tomar la siguiente operación */
				$cont--;
			}
			$cont++;
		} while ($this->Partes[$cont]->Tipo != $this->ESPARCIERRA);
	}

	/* Evalúa la expresión convertida en piezas */
	public function Evaluar() {
		$resultado = 0;
		for ($pos = 0; $pos < sizeof($this->Piezas); $pos++) {
			$tmpPieza = $this->Piezas[$pos];
			$numA=0;
			$numB=0;
			
			switch ($tmpPieza->TipoA) {
				case $this->ESNUMERO: $numA = $tmpPieza->NumeroA; break;
				case $this->ESVARIABLE: $numA = $this->VariableAlgebra[$tmpPieza->VariableA]; break;
				default: $numA = $this->Piezas[$tmpPieza->PiezaA]->ValorPieza; break;
			}

			switch ($tmpPieza->TipoB) {
				case $this->ESNUMERO: $numB = $tmpPieza->NumeroB; break;
				case $this->ESVARIABLE: $numB = $this->VariableAlgebra[$tmpPieza->VariableB]; break;
				default: $numB = $this->Piezas[$tmpPieza->PiezaB]->ValorPieza; break;
			}

			switch ($tmpPieza->Operador) {
				case '*': $resultado = $numA * $numB; break;
				case '/': if ($numB == 0) return NAN; $resultado = $numA / $numB; break;
				case '+': $resultado = $numA + $numB; break;
				case '-': $resultado = $numA - $numB; break;
				default: $resultado = pow($numA, $numB); break;
			}

			if (is_nan($resultado) || is_infinite($resultado)) return $resultado;

			switch ($tmpPieza->Funcion) {
				case 0: $resultado = sin($resultado); break;
				case 1: $resultado = cos($resultado); break;
				case 2: $resultado = tan($resultado); break;
				case 3: $resultado = abs($resultado); break;
				case 4: $resultado = asin($resultado); break;
				case 5: $resultado = acos($resultado); break;
				case 6: $resultado = atan($resultado); break;
				case 7: $resultado = log($resultado); break;
				case 8: $resultado = ceil($resultado); break;
				case 9: $resultado = exp($resultado); break;
				case 10: $resultado = sqrt($resultado); break;
				case 11: $resultado = pow($resultado, 0.3333333333333333333333); break;
			}

			if (is_nan($resultado) || is_infinite($resultado)) return $resultado;

			$tmpPieza->ValorPieza = $resultado;
		}
		return $resultado;
	}

	/* Da valor a las variables que tendrá la expresión algebraica */
	public function DarValorVariable($varAlgebra, $valor) {
		$this->VariableAlgebra[ord($varAlgebra) - ord('a')] = $valor;
	}
}

	/* ***************************************************************************************************************** */

class Parte {
	public $Tipo; /* Acumulador, función, paréntesis que abre, paréntesis que cierra, operador, número, variable */
	public $Funcion; /* Código de la función 0:seno, 1:coseno, 2:tangente, 3: valor absoluto, 4: arcoseno, 5: arcocoseno, 6: arcotangente, 7: logaritmo natural, 8: valor tope, 9: exponencial, 10: raíz cuadrada, 11: raíz cúbica */
	public $Operador; /* + suma - resta * multiplicación / división ^ potencia */
	public $Numero; /* Número literal, por ejemplo: 3.141592 */
	public $UnaVariable; /* Variable algebraica */
	public $Acumulador; /* Usado cuando la expresión se convierte en piezas. Por ejemplo:
				3 + 2 / 5  se convierte así:
				|3| |+| |2| |/| |5| 
				|3| |+| |A|  A es un identificador de acumulador */

	function __construct($tipo, $funcion, $operador, $numero, $unaVariable) {
		$this->Tipo = $tipo;
		$this->Funcion = $funcion;
		$this->Operador = $operador;
		$this->Numero = $numero;
		$this->UnaVariable = $unaVariable;
		$this->Acumulador = 0;
	}
}

	/* ***************************************************************************************************************** */

class Pieza {
	public $ValorPieza; /* Almacena el valor que genera la pieza al evaluarse */
	public $Funcion; /* Código de la función 0:seno, 1:coseno, 2:tangente, 3: valor absoluto, 4: arcoseno, 5: arcocoseno, 6: arcotangente, 7: logaritmo natural, 8: valor tope, 9: exponencial, 10: raíz cuadrada, 11: raíz cúbica */
	public $TipoA; /* La primera parte es un número o una variable o trae el valor de otra pieza */
	public $NumeroA; /* Es un número literal */
	public $VariableA; /* Es una variable */
	public $PiezaA; /* Trae el valor de otra pieza */
	public $Operador; /* + suma - resta * multiplicación / división ^ potencia */
	public $TipoB; /* La segunda parte es un número o una variable o trae el valor de otra pieza */
	public $NumeroB; /* Es un número literal */
	public $VariableB; /* Es una variable */
	public $PiezaB; /* Trae el valor de otra pieza */

	function __construct($funcion, $tipoA, $numeroA, $variableA, $piezaA, $operador, $tipoB, $numeroB, $variableB, $piezaB) {
		$this->Funcion = $funcion;

		$this->TipoA = $tipoA;
		$this->NumeroA = $numeroA;
		$this->VariableA = $variableA;
		$this->PiezaA = $piezaA;

		$this->Operador = $operador;

		$this->TipoB = $tipoB;
		$this->NumeroB = $numeroB;
		$this->VariableB = $variableB;
		$this->PiezaB = $piezaB;
		
		$this->ValorPieza = 0;
	}
}
