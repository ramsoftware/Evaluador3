using System;

namespace EvaluadorExpresiones {
	class Program {
		static void Main(string[] args) {
			UsoEvaluador();
			Console.ReadKey();
		}

		public static void UsoEvaluador() {
			/*	Una expresión algebraica:
				Números reales usan el punto decimal
				Uso de paréntesis
				Las variables deben estar en minúsculas van de la 'a' a la 'z' excepto ñ
				Las funciones (de tres letras) son:
					Sen	Seno
					Cos	Coseno
					Tan	Tangente
					Abs	Valor absoluto
					Asn	Arcoseno
					Acs	Arcocoseno
					Atn	Arcotangente
					Log	Logaritmo Natural
					Cei	Valor techo
					Exp	Exponencial
					Sqr	Raíz cuadrada
					Rcb	Raíz Cúbica
				Los operadores son:
					+ (suma)
					- (resta)
					* (multiplicación)
					/ (división)
					^ (potencia)
				No se acepta el "-" unario. Luego expresiones como: 4*-2 o (-5+3) o (-x^2) o (-x)^2 son inválidas.
			 */
			string expresion = "Cos(0.004 * x) - (Tan(1.78 /  k + h) * SEN(k ^ x) + abs (k^3-h^2))";

			//Instancia el evaluador
			Evaluador3 evaluador = new Evaluador3();

			//Analiza la expresión (valida sintaxis)
			if (evaluador.Analizar(expresion)) {
				
				//Si no hay fallos de sintaxis, puede evaluar la expresión
				
				//Da valores a las variables que deben estar en minúsculas
				evaluador.DarValorVariable('k', 1.6);
				evaluador.DarValorVariable('x', -8.3);
				evaluador.DarValorVariable('h', 9.29);
				
				//Evalúa la expresión
				double resultado = evaluador.Evaluar();
				Console.WriteLine(resultado);

				//Evalúa con ciclos
				Random azar = new Random();
				for (int num = 1; num <= 10; num++) {
					double valor = azar.NextDouble();
					evaluador.DarValorVariable('k', valor);
					resultado = evaluador.Evaluar();
					Console.WriteLine(resultado);
				}
			}
			else {
				//Si se detectó un error de sintaxis
				for (int unError = 0; unError < evaluador.Sintaxis.EsCorrecto.Length; unError++) {
					//Muestra que error de sintaxis se produjo
					if (evaluador.Sintaxis.EsCorrecto[unError] == false)
						Console.WriteLine(evaluador.Sintaxis.MensajesErrorSintaxis(unError));
				}
			}
		}
	}
}