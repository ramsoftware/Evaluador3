Public Class Pieza
	Public ValorPieza As Double 'Almacena el valor que genera la pieza al evaluarse
	Public Funcion As Integer 'Código de la función 0:seno, 1:coseno, 2:tangente, 3: valor absoluto, 4: arcoseno, 5: arcocoseno, 6: arcotangente, 7: logaritmo natural, 8: valor tope, 9: exponencial, 10: raíz cuadrada, 11: raíz cúbica
	Public TipoA As Integer 'La primera parte es un número o una variable o trae el valor de otra pieza
	Public NumeroA As Double 'Es un número literal
	Public VariableA As Integer 'Es una variable
	Public PiezaA As Integer 'Trae el valor de otra pieza */
	Public Operador As Char '+ suma - resta * multiplicación / división ^ potencia
	Public TipoB As Integer 'La segunda parte es un número o una variable o trae el valor de otra pieza
	Public NumeroB As Double 'Es un número literal
	Public VariableB As Integer 'Es una variable
	Public PiezaB As Integer 'Trae el valor de otra pieza

	Public Sub New(ByVal Funcion As Integer, ByVal TipoA As Integer, ByVal NumeroA As Double, ByVal VariableA As Integer, ByVal PiezaA As Integer, ByVal Operador As Char, ByVal TipoB As Integer, ByVal NumeroB As Double, ByVal VariableB As Integer, ByVal PiezaB As Integer)
		Me.Funcion = Funcion
		Me.TipoA = TipoA
		Me.NumeroA = NumeroA
		Me.VariableA = VariableA
		Me.PiezaA = PiezaA
		Me.Operador = Operador
		Me.TipoB = TipoB
		Me.NumeroB = NumeroB
		Me.VariableB = VariableB
		Me.PiezaB = PiezaB
	End Sub
End Class
