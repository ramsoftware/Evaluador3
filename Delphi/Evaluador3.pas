unit Evaluador3;

interface
uses
	//Requerido para el TobjectList
	Contnrs, SysUtils, Math, Partes, Piezas, EvaluaSintaxis;
type
	TEvaluador3 = class
	private
	{ Autor: Rafael Alberto Moreno Parra. 10 de abril de 2021 }

		{ Constantes de los diferentes tipos de datos que tendrán las piezas }
		ESFUNCION: integer;
		ESPARABRE: integer;
		ESPARCIERRA: integer;
		ESOPERADOR: integer;
		ESNUMERO: integer;
		ESVARIABLE: integer;
		ESACUMULA: integer;

		{ Listado de partes en que se divide la expresión
		   Toma una expresión, por ejemplo: 3.14 + sen( 4 / x ) * ( 7.2 ^ 3 - 1 ) y la divide en partes así:
		   |3.14| |+| |sen(| |4| |/| |x| |)| |*| |(| |7.2| |^| |3| |-| |1| |)|
		   Cada parte puede tener un número, un operador, una función, un paréntesis que abre o un paréntesis que cierra }
		Partes: TobjectList;

		{ Listado de piezas que ejecutan
		   Toma las partes y las divide en piezas con la siguiente estructura:
		   acumula = funcion  numero/variable/acumula  operador  numero/variable/acumula
		   Siguiendo el ejemplo anterior sería:
		   A =  7.2  ^  3
		   B =	A  -  1
		   C = seno ( 4  /  x )
		   D =	C  *  B
		   E =  3.14 + D

		   Esas piezas se evalúan de arriba a abajo y así se interpreta la ecuación }
		Piezas: TobjectList;

		{ El arreglo unidimensional que lleva el valor de las variables }
		VariableAlgebra: array[0..26] of double;
		
		procedure CrearPartes(expresion: string);
		function CadenaAReal(Numero: string): double;
		procedure CrearPiezas();
		procedure GenerarPiezasOperador(operA: char; operB: char; inicia: integer);

	public
		// Uso del chequeo de sintaxis
		Sintaxis: TEvaluaSintaxis;

		Constructor Create();
		function Analizar(expresionA: string): boolean;
		function Evaluar(): double;
		procedure DarValorVariable(varAlgebra: char; valor: double);
end;

implementation

Constructor TEvaluador3.Create;
begin
	//Constantes de los diferentes tipos de datos que tendrán las piezas
	self.ESFUNCION := 1;
	self.ESPARABRE := 2;
	self.ESPARCIERRA := 3;
	self.ESOPERADOR := 4;
	self.ESNUMERO := 5;
	self.ESVARIABLE := 6;
	self.ESACUMULA := 7;
end;


// Analiza la expresión
function TEvaluador3.Analizar(expresionA: string): boolean;
var
  expresionB: string;
  pos: integer;
  chequeo: boolean;
begin
      Sintaxis := TEvaluaSintaxis.Create();
			expresionB := Sintaxis.Transforma(expresionA);
			chequeo := Sintaxis.SintaxisCorrecta(expresionB);
			if (chequeo) then
      begin
      	Partes.Free;
      	Piezas.Free;
        Partes := TObjectList.Create;
        Piezas := TObjectList.Create;
				CrearPartes(expresionB);
				CrearPiezas();
			end;
			Result := chequeo;
end;

// Divide la expresión en partes
procedure TEvaluador3.CrearPartes(expresion: string);
var
	NuevoA: string;
	NuevoB: string;
	Numero: string;
	pos: integer;
	car: char;
  objeto: TParte;
begin
	// Debe analizarse con paréntesis
	NuevoA := '(' + expresion + ')';

	// Reemplaza las funciones de tres letras por una letra mayúscula
  NuevoB := StringReplace(NuevoA, 'sen', 'A', [rfReplaceAll, rfIgnoreCase]);
  NuevoB := StringReplace(NuevoB, 'cos', 'B', [rfReplaceAll, rfIgnoreCase]);
  NuevoB := StringReplace(NuevoB, 'tan', 'C', [rfReplaceAll, rfIgnoreCase]);
  NuevoB := StringReplace(NuevoB, 'abs', 'D', [rfReplaceAll, rfIgnoreCase]);
  NuevoB := StringReplace(NuevoB, 'asn', 'E', [rfReplaceAll, rfIgnoreCase]);
  NuevoB := StringReplace(NuevoB, 'acs', 'F', [rfReplaceAll, rfIgnoreCase]);
  NuevoB := StringReplace(NuevoB, 'atn', 'G', [rfReplaceAll, rfIgnoreCase]);
  NuevoB := StringReplace(NuevoB, 'log', 'H', [rfReplaceAll, rfIgnoreCase]);
  NuevoB := StringReplace(NuevoB, 'cei', 'I', [rfReplaceAll, rfIgnoreCase]);
  NuevoB := StringReplace(NuevoB, 'exp', 'J', [rfReplaceAll, rfIgnoreCase]);
  NuevoB := StringReplace(NuevoB, 'sqr', 'K', [rfReplaceAll, rfIgnoreCase]);
  NuevoB := StringReplace(NuevoB, 'rcb', 'L', [rfReplaceAll, rfIgnoreCase]);

	// Va de caracter en caracter
	Numero := '';
	pos := 1;
	while (pos <= Length(NuevoB)) do
	begin
		car := NuevoB[pos];
		
		// Si es un número lo va acumulando en una cadena
		if (car >= '0') and (car <= '9') or (car = '.') then
		begin
			Numero := Numero + car;
		end
		
		// Si es un operador entonces agrega número (si existía)
		else if (car = '+') or (car = '-') or (car = '*') or (car = '/') or (car = '^') then
		begin
			if (Length(Numero) > 0) then
			begin
        objeto := TParte.Create(ESNUMERO, -1, '0', CadenaAReal(Numero), 0);
				Partes.Add(objeto);
				Numero := '';
			end;
      objeto := TParte.Create(ESOPERADOR, -1, car, 0, 0);
			Partes.Add(objeto);
		end
		
		// Si es variable
		else if (car >= 'a') and (car <= 'z') then
		begin
      objeto := TParte.Create(ESVARIABLE, -1, '0', 0, ord(car) - ord('a'));
			Partes.Add(objeto);
		end
		
		// Si es una función (seno, coseno, tangente, ...)
		else if (car >= 'A') and (car <= 'L') then
		begin
      objeto := TParte.Create(ESFUNCION, ord(car) - ord('A'), '0', 0, 0);
			Partes.Add(objeto);
			Inc(pos);
		end
		
		// Si es un paréntesis que abre
		else if (car = '(') then
		begin
      objeto := TParte.Create(ESPARABRE, -1, '0', 0, 0);
			Partes.Add(objeto);
		end
		
		// Si es un paréntesis que cierra
		else 
		begin
			if (Length(Numero) > 0) then
			begin
        objeto := TParte.Create(ESNUMERO, -1, '0', CadenaAReal(Numero), 0);
				Partes.Add(objeto);
				Numero := '';
			end;
		
			// Si sólo había un número o variable dentro del paréntesis le agrega + 0 (por ejemplo:  sen(x) o 3*(2) )
			if ((Partes[Partes.Count - 2] as TParte).Tipo = ESPARABRE) or ((Partes[Partes.Count - 2] as TParte).Tipo = ESFUNCION) then
			begin
        objeto := TParte.Create(ESOPERADOR, -1, '+', 0, 0);
				Partes.Add(objeto);
        objeto := TParte.Create(ESNUMERO, -1, '0', 0, 0);
				Partes.Add(objeto);
			end;

      objeto := TParte.Create(ESPARCIERRA, -1, '0', 0, 0);
			Partes.Add(objeto);
		end;
		Inc(pos);
	end;
end;

// Convierte un número almacenado en una cadena a su valor real
function TEvaluador3.CadenaAReal(Numero: string): double;
var
	parteEntera: double;
	cont: integer;
	parteDecimal: double;
	multiplica: double;
	numeroB: double;
	num: integer;
begin
	// Parte entera
	parteEntera := 0;
	for cont := 1 to length(Numero) do
	begin
		if (Numero[cont] = '.') then break;
		parteEntera := parteEntera * 10 + (ord(Numero[cont]) - ord('0'));
	end;

	// Parte decimal
	parteDecimal := 0;
	multiplica := 1;
	for num := cont + 1 to length(Numero) do
	begin
		parteDecimal := parteDecimal * 10 + (ord(Numero[num]) - ord('0'));
		multiplica := multiplica * 10;
	end;

	numeroB := parteEntera + parteDecimal / multiplica;
	Result := numeroB;
end;

// Ahora convierte las partes en las piezas finales de ejecución
procedure TEvaluador3.CrearPiezas();
var
	cont: integer;
begin
	cont := Partes.Count - 1;
	repeat
		if ((Partes[cont] as TParte).Tipo = ESPARABRE) or ((Partes[cont] as TParte).Tipo = ESFUNCION) then
		begin
			GenerarPiezasOperador('^', '^', cont);  // Evalúa las potencias
			GenerarPiezasOperador('*', '/', cont);  // Luego evalúa multiplicar y dividir
			GenerarPiezasOperador('+', '-', cont);  // Finalmente evalúa sumar y restar

			if ((Partes[cont] as TParte).Tipo = ESFUNCION) then // Agrega la función a la última pieza
			begin
				(Piezas[Piezas.Count - 1] as TPieza).Funcion := (Partes[cont] as TParte).Funcion;
			end;

			// Quita el paréntesis/función que abre y el que cierra, dejando el centro
			Partes.Delete(cont);
			Partes.Delete(cont + 1);
		end;
		Dec(cont);
	until not (cont >= 0);
end;

// Genera las piezas buscando determinado operador
procedure TEvaluador3.GenerarPiezasOperador(operA: char; operB: char; inicia: integer);
var
	cont: integer;
  objeto: TPieza;
begin
	cont := inicia + 1;
	repeat
		if ((Partes[cont] as TParte).Tipo = ESOPERADOR) and ((Partes[cont] as TParte).Operador = operA) or ((Partes[cont] as TParte).Operador = operB) then
		begin

			// Crea Pieza
      objeto := TPieza.Create(-1,
					(Partes[cont - 1] as TParte).Tipo, (Partes[cont - 1] as TParte).Numero,
					(Partes[cont - 1] as TParte).UnaVariable, (Partes[cont - 1] as TParte).Acumulador,
					(Partes[cont] as TParte).Operador,
					(Partes[cont + 1] as TParte).Tipo, (Partes[cont + 1] as TParte).Numero,
					(Partes[cont + 1] as TParte).UnaVariable, (Partes[cont + 1] as TParte).Acumulador);
			Piezas.Add(objeto);

			// Elimina la parte del operador y la siguiente
			Partes.Delete(cont);
			Partes.Delete(cont);

			// Cambia la parte anterior por parte que acumula
			(Partes[cont - 1] as TParte).Tipo := ESACUMULA;
			(Partes[cont - 1] as TParte).Acumulador := Piezas.Count-1;

			// Retorna el contador en uno para tomar la siguiente operación
			Dec(cont);
		end;
		Inc(cont);
	until not ((Partes[cont] as TParte).Tipo <> ESPARCIERRA);
end;

// Evalúa la expresión convertida en piezas
function TEvaluador3.Evaluar(): double;
var
	resultado: double;
	numA: double;
	numB: double;
  pos: integer;
begin
	resultado := 0;

	for pos := 0 to Piezas.Count-1 do
	begin

		if (Piezas[pos] as TPieza).TipoA = ESNUMERO then begin numA := (Piezas[pos] as TPieza).NumeroA; end
		else if ((Piezas[pos] as TPieza).TipoA = ESVARIABLE) then begin numA := VariableAlgebra[(Piezas[pos] as TPieza).VariableA]; end
		else begin numA := (Piezas[(Piezas[pos] as TPieza).PiezaA] as TPieza).ValorPieza; end;

		if ((Piezas[pos] as TPieza).TipoB = ESNUMERO) then begin numB := (Piezas[pos] as TPieza).NumeroB; end
		else if ((Piezas[pos] as TPieza).TipoB = ESVARIABLE) then begin numB := VariableAlgebra[(Piezas[pos] as TPieza).VariableB]; end
		else begin numB := (Piezas[(Piezas[pos] as TPieza).PiezaB] as TPieza).ValorPieza; end;

    try
      if ((Piezas[pos] as TPieza).Operador = '*') then begin resultado := numA * numB; end
      else if ((Piezas[pos] as TPieza).Operador = '/') then begin resultado := numA / numB; end
      else if ((Piezas[pos] as TPieza).Operador = '+') then begin resultado := numA + numB; end
      else if ((Piezas[pos] as TPieza).Operador = '-') then begin resultado := numA - numB; end
      else begin resultado := power(numA, numB); end;

      case ((Piezas[pos] as TPieza).Funcion) of
        0: begin resultado := sin(resultado); end;
        1: begin resultado := cos(resultado); end;
        2: begin resultado := tan(resultado); end;
        3: begin resultado := abs(resultado); end;
        4: begin resultado := arcsin(resultado); end;
        5: begin resultado := arccos(resultado); end;
        6: begin resultado := arctan(resultado); end;
        7: begin resultado := ln(resultado); end;
        8: begin resultado := ceil(resultado); end;
        9: begin resultado := exp(resultado); end;
        10: begin resultado := sqrt(resultado); end;
        11: begin resultado := power(resultado, 0.3333333333333333333333); end;
      end;

    except //Captura el error matemático
        on EMathError do
        begin
          Result := NaN;
          Exit;
        end;
    end;
		(Piezas[pos] as TPieza).ValorPieza := resultado;
	end;
	Result := resultado;
end;

// Da valor a las variables que tendrá la expresión algebraica
procedure TEvaluador3.DarValorVariable(varAlgebra: char; valor: double);
begin
	VariableAlgebra[ord(varAlgebra) - ord('a')] := valor;
end;

end.
