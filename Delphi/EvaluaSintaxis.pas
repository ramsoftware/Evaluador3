unit EvaluaSintaxis;

interface
	uses
  SysUtils;
type
	TEvaluaSintaxis = class
	private
	//Mensajes de error de sintaxis
	const
		_mensajeError : array[0..26] of string = (
		'0. Caracteres no permitidos. Ejemplo: 3$5+2',
		'1. Un número seguido de una letra. Ejemplo: 2q-(*3)',
		'2. Un número seguido de un paréntesis que abre. Ejemplo: 7-2(5-6)',
		'3. Doble punto seguido. Ejemplo: 3..1',
		'4. Punto seguido de operador. Ejemplo: 3.*1',
		'5. Un punto y sigue una letra. Ejemplo: 3+5.w-8',
		'6. Punto seguido de paréntesis que abre. Ejemplo: 2-5.(4+1)*3',
		'7. Punto seguido de paréntesis que cierra. Ejemplo: 2-(5.)*3',
		'8. Un operador seguido de un punto. Ejemplo: 2-(4+.1)-7',
		'9. Dos operadores estén seguidos. Ejemplo: 2++4, 5-*3',
		'10. Un operador seguido de un paréntesis que cierra. Ejemplo: 2-(4+)-7',
		'11. Una letra seguida de número. Ejemplo: 7-2a-6',
		'12. Una letra seguida de punto. Ejemplo: 7-a.-6',
		'13. Un paréntesis que abre seguido de punto. Ejemplo: 7-(.4-6)',
		'14. Un paréntesis que abre seguido de un operador. Ejemplo: 2-(*3)',
		'15. Un paréntesis que abre seguido de un paréntesis que cierra. Ejemplo: 7-()-6',
		'16. Un paréntesis que cierra y sigue un número. Ejemplo: (3-5)7',
		'17. Un paréntesis que cierra y sigue un punto. Ejemplo: (3-5).',
		'18. Un paréntesis que cierra y sigue una letra. Ejemplo: (3-5)t',
		'19. Un paréntesis que cierra y sigue un paréntesis que abre. Ejemplo: (3-5)(4*5)',
		'20. Hay dos o más letras seguidas (obviando las funciones)',
		'21. Los paréntesis están desbalanceados. Ejemplo: 3-(2*4))',
		'22. Doble punto en un número de tipo real. Ejemplo: 7-6.46.1+2',
		'23. Paréntesis que abre no corresponde con el que cierra. Ejemplo: 2+3)-2*(4' ,
		'24. Inicia con operador. Ejemplo: +3*5',
		'25. Finaliza con operador. Ejemplo: 3*5*',
		'26. Letra seguida de paréntesis que abre (obviando las funciones). Ejemplo: 4*a(6-2)'
	);

		function EsUnOperador(car: char): boolean;
		function EsUnNumero(car: char): boolean;
		function EsUnaLetra(car: char): boolean;
		function BuenaSintaxis00(expresion: string): boolean;
		function BuenaSintaxis01(expresion: string): boolean;
		function BuenaSintaxis02(expresion: string): boolean;
		function BuenaSintaxis03(expresion: string): boolean;
		function BuenaSintaxis04(expresion: string): boolean;
		function BuenaSintaxis05(expresion: string): boolean;
		function BuenaSintaxis06(expresion: string): boolean;
		function BuenaSintaxis07(expresion: string): boolean;
		function BuenaSintaxis08(expresion: string): boolean;
		function BuenaSintaxis09(expresion: string): boolean;
		function BuenaSintaxis10(expresion: string): boolean;
		function BuenaSintaxis11(expresion: string): boolean;
		function BuenaSintaxis12(expresion: string): boolean;
		function BuenaSintaxis13(expresion: string): boolean;
		function BuenaSintaxis14(expresion: string): boolean;
		function BuenaSintaxis15(expresion: string): boolean;
		function BuenaSintaxis16(expresion: string): boolean;
		function BuenaSintaxis17(expresion: string): boolean;
		function BuenaSintaxis18(expresion: string): boolean;
		function BuenaSintaxis19(expresion: string): boolean;
		function BuenaSintaxis20(expresion: string): boolean;
		function BuenaSintaxis21(expresion: string): boolean;
		function BuenaSintaxis22(expresion: string): boolean;
		function BuenaSintaxis23(expresion: string): boolean;
		function BuenaSintaxis24(expresion: string): boolean;
		function BuenaSintaxis25(expresion: string): boolean;
		function BuenaSintaxis26(expresion: string): boolean;
	public
		EsCorrecto : array[0..26] of boolean;
  	function SintaxisCorrecta(ecuacion: string): boolean;
    function Transforma(expresion:string): string;
    function MensajesErrorSintaxis(codigoError: integer): string;
	end;
implementation

	//Retorna si el caracter es un operador matemático */
	function TEvaluaSintaxis.EsUnOperador(car: char): boolean;
	begin
		Result := (car = '+') or (car = '-') or (car = '*') or (car = '/') or (car = '^');
	end;

	// Retorna si el caracter es un número
	function TEvaluaSintaxis.EsUnNumero(car: char): boolean;
	begin
		Result := (car >= '0') and (car <= '9');
	end;

	// Retorna si el caracter es una letra
	function TEvaluaSintaxis.EsUnaLetra(car: char): boolean;
	begin
		Result := (car >= 'a') and (car <= 'z');
	end;

	// 0. Detecta si hay un caracter no válido
	function TEvaluaSintaxis.BuenaSintaxis00(expresion: string): boolean;
	var
		Resultado: boolean;
		permitidos: string;
		pos: integer;
	begin
		Resultado := true;
		permitidos := 'abcdefghijklmnopqrstuvwxyz0123456789.+-*/^()';
		for pos := 1 to length(expresion) do
		begin
			if (permitidos.IndexOf(expresion[pos]) = -1) then
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;

	// 1. Un número seguido de una letra. Ejemplo: 2q-(*3)
	function TEvaluaSintaxis.BuenaSintaxis01(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (EsUnNumero(carA)) and (EsUnaLetra(carB)) then
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;

	// 2. Un número seguido de un paréntesis que abre. Ejemplo: 7-2(5-6)
	function TEvaluaSintaxis.BuenaSintaxis02(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (EsUnNumero(carA)) and (carB = '(') then
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;

	// 3. Doble punto seguido. Ejemplo: 3..1
	function TEvaluaSintaxis.BuenaSintaxis03(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (carA = '.') and (carB = '.') then
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;

	// 4. Punto seguido de operador. Ejemplo: 3.*1
	function TEvaluaSintaxis.BuenaSintaxis04(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (carA = '.') and (EsUnOperador(carB)) then
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;

	// 5. Un punto y sigue una letra. Ejemplo: 3+5.w-8
	function TEvaluaSintaxis.BuenaSintaxis05(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (carA = '.') and (EsUnaLetra(carB)) then
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;

	// 6. Punto seguido de paréntesis que abre. Ejemplo: 2-5.(4+1)*3
	function TEvaluaSintaxis.BuenaSintaxis06(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (carA = '.') and (carB = '(') then
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;

	// 7. Punto seguido de paréntesis que cierra. Ejemplo: 2-(5.)*3
	function TEvaluaSintaxis.BuenaSintaxis07(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (carA = '.') and (carB = ')') then
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;

	// 8. Un operador seguido de un punto. Ejemplo: 2-(4+.1)-7
	function TEvaluaSintaxis.BuenaSintaxis08(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (EsUnOperador(carA)) and (carB = '.') then
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;

	// 9. Dos operadores estén seguidos. Ejemplo: 2++4, 5-*3
	function TEvaluaSintaxis.BuenaSintaxis09(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (EsUnOperador(carA)) and (EsUnOperador(carB)) then
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;

	// 10. Un operador seguido de un paréntesis que cierra. Ejemplo: 2-(4+)-7
	function TEvaluaSintaxis.BuenaSintaxis10(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (EsUnOperador(carA)) and (carB = ')') then
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;

	// 11. Una letra seguida de número. Ejemplo: 7-2a-6
	function TEvaluaSintaxis.BuenaSintaxis11(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (EsUnaLetra(carA)) and (EsUnNumero(carB)) then
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;

	// 12. Una letra seguida de punto. Ejemplo: 7-a.-6
	function TEvaluaSintaxis.BuenaSintaxis12(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (EsUnaLetra(carA)) and (carB = '.') then 
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;

	// 13. Un paréntesis que abre seguido de punto. Ejemplo: 7-(.4-6)
	function TEvaluaSintaxis.BuenaSintaxis13(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (carA = '(') and (carB = '.') then 
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;

	// 14. Un paréntesis que abre seguido de un operador. Ejemplo: 2-(*3)
	function TEvaluaSintaxis.BuenaSintaxis14(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (carA = '(') and (EsUnOperador(carB)) then
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;

	// 15. Un paréntesis que abre seguido de un paréntesis que cierra. Ejemplo: 7-()-6
	function TEvaluaSintaxis.BuenaSintaxis15(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (carA = '(') and (carB = ')') then 
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;

	// 16. Un paréntesis que cierra y sigue un número. Ejemplo: (3-5)7
	function TEvaluaSintaxis.BuenaSintaxis16(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (carA = ')') and (EsUnNumero(carB)) then
			begin
				Resultado := false;
      end;
		end;
		Result := Resultado;
	end;

	// 17. Un paréntesis que cierra y sigue un punto. Ejemplo: (3-5).
	function TEvaluaSintaxis.BuenaSintaxis17(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (carA = ')') and (carB = '.') then
      begin
        Resultado := false;
        break;
      end;
		end;
		Result := Resultado;
	end;

	// 18. Un paréntesis que cierra y sigue una letra. Ejemplo: (3-5)t
	function TEvaluaSintaxis.BuenaSintaxis18(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (carA = ')') and (EsUnaLetra(carB)) then
      begin
        Resultado := false;
        break;
      end;
		end;
		Result := Resultado;
	end;

	// 19. Un paréntesis que cierra y sigue un paréntesis que abre. Ejemplo: (3-5)(4*5)
	function TEvaluaSintaxis.BuenaSintaxis19(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (carA = ')') and (carB = '(') then
      begin
        Resultado := false;
        break;
      end;
		end;
		Result := Resultado;
	end;

	// 20. Si hay dos letras seguidas (después de quitar las funciones), es un error
	function TEvaluaSintaxis.BuenaSintaxis20(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (EsUnaLetra(carA)) and (EsUnaLetra(carB)) then
      begin
        Resultado := false;
        break;
      end;
		end;
		Result := Resultado;
	end;

	// 21. Los paréntesis estén desbalanceados. Ejemplo: 3-(2*4))
	function TEvaluaSintaxis.BuenaSintaxis21(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		parabre: integer;
		parcierra: integer;
	begin
		Resultado := true;
		parabre := 0; // Contador de paréntesis que abre
		parcierra := 0; // Contador de paréntesis que cierra
		for pos := 1 to length(expresion) do
		begin
      if (expresion[pos] = '(') then begin Inc(parabre); end;
      if (expresion[pos] = ')') then begin Inc(parcierra); end;
		end;
    if (parabre <> parcierra) then begin Resultado := false; end;
		Result := Resultado;
	end;

	// 22. Doble punto en un número de tipo real. Ejemplo: 7-6.46.1+2
	function TEvaluaSintaxis.BuenaSintaxis22(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		totalpuntos: integer;
	begin
		Resultado := true;
		totalpuntos := 0; // Validar los puntos decimales de un número real
		for pos := 1 to length(expresion) do
		begin
			carA := expresion[pos];
			if (EsUnOperador(carA)) then begin totalpuntos := 0; end;
			if (carA = '.') then begin Inc(totalpuntos); end;
			if (totalpuntos > 1) then
      begin
        Resultado := false;
        break;
      end;
		end;
		Result := Resultado;
	end;

	// 23. Paréntesis que abre no corresponde con el que cierra. Ejemplo: 2+3)-2*(4
	function TEvaluaSintaxis.BuenaSintaxis23(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		parabre: integer;
		parcierra: integer;
	begin
		Resultado := true;
		parabre := 0; // Contador de paréntesis que abre
		parcierra := 0; // Contador de paréntesis que cierra
		for pos := 1 to length(expresion) do
		begin
      if (expresion[pos] = '(') then begin Inc(parabre); end;
      if (expresion[pos] = ')') then begin Inc(parcierra); end;
			if (parcierra > parabre) then
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;

	// 24. Inicia con operador. Ejemplo: +3*5
	function TEvaluaSintaxis.BuenaSintaxis24(expresion: string): boolean;
  var
    Resultado: boolean;
  begin
    Resultado := not EsUnOperador(expresion[1]);
		Result := Resultado;
	end;

	// 25. Finaliza con operador. Ejemplo: 3*5*
	function TEvaluaSintaxis.BuenaSintaxis25(expresion: string): boolean;
  var
    Resultado: boolean;
  begin
    Resultado := not EsUnOperador(expresion[length(expresion)]);
    Result := Resultado;
	end;

	// 26. Encuentra una letra seguida de paréntesis que abre. Ejemplo: 3-a(7)-5
	function TEvaluaSintaxis.BuenaSintaxis26(expresion: string): boolean;
	var
		Resultado: boolean;
		pos: integer;
		carA: char;
		carB: char;
	begin
		Resultado := true;
		for pos := 1 to length(expresion)-1 do
		begin
			carA := expresion[pos];
			carB := expresion[pos+1];
			if (EsUnaLetra(carA)) and (carB = '(') then
			begin
				Resultado := false;
				break;
			end;
		end;
		Result := Resultado;
	end;


	function TEvaluaSintaxis.SintaxisCorrecta(ecuacion: string): boolean;
  var
    expresion: string;
    Resultado: boolean;
    cont: integer;
  begin
    // Reemplaza las funciones de tres letras por una variable que suma
    expresion := StringReplace(ecuacion, 'sen(', 'a+(', [rfReplaceAll, rfIgnoreCase]);
    expresion := StringReplace(expresion, 'cos(', 'a+(', [rfReplaceAll, rfIgnoreCase]);
    expresion := StringReplace(expresion, 'tan(', 'a+(', [rfReplaceAll, rfIgnoreCase]);
    expresion := StringReplace(expresion, 'abs(', 'a+(', [rfReplaceAll, rfIgnoreCase]);
    expresion := StringReplace(expresion, 'asn(', 'a+(', [rfReplaceAll, rfIgnoreCase]);
    expresion := StringReplace(expresion, 'acs(', 'a+(', [rfReplaceAll, rfIgnoreCase]);
    expresion := StringReplace(expresion, 'atn(', 'a+(', [rfReplaceAll, rfIgnoreCase]);
    expresion := StringReplace(expresion, 'log(', 'a+(', [rfReplaceAll, rfIgnoreCase]);
    expresion := StringReplace(expresion, 'cei(', 'a+(', [rfReplaceAll, rfIgnoreCase]);
    expresion := StringReplace(expresion, 'exp(', 'a+(', [rfReplaceAll, rfIgnoreCase]);
    expresion := StringReplace(expresion, 'sqr(', 'a+(', [rfReplaceAll, rfIgnoreCase]);
    expresion := StringReplace(expresion, 'rcb(', 'a+(', [rfReplaceAll, rfIgnoreCase]);

		// Hace las pruebas de sintaxis
		EsCorrecto[0] := BuenaSintaxis00(expresion);
		EsCorrecto[1] := BuenaSintaxis01(expresion);
		EsCorrecto[2] := BuenaSintaxis02(expresion);
		EsCorrecto[3] := BuenaSintaxis03(expresion);
		EsCorrecto[4] := BuenaSintaxis04(expresion);
		EsCorrecto[5] := BuenaSintaxis05(expresion);
		EsCorrecto[6] := BuenaSintaxis06(expresion);
		EsCorrecto[7] := BuenaSintaxis07(expresion);
		EsCorrecto[8] := BuenaSintaxis08(expresion);
		EsCorrecto[9] := BuenaSintaxis09(expresion);
		EsCorrecto[10] := BuenaSintaxis10(expresion);
		EsCorrecto[11] := BuenaSintaxis11(expresion);
		EsCorrecto[12] := BuenaSintaxis12(expresion);
		EsCorrecto[13] := BuenaSintaxis13(expresion);
		EsCorrecto[14] := BuenaSintaxis14(expresion);
		EsCorrecto[15] := BuenaSintaxis15(expresion);
		EsCorrecto[16] := BuenaSintaxis16(expresion);
		EsCorrecto[17] := BuenaSintaxis17(expresion);
		EsCorrecto[18] := BuenaSintaxis18(expresion);
		EsCorrecto[19] := BuenaSintaxis19(expresion);
		EsCorrecto[20] := BuenaSintaxis20(expresion);
		EsCorrecto[21] := BuenaSintaxis21(expresion);
		EsCorrecto[22] := BuenaSintaxis22(expresion);
		EsCorrecto[23] := BuenaSintaxis23(expresion);
		EsCorrecto[24] := BuenaSintaxis24(expresion);
		EsCorrecto[25] := BuenaSintaxis25(expresion);
		EsCorrecto[26] := BuenaSintaxis26(expresion);

		Resultado := true;
    for cont := 0 to 26 do
    begin
			if (EsCorrecto[cont] = false) then begin Resultado := false; end;
    end;
		Result := Resultado;
end;

function TEvaluaSintaxis.Transforma(expresion:string): string;
var
  nuevo: string;
  num: integer;
  letra: char;
begin
  //Quita espacios, tabuladores y la vuelve a minúsculas */
  nuevo := '';
	for num := 1 to expresion.Length do
  begin
				letra := expresion[num];
				if (letra >= 'A') and (letra <= 'Z') then begin letra := chr(ord(letra) + ord(' ')); end;
				if (letra <> ' ') and (letra <> '	') then begin nuevo := nuevo + letra; end;
  end;
  Result := nuevo;
end;

// Muestra mensaje de error sintáctico
function TEvaluaSintaxis.MensajesErrorSintaxis(codigoError: integer): string;
begin
	Result := _mensajeError[codigoError];
end;

end.
