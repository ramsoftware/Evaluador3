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
package com.company;

import java.util.ArrayList;

public class Evaluador3 {
	/* Constantes de los diferentes tipos de datos que tendrán las piezas */
	private static final int ESFUNCION = 1;
	private static final int ESPARABRE = 2;
	private static final int ESPARCIERRA = 3;
	private static final int ESOPERADOR = 4;
	private static final int ESNUMERO = 5;
	private static final int ESVARIABLE = 6;
	private static final int ESACUMULA = 7;

	/* Listado de partes en que se divide la expresión
	   Toma una expresión, por ejemplo: 3.14 + sen( 4 / x ) * ( 7.2 ^ 3 - 1 ) y la divide en partes así:
	   [3.14] [+] [sen(] [4] [/] [x] [)] [*] [(] [7.2] [^] [3] [-] [1] [)]
	   Cada parte puede tener un número, un operador, una función, un paréntesis que abre o un paréntesis que cierra */
	private ArrayList<Parte> Partes = new ArrayList<Parte>();

	/* ArrayListado de piezas que ejecutan
		Toma las partes y las divide en piezas con la siguiente estructura:
		acumula = funcion  numero/variable/acumula  operador  numero/variable/acumula
		Siguiendo el ejemplo anterior sería:
		A =  7.2  ^  3
		B =  A  -  1
		C =  seno ( 4  /  x )
		D =  C  *  B
		E =  3.14 + D

	   Esas piezas se evalúan de arriba a abajo y así se interpreta la ecuación */
	private ArrayList<Pieza> Piezas = new ArrayList<Pieza>();

	/* El arreglo unidimensional que lleva el valor de las variables */
	private double[] VariableAlgebra = new double[26];

	/* Uso del chequeo de sintaxis */
	public EvaluaSintaxis Sintaxis = new EvaluaSintaxis();

	/* Analiza la expresión */
	public Boolean Analizar(String expresionA) {
		String expresionB = Sintaxis.Transforma(expresionA);
		Boolean chequeo = Sintaxis.SintaxisCorrecta(expresionB);
		if (chequeo) {
			Partes.clear();
			Piezas.clear();
			CrearPartes(expresionB);
			CrearPiezas();
		}
		return chequeo;
	}

	/* Divide la expresión en partes */
	private void CrearPartes(String expresion) {
		/* Debe analizarse con paréntesis */
		String NuevoA = "(" + expresion + ")";

		/* Reemplaza las funciones de tres letras por una letra mayúscula */
		String NuevoB = NuevoA.replace("sen", "A").replace("cos", "B").replace("tan", "C").replace("abs", "D").replace("asn", "E").replace("acs", "F").replace("atn", "G").replace("log", "H").replace("cei", "I").replace("exp", "J").replace("sqr", "K").replace("rcb", "L");

		/* Va de caracter en caracter */
		String Numero = "";
		for (int pos = 0; pos < NuevoB.length(); pos++) {
			char car = NuevoB.charAt(pos);
			/* Si es un número lo va acumulando en una cadena */
			if ((car >= '0' && car <= '9') || car == '.') {
				Numero += car;
			}
			/* Si es un operador entonces agrega número (si existía) */
			else if (car == '+' || car == '-' || car == '*' || car == '/' || car == '^') {
				if (Numero.length() > 0) {
					Partes.add(new Parte(ESNUMERO, -1, '0', CadenaAReal(Numero), 0));
					Numero = "";
				}
				Partes.add(new Parte(ESOPERADOR, -1, car, 0, 0));
			}
			/* Si es variable */
			else if (car >= 'a' && car <= 'z') {
				Partes.add(new Parte(ESVARIABLE, -1, '0', 0, car - 'a'));
			}
			/* Si es una función (seno, coseno, tangente, ...) */
			else if (car >= 'A' && car <= 'L') {
				Partes.add(new Parte(ESFUNCION, car - 'A', '0', 0, 0));
				pos++;
			}
			/* Si es un paréntesis que abre */
			else if (car == '(') {
				Partes.add(new Parte(ESPARABRE, -1, '0', 0, 0));
			}
			/* Si es un paréntesis que cierra */
			else {
				if (Numero.length() > 0) {
					Partes.add(new Parte(ESNUMERO, -1, '0', CadenaAReal(Numero), 0));
					Numero = "";
				}
				/* Si sólo había un número o variable dentro del paréntesis le agrega + 0 (por ejemplo:  sen(x) o 3*(2) ) */
				if (Partes.get(Partes.size() - 2).Tipo == ESPARABRE || Partes.get(Partes.size() - 2).Tipo == ESFUNCION) {
					Partes.add(new Parte(ESOPERADOR, -1, '+', 0, 0));
					Partes.add(new Parte(ESNUMERO, -1, '0', 0, 0));
				}

				Partes.add(new Parte(ESPARCIERRA, -1, '0', 0, 0));
			}
		}
	}

	/* Convierte un número almacenado en una cadena a su valor real */
	private double CadenaAReal(String Numero) {
		//Parte entera
		double parteEntera = 0;
		int cont;
		for (cont = 0; cont < Numero.length(); cont++) {
			if (Numero.charAt(cont) == '.') break;
			parteEntera = parteEntera * 10 + (Numero.charAt(cont) - '0');
		}

		//Parte decimal
		double parteDecimal = 0;
		double multiplica = 1;
		for (int num = cont + 1; num < Numero.length(); num++) {
			parteDecimal = parteDecimal * 10 + (Numero.charAt(num) - '0');
			multiplica *= 10;
		}

		double numero = parteEntera + parteDecimal / multiplica;
		return numero;
	}

	/* Ahora convierte las partes en las piezas finales de ejecución */
	private void CrearPiezas() {
		int cont = Partes.size() - 1;
		do {
			Parte tmpParte = Partes.get(cont);
			if (tmpParte.Tipo == ESPARABRE || tmpParte.Tipo == ESFUNCION) {
				GenerarPiezasOperador('^', '^', cont);  /* Evalúa las potencias */
				GenerarPiezasOperador('*', '/', cont);  /* Luego evalúa multiplicar y dividir */
				GenerarPiezasOperador('+', '-', cont);  /* Finalmente evalúa sumar y restar */

				if (tmpParte.Tipo == ESFUNCION) { /* Agrega la función a la última pieza */
					Piezas.get(Piezas.size() - 1).Funcion = tmpParte.Funcion;
				}

				/* Quita el paréntesis/función que abre y el que cierra, dejando el centro */
				Partes.remove(cont);
				Partes.remove(cont + 1);
			}
			cont--;
		} while (cont >= 0);
	}

	/* Genera las piezas buscando determinado operador */
	private void GenerarPiezasOperador(char operA, char operB, int inicia) {
		int cont = inicia + 1;
		do {
			Parte tmpParte = Partes.get(cont);
			if (tmpParte.Tipo == ESOPERADOR && (tmpParte.Operador == operA || tmpParte.Operador == operB)) {
				Parte tmpParteIzq = Partes.get(cont - 1);
				Parte tmpParteDer = Partes.get(cont + 1);

				/* Crea Pieza */
				Piezas.add(new Pieza(-1,
						tmpParteIzq.Tipo, tmpParteIzq.Numero,
						tmpParteIzq.UnaVariable, tmpParteIzq.Acumulador,
						tmpParte.Operador,
						tmpParteDer.Tipo, tmpParteDer.Numero,
						tmpParteDer.UnaVariable, tmpParteDer.Acumulador));

				/* Elimina la parte del operador y la siguiente */
				Partes.remove(cont);
				Partes.remove(cont);

				/* Retorna el contador en uno para tomar la siguiente operación */
				cont--;

				/* Cambia la parte anterior por parte que acumula */
				tmpParteIzq.Tipo = ESACUMULA;
				tmpParteIzq.Acumulador = Piezas.size()-1;
			}
			cont++;
		} while (Partes.get(cont).Tipo != ESPARCIERRA);
	}

	/* Evalúa la expresión convertida en piezas */
	public double Evaluar() {
		double resultado = 0;

		for (int pos = 0; pos < Piezas.size(); pos++) {
			Pieza tmpPieza = Piezas.get(pos);
			double numA, numB;

			switch (tmpPieza.TipoA) {
				case ESNUMERO: numA = tmpPieza.NumeroA; break;
				case ESVARIABLE: numA = VariableAlgebra[tmpPieza.VariableA]; break;
				default: numA = Piezas.get(tmpPieza.PiezaA).ValorPieza; break;
			}

			switch (tmpPieza.TipoB) {
				case ESNUMERO: numB = tmpPieza.NumeroB; break;
				case ESVARIABLE: numB = VariableAlgebra[tmpPieza.VariableB]; break;
				default: numB = Piezas.get(tmpPieza.PiezaB).ValorPieza; break;
			}

			switch (tmpPieza.Operador) {
				case '*': resultado = numA * numB; break;
				case '/': resultado = numA / numB; break;
				case '+': resultado = numA + numB; break;
				case '-': resultado = numA - numB; break;
				default: resultado = Math.pow(numA, numB); break;
			}

			if (Double.isNaN(resultado) || Double.isInfinite(resultado)) return resultado;

			switch (tmpPieza.Funcion) {
				case 0: resultado = Math.sin(resultado); break;
				case 1: resultado = Math.cos(resultado); break;
				case 2: resultado = Math.tan(resultado); break;
				case 3: resultado = Math.abs(resultado); break;
				case 4: resultado = Math.asin(resultado); break;
				case 5: resultado = Math.acos(resultado); break;
				case 6: resultado = Math.atan(resultado); break;
				case 7: resultado = Math.log(resultado); break;
				case 8: resultado = Math.ceil(resultado); break;
				case 9: resultado = Math.exp(resultado); break;
				case 10: resultado = Math.sqrt(resultado); break;
				case 11: resultado = Math.pow(resultado, 0.3333333333333333333333); break;
			}

			if (Double.isNaN(resultado) || Double.isInfinite(resultado)) return resultado;

			tmpPieza.ValorPieza = resultado;
		}
		return resultado;
	}

	/* Da valor a las variables que tendrá la expresión algebraica */
	public void DarValorVariable(char varAlgebra, double valor) {
		VariableAlgebra[varAlgebra - 'a'] = valor;
	}
}
