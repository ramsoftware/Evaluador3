Module Module1

	Sub Main()
		UsoEvaluador()
		Console.ReadKey()
	End Sub

	Sub UsoEvaluador()
		'Una expresión algebraica
		'Números reales usan el punto decimal
		'Uso de paréntesis
		'Las variables deben estar en minúsculas van de la 'a' a la 'z' excepto ñ
		'Las funciones(de tres letras) son
		'	Sen     Seno
		'	Cos     Coseno
		'	Tan     Tangente
		'	Abs     Valor absoluto
		'	Asn     Arcoseno
		'	Acs     Arcocoseno
		'	Atn     Arcotangente
		'	Log     Logaritmo Natural
		'	Cei     Valor techo
		'	Exp     Exponencial
		'	Sqr     Raíz cuadrada
		'	Rcb     Raíz Cúbica
		'Los operadores son: 
		'	+ (suma)
		'	- (resta)
		'	* (multiplicación)
		'	/ (división)
		'	^ (potencia)
		'No se acepta el "-" unario. Luego expresiones como:  4*-2 o (-5+3) o (-x^2) o (-x)^2 son inválidas.
		Dim expresion As String = "Cos(0.004 * x) - (Tan(1.78 /  k + h) * SEN(k ^ x) + abs (k^3-h^2))"

		'Instancia el evaluador
		Dim evaluador As Evaluador3 = New Evaluador3()

		'Analiza la expresión (valida sintaxis)
		If evaluador.Analizar(expresion) Then
			'Si no hay fallos de sintaxis, puede evaluar la expresión

			'Da valores a las variables que deben estar en minúsculas
			evaluador.DarValorVariable("k", 1.6)
			evaluador.DarValorVariable("x", -8.3)
			evaluador.DarValorVariable("h", 9.29)

			'Evalúa la expresión
			Dim resultado As Double = evaluador.Evaluar()
			Console.WriteLine(resultado)

			'Evalúa con ciclos
			Dim azar As New Random()
			For num As Integer = 1 To 10 Step 1
				Dim valor As Double = azar.NextDouble()
				evaluador.DarValorVariable("k", valor)
				resultado = evaluador.Evaluar()
				Console.WriteLine(resultado)
			Next
		Else
			For unError As Integer = 0 To evaluador.Sintaxis.EsCorrecto.Length - 1 Step 1
				If evaluador.Sintaxis.EsCorrecto(unError) = False Then
					Console.WriteLine(evaluador.Sintaxis.MensajesErrorSintaxis(unError))
				End If
			Next
		End If
	End Sub
End Module
