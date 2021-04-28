unit Partes;

interface
type
  TParte = class
  public
    Tipo: integer; // Acumulador, función, paréntesis que abre, paréntesis que cierra, operador, número, variable
    Funcion: integer; // Código de la función 1:seno, 2:coseno, 3:tangente, 4: valor absoluto, 5: arcoseno, 6: arcocoseno, 7: arcotangente, 8: logaritmo natural, 9: valor tope, 10: exponencial, 11: raíz cuadrada, 12: raíz cúbica
    Operador: char; // + suma - resta * multiplicación / división ^ potencia
    Numero: double; // Número literal, por ejemplo: 3.141592
    UnaVariable: integer; // Variable algebraica */
    Acumulador: integer; { Usado cuando la expresión se convierte en piezas. Por ejemplo:
      							3 + 2 / 5  se convierte así:
			      				|3| |+| |2| |/| |5|
      							|3| |+| |A|  A es un identificador de acumulador }
    Constructor Create(Tipo: integer; Funcion: integer; Operador: char; Numero: double; UnaVariable: integer);
  end;

implementation

Constructor TParte.Create(Tipo: integer; Funcion: integer; Operador: char; Numero: double; UnaVariable: integer);
begin
  self.Tipo := Tipo;
  self.Funcion := Funcion;
  self.Operador := Operador;
  self.Numero := Numero;
  self.UnaVariable := UnaVariable;
  self.Acumulador := 0;
end;
end.
