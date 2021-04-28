namespace EvaluadorExpresiones {
	public class EvaluaSintaxis {
		/* Mensajes de error de sintaxis */
		private readonly string[] _mensajeError = {
			"0. Caracteres no permitidos. Ejemplo: 3$5+2",
			"1. Un número seguido de una letra. Ejemplo: 2q-(*3)",
			"2. Un número seguido de un paréntesis que abre. Ejemplo: 7-2(5-6)",
			"3. Doble punto seguido. Ejemplo: 3..1",
			"4. Punto seguido de operador. Ejemplo: 3.*1",
			"5. Un punto y sigue una letra. Ejemplo: 3+5.w-8",
			"6. Punto seguido de paréntesis que abre. Ejemplo: 2-5.(4+1)*3",
			"7. Punto seguido de paréntesis que cierra. Ejemplo: 2-(5.)*3",
			"8. Un operador seguido de un punto. Ejemplo: 2-(4+.1)-7",
			"9. Dos operadores estén seguidos. Ejemplo: 2++4, 5-*3",
			"10. Un operador seguido de un paréntesis que cierra. Ejemplo: 2-(4+)-7",
			"11. Una letra seguida de número. Ejemplo: 7-2a-6",
			"12. Una letra seguida de punto. Ejemplo: 7-a.-6",
			"13. Un paréntesis que abre seguido de punto. Ejemplo: 7-(.4-6)",
			"14. Un paréntesis que abre seguido de un operador. Ejemplo: 2-(*3)",
			"15. Un paréntesis que abre seguido de un paréntesis que cierra. Ejemplo: 7-()-6",
			"16. Un paréntesis que cierra y sigue un número. Ejemplo: (3-5)7",
			"17. Un paréntesis que cierra y sigue un punto. Ejemplo: (3-5).",
			"18. Un paréntesis que cierra y sigue una letra. Ejemplo: (3-5)t",
			"19. Un paréntesis que cierra y sigue un paréntesis que abre. Ejemplo: (3-5)(4*5)",
			"20. Hay dos o más letras seguidas (obviando las funciones)",
			"21. Los paréntesis están desbalanceados. Ejemplo: 3-(2*4))",
			"22. Doble punto en un número de tipo real. Ejemplo: 7-6.46.1+2",
			"23. Paréntesis que abre no corresponde con el que cierra. Ejemplo: 2+3)-2*(4" ,
			"24. Inicia con operador. Ejemplo: +3*5",
			"25. Finaliza con operador. Ejemplo: 3*5*",
			"26. Letra seguida de paréntesis que abre (obviando las funciones). Ejemplo: 4*a(6-2)"
		};

		public bool[] EsCorrecto = new bool[27];

		/* Retorna si el caracter es un operador matemático */
		private bool EsUnOperador(char car) {
			return car == '+' || car == '-' || car == '*' || car == '/' || car == '^';
		}

		/* Retorna si el caracter es un número */
		private bool EsUnNumero(char car) {
			return car >= '0' && car <= '9';
		}

		/* Retorna si el caracter es una letra */
		private bool EsUnaLetra(char car) {
			return car >= 'a' && car <= 'z';
		}

		/* 0. Detecta si hay un caracter no válido */
		private bool BuenaSintaxis00(string expresion) {
			bool Resultado = true;
			const string permitidos = "abcdefghijklmnopqrstuvwxyz0123456789.+-*/^()";
			for (int pos = 0; pos < expresion.Length && Resultado; pos++)
				if (permitidos.IndexOf(expresion[pos]) == -1)
					Resultado = false;
			return Resultado;
		}

		/* 1. Un número seguido de una letra. Ejemplo: 2q-(*3) */
		private bool BuenaSintaxis01(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (EsUnNumero(carA) && EsUnaLetra(carB)) Resultado = false;
			}
			return Resultado;
		}

		/* 2. Un número seguido de un paréntesis que abre. Ejemplo: 7-2(5-6) */
		private bool BuenaSintaxis02(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (EsUnNumero(carA) && carB == '(') Resultado = false;
			}
			return Resultado;
		}

		/* 3. Doble punto seguido. Ejemplo: 3..1 */
		private bool BuenaSintaxis03(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (carA == '.' && carB == '.') Resultado = false;
			}
			return Resultado;
		}

		/* 4. Punto seguido de operador. Ejemplo: 3.*1 */
		private bool BuenaSintaxis04(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (carA == '.' && EsUnOperador(carB)) Resultado = false;
			}
			return Resultado;
		}

		/* 5. Un punto y sigue una letra. Ejemplo: 3+5.w-8 */
		private bool BuenaSintaxis05(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (carA == '.' && EsUnaLetra(carB)) Resultado = false;
			}
			return Resultado;
		}

		/* 6. Punto seguido de paréntesis que abre. Ejemplo: 2-5.(4+1)*3 */
		private bool BuenaSintaxis06(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (carA == '.' && carB == '(') Resultado = false;
			}
			return Resultado;
		}

		/* 7. Punto seguido de paréntesis que cierra. Ejemplo: 2-(5.)*3 */
		private bool BuenaSintaxis07(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (carA == '.' && carB == ')') Resultado = false;
			}
			return Resultado;
		}

		/* 8. Un operador seguido de un punto. Ejemplo: 2-(4+.1)-7 */
		private bool BuenaSintaxis08(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (EsUnOperador(carA) && carB == '.') Resultado = false;
			}
			return Resultado;
		}

		/* 9. Dos operadores estén seguidos. Ejemplo: 2++4, 5-*3 */
		private bool BuenaSintaxis09(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (EsUnOperador(carA) && EsUnOperador(carB)) Resultado = false;
			}
			return Resultado;
		}

		/* 10. Un operador seguido de un paréntesis que cierra. Ejemplo: 2-(4+)-7 */
		private bool BuenaSintaxis10(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (EsUnOperador(carA) && carB == ')') Resultado = false;
			}
			return Resultado;
		}

		/* 11. Una letra seguida de número. Ejemplo: 7-2a-6 */
		private bool BuenaSintaxis11(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (EsUnaLetra(carA) && EsUnNumero(carB)) Resultado = false;
			}
			return Resultado;
		}

		/* 12. Una letra seguida de punto. Ejemplo: 7-a.-6 */
		private bool BuenaSintaxis12(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (EsUnaLetra(carA) && carB == '.') Resultado = false;
			}
			return Resultado;
		}

		/* 13. Un paréntesis que abre seguido de punto. Ejemplo: 7-(.4-6) */
		private bool BuenaSintaxis13(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (carA == '(' && carB == '.') Resultado = false;
			}
			return Resultado;
		}

		/* 14. Un paréntesis que abre seguido de un operador. Ejemplo: 2-(*3) */
		private bool BuenaSintaxis14(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (carA == '(' && EsUnOperador(carB)) Resultado = false;
			}
			return Resultado;
		}

		/* 15. Un paréntesis que abre seguido de un paréntesis que cierra. Ejemplo: 7-()-6 */
		private bool BuenaSintaxis15(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (carA == '(' && carB == ')') Resultado = false;
			}
			return Resultado;
		}

		/* 16. Un paréntesis que cierra y sigue un número. Ejemplo: (3-5)7 */
		private bool BuenaSintaxis16(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (carA == ')' && EsUnNumero(carB)) Resultado = false;
			}
			return Resultado;
		}

		/* 17. Un paréntesis que cierra y sigue un punto. Ejemplo: (3-5). */
		private bool BuenaSintaxis17(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (carA == ')' && carB == '.') Resultado = false;
			}
			return Resultado;
		}

		/* 18. Un paréntesis que cierra y sigue una letra. Ejemplo: (3-5)t */
		private bool BuenaSintaxis18(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (carA == ')' && EsUnaLetra(carB)) Resultado = false;
			}
			return Resultado;
		}

		/* 19. Un paréntesis que cierra y sigue un paréntesis que abre. Ejemplo: (3-5)(4*5) */
		private bool BuenaSintaxis19(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (carA == ')' && carB == '(') Resultado = false;
			}
			return Resultado;
		}

		/* 20. Si hay dos letras seguidas (después de quitar las funciones), es un error */
		private bool BuenaSintaxis20(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (EsUnaLetra(carA) && EsUnaLetra(carB)) Resultado = false;
			}
			return Resultado;
		}

		/* 21. Los paréntesis estén desbalanceados. Ejemplo: 3-(2*4)) */
		private bool BuenaSintaxis21(string expresion) {
			int parabre = 0; /* Contador de paréntesis que abre */
			int parcierra = 0; /* Contador de paréntesis que cierra */
			for (int pos = 0; pos < expresion.Length; pos++) {
				switch (expresion[pos]) {
					case '(': parabre++; break;
					case ')': parcierra++; break;
				}
			}
			return parcierra == parabre;
		}

		/* 22. Doble punto en un número de tipo real. Ejemplo: 7-6.46.1+2 */
		private bool BuenaSintaxis22(string expresion) {
			bool Resultado = true;
			int totalpuntos = 0; /* Validar los puntos decimales de un número real */
			for (int pos = 0; pos < expresion.Length && Resultado; pos++) {
				char carA = expresion[pos];
				if (EsUnOperador(carA)) totalpuntos = 0;
				if (carA == '.') totalpuntos++;
				if (totalpuntos > 1) Resultado = false;
			}
			return Resultado;
		}

		/* 23. Paréntesis que abre no corresponde con el que cierra. Ejemplo: 2+3)-2*(4 */
		private bool BuenaSintaxis23(string expresion) {
			bool Resultado = true;
			int parabre = 0; /* Contador de paréntesis que abre */
			int parcierra = 0; /* Contador de paréntesis que cierra */
			for (int pos = 0; pos < expresion.Length && Resultado; pos++) {
				switch (expresion[pos]) {
					case '(': parabre++; break;
					case ')': parcierra++; break;
				}
				if (parcierra > parabre) Resultado = false;
			}
			return Resultado;
		}

		/* 24. Inicia con operador. Ejemplo: +3*5 */
		private bool BuenaSintaxis24(string expresion) {
			char carA = expresion[0];
			return !EsUnOperador(carA);
		}

		/* 25. Finaliza con operador. Ejemplo: 3*5* */
		private bool BuenaSintaxis25(string expresion) {
			char carA = expresion[expresion.Length - 1];
			return !EsUnOperador(carA);
		}

		/* 26. Encuentra una letra seguida de paréntesis que abre. Ejemplo: 3-a(7)-5 */
		private bool BuenaSintaxis26(string expresion) {
			bool Resultado = true;
			for (int pos = 0; pos < expresion.Length - 1 && Resultado; pos++) {
				char carA = expresion[pos];
				char carB = expresion[pos + 1];
				if (EsUnaLetra(carA) && carB == '(') Resultado = false;
			}
			return Resultado;
		}

		public bool SintaxisCorrecta(string expresionA) {
			/* Reemplaza las funciones de tres letras por una variable que suma */
			string expresionB = expresionA.Replace("sen(", "a+(").Replace("cos(", "a+(").Replace("tan(", "a+(").Replace("abs(", "a+(").Replace("asn(", "a+(").Replace("acs(", "a+(").Replace("atn(", "a+(").Replace("log(", "a+(").Replace("cei(", "a+(").Replace("exp(", "a+(").Replace("sqr(", "a+(").Replace("rcb(", "a+(");

			/* Hace las pruebas de sintaxis */
			EsCorrecto[0] = BuenaSintaxis00(expresionB);
			EsCorrecto[1] = BuenaSintaxis01(expresionB);
			EsCorrecto[2] = BuenaSintaxis02(expresionB);
			EsCorrecto[3] = BuenaSintaxis03(expresionB);
			EsCorrecto[4] = BuenaSintaxis04(expresionB);
			EsCorrecto[5] = BuenaSintaxis05(expresionB);
			EsCorrecto[6] = BuenaSintaxis06(expresionB);
			EsCorrecto[7] = BuenaSintaxis07(expresionB);
			EsCorrecto[8] = BuenaSintaxis08(expresionB);
			EsCorrecto[9] = BuenaSintaxis09(expresionB);
			EsCorrecto[10] = BuenaSintaxis10(expresionB);
			EsCorrecto[11] = BuenaSintaxis11(expresionB);
			EsCorrecto[12] = BuenaSintaxis12(expresionB);
			EsCorrecto[13] = BuenaSintaxis13(expresionB);
			EsCorrecto[14] = BuenaSintaxis14(expresionB);
			EsCorrecto[15] = BuenaSintaxis15(expresionB);
			EsCorrecto[16] = BuenaSintaxis16(expresionB);
			EsCorrecto[17] = BuenaSintaxis17(expresionB);
			EsCorrecto[18] = BuenaSintaxis18(expresionB);
			EsCorrecto[19] = BuenaSintaxis19(expresionB);
			EsCorrecto[20] = BuenaSintaxis20(expresionB);
			EsCorrecto[21] = BuenaSintaxis21(expresionB);
			EsCorrecto[22] = BuenaSintaxis22(expresionB);
			EsCorrecto[23] = BuenaSintaxis23(expresionB);
			EsCorrecto[24] = BuenaSintaxis24(expresionB);
			EsCorrecto[25] = BuenaSintaxis25(expresionB);
			EsCorrecto[26] = BuenaSintaxis26(expresionB);

			bool Resultado = true;
			for (int cont = 0; cont < EsCorrecto.Length && Resultado; cont++)
				if (EsCorrecto[cont] == false) Resultado = false;
			return Resultado;
		}

		/* Transforma la expresión para ser chequeada y analizada */
		public string Transforma(string expresion) {
			/* Quita espacios, tabuladores y la vuelve a minúsculas */
			string nuevo = "";
			for (int num = 0; num < expresion.Length; num++) {
				char letra = expresion[num];
				if (letra >= 'A' && letra <= 'Z') letra += ' ';
				if (letra != ' ' && letra != '	') nuevo += letra.ToString();
			}
			return nuevo;
		}

		/* Muestra mensaje de error sintáctico */
		public string MensajesErrorSintaxis(int codigoError) {
			return _mensajeError[codigoError];
		}
	}
}
