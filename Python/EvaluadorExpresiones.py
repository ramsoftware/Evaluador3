import math
import random

#Clases propias
import Evaluador3

def UsoEvaluador():
#Una expresión algebraica:
#   Números reales usan el punto decimal
#   Uso de paréntesis
#   Las variables deben estar en minúsculas van de la 'a' a la 'z' excepto ñ
#   Las funciones (de tres letras) son:
#       Sen	Seno
#       Cos	Coseno
#       Tan	Tangente
#       Abs	Valor absoluto
#       Asn	Arcoseno
#       Acs	Arcocoseno
#       Atn	Arcotangente
#       Log	Logaritmo Natural
#       Cei	Valor techo
#       Exp	Exponencial
#       Sqr	Raíz cuadrada
#       Rcb	Raíz Cúbica
#   Los operadores son:
#       + (suma)
#       - (resta)
#       * (multiplicación)
#       / (división)
#       ^ (potencia)
#   No se acepta el "-" unario. Luego expresiones como: 4*-2 o (-5+3) o (-x^2) o (-x)^2 son inválidas.
    expresion = "Ckos(0.004 * x) - (Tan(1.78 /  k + h) * SEN(k ^ x) + abs (k^3-h^2))"

    #Instancia el evaluador
    evaluador = Evaluador3.Evaluador3()

    #Analiza la expresión (valida sintaxis)
    if evaluador.Analizar(expresion):

        #Si no hay fallos de sintaxis, puede evaluar la expresión

        #Da valores a las variables que deben estar en minúsculas
        evaluador.DarValorVariable('k', 1.6)
        evaluador.DarValorVariable('x', -8.3)
        evaluador.DarValorVariable('h', 9.29)

        #Evalúa la expresión
        resultado = evaluador.Evaluar()
        print(resultado)

        #Evalúa con ciclos
        for num  in range (1, 10, 1):
            valor = random.random()
            evaluador.DarValorVariable('k', valor)
            resultado = evaluador.Evaluar()
            print(resultado)
    else:
        #Si se detectó un error de sintaxis
        unError = 0
        while unError < len(evaluador.Sintaxis.EsCorrecto): 
            #Muestra que error de sintaxis se produjo
            if evaluador.Sintaxis.EsCorrecto[unError] == False:
                print(evaluador.Sintaxis.MensajesErrorSintaxis(unError))
            unError = unError + 1

#Inicia la aplicación
UsoEvaluador()

