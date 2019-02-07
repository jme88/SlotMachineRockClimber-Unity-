using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slots : MonoBehaviour
{

    public Reel[] reel;
    public bool endTurn;
    public bool changeInformation;
    public AudioClip winCoins;
    public AudioSource audioSource;

    TicketsControl tickets;

    private bool startSpin;
    private int precioPorTirada;
    private int numTickets;
    private int award;
    private int awardEsteTurno;
    private int lineasActivasTurno;
    private int numeroSlot;

    private List<List<int>> lAwardLists = new List<List<int>>();

    #region
    //Declaramos las nueve lineas que se tendran que verificar.
    private int[] line1 = new int[5];
    private int[] line2 = new int[5];
    private int[] line3 = new int[5];
    private int[] line4 = new int[5];
    private int[] line5 = new int[5];
    private int[] line6 = new int[5];
    private int[] line7 = new int[5];
    private int[] line8 = new int[5];
    private int[] line9 = new int[5];

    private List<int> uno = new List<int>();
    private List<int> dos = new List<int>();
    private List<int> tres = new List<int>();
    private List<int> cuatro = new List<int>();
    private List<int> cinco = new List<int>();
    private List<int> seis = new List<int>();
    private List<int> siete = new List<int>();
    private List<int> ocho = new List<int>();
    private List<int> nueve = new List<int>();

    #endregion

    public Text Info1;
    public Text Info2;

    //Declaramos la matriz que utilizaremos para comprobar si hay premios
    private int[,] slotsToCheck = new int[3, 5];

    void Start()
    {
        changeInformation = true;
        startSpin = false;
        endTurn = false;
        tickets = GameObject.FindGameObjectWithTag("ticketManager").GetComponent<TicketsControl>();
    }

    void Update()
    {
        Info1.text = changeInformation.ToString();

        //Esperamos a que haya finalizado la tirada anterior para volver a tirar.
        if (!startSpin)
        {
            //Realizamos una nueva tirada.
            if (tickets.realizarTiradaBtn() || tickets.autoSpinIsActive())
            {
                //Impedimos otra tirada hasta finalizar esta.
                tickets.modificarRealizarTiradaBtn();

                //Miramos el coste de la tirada
                precioPorTirada = tickets.precioPorTirada();
                numTickets = tickets.numTickets();

                //Si el coste de la tirada es inferior al dinero que tenemos, realizamos la tirada
                if (numTickets >= precioPorTirada)
                {
                    //Impedimos que se pueda modificar el numero de lineas y la apuesta por linea.
                    changeInformation = false;
                    //Vibramos el movil con la tirada
                    tickets.loseTickets(precioPorTirada);
                    startSpin = true;
                    StartCoroutine(Spinning());
                }
                else
                {
                    if(tickets.numTickets() < 500)
                    {
                        //Regalamos al jugador 1000 tickets si se queda sin tiquets.
                        tickets.winTickets(1000);
                    }
                }
            }
        }

        if(endTurn == true)
        {
            //Verifica la recompensa de la tirada.
            checkReward();
        }
    }

    IEnumerator Spinning()
    {
        foreach (Reel spinner in reel)
        {
            //Indica a cada spin que empiece a girar.
            spinner.spin = true;
        }

        for (int i = 0; i < reel.Length; i++)
        {
            //Los spins giran durante un periodo de tiempo, finalmente se detienen con una posicion aleatoria para cada casilla del spin.
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1));
            reel[i].spin = false;
            reel[i].RandomPosition();
        }

        //Indicamos a la maquina que esta preparada para una nueva tirada.
        startSpin = false;
        endTurn = true;
    }

    //Funcion que se encarga de dar la recompensa y mostrarla.
    void checkReward()
    {
        //Creamos la matriz a partir de los datos que han salido.
        createMatrixToCheck();

        //Creamos las lineas a partir de los valores obtenidos de la tirada.
        crearLineas();

        //Las siguientes lineas indican si la linea tiene recompensa y en que posicion estan las casillas ganadoras.
        uno = lookLines(line1);
        dos = lookLines(line2);
        tres = lookLines(line3);
        cuatro = lookLines(line4);
        cinco = lookLines(line5);
        seis = lookLines(line6);
        siete = lookLines(line7);
        ocho = lookLines(line8);
        nueve = lookLines(line9);

        lAwardLists.Add(uno);
        lAwardLists.Add(dos);
        lAwardLists.Add(tres);
        lAwardLists.Add(cuatro);
        lAwardLists.Add(cinco);
        lAwardLists.Add(seis);
        lAwardLists.Add(siete);
        lAwardLists.Add(ocho);
        lAwardLists.Add(nueve);

        //Indicamos que la recompensa este turno antes de mirar las lineas es 0.
        awardEsteTurno = 0;

        //Miramos el numero de lineas al que jugamos.
        lineasActivasTurno = tickets.numLinesActivas();

        //Recorremos las listas y visualizamos por UI las casillas ganadoras.
        for (int i = 0; i < lAwardLists.Count; i ++)
        {
            if(lAwardLists[i].Count >= 3 && (lineasActivasTurno >= i + 1))
            {
                awardLines(lAwardLists[i], i);
            }
        }

        tickets.ultimaRecompensaAct(awardEsteTurno);

        if(awardEsteTurno > 0)
        {
            //Seleccionamos el fragmento de audio que reproducimos, dependiendo del premio obtenido.
            if(awardEsteTurno < 1000)
            {
                audioSource.clip = tickets.ganarTirada(winCoins, 5, 6f);
            }

            if(awardEsteTurno >= 1000)
            {
                audioSource.clip = tickets.ganarTirada(winCoins, 4, 6f);
            }

            if (awardEsteTurno >= 5000)
            {
                audioSource.clip = tickets.ganarTirada(winCoins, 3, 6f);
            }

            //Reproducir sonido
            audioSource.Play();
        }

        lAwardLists.Clear();

        endTurn = false;
        changeInformation = true;
        tickets.Save();
    }

    //TODO Para un futuro juego mas serio, esto se tiene que modificar para no dar errores con los diferentes dispositivos moviles(SCREEN SIZE).
    //Generamos la matriz a partir de los datos que se han generado con la tirada.
    void createMatrixToCheck()
    {
        for (int i = 0; i < reel.Length; i++)
        {
            foreach (Transform image in reel[i].transform)
            {
                float pos = image.transform.localPosition.y;

                if (pos >= 150 && pos <= 250 )
                {
                    slotsToCheck[0, i] = Int32.Parse(image.name);
                }

                if (pos >= -50 && pos <= 50)
                {
                    slotsToCheck[1, i] = Int32.Parse(image.name);
                }

                if (pos >= -250 && pos <= -150)
                {
                    slotsToCheck[2, i] = Int32.Parse(image.name);
                }
            }
        }
    }

    //Verificamos si la linea tiene alguna recompensa.
    List<int> lookLines(int[] line)
    {
        List<int> longestSequence = new List<int>();
        List<int> currentSequence = new List<int>();

        for (int i = 0; i < line.Length - 1; i++)
        {
            if (line[i] == line[i + 1])
            {
                currentSequence.Add(i);

                //En el caso de hacer la ultima verificacion, tambien incluimos la ultima fila
                if(i == 3)
                {
                    currentSequence.Add(i + 1);
                }
            }
            else
            {
                currentSequence.Add(i);
                if(currentSequence.Count > longestSequence.Count)
                {
                    longestSequence = new List<Int32>(currentSequence);
                }
                currentSequence.Clear();
            }
            if (currentSequence.Count > longestSequence.Count)
            {
                longestSequence = new List<Int32>(currentSequence);
            }
        }

        return longestSequence;
    }

    //Mostramos por UI las lineas ganadoras y la recompensa obtenida
    void awardLines(List<int> lista, int linea)
    {
        //Regulamos la linea
        linea = linea + 1;

        int numero = cehckNumSlot(linea);

        //Obtenemos la recompensa obtenida en la tirada
        award = tickets.recompensaActual(lista.Count, numero);

        awardEsteTurno = awardEsteTurno + award;

        StartCoroutine(showAward(awardEsteTurno));

        //Añadimos los tickets
        tickets.winTickets(award);

        //Activar la linea que a sido premiada
        GameObject winLines = GameObject.FindGameObjectWithTag("WinLine");

        foreach (Transform children in winLines.transform)
        {
            string valor = linea.ToString();
            if (children.name == valor)
            {
                //children.gameObject.SetActive(true);
                StartCoroutine(lineWinEnableLanes(children.gameObject));
            }
        }
    }

    //Muesta el valor de recompensa
    IEnumerator showAward(int award)
    {
        GameObject showAward = GameObject.FindGameObjectWithTag("recomp");
        showAward.GetComponentInChildren<Text>().text = award.ToString();

        foreach (Transform children in showAward.transform)
        {
            if (children.name == "RecomenpsaUI")
            {
                StartCoroutine(lineWinEnable(children.gameObject));
            }
        }
        yield return new WaitForSeconds(1);
        showAward.GetComponentInChildren<Text>().text = "";

    }

    //Activa el gameObject que muestra la recompensa en UI
    IEnumerator lineWinEnable(GameObject line)
    {
        line.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        line.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        line.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        line.SetActive(false);
    }

    //Activa el gameObject que muestra las lineas en UI
    IEnumerator lineWinEnableLanes(GameObject line)
    {
        line.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        line.SetActive(false);
    }

    //Funcion que encuentra el numero que mas se repite en una sequancia
    private int mirarSlotEnReel(int []linea, int n)
    {
        // Sort the array 
        Array.Sort(linea);

        // find the max frequency using  
        // linear traversal 
        int max_count = 1;
        int res = linea[0];
        int curr_count = 1;

        for (int i = 1; i < n; i++)
        {
            if (linea[i] == linea[i - 1])
                curr_count++;
            else
            {
                if (curr_count > max_count)
                {
                    max_count = curr_count;
                    res = linea[i - 1];
                }
                curr_count = 1;
            }
        }

        // If last element is most frequent 
        if (curr_count > max_count)
        {
            max_count = curr_count;
            res = linea[n - 1];
        }

        return res;
    }

    //Retorna el valor que mas se repite en una sequancia a partir de la linea.
    private int cehckNumSlot(int linea)
    {
        if (linea == 1)
        {
            numeroSlot = mirarSlotEnReel(line1, line1.Length);
        }
        if (linea == 2)
        {
            numeroSlot = mirarSlotEnReel(line2, line2.Length);
        }
        if (linea == 3)
        {
            numeroSlot = mirarSlotEnReel(line3, line3.Length);
        }
        if (linea == 4)
        {
            numeroSlot = mirarSlotEnReel(line4, line4.Length);
        }
        if (linea == 5)
        {
            numeroSlot = mirarSlotEnReel(line5, line5.Length);
        }
        if (linea == 6)
        {
            numeroSlot = mirarSlotEnReel(line6, line6.Length);
        }
        if (linea == 7)
        {
            numeroSlot = mirarSlotEnReel(line7, line7.Length);
        }
        if (linea == 8)
        {
            numeroSlot = mirarSlotEnReel(line8, line8.Length);
        }
        if (linea == 9)
        {
            numeroSlot = mirarSlotEnReel(line9, line9.Length);
        }
        return numeroSlot;
    }

    //Generamos la logica de lineas.
    private void crearLineas()
    {
        //Linea 1
        //[1,0] - [1,1] - [1,2] - [1,3] - [1,4]
        line1[0] = slotsToCheck[1, 0];
        line1[1] = slotsToCheck[1, 1];
        line1[2] = slotsToCheck[1, 2];
        line1[3] = slotsToCheck[1, 3];
        line1[4] = slotsToCheck[1, 4];

        //Linea 2
        //[0,0] - [0,1] - [0,2] - [0,3] - [0,4]
        line2[0] = slotsToCheck[0, 0];
        line2[1] = slotsToCheck[0, 1];
        line2[2] = slotsToCheck[0, 2];
        line2[3] = slotsToCheck[0, 3];
        line2[4] = slotsToCheck[0, 4];

        //Linea 3
        //[2,0] - [2,1] - [2,2] - [2,3] - [2,4]
        line3[0] = slotsToCheck[2, 0];
        line3[1] = slotsToCheck[2, 1];
        line3[2] = slotsToCheck[2, 2];
        line3[3] = slotsToCheck[2, 3];
        line3[4] = slotsToCheck[2, 4];

        //Linea 4
        //[0,0] - [1,1] - [2,2] - [1,3] - [0,4]
        line4[0] = slotsToCheck[0, 0];
        line4[1] = slotsToCheck[1, 1];
        line4[2] = slotsToCheck[2, 2];
        line4[3] = slotsToCheck[1, 3];
        line4[4] = slotsToCheck[0, 4];

        //Linea 5
        //[2,0] - [1,1] - [0,2] - [1,3] - [2,4]
        line5[0] = slotsToCheck[2, 0];
        line5[1] = slotsToCheck[1, 1];
        line5[2] = slotsToCheck[0, 2];
        line5[3] = slotsToCheck[1, 3];
        line5[4] = slotsToCheck[2, 4];

        //Linea 6
        //[0,0] - [0,1] - [1,2] - [0,3] - [0,4]
        line6[0] = slotsToCheck[0, 0];
        line6[1] = slotsToCheck[0, 1];
        line6[2] = slotsToCheck[1, 2];
        line6[3] = slotsToCheck[0, 3];
        line6[4] = slotsToCheck[0, 4];

        //Linea 7
        //[2,0] - [2,1] - [1,2] - [2,3] - [2,4]
        line7[0] = slotsToCheck[2, 0];
        line7[1] = slotsToCheck[2, 1];
        line7[2] = slotsToCheck[1, 2];
        line7[3] = slotsToCheck[2, 3];
        line7[4] = slotsToCheck[2, 4];

        //Linea 8
        //[1,0] - [0,1] - [0,2] - [0,3] - [1,4]
        line8[0] = slotsToCheck[1, 0];
        line8[1] = slotsToCheck[0, 1];
        line8[2] = slotsToCheck[0, 2];
        line8[3] = slotsToCheck[0, 3];
        line8[4] = slotsToCheck[1, 4];

        //Linea 9
        //[1,0] - [2,1] - [2,2] - [2,3] - [1,4]
        line9[0] = slotsToCheck[1, 0];
        line9[1] = slotsToCheck[2, 1];
        line9[2] = slotsToCheck[2, 2];
        line9[3] = slotsToCheck[2, 3];
        line9[4] = slotsToCheck[1, 4];

    }

}