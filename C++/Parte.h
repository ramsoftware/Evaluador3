class Parte{
public:
	int Tipo; /* Acumulador, función, paréntesis que abre, paréntesis que cierra, operador, número, variable */
	int Funcion; /* Código de la función 1:seno, 2:coseno, 3:tangente, 4: valor absoluto, 5: arcoseno, 6: arcocoseno, 7: arcotangente, 8: logaritmo natural, 9: valor tope, 10: exponencial, 11: raíz cuadrada, 12: raíz cúbica */
	char Operador; /* + suma - resta * multiplicación / división ^ potencia */
	double Numero; /* Número literal, por ejemplo: 3.141592 */
	int UnaVariable; /* Variable algebraica */
	int Acumulador; /* Usado cuando la expresión se convierte en piezas. Por ejemplo:
							3 + 2 / 5  se convierte así:
							|3| |+| |2| |/| |5| 
							|3| |+| |A|  A es un identificador de acumulador */
	Parte(int tipo, int funcion, char operador, double numero, int unaVariable);
};
