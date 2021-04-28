#include <string>
#include <algorithm>
#include <iostream>

class EvaluaSintaxis{
private:
		/* Mensajes de error de sintaxis */
		std::string _mensajeError[27] = {
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
			"23. Paréntesis que abre no corresponde con el que cierra. Ejemplo: 2+3)-2*(4",
			"24. Inicia con operador. Ejemplo: +3*5",
			"25. Finaliza con operador. Ejemplo: 3*5*",
			"26. Letra seguida de paréntesis que abre (obviando las funciones). Ejemplo: 4*a(6-2)*"
		};
		

	bool EsUnOperador(char car);
	bool EsUnNumero(char car);
	bool EsUnaLetra(char car);
	bool BuenaSintaxis00(std::string expresion);
	bool BuenaSintaxis01(std::string expresion);
	bool BuenaSintaxis02(std::string expresion);
	bool BuenaSintaxis03(std::string expresion);
	bool BuenaSintaxis04(std::string expresion);
	bool BuenaSintaxis05(std::string expresion);
	bool BuenaSintaxis06(std::string expresion);
	bool BuenaSintaxis07(std::string expresion);
	bool BuenaSintaxis08(std::string expresion);
	bool BuenaSintaxis09(std::string expresion);
	bool BuenaSintaxis10(std::string expresion);
	bool BuenaSintaxis11(std::string expresion);
	bool BuenaSintaxis12(std::string expresion);
	bool BuenaSintaxis13(std::string expresion);
	bool BuenaSintaxis14(std::string expresion);
	bool BuenaSintaxis15(std::string expresion);
	bool BuenaSintaxis16(std::string expresion);
	bool BuenaSintaxis17(std::string expresion);
	bool BuenaSintaxis18(std::string expresion);
	bool BuenaSintaxis19(std::string expresion);
	bool BuenaSintaxis20(std::string expresion);
	bool BuenaSintaxis21(std::string expresion);
	bool BuenaSintaxis22(std::string expresion);
	bool BuenaSintaxis23(std::string expresion);
	bool BuenaSintaxis24(std::string expresion);
	bool BuenaSintaxis25(std::string expresion);
	bool BuenaSintaxis26(std::string expresion);
	std::string ReplaceAll(std::string Cadena, const std::string& Buscar, const std::string& Reemplazar);
public:
	bool EsCorrecto[27];	
	int SintaxisCorrecta(std::string expresion);
	std::string Transforma(std::string expresion);
	std::string MensajesErrorSintaxis(int CodigoError);
};
