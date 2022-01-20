import math
import random
from time import time

#Clases propias
import Evaluador3

#Hacer pruebas al que chequea la sintaxis
def ProbarSintaxis():
    exprAlgebraica = [ "7#3*12-5", "2q-(*3)", "7-2(5-6)", "3..1", "3.*1", "3+5.w-8", "2-5.(4+1)*3", "2-(5.)*3", "2-(4+.1)-7",
                      "5-*3", "2-(4+)-7", "7-a2-6", "7-a.4*3", "7-(.4-6)", "2-(*3)", "7-()-6", "(3-5)7", "(3-5).",
                      "(3-5)t", "(3-5)(4*5)", "3-ab*7", "3-(2*4))", "7-6.46.1+2", "2+3)-2*(4", "4+art(tan(5))", "+3*5", "3*5*",
                      "3-a(7)-5" ]
    evaluador2021 = Evaluador3.Evaluador3()
    num = 0;
    while num < len(exprAlgebraica):
        print("\r\nExpresión " + str(num) + ": " + exprAlgebraica[num])
        if evaluador2021.Sintaxis.SintaxisCorrecta(exprAlgebraica[num]) == False:
            unError = 0
            while unError < len(evaluador2021.Sintaxis.EsCorrecto): 
                if evaluador2021.Sintaxis.EsCorrecto[unError] == False:
                    print(evaluador2021.Sintaxis.MensajesErrorSintaxis(unError))
                unError = unError + 1
        num = num + 1

def EcuacionAzar(longitud):
    cont = 0
    numParentesisAbre = 0

    ecuacion = ""
    while cont < longitud:
        #Función o paréntesis o nada
        valorazar = random.randint(0,2)
        if valorazar == 0:
            ecuacion = ecuacion + "(";
            numParentesisAbre = numParentesisAbre + 1;
            cont = cont + 1;
        elif valorazar == 1:
            ecuacion = ecuacion + funcionAzar() + "("
            numParentesisAbre = numParentesisAbre + 1
            cont = cont + 1;

        #Variable o número
        cont = cont + 1
        valorazar = random.randint(0,3)
        if valorazar == 0:
            ecuacion = ecuacion + numeroAzar()
        elif valorazar == 1:
            ecuacion = ecuacion + "x"
        elif valorazar == 2:
            ecuacion = ecuacion + "y"
        else:
            ecuacion = ecuacion + "z"

        #Paréntesis que cierra
        numParentesisCierra = random.randint(0, numParentesisAbre + 1)
        for num in range(1, numParentesisCierra, 1):
            ecuacion = ecuacion + ")"
            numParentesisAbre = numParentesisAbre - 1
            cont = cont + 1

        #Operador
        cont = cont + 1
        ecuacion = ecuacion + operadorAzar();

    #Variable o número
    valorazar = random.randint(0,3)
    if valorazar == 0:
        ecuacion = ecuacion + numeroAzar()
    elif valorazar == 1:
        ecuacion = ecuacion + "x"
    elif valorazar == 2:
        ecuacion = ecuacion + "y"
    else:
        ecuacion = ecuacion + "z"

    #Pone el resto de paréntesis que cierra
    for num in range(0, numParentesisAbre, 1):
        ecuacion = ecuacion + ")"

    return ecuacion

def funcionAzar():
    funciones = [ "sen", "cos", "tan", "abs", "asn", "acs", "atn", "log", "cei", "exp", "sqr" ]
    return funciones[random.randint(0, len(funciones)-1)]

def operadorAzar():
    operadores = [ "+", "-", "*", "/", "^" ]
    return operadores[random.randint(0, len(operadores)-1)]

def numeroAzar():
    undecimal = random.randint(1, 1000)
    return "1." + str(undecimal);

def Comparar2021vsInterno():
    evaluador = Evaluador3.Evaluador3()
    #Arreglos que guardan valores de X, Y, Z
    arregloX = [0] * 100;
    arregloY = [0] * 100;
    arregloZ = [0] * 100;
    for cont in range(0, 100, 1):
        arregloX[cont] = random.random() - random.random()
        arregloY[cont] = random.random() + random.random()
        arregloZ[cont] = random.random() * random.random()

    # Haciendo múltiples pruebas
    valorTotal2021 = 0
    valorTotalInterno = 0
    tiempoEvalua2021Total = 0
    tiempoTotalInterno = 0
    for num in range(1, 20, 1):
        ecuacionPrueba = EcuacionAzar(25)
        print(ecuacionPrueba)

        # Tiempo del evaluador de expresiones 2021
        evaluador.Analizar(ecuacionPrueba)

        tiempoEvalua2021Inicia = time()
        valor2021 = 0
        for cont  in range(0, 100, 1):
            evaluador.DarValorVariable('x', arregloX[cont])
            evaluador.DarValorVariable('y', arregloY[cont])
            evaluador.DarValorVariable('z', arregloZ[cont])
            valor2021 += evaluador.Evaluar()
        tiempoEvalua2021Total = tiempoEvalua2021Total + time() - tiempoEvalua2021Inicia

        # Tiempo del evaluador propio interno
        ecuacionPrueba = ecuacionPrueba.replace("sen", "math.sin")
        ecuacionPrueba = ecuacionPrueba.replace("cos", "math.cos")
        ecuacionPrueba = ecuacionPrueba.replace("tan", "math.tan")
        ecuacionPrueba = ecuacionPrueba.replace("abs", "math.fabs")
        ecuacionPrueba = ecuacionPrueba.replace("asn", "math.asin")
        ecuacionPrueba = ecuacionPrueba.replace("acs", "math.acos")
        ecuacionPrueba = ecuacionPrueba.replace("atn", "math.atan")
        ecuacionPrueba = ecuacionPrueba.replace("log", "math.log")
        ecuacionPrueba = ecuacionPrueba.replace("cei", "math.ceil")
        ecuacionPrueba = ecuacionPrueba.replace("exp", "math.exp")
        ecuacionPrueba = ecuacionPrueba.replace("sqr", "math.sqrt")
        
        tiempoInicioEvaluadorInterno = time();
        valorReal = 0
        for cont  in range(0, 100, 1):
            x = arregloX[cont]
            y = arregloY[cont]
            z = arregloZ[cont]
            try:
                valorReal += eval(ecuacionPrueba)
            except:
                valorReal = float('nan')
        
        tiempoTotalInterno = tiempoTotalInterno + time() - tiempoInicioEvaluadorInterno

        if math.isnan(valorReal):
            continue

        # Chequea si hay un fallo en el cálculo
        if abs(valor2021-valorReal)>0.001:
            print(ecuacionPrueba + ": " + str(valor2021) + " vs " + str(valorReal))

        valorTotal2021 += valor2021
        valorTotalInterno += valorReal

    print("Tiempo evaluador 2021: %0.10f seconds." % tiempoEvalua2021Total)
    print("Tiempo evaluador interno: %0.10f seconds." % tiempoTotalInterno)
    print("Acumula evaluador 2021: %0.10f " % valorTotal2021)
    print("Acumula evaluador interno: %0.10f " % valorTotalInterno)
    print("Si los acumulados son iguales, significa que el evaluador está correcto")


#Inicia la aplicación
ProbarSintaxis()
print("\r\nProbando varias ecuaciones enfrentando el evaluador de expresiones vs el interno")
Comparar2021vsInterno()
