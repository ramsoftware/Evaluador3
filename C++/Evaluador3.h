/* Evaluador de expresiones. Versión 3.0
 * Autor: Rafael Alberto Moreno Parra
 * Fecha: 24 de abril de 2021
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

#include <string>
#include <algorithm>
#include <iostream>
#include <vector>
#include "Parte.h"
#include "Pieza.h"
#include "EvaluaSintaxis.h"

class Evaluador3{
private:
		static const int ESFUNCION = 1;
		static const int ESPARABRE = 2;
		static const int ESPARCIERRA = 3;
		static const int ESOPERADOR = 4;
		static const int ESNUMERO = 5;
		static const int ESVARIABLE = 6;
		static const int ESACUMULA = 7;
		
		/* Listado de partes en que se divide la expresión
		   Toma una expresión, por ejemplo: 3.14 + sen( 4 / x ) * ( 7.2 ^ 3 - 1 ) y la divide en partes así:
		   [3.14] [+] [sen(] [4] [/] [x] [)] [*] [(] [7.2] [^] [3] [-] [1] [)]
		   Cada parte puede tener un número, un operador, una función, un paréntesis que abre o un paréntesis que cierra */
		std::vector<Parte> Partes;
		
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
		std::vector<Pieza> Piezas;

		/* El arreglo unidimensional que lleva el valor de las variables */
		double VariableAlgebra[26];
		
	void CrearPartes(std::string expresion);
	double CadenaAReal(std::string Numero);
	void CrearPiezas();
	void GenerarPiezasOperador(char operA, char operB, int inicia);
	std::string ReplaceAll(std::string Cadena, const std::string& Buscar, const std::string& Reemplazar);
public:
	/* Uso del chequeo de sintaxis */
	EvaluaSintaxis Sintaxis;

	bool Analizar(std::string expresionA);
	double Evaluar();
	void DarValorVariable(char varAlgebra, double valor);
};
