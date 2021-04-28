namespace EvaluadorExpresiones {
	public class Pieza {
		public double ValorPieza; /* Almacena el valor que genera la pieza al evaluarse */
		public int Funcion; /* Código de la función 0:seno, 1:coseno, 2:tangente, 3: valor absoluto, 4: arcoseno, 5: arcocoseno, 6: arcotangente, 7: logaritmo natural, 8: valor tope, 9: exponencial, 10: raíz cuadrada, 11: raíz cúbica */
		public int TipoA; /* La primera parte es un número o una variable o trae el valor de otra pieza */
		public double NumeroA; /* Es un número literal */
		public int VariableA; /* Es una variable */
		public int PiezaA; /* Trae el valor de otra pieza */
		public char Operador; /* + suma - resta * multiplicación / división ^ potencia */
		public int TipoB; /* La segunda parte es un número o una variable o trae el valor de otra pieza */
		public double NumeroB; /* Es un número literal */
		public int VariableB; /* Es una variable */
		public int PiezaB; /* Trae el valor de otra pieza */

		public Pieza(int funcion, int tipoA, double numeroA, int variableA, int piezaA, char operador, int tipoB, double numeroB, int variableB, int piezaB) {
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
	}
}
