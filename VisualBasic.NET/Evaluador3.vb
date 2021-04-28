' Evaluador de expresiones. Versión 3.0
' Autor: Rafael Alberto Moreno Parra
' Fecha: 26 de abril de 2021
' 
' Pasos para la evaluación de expresiones algebraicas
' I. Toma una expresión, por ejemplo: 3.14 + sen( 4 / x ) * ( 7.2 ^ 3 - 1 ) y la divide en partes así:
'	|3.14| |+| |sen(| |4| |/| |x| |)| |*| |(| |7.2| |^| |3| |-| |1| |)|
'
' II. Toma las partes y las divide en piezas con la siguiente estructura:
'		  acumula = funcion  numero/variable/acumula  operador  numero/variable/acumula
'		  Siguiendo el ejemplo anterior sería:
'		  A =  7.2  ^  3
'		  B =  A  -  1
'		  C =  seno ( 4  /  x )
'		  D =  C  *  B
'		  E =  3.14 + D
'
'		  Esas piezas se evalúan de arriba a abajo y así se interpreta la ecuación

Public Class Evaluador3
	'Constantes de los diferentes tipos de datos que tendrán las piezas */
	Private Const ESFUNCION As Integer = 1
	Private Const ESPARABRE As Integer = 2
	Private Const ESPARCIERRA As Integer = 3
	Private Const ESOPERADOR As Integer = 4
	Private Const ESNUMERO As Integer = 5
	Private Const ESVARIABLE As Integer = 6
	Private Const ESACUMULA As Integer = 7

	' Listado de partes en que se divide la expresión
	'   Toma una expresión, por ejemplo: 3.14 + sen( 4 / x ) * ( 7.2 ^ 3 - 1 ) y la divide en partes así:
	'   |3.14| |+| |sen(| |4| |/| |x| |)| |*| |(| |7.2| |^| |3| |-| |1| |)|
	'   Cada parte puede tener un número, un operador, una función, un paréntesis que abre o un paréntesis que cierra
	Private Partes As List(Of Parte) = New List(Of Parte)()

	' Listado de piezas que ejecutan
	'   Toma las partes y las divide en piezas con la siguiente estructura:
	'   acumula = funcion  numero/variable/acumula  operador  numero/variable/acumula
	'   Siguiendo el ejemplo anterior sería:
	'   A =  7.2  ^  3
	'   B =	A  -  1
	'   C = seno ( 4  /  x )
	'   D =	C  *  B
	'   E =  3.14 + D

	'   Esas piezas se evalúan de arriba a abajo y así se interpreta la ecuación
	Private Piezas As List(Of Pieza) = New List(Of Pieza)()

	'El arreglo unidimensional que lleva el valor de las variables
	Private VariableAlgebra As Double() = New Double(25) {}

	'Uso del chequeo de sintaxis
	Public Sintaxis As EvaluaSintaxis = New EvaluaSintaxis()

	'Analiza la expresión
	Public Function Analizar(ByVal expresionA As String) As Boolean
		Dim expresionB As String = Sintaxis.Transforma(expresionA)
		Dim chequeo As Boolean = Sintaxis.SintaxisCorrecta(expresionB)
		If chequeo Then
			Partes.Clear()
			Piezas.Clear()
			CrearPartes(expresionB)
			CrearPiezas()
		End If
		Return chequeo
	End Function

	Private Sub CrearPartes(ByVal expresion As String)
		'Debe analizarse con paréntesis
		Dim NuevoA As String = "(" & expresion & ")"
		
		'Reemplaza las funciones de tres letras por una letra mayúscula */
		Dim NuevoB As String = NuevoA.Replace("sen", "A").Replace("cos", "B").Replace("tan", "C").Replace("abs", "D").Replace("asn", "E").Replace("acs", "F").Replace("atn", "G").Replace("log", "H").Replace("cei", "I").Replace("exp", "J").Replace("sqr", "K").Replace("rcb", "L")

		'Va de caracter en caracter
		Dim Numero As String = ""
		For pos As Integer = 0 To NuevoB.Length - 1
			Dim car As Char = NuevoB(pos)
			
			'Si es un número lo va acumulando en una cadena
			If (car >= "0"c AndAlso car <= "9"c) OrElse car = "."c Then
				Numero += car.ToString()
			'Si es un operador entonces agrega número (si existía)
			ElseIf car = "+"c OrElse car = "-"c OrElse car = "*"c OrElse car = "/"c OrElse car = "^"c Then
				If Numero.Length > 0 Then
					Partes.Add(New Parte(ESNUMERO, -1, "0"c, CadenaAReal(Numero), 0))
					Numero = ""
				End If
				Partes.Add(New Parte(ESOPERADOR, -1, car, 0, 0))
			'Si es variable */
			ElseIf car >= "a"c AndAlso car <= "z"c Then
				Partes.Add(New Parte(ESVARIABLE, -1, "0"c, 0, Asc(car) - Asc("a"c)))
			'Si es una función (seno, coseno, tangente, ...)
			ElseIf car >= "A"c AndAlso car <= "L"c Then
				Partes.Add(New Parte(ESFUNCION, Asc(car) - Asc("A"c), "0"c, 0, 0))
				pos += 1
			'Si es un paréntesis que abre
			ElseIf car = "("c Then
				Partes.Add(New Parte(ESPARABRE, -1, "0"c, 0, 0))
			'Si es un paréntesis que cierra
			Else
				If Numero.Length > 0 Then
					Partes.Add(New Parte(ESNUMERO, -1, "0"c, CadenaAReal(Numero), 0))
					Numero = ""
				End If
				'Si sólo había un número o variable dentro del paréntesis le agrega + 0 (por ejemplo:  sen(x) o 3*(2) ) */
				If Partes(Partes.Count - 2).Tipo = ESPARABRE OrElse Partes(Partes.Count - 2).Tipo = ESFUNCION Then
					Partes.Add(New Parte(ESOPERADOR, -1, "+"c, 0, 0))
					Partes.Add(New Parte(ESNUMERO, -1, "0"c, 0, 0))
				End If

				Partes.Add(New Parte(ESPARCIERRA, -1, "0"c, 0, 0))
			End If
		Next
	End Sub

	'Convierte un número almacenado en una cadena a su valor real */
	Private Function CadenaAReal(ByVal Numero As String) As Double
		'Parte entera
		Dim parteEntera As Double = 0
		Dim cont As Integer

		For cont = 0 To Numero.Length - 1
			If Numero(cont) = "."c Then Exit For
			parteEntera = (parteEntera * 10) + (Asc(Numero(cont)) - Asc("0"c))
		Next

		'Parte decimal
		Dim parteDecimal As Double = 0
		Dim multiplica As Double = 1

		For num As Integer = cont + 1 To Numero.Length - 1
			parteDecimal = (parteDecimal * 10) + (Asc(Numero(num)) - Asc("0"c))
			multiplica = multiplica * 10
		Next

		Dim numeroB As Double = parteEntera + parteDecimal / multiplica
		Return numeroB
	End Function

	'Ahora convierte las partes en las piezas finales de ejecución
	Private Sub CrearPiezas()
		Dim tmpParte As Parte
		Dim cont As Integer = Partes.Count - 1

		Do
			tmpParte = Partes(cont)

			If tmpParte.Tipo = ESPARABRE OrElse tmpParte.Tipo = ESFUNCION Then
				GenerarPiezasOperador("^"c, "^"c, cont) 'Evalúa las potencias
				GenerarPiezasOperador("*"c, "/"c, cont) 'Luego evalúa multiplicar y dividir
				GenerarPiezasOperador("+"c, "-"c, cont) 'Finalmente evalúa sumar y restar

				If tmpParte.Tipo = ESFUNCION Then 'Agrega la función a la última pieza
					Piezas(Piezas.Count - 1).Funcion = tmpParte.Funcion
				End If

				'Quita el paréntesis/función que abre y el que cierra, dejando el centro
				Partes.RemoveAt(cont)
				Partes.RemoveAt(cont + 1)
			End If

			cont -= 1
		Loop While cont >= 0
	End Sub

	'Genera las piezas buscando determinado operador
	Private Sub GenerarPiezasOperador(ByVal operA As Char, ByVal operB As Char, ByVal ini As Integer)
		Dim tmpParte, tmpParteIzq, tmpParteDer As Parte
		Dim cont As Integer = ini + 1

		Do
			tmpParte = Partes(cont)

			If tmpParte.Tipo = ESOPERADOR AndAlso (tmpParte.Operador = operA OrElse tmpParte.Operador = operB) Then
				tmpParteIzq = Partes(cont - 1)
				tmpParteDer = Partes(cont + 1)

				'Crea Pieza
				Piezas.Add(New Pieza(-1, tmpParteIzq.Tipo, tmpParteIzq.Numero, tmpParteIzq.Variable, tmpParteIzq.Acumulador, tmpParte.Operador, tmpParteDer.Tipo, tmpParteDer.Numero, tmpParteDer.Variable, tmpParteDer.Acumulador))

				'Elimina la parte del operador y la siguiente
				Partes.RemoveAt(cont)
				Partes.RemoveAt(cont)

				'Retorna el contador en uno para tomar la siguiente operación
				cont -= 1

				'Cambia la parte anterior por parte que acumula
				tmpParteIzq.Tipo = ESACUMULA
				tmpParteIzq.Acumulador = Piezas.Count - 1
			End If

			cont += 1
		Loop While Partes(cont).Tipo <> ESPARCIERRA
	End Sub

	'Evalúa la expresión convertida en piezas */
	Public Function Evaluar() As Double
		Dim numA, numB As Double, resultado As Double = 0
		Dim tmpPieza As Pieza

		For pos As Integer = 0 To Piezas.Count - 1
			tmpPieza = Piezas(pos)

			Select Case tmpPieza.TipoA
				Case ESNUMERO
					numA = tmpPieza.NumeroA
				Case ESVARIABLE
					numA = VariableAlgebra(tmpPieza.VariableA)
				Case Else
					numA = Piezas(tmpPieza.PiezaA).ValorPieza
			End Select

			Select Case tmpPieza.TipoB
				Case ESNUMERO
					numB = tmpPieza.NumeroB
				Case ESVARIABLE
					numB = VariableAlgebra(tmpPieza.VariableB)
				Case Else
					numB = Piezas(tmpPieza.PiezaB).ValorPieza
			End Select

			Select Case tmpPieza.Operador
				Case "*"c
					resultado = numA * numB
				Case "/"c
					resultado = numA / numB
				Case "+"c
					resultado = numA + numB
				Case "-"c
					resultado = numA - numB
				Case Else
					resultado = Math.Pow(numA, numB)
			End Select

			If Double.IsNaN(resultado) Or Double.IsInfinity(resultado) Then Return resultado

			Select Case tmpPieza.Funcion
				Case 0
					resultado = Math.Sin(resultado)
				Case 1
					resultado = Math.Cos(resultado)
				Case 2
					resultado = Math.Tan(resultado)
				Case 3
					resultado = Math.Abs(resultado)
				Case 4
					resultado = Math.Asin(resultado)
				Case 5
					resultado = Math.Acos(resultado)
				Case 6
					resultado = Math.Atan(resultado)
				Case 7
					resultado = Math.Log(resultado)
				Case 8
					resultado = Math.Ceiling(resultado)
				Case 9
					resultado = Math.Exp(resultado)
				Case 10
					resultado = Math.Sqrt(resultado)
				Case 11
					resultado = Math.Pow(resultado, 0.33333333333333331)
			End Select

			If Double.IsNaN(resultado) Or Double.IsInfinity(resultado) Then Return resultado
			tmpPieza.ValorPieza = resultado
		Next

		Return resultado
	End Function

	'Da valor a las variables que tendrá la expresión algebraica
	Public Sub DarValorVariable(ByVal varAlgebra As Char, ByVal valor As Double)
		VariableAlgebra(Asc(varAlgebra) - Asc("a"c)) = valor
	End Sub

End Class

