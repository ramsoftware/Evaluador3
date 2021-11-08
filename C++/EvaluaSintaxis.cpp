#include "EvaluaSintaxis.h"

/* Retorna si el caracter es un operador matemático */
bool EvaluaSintaxis::EsUnOperador(char car) {
	return car == '+' || car == '-' || car == '*' || car == '/' || car == '^';
}

/* Retorna si el caracter es un número */
bool EvaluaSintaxis::EsUnNumero(char car) {
	return car >= '0' && car <= '9';
}

/* Retorna si el caracter es una letra */
bool EvaluaSintaxis::EsUnaLetra(char car) {
	return car >= 'a' && car <= 'z';
}

/* 0. Detecta si hay un caracter no válido */
bool EvaluaSintaxis::BuenaSintaxis00(std::string expresion){
	bool Resultado = true;
	std::string permitidos = "abcdefghijklmnopqrstuvwxyz0123456789.+-*/^()";
	for (int pos = 0; pos < expresion.length() && Resultado; pos++){
		char caracter = expresion.at(pos);
		std::size_t encuentra = permitidos.find(caracter);
		if (encuentra==std::string::npos)
			Resultado = false; //No encontró el caracter
	}
	return Resultado;
}

/* 1. Un número seguido de una letra. Ejemplo: 2q-(*3) */
bool EvaluaSintaxis::BuenaSintaxis01(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (EsUnNumero(carA) && EsUnaLetra(carB)) Resultado = false;
	}
	return Resultado;
}

/* 2. Un número seguido de un paréntesis que abre. Ejemplo: 7-2(5-6) */
bool EvaluaSintaxis::BuenaSintaxis02(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (EsUnNumero(carA) && carB == '(') Resultado = false;
	}
	return Resultado;
}

/* 3. Doble punto seguido. Ejemplo: 3..1 */
bool EvaluaSintaxis::BuenaSintaxis03(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (carA == '.' && carB == '.') Resultado = false;
	}
	return Resultado;
}

/* 4. Punto seguido de operador. Ejemplo: 3.*1 */
bool EvaluaSintaxis::BuenaSintaxis04(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (carA == '.' && EsUnOperador(carB)) Resultado = false;
	}
	return Resultado;
}

/* 5. Un punto y sigue una letra. Ejemplo: 3+5.w-8 */
bool EvaluaSintaxis::BuenaSintaxis05(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (carA == '.' && EsUnaLetra(carB)) Resultado = false;
	}
	return Resultado;
}

/* 6. Punto seguido de paréntesis que abre. Ejemplo: 2-5.(4+1)*3 */
bool EvaluaSintaxis::BuenaSintaxis06(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (carA == '.' && carB == '(') Resultado = false;
	}
	return Resultado;
}

/* 7. Punto seguido de paréntesis que cierra. Ejemplo: 2-(5.)*3 */
bool EvaluaSintaxis::BuenaSintaxis07(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (carA == '.' && carB == ')') Resultado = false;
	}
	return Resultado;
}

/* 8. Un operador seguido de un punto. Ejemplo: 2-(4+.1)-7 */
bool EvaluaSintaxis::BuenaSintaxis08(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (EsUnOperador(carA) && carB == '.') Resultado = false;
	}
	return Resultado;
}

/* 9. Dos operadores estén seguidos. Ejemplo: 2++4, 5-*3 */
bool EvaluaSintaxis::BuenaSintaxis09(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (EsUnOperador(carA) && EsUnOperador(carB)) Resultado = false;
	}
	return Resultado;
}

/* 10. Un operador seguido de un paréntesis que cierra. Ejemplo: 2-(4+)-7 */
bool EvaluaSintaxis::BuenaSintaxis10(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (EsUnOperador(carA) && carB == ')') Resultado = false;
	}
	return Resultado;
}

/* 11. Una letra seguida de número. Ejemplo: 7-2a-6 */
bool EvaluaSintaxis::BuenaSintaxis11(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (EsUnaLetra(carA) && EsUnNumero(carB)) Resultado = false;
	}
	return Resultado;
}

/* 12. Una letra seguida de punto. Ejemplo: 7-a.-6 */
bool EvaluaSintaxis::BuenaSintaxis12(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (EsUnaLetra(carA) && carB == '.') Resultado = false;
	}
	return Resultado;
}

/* 13. Un paréntesis que abre seguido de punto. Ejemplo: 7-(.4-6) */
bool EvaluaSintaxis::BuenaSintaxis13(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (carA == '(' && carB == '.') Resultado = false;
	}
	return Resultado;
}

/* 14. Un paréntesis que abre seguido de un operador. Ejemplo: 2-(*3) */
bool EvaluaSintaxis::BuenaSintaxis14(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (carA == '(' && EsUnOperador(carB)) Resultado = false;
	}
	return Resultado;
}

/* 15. Un paréntesis que abre seguido de un paréntesis que cierra. Ejemplo: 7-()-6 */
bool EvaluaSintaxis::BuenaSintaxis15(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (carA == '(' && carB == ')') Resultado = false;
	}
	return Resultado;
}

/* 16. Un paréntesis que cierra y sigue un número. Ejemplo: (3-5)7 */
bool EvaluaSintaxis::BuenaSintaxis16(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (carA == ')' && EsUnNumero(carB)) Resultado = false;
	}
	return Resultado;
}

/* 17. Un paréntesis que cierra y sigue un punto. Ejemplo: (3-5). */
bool EvaluaSintaxis::BuenaSintaxis17(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (carA == ')' && carB == '.') Resultado = false;
	}
	return Resultado;
}

/* 18. Un paréntesis que cierra y sigue una letra. Ejemplo: (3-5)t */
bool EvaluaSintaxis::BuenaSintaxis18(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (carA == ')' && EsUnaLetra(carB)) Resultado = false;
	}
	return Resultado;
}

/* 19. Un paréntesis que cierra y sigue un paréntesis que abre. Ejemplo: (3-5)(4*5) */
bool EvaluaSintaxis::BuenaSintaxis19(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (carA == ')' && carB == '(') Resultado = false;
	}
	return Resultado;
}

/* 20. Si hay dos letras seguidas (después de quitar las funciones), es un error */
bool EvaluaSintaxis::BuenaSintaxis20(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (EsUnaLetra(carA) && EsUnaLetra(carB)) Resultado = false;
	}
	return Resultado;
}

/* 21. Los paréntesis estén desbalanceados. Ejemplo: 3-(2*4)) */
bool EvaluaSintaxis::BuenaSintaxis21(std::string expresion) {
	int parabre = 0; /* Contador de paréntesis que abre */
	int parcierra = 0; /* Contador de paréntesis que cierra */
	for (int pos = 0; pos < expresion.length(); pos++) {
		char carA = expresion.at(pos);
		if (carA == '(') parabre++;
		if (carA == ')') parcierra++;
	}
	return parcierra == parabre;
}

/* 22. Doble punto en un número de tipo real. Ejemplo: 7-6.46.1+2 */
bool EvaluaSintaxis::BuenaSintaxis22(std::string expresion) {
	bool Resultado = true;
	int totalpuntos = 0; /* Validar los puntos decimales de un número real */
	for (int pos = 0; pos < expresion.length() && Resultado; pos++) {
		char carA = expresion.at(pos);
		if (EsUnOperador(carA)) totalpuntos = 0;
		if (carA == '.') totalpuntos++;
		if (totalpuntos > 1) Resultado = false;
	}
	return Resultado;
}

/* 23. Paréntesis que abre no corresponde con el que cierra. Ejemplo: 2+3)-2*(4"; */
bool EvaluaSintaxis::BuenaSintaxis23(std::string expresion) {
	bool Resultado = true;
	int parabre = 0; /* Contador de paréntesis que abre */
	int parcierra = 0; /* Contador de paréntesis que cierra */
	for (int pos = 0; pos < expresion.length() && Resultado; pos++) {
		char carA = expresion.at(pos);
		if (carA == '(') parabre++;
		if (carA == ')') parcierra++;
		if (parcierra > parabre) Resultado = false;
	}
	return Resultado;
}

/* 24. Inicia con operador. Ejemplo: +3*5 */
bool EvaluaSintaxis::BuenaSintaxis24(std::string expresion) {
	char carA = expresion[0];
	return !EsUnOperador(carA);
}

/* 25. Finaliza con operador. Ejemplo: 3*5* */
bool EvaluaSintaxis::BuenaSintaxis25(std::string expresion) {
	char carA = expresion[expresion.length() - 1];
	return !EsUnOperador(carA);
}

/* 26. Encuentra una letra seguida de paréntesis que abre. Ejemplo: 3-a(7)-5 */
bool EvaluaSintaxis::BuenaSintaxis26(std::string expresion) {
	bool Resultado = true;
	for (int pos = 0; pos < expresion.length() - 1 && Resultado; pos++) {
		char carA = expresion.at(pos);
		char carB = expresion.at(pos+1);
		if (EsUnaLetra(carA) && carB == '(') Resultado = false;
	}
	return Resultado;
}

int EvaluaSintaxis::SintaxisCorrecta(std::string expresion) {
	/* Arreglo de funciones matemáticas */
	std::string funciones[] = { "sen(", "cos(", "tan(", "abs(", "asn(", "acs(", "atn(", "log(", "cei(", "exp(", "sqr(", "rcb(" };

	/* Reemplaza las funciones por a+. Ejemplo: 3*cos(2+x) => 3*a+(2+x) */
	for (int fncn = 0; fncn < sizeof(funciones)/sizeof(funciones[0]); fncn++)
		expresion = ReplaceAll(expresion, funciones[fncn], std::string("a+("));

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
	bool Resultado = true;
	int cont = 0;

	while (cont < sizeof(EsCorrecto)/sizeof(EsCorrecto[0]) && Resultado){
		if (EsCorrecto[cont] == false)
			Resultado = false;
		cont += 1;
	}

	return Resultado;
}

/* Transforma la expresión para ser chequeada y analizada */
std::string EvaluaSintaxis::Transforma(std::string expresion) {
	/* Quita espacios, tabuladores y la vuelve a minúsculas */
	std::string nuevo = "";
	for (int num = 0; num < expresion.length(); num++) {
		char letra = expresion[num];
		if (letra >= 'A' && letra <= 'Z') letra += ' ';
		if (letra != ' ' && letra != '	') nuevo += letra;
	}
	
	/* Cambia los )) por )+0) porque es requerido al crear las piezas */
	while (nuevo.find(std::string("))"), 0) != -1) nuevo = ReplaceAll(nuevo, std::string("))"), std::string(")+0)"));
	
	return nuevo;
}

/* Reemplaza todas las ocurrencias de Buscar por Reemplazar */
std::string EvaluaSintaxis::ReplaceAll(std::string Cadena, const std::string& Buscar, const std::string& Reemplazar) {
	size_t posicion = 0;
	while((posicion = Cadena.find(Buscar, posicion)) != std::string::npos) {
		Cadena.replace(posicion, Buscar.length(), Reemplazar);
		posicion += Reemplazar.length();
	}
	return Cadena;
}

/* Muestra mensaje de error sintáctico */
std::string EvaluaSintaxis::MensajesErrorSintaxis(int CodigoError) {
	return _mensajeError[CodigoError];
}


