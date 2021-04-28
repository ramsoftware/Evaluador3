/* Evaluador de expresiones. Versión 3.0
 * Autor: Rafael Alberto Moreno Parra
 * Fecha: 24 de abril de 2021
 *
 * Pasos para la evaluación de expresiones algebraicas
 * I. Toma una expresión, por ejemplo: 3.14 + sen( 4 / x ) * ( 7.2 ^ 3 - 1 ) y la divide en partes así­:
 *	[3.14] [+] [sen(] [4] [/] [x] [)] [*] [(] [7.2] [^] [3] [-] [1] [)]
 *
 * II. Toma las partes y las divide en piezas con la siguiente estructura:
 *		acumula = funcion  numero/variable/acumula  operador  numero/variable/acumula
 *		Siguiendo el ejemplo anterior serí­a:
 *		A = 7.2  ^  3
 *		B = A  -  1
 *		C = seno ( 4  /  x )
 *		D = C  *  B
 *		E = 3.14  +  D
 *
 *		Esas piezas se evalúan de arriba a abajo y así­ se interpreta la ecuación
 *
 * */
#include "Evaluador3.h"
#include <string.h>
#include <math.h>
#include <iostream>
#include <string>

 /* Analiza la expresión */
bool Evaluador3::Analizar(std::string expresionA) {
	std::string expresionB = Sintaxis.Transforma(expresionA);
	bool chequeo = Sintaxis.SintaxisCorrecta(expresionB);
	if (chequeo) {
		Partes.clear();
		Piezas.clear();
		CrearPartes(expresionB);
		CrearPiezas();
	}
	return chequeo;
}

/* Divide la expresión en partes */
void Evaluador3::CrearPartes(std::string expresion) {
	/* Reemplaza las funciones de tres letras por una letra mayúscula */
	std::string NuevoA = "(" + expresion + ")";
	std::string NuevoB = ReplaceAll(NuevoA, "sen", "A");
	NuevoB = ReplaceAll(NuevoB, "cos", "B");
	NuevoB = ReplaceAll(NuevoB, "tan", "C");
	NuevoB = ReplaceAll(NuevoB, "abs", "D");
	NuevoB = ReplaceAll(NuevoB, "asn", "E");
	NuevoB = ReplaceAll(NuevoB, "acs", "F");
	NuevoB = ReplaceAll(NuevoB, "atn", "G");
	NuevoB = ReplaceAll(NuevoB, "log", "H");
	NuevoB = ReplaceAll(NuevoB, "cei", "I");
	NuevoB = ReplaceAll(NuevoB, "exp", "J");
	NuevoB = ReplaceAll(NuevoB, "sqr", "K");
	NuevoB = ReplaceAll(NuevoB, "rcb", "L");

	std::string Numero = "";
	for (int pos = 0; pos < NuevoB.length(); pos++) {
		char car = NuevoB[pos];
		/* Si es un número lo va acumulando en una cadena */
		if ((car >= '0' && car <= '9') || car == '.') {
			Numero += car;
		}
		/* Si es un operador entonces agrega número (si existí­a) */
		else if (car == '+' || car == '-' || car == '*' || car == '/' || car == '^') {
			if (Numero.length() > 0) {
				Parte objeto(ESNUMERO, -1, '0', CadenaAReal(Numero), 0);
				Partes.push_back(objeto);
				Numero = "";
			}
			Parte objeto(ESOPERADOR, -1, car, 0, 0);
			Partes.push_back(objeto);
		}
		/* Si es variable */
		else if (car >= 'a' && car <= 'z') {
			Parte objeto(ESVARIABLE, -1, '0', 0, car - 'a');
			Partes.push_back(objeto);
		}
		/* Si es una función (seno, coseno, tangente, ...) */
		else if (car >= 'A' && car <= 'L') {
			Parte objeto(ESFUNCION, car - 'A', '0', 0, 0);
			Partes.push_back(objeto);
			pos++;
		}
		/* Si es un paréntesis que abre */
		else if (car == '(') {
			Parte objeto(ESPARABRE, -1, '0', 0, 0);
			Partes.push_back(objeto);
		}
		/* Si es un paréntesis que cierra */
		else {
			if (Numero.length() > 0) {
				Parte objeto(ESNUMERO, -1, '0', CadenaAReal(Numero), 0);
				Partes.push_back(objeto);
				Numero = "";
			}
			/* Si sólo había un número o variable dentro del paréntesis le agrega + 0 */
			if (Partes[Partes.size() - 2].Tipo == ESPARABRE || Partes[Partes.size() - 2].Tipo == ESFUNCION) {
				Parte objeto(ESOPERADOR, -1, '+', 0, 0);
				Partes.push_back(objeto);
				Parte objetoB(ESNUMERO, -1, '0', 0, 0);
				Partes.push_back(objetoB);
			}
			Parte objeto(ESPARCIERRA, -1, '0', 0, 0);
			Partes.push_back(objeto);
		}
	}
}

/* Reemplaza todas las ocurrencias de Buscar por Reemplazar */
std::string Evaluador3::ReplaceAll(std::string Cadena, const std::string& Buscar, const std::string& Reemplazar) {
	size_t posicion = 0;
	while ((posicion = Cadena.find(Buscar, posicion)) != std::string::npos) {
		Cadena.replace(posicion, Buscar.length(), Reemplazar);
		posicion += Reemplazar.length();
	}
	return Cadena;
}

/* Convierte un número almacenado en una cadena a su valor real */
double Evaluador3::CadenaAReal(std::string Numero) {
	/* Parte entera */
	double parteEntera = 0;
	int cont;
	for (cont = 0; cont < Numero.length(); cont++) {
		if (Numero[cont] == '.') break;
		parteEntera = parteEntera * 10 + (Numero[cont] - '0');
	}

	/* Parte decimal */
	double parteDecimal = 0;
	double multiplica = 1;
	for (int num = cont + 1; num < Numero.length(); num++) {
		parteDecimal = parteDecimal * 10 + (Numero[num] - '0');
		multiplica *= 10;
	}

	double numero = parteEntera + parteDecimal / multiplica;
	return numero;
}

/* Ahora convierte las partes en las piezas finales de ejecución */
void Evaluador3::CrearPiezas() {
	int cont = Partes.size() - 1;
	do {
		Parte tmpParte = Partes[cont];
		if (tmpParte.Tipo == ESPARABRE || tmpParte.Tipo == ESFUNCION) {
			GenerarPiezasOperador('^', '^', cont);  /* Evalúa las potencias */
			GenerarPiezasOperador('*', '/', cont);  /* Luego evalúa multiplicar y dividir */
			GenerarPiezasOperador('+', '-', cont);  /* Finalmente evalúa sumar y restar */

			if (tmpParte.Tipo == ESFUNCION) { /* Agrega la función a la última pieza */
				Piezas[Piezas.size() - 1].Funcion = tmpParte.Funcion;
			}

			/* Quita el paréntesis/función que abre y el que cierra, dejando el centro */
			Partes.erase(Partes.begin() + cont);
			Partes.erase(Partes.begin() + cont + 1);
		}
		cont--;
	} while (cont >= 0);
}

/* Genera las piezas buscando determinado operador */
void Evaluador3::GenerarPiezasOperador(char operA, char operB, int inicia) {
	int cont = inicia + 1;
	do {
		if (Partes[cont].Tipo == ESOPERADOR && (Partes[cont].Operador == operA || Partes[cont].Operador == operB)) {

			/* Crea Pieza */
			Pieza objeto(-1,
				Partes[cont - 1].Tipo, Partes[cont - 1].Numero,
				Partes[cont - 1].UnaVariable, Partes[cont - 1].Acumulador,
				Partes[cont].Operador,
				Partes[cont + 1].Tipo, Partes[cont + 1].Numero,
				Partes[cont + 1].UnaVariable, Partes[cont + 1].Acumulador);
			Piezas.push_back(objeto);

			/* Elimina la parte del operador y la siguiente */
			Partes.erase(Partes.begin() + cont);
			Partes.erase(Partes.begin() + cont);

			/* Cambia la parte anterior por parte que acumula */
			Partes[cont - 1].Tipo = ESACUMULA;
			Partes[cont - 1].Acumulador = Piezas.size() - 1;

			/* Retorna el contador en uno para tomar la siguiente operación */
			cont--;
		}
		cont++;
	} while (Partes[cont].Tipo != ESPARCIERRA);
}

/* Evalúa la expresión convertida en piezas */
double Evaluador3::Evaluar() {
	double resultado = 0;

	for (int pos = 0; pos < Piezas.size(); pos++) {
		double numA, numB;

		switch (Piezas[pos].TipoA) {
		case ESNUMERO: numA = Piezas[pos].NumeroA; break;
		case ESVARIABLE: numA = VariableAlgebra[Piezas[pos].VariableA]; break;
		default: numA = Piezas[Piezas[pos].PiezaA].ValorPieza; break;
		}

		switch (Piezas[pos].TipoB) {
		case ESNUMERO: numB = Piezas[pos].NumeroB; break;
		case ESVARIABLE: numB = VariableAlgebra[Piezas[pos].VariableB]; break;
		default: numB = Piezas[Piezas[pos].PiezaB].ValorPieza; break;
		}

		switch (Piezas[pos].Operador) {
		case '*': resultado = numA * numB; break;
		case '/': resultado = numA / numB; break;
		case '+': resultado = numA + numB; break;
		case '-': resultado = numA - numB; break;
		default: resultado = pow(numA, numB); break;
		}

		if (_isnan(resultado) || !_finite(resultado)) return resultado;

		switch (Piezas[pos].Funcion) {
		case 0: resultado = sin(resultado); break;
		case 1: resultado = cos(resultado); break;
		case 2: resultado = tan(resultado); break;
		case 3: resultado = abs(resultado); break;
		case 4: resultado = asin(resultado); break;
		case 5: resultado = acos(resultado); break;
		case 6: resultado = atan(resultado); break;
		case 7: resultado = log(resultado); break;
		case 8: resultado = ceil(resultado); break;
		case 9: resultado = exp(resultado); break;
		case 10: resultado = sqrt(resultado); break;
		case 11: resultado = pow(resultado, 0.3333333333333333333333); break;
		}
		if (_isnan(resultado) || !_finite(resultado)) return resultado;

		Piezas[pos].ValorPieza = resultado;
	}
	return resultado;
}

/* Da valor a las variables que tendrá la expresión algebraica */
void Evaluador3::DarValorVariable(char varAlgebra, double valor) {
	VariableAlgebra[varAlgebra - 'a'] = valor;
}
