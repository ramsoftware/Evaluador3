import math
import EvaluaSintaxis

class Parte:
    Tipo = 0 # Acumulador, función, paréntesis que abre, paréntesis que cierra, operador, número, variable
    Funcion = 0 # Código de la función 0:seno, 1:coseno, 2:tangente, 3: valor absoluto, 4: arcoseno, 5: arcocoseno, 6: arcotangente, 7: logaritmo natural, 8: valor tope, 9: exponencial, 10: raíz cuadrada, 11: raíz cúbica
    Operador = '?' # + - * / ^
    Numero = 0 # Número literal, por ejemplo: 3.141592
    UnaVariable = 0 # Variable algebraica
    Acumulador = 0 # Usado cuando la expresión se convierte en piezas. Por ejemplo:
				   # 3 + 2 / 5  se convierte así:
				   #|3| |+| |2| |/| |5| 
				   #|3| |+| |A|  A es un identificador de acumulador

    def __init__(self, tipo, funcion,  operador, numero, variable):
        self.Tipo = tipo
        self.Funcion = funcion
        self.Operador = operador
        self.Numero = numero
        self.UnaVariable = variable
        self.Acumulador = 0

class Pieza:
    ValorPieza = 0 # Almacena el valor que genera la pieza al evaluarse
    Funcion = 0 # Código de la función 0:seno, 1:coseno, 2:tangente, 3: valor absoluto, 4: arcoseno, 5: arcocoseno, 6: arcotangente, 7: logaritmo natural, 8: valor tope, 9: exponencial, 10: raíz cuadrada, 11: raíz cúbica
    TipoA = 0 # La primera parte es un número o una variable o trae el valor de otra pieza
    NumeroA = 0 # Es un número literal
    VariableA = 0 # Es una variable
    PiezaA = 0 # Trae el valor de otra pieza
    Operador = '?' # + suma - resta * multiplicación / división ^ potencia
    TipoB = 0 # La segunda parte es un número o una variable o trae el valor de otra pieza
    NumeroB = 0 # Es un número literal
    VariableB = 0 # Es una variable
    PiezaB = 0 # Trae el valor de otra pieza

    def __init__(self, funcion, tipoA, numA, varA, piezaA, operador, tipoB, numB, varB, piezaB):
        self.Funcion = funcion

        self.TipoA = tipoA
        self.NumeroA = numA
        self.VariableA = varA
        self.PiezaA = piezaA

        self.Operador = operador

        self.TipoB = tipoB
        self.NumeroB = numB
        self.VariableB = varB
        self.PiezaB = piezaB

class Evaluador3:
    # Autor: Rafael Alberto Moreno Parra. 02 de abril de 2021
    # Constantes de los diferentes tipos de datos que tendrán las piezas
    ESFUNCION = 1
    ESPARABRE = 2
    ESPARCIERRA = 3
    ESOPERADOR = 4
    ESNUMERO = 5
    ESVARIABLE = 6
    ESACUMULA = 7

    """ Listado de partes en que se divide la expresión
        Toma una expresión, por ejemplo: 3.14 + sen( 4 / x ) * ( 7.2 ^ 3 - 1 ) y la divide en partes así:
        |3.14| |+| |sen(| |4| |/| |x| |)| |*| |(| |7.2| |^| |3| |-| |1| |)|
        Cada parte puede tener un número, un operador, una función, un paréntesis que abre o un paréntesis que cierra """
    Partes = [];

    """ Listado de piezas que ejecutan
        Toma las partes y las divide en piezas con la siguiente estructura:
        acumula = funcion  numero/variable/acumula  operador  numero/variable/acumula
        Siguiendo el ejemplo anterior sería:
        A =  7.2  ^  3
        B =    A  -  1
        C = seno ( 4  /  x )
        D =    C  *  B
        E =  3.14 + D

        Esas piezas se evalúan de arriba a abajo y así se interpreta la ecuación """
    Piezas = [];

    # El arreglo unidimensional que lleva el valor de las variables
    VariableAlgebra = [None] * 26;

	# Uso del chequeo de sintaxis
    Sintaxis = EvaluaSintaxis.EvaluaSintaxis();

    # Analiza la expresión
    def Analizar(self, expresionA):
        expresionB = self.Sintaxis.Transforma(expresionA);
        chequeo = self.Sintaxis.SintaxisCorrecta(expresionB);
        if (chequeo):
            self.Partes.clear();
            self.Piezas.clear();
            self.CrearPartes(expresionB);
            self.CrearPiezas();
        return chequeo

    # Divide la expresión en partes
    def CrearPartes(self, expresion):
        # Debe analizarse con paréntesis
        NuevoA = "(" + expresion + ")"
        
        # Reemplaza las funciones de tres letras por una letra mayúscula
        NuevoB = NuevoA.replace("sen", "A").replace("cos", "B").replace("tan", "C").replace("abs", "D").replace("asn", "E").replace("acs", "F").replace("atn", "G").replace("log", "H").replace("cei", "I").replace("exp", "J").replace("sqr", "K").replace("rcb", "L")
        Numero = ""

        pos = 0
        while (pos < len(NuevoB)):
            car = NuevoB[pos]

            if car >= '0' and car <= '9' or car == '.':
                Numero += car
            elif car == "+" or car == "-" or car == "*" or car == "/" or car == "^":
                if len(Numero) > 0:
                    self.Partes.append(Parte(self.ESNUMERO, -1, "0", self.CadenaAReal(Numero), 0))
                    Numero = ""
                self.Partes.append(Parte(self.ESOPERADOR, -1, car, 0, 0))
            elif car >= 'a' and car <= 'z':
                self.Partes.append(Parte(self.ESVARIABLE, -1, "0", 0, ord(car) - ord('a')))
            elif car >= 'A' and car <= 'L':
                self.Partes.append(Parte(self.ESFUNCION, ord(car) - ord('A'), "0", 0, 0))
                pos += 1
            elif car == '(':
                self.Partes.append(Parte(self.ESPARABRE, -1, "0", 0, 0))
            else:
                if len(Numero) > 0:
                    self.Partes.append(Parte(self.ESNUMERO, -1, "0", self.CadenaAReal(Numero), 0))
                    Numero = ""

                if self.Partes[len(self.Partes) - 2].Tipo == self.ESPARABRE or self.Partes[len(self.Partes) - 2].Tipo == self.ESFUNCION:
                    self.Partes.append(Parte(self.ESOPERADOR, -1, "+", 0, 0))
                    self.Partes.append(Parte(self.ESNUMERO, -1, "0", 0, 0))

                self.Partes.append(Parte(self.ESPARCIERRA, -1, "0", 0, 0))
            pos = pos + 1

    # Convierte número en cadena a real
    def CadenaAReal(self, Numero):
        parteEntera = 0
        cont = 0

        for cont in range(0, len(Numero), 1):
            if Numero[cont] == '.':
               break
            parteEntera = (parteEntera * 10) + (ord(Numero[cont]) - ord('0'))

        parteDecimal = 0
        multiplica = 1

        for num in range (cont + 1, len(Numero), 1):
            parteDecimal = (parteDecimal * 10) + (ord(Numero[num]) - ord('0'))
            multiplica = multiplica * 10
        
        numeroB = parteEntera + parteDecimal / multiplica
        return numeroB

    # Ahora convierte las partes en las piezas finales de ejecución
    def CrearPiezas(self):
        cont = len(self.Partes)-1

        while True:
            tmpParte = self.Partes[cont];
            if tmpParte.Tipo == self.ESPARABRE or tmpParte.Tipo == self.ESFUNCION:
                self.GenerarPiezasOperador('^', '^', cont);  # Evalúa las potencias
                self.GenerarPiezasOperador('*', '/', cont);  # Luego evalúa multiplicar y dividir
                self.GenerarPiezasOperador('+', '-', cont);  # Finalmente evalúa sumar y restar

                if tmpParte.Tipo == self.ESFUNCION: # Agrega la función a la última pieza
                    self.Piezas[len(self.Piezas) - 1].Funcion = tmpParte.Funcion;

                # Quita el paréntesis/función que abre y el que cierra, dejando el centro
                self.Partes.pop(cont)
                self.Partes.pop(cont + 1)
                
            cont = cont - 1
            if cont < 0:
               break

    def GenerarPiezasOperador(self, operA, operB, ini):
        cont = ini + 1

        while True:
            tmpParte = self.Partes[cont];
            if tmpParte.Tipo == self.ESOPERADOR and (tmpParte.Operador == operA or tmpParte.Operador == operB):
                tmpParteIzq = self.Partes[cont - 1];
                tmpParteDer = self.Partes[cont + 1];
                    
                # Crea Pieza
                self.Piezas.append(Pieza(-1,
                            tmpParteIzq.Tipo, tmpParteIzq.Numero,
                            tmpParteIzq.UnaVariable, tmpParteIzq.Acumulador,
                            tmpParte.Operador,
                            tmpParteDer.Tipo, tmpParteDer.Numero,
                            tmpParteDer.UnaVariable, tmpParteDer.Acumulador))

                # Elimina la parte del operador y la siguiente
                self.Partes.pop(cont)
                self.Partes.pop(cont)

                # Retorna el contador en uno para tomar la siguiente operación
                cont = cont - 1

                # Cambia la parte anterior por parte que acumula
                tmpParteIzq.Tipo = self.ESACUMULA
                tmpParteIzq.Acumulador = len(self.Piezas) - 1

            cont = cont + 1

            if self.Partes[cont].Tipo == self.ESPARCIERRA:
                break

    # Evalúa la expresión convertida en piezas
    def Evaluar(self):
        resultado = 0
        for pos in range(0, len(self.Piezas), 1):
            tmpPieza = self.Piezas[pos];

            if tmpPieza.TipoA == self.ESNUMERO:
               numA = tmpPieza.NumeroA
            elif tmpPieza.TipoA == self.ESVARIABLE:
               numA = self.VariableAlgebra[tmpPieza.VariableA]
            else:
                numA = self.Piezas[tmpPieza.PiezaA].ValorPieza

            if tmpPieza.TipoB == self.ESNUMERO: 
                numB = tmpPieza.NumeroB
            elif  tmpPieza.TipoB == self.ESVARIABLE:
               numB = self.VariableAlgebra[tmpPieza.VariableB]
            else:
                numB = self.Piezas[tmpPieza.PiezaB].ValorPieza

            if tmpPieza.Operador == '*':
                resultado = numA * numB
            elif tmpPieza.Operador == '/':
                try:
                    resultado = numA / numB
                except ZeroDivisionError:
                    return float('NaN')
            elif tmpPieza.Operador == '+':
                resultado = numA + numB
            elif tmpPieza.Operador == '-':
                resultado = numA - numB
            else:
                try:
                    resultado = math.pow(numA, numB)
                except:
                    return float('NaN')

            if tmpPieza.Funcion == 0:
                resultado = math.sin(resultado)
            elif tmpPieza.Funcion == 1:
               resultado = math.cos(resultado)
            elif tmpPieza.Funcion == 2:
                resultado = math.tan(resultado)
            elif tmpPieza.Funcion == 3:
                resultado = math.fabs(resultado)
            elif tmpPieza.Funcion == 4:
                try:
                    resultado = math.asin(resultado)
                except:
                    return float('NaN')
            elif tmpPieza.Funcion == 5:
                try:
                    resultado = math.acos(resultado)
                except:
                    return float('NaN')
            elif tmpPieza.Funcion == 6:
                resultado = math.atan(resultado)
            elif tmpPieza.Funcion == 7:
                try:
                    resultado = math.log(resultado)
                except:
                    return float('NaN')
            elif tmpPieza.Funcion == 8:
                resultado = math.ceil(resultado)
            elif tmpPieza.Funcion == 9:
                try:
                    resultado = math.exp(resultado)
                except:
                    return float('NaN')
            elif tmpPieza.Funcion == 10:
                try:
                    resultado = math.sqrt(resultado)
                except:
                    return float('NaN')
            elif tmpPieza.Funcion == 11:
                resultado = math.pow(resultado, 0.3333333333333333333333)

            if math.isnan(resultado) or math.isinf(resultado):
               return resultado;

            tmpPieza.ValorPieza = resultado;
            
        return resultado;

    # Da valor a las variables que tendrá la expresión algebraica
    def DarValorVariable(self, varAlgebra, valor):
        self.VariableAlgebra[ord(varAlgebra) - ord('a')] = valor;