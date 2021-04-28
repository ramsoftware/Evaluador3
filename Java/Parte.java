package com.company;

public class Parte {
	public int Tipo; /* Acumulador, función, paréntesis que abre, paréntesis que cierra, operador, número, variable */
	public int Funcion; /* Código de la función 0:seno, 1:coseno, 2:tangente, 3: valor absoluto, 4: arcoseno, 5: arcocoseno, 6: arcotangente, 7: logaritmo natural, 8: valor tope, 9: exponencial, 10: raíz cuadrada, 11: raíz cúbica */
	public char Operador; /* + suma - resta * multiplicación / división ^ potencia */
	public double Numero; /* Número literal, por ejemplo: 3.141592 */
	public int UnaVariable; /* Variable algebraica */
	public int Acumulador; /* Usado cuando la expresión se convierte en piezas. Por ejemplo:
							3 + 2 / 5  se convierte así:
							|3| |+| |2| |/| |5|
							|3| |+| |A|  A es un identificador de acumulador */

	public Parte(int tipo, int funcion, char operador, double numero, int unaVariable) {
		Tipo = tipo;
		Funcion = funcion;
		Operador = operador;
		Numero = numero;
		UnaVariable = unaVariable;
	}
}
