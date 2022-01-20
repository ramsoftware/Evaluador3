class EvaluaSintaxis:
    # Mensajes de error de sintaxis
    _mensajeError = [
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
            ]

    EsCorrecto = [None] * 27;

    # Retorna si el caracter es un operador matemático
    def EsUnOperador(self, car):
        return car == '+' or car == '-' or car == '*' or car == '/' or car == '^'

    # Retorna si el caracter es un número */
    def EsUnNumero(self, car):
        return car >= '0' and car <= '9'

    # Retorna si el caracter es una letra */
    def EsUnaLetra(self, car):
        return car >= 'a' and car <= 'z'

    # 0. Detecta si hay un caracter no válido
    def BuenaSintaxis00(self, expresion):
        Resultado = True
        permitidos = "abcdefghijklmnopqrstuvwxyz0123456789.+-*/^()"
        pos = 0
        while pos < len(expresion) and Resultado == True:
            if permitidos.find(expresion[pos]) == -1:
                Resultado = False
            pos = pos + 1
        return Resultado

    # 1. Un número seguido de una letra. Ejemplo: 2q-(*3)
    def BuenaSintaxis01(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if self.EsUnNumero(carA) and self.EsUnaLetra(carB):
               Resultado = False
            pos = pos + 1
        return Resultado

    # 2. Un número seguido de un paréntesis que abre. Ejemplo: 7-2(5-6)
    def BuenaSintaxis02(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if self.EsUnNumero(carA) and carB == '(':
               Resultado = False
            pos = pos + 1
        return Resultado

    # 3. Doble punto seguido. Ejemplo: 3..1
    def BuenaSintaxis03(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos+1]
            if carA == '.' and carB == '.': 
                Resultado = False
            pos = pos + 1
        return Resultado

    # 4. Punto seguido de operador. Ejemplo: 3.*1
    def BuenaSintaxis04(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if carA == '.' and self.EsUnOperador(carB):
               Resultado = False
            pos = pos + 1
        return Resultado

     # 5. Un punto y sigue una letra. Ejemplo: 3+5.w-8
    def BuenaSintaxis05(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if carA == '.' and self.EsUnaLetra(carB):
               Resultado = False
            pos = pos + 1
        return Resultado

    # 6. Punto seguido de paréntesis que abre. Ejemplo: 2-5.(4+1)*3
    def BuenaSintaxis06(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if carA == '.' and carB == '(':
               Resultado = False
            pos = pos + 1
        return Resultado;

    # 7. Punto seguido de paréntesis que cierra. Ejemplo: 2-(5.)*3
    def BuenaSintaxis07(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if carA == '.' and carB == ')':
               Resultado = False
            pos = pos + 1
        return Resultado

    # 8. Un operador seguido de un punto. Ejemplo: 2-(4+.1)-7
    def BuenaSintaxis08(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if self.EsUnOperador(carA) and carB == '.':
               Resultado = False
            pos = pos + 1
        return Resultado

    # 9. Dos operadores estén seguidos. Ejemplo: 2++4, 5-*3
    def BuenaSintaxis09(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if self.EsUnOperador(carA) and self.EsUnOperador(carB):
               Resultado = False
            pos = pos + 1
        return Resultado

    # 10. Un operador seguido de un paréntesis que cierra. Ejemplo: 2-(4+)-7
    def BuenaSintaxis10(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if self.EsUnOperador(carA) and carB == ')':
               Resultado = False
            pos = pos + 1
        return Resultado

    # 11. Una letra seguida de número. Ejemplo: 7-2a-6
    def BuenaSintaxis11(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if self.EsUnaLetra(carA) and self.EsUnNumero(carB):
               Resultado = False
            pos = pos + 1
        return Resultado

    # 12. Una letra seguida de punto. Ejemplo: 7-a.-6
    def BuenaSintaxis12(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if self.EsUnaLetra(carA) and carB == '.':
               Resultado = False
            pos = pos + 1
        return Resultado

    # 13. Un paréntesis que abre seguido de punto. Ejemplo: 7-(.4-6)
    def BuenaSintaxis13(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if carA == '(' and carB == '.':
               Resultado = False
            pos = pos + 1
        return Resultado

    # 14. Un paréntesis que abre seguido de un operador. Ejemplo: 2-(*3)
    def BuenaSintaxis14(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if carA == '(' and self.EsUnOperador(carB):
               Resultado = False
            pos = pos + 1
        return Resultado

    # 15. Un paréntesis que abre seguido de un paréntesis que cierra. Ejemplo: 7-()-6
    def BuenaSintaxis15(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if carA == '(' and carB == ')':
               Resultado = False
            pos = pos + 1
        return Resultado

    # 16. Un paréntesis que cierra y sigue un número. Ejemplo: (3-5)7
    def BuenaSintaxis16(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if carA == ')' and self.EsUnNumero(carB):
               Resultado = False
            pos = pos + 1
        return Resultado

    # 17. Un paréntesis que cierra y sigue un punto. Ejemplo: (3-5).
    def BuenaSintaxis17(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos];
            carB = expresion[pos + 1];
            if carA == ')' and carB == '.':
               Resultado = False
            pos = pos + 1
        return Resultado

    # 18. Un paréntesis que cierra y sigue una letra. Ejemplo: (3-5)t
    def BuenaSintaxis18(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if carA == ')' and self.EsUnaLetra(carB):
               Resultado = False
            pos = pos + 1
        return Resultado

    # 19. Un paréntesis que cierra y sigue un paréntesis que abre. Ejemplo: (3-5)(4*5)
    def BuenaSintaxis19(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if carA == ')' and carB == '(':
               Resultado = False
            pos = pos + 1
        return Resultado;

    # 20. Si hay dos letras seguidas (después de quitar las funciones), es un error
    def BuenaSintaxis20(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos]
            carB = expresion[pos + 1]
            if self.EsUnaLetra(carA) and self.EsUnaLetra(carB):
               Resultado = False
            pos = pos + 1
        return Resultado

    # 21. Los paréntesis estén desbalanceados. Ejemplo: 3-(2*4))
    def BuenaSintaxis21(self, expresion):
        parabre = 0 # Contador de paréntesis que abre
        parcierra = 0 # Contador de paréntesis que cierra
        pos = 0
        while pos < len(expresion):
            carA = expresion[pos]
            if carA == '(': parabre += 1
            if carA == ')': parcierra += 1
            pos = pos + 1
        return parcierra == parabre;

    # 22. Doble punto en un número de tipo real. Ejemplo: 7-6.46.1+2
    def BuenaSintaxis22(self, expresion):
        Resultado = True
        totalpuntos = 0 # Validar los puntos decimales de un número real
        pos = 0
        while pos < len(expresion) and Resultado == True:
            carA = expresion[pos]
            if self.EsUnOperador(carA): totalpuntos = 0;
            if carA == '.': totalpuntos += 1
            if totalpuntos > 1: Resultado = False
            pos = pos + 1
        return Resultado

    # 23. Paréntesis que abre no corresponde con el que cierra. Ejemplo: 2+3)-2*(4";
    def BuenaSintaxis23(self, expresion):
        Resultado = True
        parabre = 0 # Contador de paréntesis que abre
        parcierra = 0 # Contador de paréntesis que cierra
        pos = 0
        while pos < len(expresion) and Resultado == True:
            carA = expresion[pos]
            if carA == '(': parabre += 1
            if carA == ')': parcierra += 1
            if parcierra > parabre:
               Resultado = False
            pos = pos + 1
        return Resultado

    # 24. Inicia con operador. Ejemplo: +3*5
    def BuenaSintaxis24(self, expresion):
        carA = expresion[0]
        return not self.EsUnOperador(carA)

    # 25. Finaliza con operador. Ejemplo: 3*5*
    def BuenaSintaxis25(self, expresion):
        carA = expresion[len(expresion) - 1]
        return not self.EsUnOperador(carA)

    # 26. Encuentra una letra seguida de paréntesis que abre. Ejemplo: 3-a(7)-5
    def BuenaSintaxis26(self, expresion):
        Resultado = True
        pos = 0
        while pos < len(expresion)-1 and Resultado == True:
            carA = expresion[pos];
            carB = expresion[pos + 1];
            if self.EsUnaLetra(carA) and carB == '(':
               Resultado = False
            pos = pos + 1
        return Resultado

    def SintaxisCorrecta(self, ecuacion):
            # Reemplaza las funciones de tres letras por una letra mayúscula
            expresion = ecuacion.replace("sen(", "a+(").replace("cos(", "a+(").replace("tan(", "a+(").replace("abs(", "a+(").replace("asn(", "a+(").replace("acs(", "a+(").replace("atn(", "a+(").replace("log(", "a+(").replace("cei(", "a+(").replace("exp(", "a+(").replace("sqr(", "a+(").replace("rcb(", "a+(");

			# Hace las pruebas de sintaxis
            self.EsCorrecto[0] = self.BuenaSintaxis00(expresion);
            self.EsCorrecto[1] = self.BuenaSintaxis01(expresion);
            self.EsCorrecto[2] = self.BuenaSintaxis02(expresion);
            self.EsCorrecto[3] = self.BuenaSintaxis03(expresion);
            self.EsCorrecto[4] = self.BuenaSintaxis04(expresion);
            self.EsCorrecto[5] = self.BuenaSintaxis05(expresion);
            self.EsCorrecto[6] = self.BuenaSintaxis06(expresion);
            self.EsCorrecto[7] = self.BuenaSintaxis07(expresion);
            self.EsCorrecto[8] = self.BuenaSintaxis08(expresion);
            self.EsCorrecto[9] = self.BuenaSintaxis09(expresion);
            self.EsCorrecto[10] = self.BuenaSintaxis10(expresion);
            self.EsCorrecto[11] = self.BuenaSintaxis11(expresion);
            self.EsCorrecto[12] = self.BuenaSintaxis12(expresion);
            self.EsCorrecto[13] = self.BuenaSintaxis13(expresion);
            self.EsCorrecto[14] = self.BuenaSintaxis14(expresion);
            self.EsCorrecto[15] = self.BuenaSintaxis15(expresion);
            self.EsCorrecto[16] = self.BuenaSintaxis16(expresion);
            self.EsCorrecto[17] = self.BuenaSintaxis17(expresion);
            self.EsCorrecto[18] = self.BuenaSintaxis18(expresion);
            self.EsCorrecto[19] = self.BuenaSintaxis19(expresion);
            self.EsCorrecto[20] = self.BuenaSintaxis20(expresion);
            self.EsCorrecto[21] = self.BuenaSintaxis21(expresion);
            self.EsCorrecto[22] = self.BuenaSintaxis22(expresion);
            self.EsCorrecto[23] = self.BuenaSintaxis23(expresion);
            self.EsCorrecto[24] = self.BuenaSintaxis24(expresion);
            self.EsCorrecto[25] = self.BuenaSintaxis25(expresion);
            self.EsCorrecto[26] = self.BuenaSintaxis26(expresion);

            Resultado = True
            cont = 0
            while cont < len(self.EsCorrecto) and Resultado == True:
                   if self.EsCorrecto[cont] == False:
                       Resultado = False;
                   cont = cont + 1
            return Resultado

    #Transforma la expresión para ser chequeada y analizada
    def Transforma(self, expresion):
        #Quita espacios, tabuladores y la vuelve a minúsculas
        nuevo = ""
        for num  in range(0, len(expresion), 1):
            letra = expresion[num]
            if letra >= 'A' and letra <= 'Z':
               letra = chr(ord(letra) + ord(' '));
            if letra != ' ' and letra != '	':
               nuevo = nuevo + letra;
        
        #Cambia los )) por )+0) porque es requerido al crear las piezas.
        while (nuevo.find("))") != -1):
            nuevo = nuevo.replace("))", ")+0)");
        
        return nuevo

    # Muestra mensaje de error sintáctico
    def MensajesErrorSintaxis(self, CodigoError):
        return self._mensajeError[CodigoError]




