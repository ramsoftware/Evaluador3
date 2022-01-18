package com.company;

public class EvaluaSintaxis {
	/* Mensajes de error de sintaxis */
	private String[] _mensajeError = {
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

	public boolean[] EsCorrecto = new boolean[27];

	/* Retorna si el caracter es un operador matemático */
	private boolean EsUnOperador(char car) {
		return car == '+' || car == '-' || car == '*' || car == '/' || car == '^';
	}

	/* Retorna si el caracter es un número */
	private boolean EsUnNumero(char car) {
		return car >= '0' && car <= '9';
	}

	/* Retorna si el caracter es una letra */
	private boolean EsUnaLetra(char car) {
		return car >= 'a' && car <= 'z';
	}

	/* 0. Detecta si hay un caracter no válido */
	private boolean BuenaSintaxis00(String expresion) {
		boolean Resultado = true;
		String permitidos = "abcdefghijklmnopqrstuvwxyz0123456789.+-*/^()";
		for (int pos = 0; pos < expresion.length() && Resultado; pos++)
			if (permitidos.indexOf(expresion.charAt(pos)) == -1)
				Resultado = false;
		return Resultado;
	}

	/* 1. Un número seguido de una letra. Ejemplo: 2q-(*3) */
	private boolean BuenaSintaxis01(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (EsUnNumero(carA) && EsUnaLetra(carB)) Resultado = false;
		}
		return Resultado;
	}

	/* 2. Un número seguido de un paréntesis que abre. Ejemplo: 7-2(5-6) */
	private boolean BuenaSintaxis02(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (EsUnNumero(carA) && carB == '(') Resultado = false;
		}
		return Resultado;
	}

	/* 3. Doble punto seguido. Ejemplo: 3..1 */
	private boolean BuenaSintaxis03(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (carA == '.' && carB == '.') Resultado = false;
		}
		return Resultado;
	}

	/* 4. Punto seguido de operador. Ejemplo: 3.*1 */
	private boolean BuenaSintaxis04(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (carA == '.' && EsUnOperador(carB)) Resultado = false;
		}
		return Resultado;
	}

	/* 5. Un punto y sigue una letra. Ejemplo: 3+5.w-8 */
	private boolean BuenaSintaxis05(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (carA == '.' && EsUnaLetra(carB)) Resultado = false;
		}
		return Resultado;
	}

	/* 6. Punto seguido de paréntesis que abre. Ejemplo: 2-5.(4+1)*3 */
	private boolean BuenaSintaxis06(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (carA == '.' && carB == '(') Resultado = false;
		}
		return Resultado;
	}

	/* 7. Punto seguido de paréntesis que cierra. Ejemplo: 2-(5.)*3 */
	private boolean BuenaSintaxis07(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (carA == '.' && carB == ')') Resultado = false;
		}
		return Resultado;
	}

	/* 8. Un operador seguido de un punto. Ejemplo: 2-(4+.1)-7 */
	private boolean BuenaSintaxis08(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (EsUnOperador(carA) && carB == '.') Resultado = false;
		}
		return Resultado;
	}

	/* 9. Dos operadores estén seguidos. Ejemplo: 2++4, 5-*3 */
	private boolean BuenaSintaxis09(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (EsUnOperador(carA) && EsUnOperador(carB)) Resultado = false;
		}
		return Resultado;
	}

	/* 10. Un operador seguido de un paréntesis que cierra. Ejemplo: 2-(4+)-7 */
	private boolean BuenaSintaxis10(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (EsUnOperador(carA) && carB == ')') Resultado = false;
		}
		return Resultado;
	}

	/* 11. Una letra seguida de número. Ejemplo: 7-2a-6 */
	private boolean BuenaSintaxis11(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (EsUnaLetra(carA) && EsUnNumero(carB)) Resultado = false;
		}
		return Resultado;
	}

	/* 12. Una letra seguida de punto. Ejemplo: 7-a.-6 */
	private boolean BuenaSintaxis12(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (EsUnaLetra(carA) && carB == '.') Resultado = false;
		}
		return Resultado;
	}

	/* 13. Un paréntesis que abre seguido de punto. Ejemplo: 7-(.4-6) */
	private boolean BuenaSintaxis13(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (carA == '(' && carB == '.') Resultado = false;
		}
		return Resultado;
	}

	/* 14. Un paréntesis que abre seguido de un operador. Ejemplo: 2-(*3) */
	private boolean BuenaSintaxis14(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (carA == '(' && EsUnOperador(carB)) Resultado = false;
		}
		return Resultado;
	}

	/* 15. Un paréntesis que abre seguido de un paréntesis que cierra. Ejemplo: 7-()-6 */
	private boolean BuenaSintaxis15(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (carA == '(' && carB == ')') Resultado = false;
		}
		return Resultado;
	}

	/* 16. Un paréntesis que cierra y sigue un número. Ejemplo: (3-5)7 */
	private boolean BuenaSintaxis16(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (carA == ')' && EsUnNumero(carB)) Resultado = false;
		}
		return Resultado;
	}

	/* 17. Un paréntesis que cierra y sigue un punto. Ejemplo: (3-5). */
	private boolean BuenaSintaxis17(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (carA == ')' && carB == '.') Resultado = false;
		}
		return Resultado;
	}

	/* 18. Un paréntesis que cierra y sigue una letra. Ejemplo: (3-5)t */
	private boolean BuenaSintaxis18(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (carA == ')' && EsUnaLetra(carB)) Resultado = false;
		}
		return Resultado;
	}

	/* 19. Un paréntesis que cierra y sigue un paréntesis que abre. Ejemplo: (3-5)(4*5) */
	private boolean BuenaSintaxis19(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (carA == ')' && carB == '(') Resultado = false;
		}
		return Resultado;
	}

	/* 20. Si hay dos letras seguidas (después de quitar las funciones), es un error */
	private boolean BuenaSintaxis20(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (EsUnaLetra(carA) && EsUnaLetra(carB)) Resultado = false;
		}
		return Resultado;
	}

	/* 21. Los paréntesis estén desbalanceados. Ejemplo: 3-(2*4)) */
	private boolean BuenaSintaxis21(String expresion) {
		int parabre = 0; /* Contador de paréntesis que abre */
		int parcierra = 0; /* Contador de paréntesis que cierra */
		for (int pos = 0; pos < expresion.length(); pos++) {
			switch (expresion.charAt(pos)) {
				case '(': parabre++; break;
				case ')': parcierra++; break;
			}
		}
		return parcierra == parabre;
	}

	/* 22. Doble punto en un número de tipo real. Ejemplo: 7-6.46.1+2 */
	private boolean BuenaSintaxis22(String expresion) {
		boolean Resultado = true;
		int totalpuntos = 0; /* Validar los puntos decimales de un número real */
		for (int pos = 0; pos < expresion.length() && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			if (EsUnOperador(carA)) totalpuntos = 0;
			if (carA == '.') totalpuntos++;
			if (totalpuntos > 1) Resultado = false;
		}
		return Resultado;
	}

	/* 23. Paréntesis que abre no corresponde con el que cierra. Ejemplo: 2+3)-2*(4"; */
	private boolean BuenaSintaxis23(String expresion) {
		boolean Resultado = true;
		int parabre = 0; /* Contador de paréntesis que abre */
		int parcierra = 0; /* Contador de paréntesis que cierra */
		for (int pos = 0; pos < expresion.length() && Resultado; pos++) {
			switch (expresion.charAt(pos)) {
				case '(': parabre++; break;
				case ')': parcierra++; break;
			}
			if (parcierra > parabre) Resultado = false;
		}
		return Resultado;
	}

	/* 24. Inicia con operador. Ejemplo: +3*5 */
	private boolean BuenaSintaxis24(String expresion) {
		char carA = expresion.charAt(0);
		return !EsUnOperador(carA);
	}

	/* 25. Finaliza con operador. Ejemplo: 3*5* */
	private boolean BuenaSintaxis25(String expresion) {
		char carA = expresion.charAt(expresion.length() - 1);
		return !EsUnOperador(carA);
	}

	/* 26. Encuentra una letra seguida de paréntesis que abre. Ejemplo: 3-a(7)-5 */
	private boolean BuenaSintaxis26(String expresion) {
		boolean Resultado = true;
		for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
			char carA = expresion.charAt(pos);
			char carB = expresion.charAt(pos+1);
			if (EsUnaLetra(carA) && carB == '(') Resultado = false;
		}
		return Resultado;
	}

	public boolean SintaxisCorrecta(String ecuacion) {
		/* Reemplaza las funciones de tres letras por una letra */
		String expresion = ecuacion.replace("sen(", "a+(").replace("cos(", "a+(").replace("tan(", "a+(").replace("abs(","a+(").replace("asn(", "a+(").replace("acs(", "a+(").replace("atn(", "a+(").replace("log(", "a+(").replace("cei(", "a+(").replace("exp(", "a+(").replace("sqr(", "a+(").replace("rcb(", "a+(");

		/* Hace las pruebas de sintaxis */
		EsCorrecto[0] = BuenaSintaxis00(expresion);
		EsCorrecto[1] = BuenaSintaxis01(expresion);
		EsCorrecto[2] = BuenaSintaxis02(expresion);
		EsCorrecto[3] = BuenaSintaxis03(expresion);
		EsCorrecto[4] = BuenaSintaxis04(expresion);
		EsCorrecto[5] = BuenaSintaxis05(expresion);
		EsCorrecto[6] = BuenaSintaxis06(expresion);
		EsCorrecto[7] = BuenaSintaxis07(expresion);
		EsCorrecto[8] = BuenaSintaxis08(expresion);
		EsCorrecto[9] = BuenaSintaxis09(expresion);
		EsCorrecto[10] = BuenaSintaxis10(expresion);
		EsCorrecto[11] = BuenaSintaxis11(expresion);
		EsCorrecto[12] = BuenaSintaxis12(expresion);
		EsCorrecto[13] = BuenaSintaxis13(expresion);
		EsCorrecto[14] = BuenaSintaxis14(expresion);
		EsCorrecto[15] = BuenaSintaxis15(expresion);
		EsCorrecto[16] = BuenaSintaxis16(expresion);
		EsCorrecto[17] = BuenaSintaxis17(expresion);
		EsCorrecto[18] = BuenaSintaxis18(expresion);
		EsCorrecto[19] = BuenaSintaxis19(expresion);
		EsCorrecto[20] = BuenaSintaxis20(expresion);
		EsCorrecto[21] = BuenaSintaxis21(expresion);
		EsCorrecto[22] = BuenaSintaxis22(expresion);
		EsCorrecto[23] = BuenaSintaxis23(expresion);
		EsCorrecto[24] = BuenaSintaxis24(expresion);
		EsCorrecto[25] = BuenaSintaxis25(expresion);
		EsCorrecto[26] = BuenaSintaxis26(expresion);

		boolean Resultado = true;
		for (int cont = 0; cont < EsCorrecto.length && Resultado; cont++)
			if (EsCorrecto[cont] == false) Resultado = false;
		return Resultado;
	}
	
	/* Transforma la expresión para ser chequeada y analizada */
	public String Transforma(String expresion) {
		/* Quita espacios, tabuladores y la vuelve a minúsculas */
		String nuevo = "";
		for (int num = 0; num < expresion.length(); num++) {
			char letra = expresion.charAt(num);
			if (letra >= 'A' && letra <= 'Z') letra += ' ';
			if (letra != ' ' && letra != '	') nuevo += letra;
		}
		
		/* Cambia los )) por )+0) porque es requerido al crear las piezas */
		while (nuevo.indexOf("))") != -1) nuevo = nuevo.replace("))", ")+0)");
		
		return nuevo;
	}

	/* Muestra mensaje de error sintáctico */
	public String MensajesErrorSintaxis(int codigoError) {
		return _mensajeError[codigoError];
	}
}
