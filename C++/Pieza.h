class Pieza{
public:
	double ValorPieza; /* Almacena el valor que genera la pieza al evaluarse */
	int Funcion; /* Código de la función 1:seno, 2:coseno, 3:tangente, 4: valor absoluto, 5: arcoseno, 6: arcocoseno, 7: arcotangente, 8: logaritmo natural, 9: valor tope, 10: exponencial, 11: raíz cuadrada, 12: raíz cúbica */
	int TipoA; /* La primera parte es un número o una variable o trae el valor de otra pieza */
	double NumeroA; /* Es un número literal */
	int VariableA; /* Es una variable */
	int PiezaA; /* Trae el valor de otra pieza */
	char Operador; /* + suma - resta * multiplicación / división ^ potencia */
	int TipoB; /* La segunda parte es un número o una variable o trae el valor de otra pieza */
	double NumeroB; /* Es un número literal */
	int VariableB; /* Es una variable */
	int PiezaB; /* Trae el valor de otra pieza */

	Pieza(int funcion, int tipoA, double numeroA, int variableA, int piezaA, char operador, int tipoB, double numeroB, int variableB, int piezaB);
};
