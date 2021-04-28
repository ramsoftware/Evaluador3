<?php
class EvaluaSintaxis {
	/* Mensajes de error de sintaxis */
	var $_mensajeError = [
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
	];

	var $EsCorrecto = array();

	/* Retorna si el caracteres un operador matemático */
	function EsUnOperador($car) {
		return $car === '+' || $car === '-' || $car === '*' || $car === '/' || $car === '^';
	}

	/* Retorna si el caracter es un número */
	function EsUnNumero($car) {
		return $car >= '0' && $car <= '9';
	}

	/* Retorna si el caracter es una letra */
	function EsUnaLetra($car) {
		return $car >= 'a' && $car <= 'z';
	}

	/* 0. Detecta si hay un caracter no válido */
	function BuenaSintaxis00($expresion) {
		$Resultado = true;
		$permitidos = "aabcdefghijklmnopqrstuvwxyz0123456789.+-*/^()";
		for ($pos = 0; $pos < strlen($expresion) && $Resultado; $pos++)
		if (strpos($permitidos, $expresion[$pos]) === false)
			$Resultado = false;
		return $Resultado;
	}

	/* 1. Un número seguido de una letra. Ejemplo: 2q-(*3) */
	function BuenaSintaxis01($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($this->EsUnNumero($carA) && $this->EsUnaLetra($carB)) $Resultado = false;
		}
		return $Resultado;
	}

	/* 2. Un número seguido de un paréntesis que abre. Ejemplo: 7-2(5-6) */
	function BuenaSintaxis02($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($this->EsUnNumero($carA) && $carB === '(') $Resultado = false;
		}
		return $Resultado;
	}

	/* 3. Doble punto seguido. Ejemplo: 3..1 */
	function BuenaSintaxis03($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($carA === '.' && $carB === '.') $Resultado = false;
		}
		return $Resultado;
	}

	/* 4. Punto seguido de operador. Ejemplo: 3.*1 */
	function BuenaSintaxis04($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($carA === '.' && $this->EsUnOperador($carB)) $Resultado = false;
		}
		return $Resultado;
	}

	/* 5. Un punto y sigue una letra. Ejemplo: 3+5.w-8 */
	function BuenaSintaxis05($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($carA === '.' && $this->EsUnaLetra($carB)) $Resultado = false;
		}
		return $Resultado;
	}

	/* 6. Punto seguido de paréntesis que abre. Ejemplo: 2-5.(4+1)*3 */
	function BuenaSintaxis06($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($carA === '.' && $carB === '(') $Resultado = false;
		}
		return $Resultado;
	}

	/* 7. Punto seguido de paréntesis que cierra. Ejemplo: 2-(5.)*3 */
	function BuenaSintaxis07($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($carA === '.' && $carB === ')') $Resultado = false;
		}
		return $Resultado;
	}

	/* 8. Un operador seguido de un punto. Ejemplo: 2-(4+.1)-7 */
	function BuenaSintaxis08($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($this->EsUnOperador($carA) && $carB === '.') $Resultado = false;
		}
		return $Resultado;
	}

	/* 9. Dos operadores estén seguidos. Ejemplo: 2++4, 5-*3 */
	function BuenaSintaxis09($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($this->EsUnOperador($carA) && $this->EsUnOperador($carB)) $Resultado = false;
		}
		return $Resultado;
	}

	/* 10. Un operador seguido de un paréntesis que cierra. Ejemplo: 2-(4+)-7 */
	function BuenaSintaxis10($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($this->EsUnOperador($carA) && $carB === ')') $Resultado = false;
		}
		return $Resultado;
	}

	/* 11. Una letra seguida de número. Ejemplo: 7-2a-6 */
	function BuenaSintaxis11($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($this->EsUnaLetra($carA) && $this->EsUnNumero($carB)) $Resultado = false;
		}
		return $Resultado;
	}

	/* 12. Una letra seguida de punto. Ejemplo: 7-a.-6 */
	function BuenaSintaxis12($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($this->EsUnaLetra($carA) && $carB === '.') $Resultado = false;
		}
		return $Resultado;
	}

	/* 13. Un paréntesis que abre seguido de punto. Ejemplo: 7-(.4-6) */
	function BuenaSintaxis13($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($carA === '(' && $carB === '.') $Resultado = false;
		}
		return $Resultado;
	}

	/* 14. Un paréntesis que abre seguido de un operador. Ejemplo: 2-(*3) */
	function BuenaSintaxis14($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($carA === '(' && $this->EsUnOperador($carB)) $Resultado = false;
		}
		return $Resultado;
	}

	/* 15. Un paréntesis que abre seguido de un paréntesis que cierra. Ejemplo: 7-()-6 */
	function BuenaSintaxis15($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($carA === '(' && $carB === ')') $Resultado = false;
		}
		return $Resultado;
	}

	/* 16. Un paréntesis que cierra y sigue un número. Ejemplo: (3-5)7 */
	function BuenaSintaxis16($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($carA === ')' && $this->EsUnNumero($carB)) $Resultado = false;
		}
		return $Resultado;
	}

	/* 17. Un paréntesis que cierra y sigue un punto. Ejemplo: (3-5). */
	function BuenaSintaxis17($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($carA === ')' && $carB === '.') $Resultado = false;
		}
		return $Resultado;
	}

	/* 18. Un paréntesis que cierra y sigue una letra. Ejemplo: (3-5)t */
	function BuenaSintaxis18($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($carA === ')' && $this->EsUnaLetra($carB)) $Resultado = false;
		}
		return $Resultado;
	}

	/* 19. Un paréntesis que cierra y sigue un paréntesis que abre. Ejemplo: (3-5)(4*5) */
	function BuenaSintaxis19($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($carA === ')' && $carB === '(') $Resultado = false;
		}
		return $Resultado;
	}

	/* 20. Si hay dos letras seguidas (después de quitar las funciones), es un error */
	function BuenaSintaxis20($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($this->EsUnaLetra($carA) && $this->EsUnaLetra($carB)) $Resultado = false;
		}
		return $Resultado;
	}

	/* 21. Los paréntesis estén desbalanceados. Ejemplo: 3-(2*4)) */
	function BuenaSintaxis21($expresion) {
		$parabre = 0; /* Contador de paréntesis que abre */
		$parcierra = 0; /* Contador de paréntesis que cierra */
		for ($pos = 0; $pos < strlen($expresion); $pos++) {
			switch ($expresion[$pos]) {
				case '(': $parabre++; break;
				case ')': $parcierra++; break;
			}
		}
		return $parcierra == $parabre;
	}

	/* 22. Doble punto en un número de tipo real. Ejemplo: 7-6.46.1+2 */
	function BuenaSintaxis22($expresion) {
		$Resultado = true;
		$totalpuntos = 0; /* Validar los puntos decimales de un número real */
		for ($pos = 0; $pos < strlen($expresion) && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			if ($this->EsUnOperador($carA)) $totalpuntos = 0;
			if ($carA === '.') $totalpuntos++;
			if ($totalpuntos > 1) $Resultado = false;
		}
		return $Resultado;
	}

	/* 23. Paréntesis que abre no corresponde con el que cierra. Ejemplo: 2+3)-2*(4"; */
	function BuenaSintaxis23($expresion) {
		$Resultado = true;
		$parabre = 0; /* Contador de paréntesis que abre */
		$parcierra = 0; /* Contador de paréntesis que cierra */
		for ($pos = 0; $pos < strlen($expresion) && $Resultado; $pos++) {
			switch ($expresion[$pos]) {
				case '(': $parabre++; break;
				case ')': $parcierra++; break;
			}
			if ($parcierra > $parabre) $Resultado = false;
		}
		return $Resultado;
	}

	/* 24. Inicia con operador. Ejemplo: +3*5 */
	function BuenaSintaxis24($expresion) {
		return !$this->EsUnOperador($expresion[0]);
	}

	/* 25. Finaliza con operador. Ejemplo: 3*5* */
	function BuenaSintaxis25($expresion) {
		return !$this->EsUnOperador($expresion[strlen($expresion) - 1]);
	}

	/* 26. Encuentra una letra seguida de paréntesis que abre. Ejemplo: 3-a(7)-5 */
	function BuenaSintaxis26($expresion) {
		$Resultado = true;
		for ($pos = 0; $pos < strlen($expresion) - 1 && $Resultado; $pos++) {
			$carA = $expresion[$pos];
			$carB = $expresion[$pos+1];
			if ($this->EsUnaLetra($carA) && $carB === '(') $Resultado = false;
		}
		return $Resultado;
	}

	function SintaxisCorrecta($ecuacion) {
		/* Reemplaza las funciones de tres letras por una letra mayúscula */
		$expresion = $ecuacion;
		$expresion = str_replace("sen(", "a+(", $expresion);
		$expresion = str_replace("cos(", "a+(", $expresion);
		$expresion = str_replace("tan(", "a+(", $expresion);
		$expresion = str_replace("abs(", "a+(", $expresion);
		$expresion = str_replace("asn(", "a+(", $expresion);
		$expresion = str_replace("acs(", "a+(", $expresion);
		$expresion = str_replace("atn(", "a+(", $expresion);
		$expresion = str_replace("log(", "a+(", $expresion);
		$expresion = str_replace("cei(", "a+(", $expresion);
		$expresion = str_replace("exp(", "a+(", $expresion);
		$expresion = str_replace("sqr(", "a+(", $expresion);
		$expresion = str_replace("rcb(", "a+(", $expresion);
		
		/* Hace las pruebas de sintaxis */
		$this->EsCorrecto[0] = $this->BuenaSintaxis00($expresion);
		$this->EsCorrecto[1] = $this->BuenaSintaxis01($expresion);
		$this->EsCorrecto[2] = $this->BuenaSintaxis02($expresion);
		$this->EsCorrecto[3] = $this->BuenaSintaxis03($expresion);
		$this->EsCorrecto[4] = $this->BuenaSintaxis04($expresion);
		$this->EsCorrecto[5] = $this->BuenaSintaxis05($expresion);
		$this->EsCorrecto[6] = $this->BuenaSintaxis06($expresion);
		$this->EsCorrecto[7] = $this->BuenaSintaxis07($expresion);
		$this->EsCorrecto[8] = $this->BuenaSintaxis08($expresion);
		$this->EsCorrecto[9] = $this->BuenaSintaxis09($expresion);
		$this->EsCorrecto[10] = $this->BuenaSintaxis10($expresion);
		$this->EsCorrecto[11] = $this->BuenaSintaxis11($expresion);
		$this->EsCorrecto[12] = $this->BuenaSintaxis12($expresion);
		$this->EsCorrecto[13] = $this->BuenaSintaxis13($expresion);
		$this->EsCorrecto[14] = $this->BuenaSintaxis14($expresion);
		$this->EsCorrecto[15] = $this->BuenaSintaxis15($expresion);
		$this->EsCorrecto[16] = $this->BuenaSintaxis16($expresion);
		$this->EsCorrecto[17] = $this->BuenaSintaxis17($expresion);
		$this->EsCorrecto[18] = $this->BuenaSintaxis18($expresion);
		$this->EsCorrecto[19] = $this->BuenaSintaxis19($expresion);
		$this->EsCorrecto[20] = $this->BuenaSintaxis20($expresion);
		$this->EsCorrecto[21] = $this->BuenaSintaxis21($expresion);
		$this->EsCorrecto[22] = $this->BuenaSintaxis22($expresion);
		$this->EsCorrecto[23] = $this->BuenaSintaxis23($expresion);
		$this->EsCorrecto[24] = $this->BuenaSintaxis24($expresion);
		$this->EsCorrecto[25] = $this->BuenaSintaxis25($expresion);
		$this->EsCorrecto[26] = $this->BuenaSintaxis26($expresion);

		$Resultado = true;
		for ($cont = 0; $cont < 27 && $Resultado; $cont++)
			if ($this->EsCorrecto[$cont] === false) $Resultado = false;
		return $Resultado;
	}

	/* Transforma la expresión para ser chequeada y analizada */
	function Transforma($expresion) {
		/* Quita espacios, tabuladores y la vuelve a minúsculas */
		$nuevo = "";
		for ($num = 0; $num < strlen($expresion); $num++) {
			$letra = $expresion[$num];
			if ($letra >= 'A' && $letra <= 'Z') $letra = chr(ord($letra) + ord(' '));
			if ($letra != ' ' && $letra != '	') $nuevo .= $letra;
		}
		return $nuevo;
	}

	/* Muestra mensaje de error sintáctico */
	function MensajesErrorSintaxis($codigoError) {
		return $this->_mensajeError[$codigoError];
	}
}
