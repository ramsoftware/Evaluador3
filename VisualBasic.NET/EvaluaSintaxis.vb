Public Class EvaluaSintaxis
	'Mensajes de error de sintaxis
	Private ReadOnly _mensajeError As String() = {
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
			"26. Letra seguida de paréntesis que abre (obviando las funciones). Ejemplo: 4*a(6-2)*"}

	Public EsCorrecto As Boolean() = New Boolean(26) {}

	'Retorna si el caracter es un operador matemático
	Private Function EsUnOperador(ByVal car As Char) As Boolean
		Return car = "+"c OrElse car = "-"c OrElse car = "*"c OrElse car = "/"c OrElse car = "^"c
	End Function

	'Retorna si el caracter es un número
	Private Function EsUnNumero(ByVal car As Char) As Boolean
		Return car >= "0"c AndAlso car <= "9"c
	End Function

	'Retorna si el caracter es una letra
	Private Function EsUnaLetra(ByVal car As Char) As Boolean
		Return car >= "a"c AndAlso car <= "z"c
	End Function

	'0. Detecta si hay un caracter no válido
	Private Function BuenaSintaxis00(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Const permitidos As String = "abcdefghijklmnopqrstuvwxyz0123456789.+-*/^()"
		Dim pos As Integer = 0

		While pos < expresion.Length AndAlso Resultado
			If permitidos.IndexOf(expresion(pos)) = -1 Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'1. Un número seguido de una letra. Ejemplo: 2q-(*3)
	Private Function BuenaSintaxis01(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If EsUnNumero(carA) AndAlso EsUnaLetra(carB) Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'2. Un número seguido de un paréntesis que abre. Ejemplo: 7-2(5-6)
	Private Function BuenaSintaxis02(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If EsUnNumero(carA) AndAlso carB = "("c Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'3. Doble punto seguido. Ejemplo: 3..1
	Private Function BuenaSintaxis03(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If carA = "."c AndAlso carB = "."c Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'4. Punto seguido de operador. Ejemplo: 3.*1
	Private Function BuenaSintaxis04(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If carA = "."c AndAlso EsUnOperador(carB) Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'5. Un punto y sigue una letra. Ejemplo: 3+5.w-8
	Private Function BuenaSintaxis05(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If carA = "."c AndAlso EsUnaLetra(carB) Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'6. Punto seguido de paréntesis que abre. Ejemplo: 2-5.(4+1)*3
	Private Function BuenaSintaxis06(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If carA = "."c AndAlso carB = "("c Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'7. Punto seguido de paréntesis que cierra. Ejemplo: 2-(5.)*3
	Private Function BuenaSintaxis07(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If carA = "."c AndAlso carB = ")"c Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'8. Un operador seguido de un punto. Ejemplo: 2-(4+.1)-7
	Private Function BuenaSintaxis08(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If EsUnOperador(carA) AndAlso carB = "."c Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'9. Dos operadores estén seguidos. Ejemplo: 2++4, 5-*3
	Private Function BuenaSintaxis09(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If EsUnOperador(carA) AndAlso EsUnOperador(carB) Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'10. Un operador seguido de un paréntesis que cierra. Ejemplo: 2-(4+)-7
	Private Function BuenaSintaxis10(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If EsUnOperador(carA) AndAlso carB = ")"c Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'11. Una letra seguida de número. Ejemplo: 7-2a-6
	Private Function BuenaSintaxis11(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If EsUnaLetra(carA) AndAlso EsUnNumero(carB) Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'12. Una letra seguida de punto. Ejemplo: 7-a.-6 */
	Private Function BuenaSintaxis12(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If EsUnaLetra(carA) AndAlso carB = "."c Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'13. Un paréntesis que abre seguido de punto. Ejemplo: 7-(.4-6)
	Private Function BuenaSintaxis13(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If carA = "("c AndAlso carB = "."c Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'14. Un paréntesis que abre seguido de un operador. Ejemplo: 2-(*3)
	Private Function BuenaSintaxis14(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If carA = "("c AndAlso EsUnOperador(carB) Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'15. Un paréntesis que abre seguido de un paréntesis que cierra. Ejemplo: 7-()-6
	Private Function BuenaSintaxis15(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If carA = "("c AndAlso carB = ")"c Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'16. Un paréntesis que cierra y sigue un número. Ejemplo: (3-5)7
	Private Function BuenaSintaxis16(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If carA = ")"c AndAlso EsUnNumero(carB) Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'17. Un paréntesis que cierra y sigue un punto. Ejemplo: (3-5).
	Private Function BuenaSintaxis17(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If carA = ")"c AndAlso carB = "."c Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'18. Un paréntesis que cierra y sigue una letra. Ejemplo: (3-5)t
	Private Function BuenaSintaxis18(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If carA = ")"c AndAlso EsUnaLetra(carB) Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'19. Un paréntesis que cierra y sigue un paréntesis que abre. Ejemplo: (3-5)(4*5)
	Private Function BuenaSintaxis19(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If carA = ")"c AndAlso carB = "("c Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'20. Si hay dos letras seguidas (después de quitar las funciones), es un error
	Private Function BuenaSintaxis20(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If EsUnaLetra(carA) AndAlso EsUnaLetra(carB) Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'21. Los paréntesis estén desbalanceados. Ejemplo: 3-(2*4))
	Private Function BuenaSintaxis21(ByVal expresion As String) As Boolean
		Dim parabre As Integer = 0 'Contador de paréntesis que abre
		Dim parcierra As Integer = 0 'Contador de paréntesis que cierra

		For pos As Integer = 0 To expresion.Length - 1

			Select Case expresion(pos)
				Case "("c
					parabre += 1
				Case ")"c
					parcierra += 1
			End Select
		Next

		Return parcierra = parabre
	End Function

	'22. Doble punto en un número de tipo real. Ejemplo: 7-6.46.1+2
	Private Function BuenaSintaxis22(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim totalpuntos As Integer = 0 'Validar los puntos decimales de un número real
		Dim pos As Integer = 0

		While pos < expresion.Length AndAlso Resultado
			Dim carA As Char = expresion(pos)
			If EsUnOperador(carA) Then totalpuntos = 0
			If carA = "."c Then totalpuntos += 1
			If totalpuntos > 1 Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'23. Paréntesis que abre no corresponde con el que cierra. Ejemplo: 2+3)-2*(4
	Private Function BuenaSintaxis23(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim parabre As Integer = 0 'Contador de paréntesis que abre
		Dim parcierra As Integer = 0 'Contador de paréntesis que cierra
		Dim pos As Integer = 0

		While pos < expresion.Length AndAlso Resultado

			Select Case expresion(pos)
				Case "("c
					parabre += 1
				Case ")"c
					parcierra += 1
			End Select

			If parcierra > parabre Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	'24. Inicia con operador. Ejemplo: +3*5
	Private Function BuenaSintaxis24(ByVal expresion As String) As Boolean
		Dim carA As Char = expresion(0)
		Return Not EsUnOperador(carA)
	End Function

	'25. Finaliza con operador. Ejemplo: 3*5*
	Private Function BuenaSintaxis25(ByVal expresion As String) As Boolean
		Dim carA As Char = expresion(expresion.Length - 1)
		Return Not EsUnOperador(carA)
	End Function

	'26. Encuentra una letra seguida de paréntesis que abre. Ejemplo: 3-a(7)-5
	Private Function BuenaSintaxis26(ByVal expresion As String) As Boolean
		Dim Resultado As Boolean = True
		Dim pos As Integer = 0

		While pos < expresion.Length - 1 AndAlso Resultado
			Dim carA As Char = expresion(pos)
			Dim carB As Char = expresion(pos + 1)
			If EsUnaLetra(carA) AndAlso carB = "("c Then Resultado = False
			pos += 1
		End While

		Return Resultado
	End Function

	Public Function SintaxisCorrecta(ByVal ecuacion As String) As Boolean
		'Reemplaza las funciones de tres letras por una variable que suma
		Dim expresion As String = ecuacion.Replace("sen(", "a+(").Replace("cos(", "a+(").Replace("tan(", "a+(").Replace("abs(", "a+(").Replace("asn(", "a+(").Replace("acs(", "a+(").Replace("atn(", "a+(").Replace("log(", "a+(").Replace("cei(", "a+(").Replace("exp(", "a+(").Replace("sqr(", "a+(").Replace("rcb(", "a+(")

		'Hace las pruebas de sintaxis
		EsCorrecto(0) = BuenaSintaxis00(expresion)
		EsCorrecto(1) = BuenaSintaxis01(expresion)
		EsCorrecto(2) = BuenaSintaxis02(expresion)
		EsCorrecto(3) = BuenaSintaxis03(expresion)
		EsCorrecto(4) = BuenaSintaxis04(expresion)
		EsCorrecto(5) = BuenaSintaxis05(expresion)
		EsCorrecto(6) = BuenaSintaxis06(expresion)
		EsCorrecto(7) = BuenaSintaxis07(expresion)
		EsCorrecto(8) = BuenaSintaxis08(expresion)
		EsCorrecto(9) = BuenaSintaxis09(expresion)
		EsCorrecto(10) = BuenaSintaxis10(expresion)
		EsCorrecto(11) = BuenaSintaxis11(expresion)
		EsCorrecto(12) = BuenaSintaxis12(expresion)
		EsCorrecto(13) = BuenaSintaxis13(expresion)
		EsCorrecto(14) = BuenaSintaxis14(expresion)
		EsCorrecto(15) = BuenaSintaxis15(expresion)
		EsCorrecto(16) = BuenaSintaxis16(expresion)
		EsCorrecto(17) = BuenaSintaxis17(expresion)
		EsCorrecto(18) = BuenaSintaxis18(expresion)
		EsCorrecto(19) = BuenaSintaxis19(expresion)
		EsCorrecto(20) = BuenaSintaxis20(expresion)
		EsCorrecto(21) = BuenaSintaxis21(expresion)
		EsCorrecto(22) = BuenaSintaxis22(expresion)
		EsCorrecto(23) = BuenaSintaxis23(expresion)
		EsCorrecto(24) = BuenaSintaxis24(expresion)
		EsCorrecto(25) = BuenaSintaxis25(expresion)
		EsCorrecto(26) = BuenaSintaxis26(expresion)
		Dim Resultado As Boolean = True
		Dim cont As Integer = 0

		While cont < EsCorrecto.Length AndAlso Resultado
			If EsCorrecto(cont) = False Then Resultado = False
			cont += 1
		End While

		Return Resultado
	End Function

	'Transforma la expresión para ser chequeada y analizada */
	Public Function Transforma(expresion As String) As String
		'Quita espacios, tabuladores y la vuelve a minúsculas */
		Dim nuevo As String = ""
		For num As Integer = 0 To expresion.Length - 1 Step 1
			Dim letra As Char = expresion(num)
			If letra >= "A" And letra <= "Z" Then letra = Chr(Asc(letra) + Asc(" "c))
			If letra <> " " And letra <> "	" Then nuevo += letra.ToString()
		Next
		Return nuevo
	End Function

	'Muestra mensaje de error sintáctico
	Public Function MensajesErrorSintaxis(ByVal codigoError As Integer) As String
		Return _mensajeError(codigoError)
	End Function
End Class
