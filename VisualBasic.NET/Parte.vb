Public Class Parte
	Public Tipo As Integer 'Acumulador, función, paréntesis que abre, paréntesis que cierra, operador, número, variable
	Public Funcion As Integer 'Código de la función 0:seno, 1:coseno, 2:tangente, 3: valor absoluto, 4: arcoseno, 5: arcocoseno, 6: arcotangente, 7: logaritmo natural, 8: valor tope, 9: exponencial, 10: raíz cuadrada, 11: raíz cúbica
	Public Operador As Char '+ suma - resta * multiplicación / división ^ potencia
	Public Numero As Double 'Número literal, por ejemplo: 3.141592
	Public Variable As Integer 'Variable algebraica
	Public Acumulador As Integer 'Usado cuando la expresión se convierte en piezas. Por ejemplo:
	'3 + 2 / 5  se convierte así:
	'|3| |+| |2| |/| |5| 
	'|3| |+| |A|  A es un identificador de acumulador

	Public Sub New(ByVal tipo As Integer, ByVal funcion As Integer, ByVal operador As Char, ByVal numero As Double, ByVal variable As Integer)
		Me.Tipo = tipo
		Me.Funcion = funcion
		Me.Operador = operador
		Me.Numero = numero
		Me.Variable = variable
	End Sub
End Class
