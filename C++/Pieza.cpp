#include "Pieza.h"

/* Constructor */
Pieza::Pieza(int funcion, int tipoA, double numeroA, int variableA, int piezaA, char operador, int tipoB, double numeroB, int variableB, int piezaB) {
	Funcion = funcion;

	TipoA = tipoA;
	NumeroA = numeroA;
	VariableA = variableA;
	PiezaA = piezaA;

	Operador = operador;

	TipoB = tipoB;
	NumeroB = numeroB;
	VariableB = variableB;
	PiezaB = piezaB;
}
