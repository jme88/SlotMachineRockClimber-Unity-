using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketsControl : MonoBehaviour {

    private int tickets;
    private int numLineasActivas;
    private bool changeNumLines;
    private int ticketsPerLine;
    private bool ticketsPerLineMod;
    private int precioPorTiradaR;
    private bool ticketsPorTirada;
    private int recompensa;
    private int ultimateRecompensa;
    private int prevRec;
    private bool realizarTirada;
    private bool autoSpin;

    // Use this for initialization
    void Start ()
    {
        recompensa = 0;

        //En el caso de ser la primera partida o que el jugador se quede sin monedas.
        if(tickets == 0)
        {
            tickets = 1000;
        }
        
        numLineasActivas = 1;
        changeNumLines = true;
        ticketsPerLine = 10;
        ticketsPerLineMod = true;
        ticketsPorTirada = true;
        realizarTirada = false;
        autoSpin = false;
    }

    //------------------------------------------------------ TICKETS DEL JUGADOR --------------------------------------------------------//

    //Restamos los tickets que el jugador a perdido.
    public void loseTickets(int num)
    {
        tickets = tickets - num;
    }

    //Incrementamos los tickets que el jugador a ganado.
    public void winTickets(int num)
    {
        tickets = tickets + num;
    }

    //Retorna el numero de tickets que tiene el jugador.
    public int numTickets()
    {
        return tickets;
    }

    //Se modifica el numero de tickets por el valor que se pasa.
    private void modificarNumTickets(int num)
    {
        tickets = num;
    }

    //------------------------------------------------------ TICKETS DEL JUGADOR --------------------------------------------------------//

    //------------------------------------------------------ NUMERO DE LINEAS JUGADAS --------------------------------------------------------//

    //Incrementa el numero de lineas que el jugador esta jugando (si el valor supera 9, vuelve a empezar a 1)
    public void incNumLineas()
    {
        numLineasActivas = numLineasActivas + 2;
        if (numLineasActivas > 9)
        {
            numLineasActivas = 1;
        }
    }

    //Retorna el numero de lineas que el jugador esta jugando
    public int numLinesActivas()
    {
        return numLineasActivas;
    }

    //Modifica el valor si la UI de numero de lineas necesita modificar-se o no.
    public void changeNumLine()
    {
        changeNumLines = !changeNumLines;
    }

    //Retorna si hace falta modificar la UI de numero de lineas.
    public bool needChangeLines()
    {
        return changeNumLines;
    }

    //------------------------------------------------------ NUMERO DE LINEAS JUGADAS --------------------------------------------------------//

    //------------------------------------------------------ TICKETS POR LINEA JUGADOS --------------------------------------------------------//

    //Modifica elverificador de numero de tickets por linea.
    public void changeTicketsPorLinea()
    {
        ticketsPerLineMod = !ticketsPerLineMod;
    }

    //Retorna si hace falta modificar la UI del numero tickets jugados por linea.
    public bool needChangeTicketsPorLinea()
    {
        return ticketsPerLineMod;
    }

    //Retorna el numero de ticket que se estan jungando por linea.
    public int numTicketPorLinea()
    {
        return ticketsPerLine;
    }

    //Doblamos el numero de tickets jugados por linea.
    public void incTicketsPorLinea()
    {
        if(ticketsPerLine < 10240)
            ticketsPerLine = ticketsPerLine * 2;
    }

    //Reiniciamos a uno el numero de tickets jugados por linea.
    public void decTicketsPorLinea()
    {
        ticketsPerLine = 10;
    }

    //Modificamos a el maximo posible el numero de tickets jugados por linea.
    public void maxTicketsPorLinea()
    { 
        ticketsPerLine = 10240;
    }

    //------------------------------------------------------ TICKETS POR LINEA JUGADOS --------------------------------------------------------//

    //------------------------------------------------------ PRECIO POR TIRADA --------------------------------------------------------//

    //Retorna el precio de la tirada
    public int precioPorTirada()
    {
        precioPorTiradaR = ticketsPerLine * numLineasActivas;
        return precioPorTiradaR;
    }

    //Modifica el verificador de numero de tickets por tirada.
    public void changeTicketsPorTirada()
    {
        ticketsPorTirada = !ticketsPorTirada;
    }

    //Retorna si hace falta modificar la UI del numero de tickets por tirada.
    public bool needChangeTicketsPorTirada()
    {
        return ticketsPorTirada;
    }

    //------------------------------------------------------ PRECIO POR TIRADA --------------------------------------------------------//

    //------------------------------------------------------ RECOMPENSAS --------------------------------------------------------//

    public int recompensaActual(int numAciertos, int num)
    {
        recompensa = 0;

        if (num == 1)
        {
            recompensa = 10 * ticketsPerLine;
        }
        if (num == 2)
        {
            recompensa = 12* ticketsPerLine;
        }
        if (num == 3)
        {
            recompensa = 14 * ticketsPerLine;
        }
        if (num == 4)
        {
            recompensa = 16 * ticketsPerLine;
        }
        if (num == 5)
        {
            recompensa = 18 * ticketsPerLine;
        }
        if (num == 6)
        {
            recompensa = 20 * ticketsPerLine;
        }
        if (num == 7)
        {
            recompensa = 22 * ticketsPerLine;
        }
        if (num == 8)
        {
            recompensa = 24 * ticketsPerLine;
        }
        
        if(numAciertos == 4)
        {
            recompensa = recompensa * 5;
        }

        if (numAciertos == 5)
        {
            recompensa = recompensa * 50;
        }

        return recompensa;
    }

    public void ultimaRecompensaAct(int recompensaAct)
    {
        ultimateRecompensa = recompensaAct;
    }

    public int retornaRecompensaAct()
    {
        return ultimateRecompensa;
    }

    //------------------------------------------------------ RECOMPENSAS --------------------------------------------------------//

    //-------------------------------------------------------- REALIZAR TIRADA --------------------------------------------------//

    public void modificarRealizarTiradaBtn()
    {
        realizarTirada = !realizarTirada;
    }

    public bool realizarTiradaBtn()
    {
        return realizarTirada;
    }

    public void changeAutoSpin()
    {
        autoSpin = !autoSpin;
    }

    public bool autoSpinIsActive()
    {
        return autoSpin;
    }


    public AudioClip ganarTirada(AudioClip clip, float start, float stop)
    {
        int frequency = clip.frequency;
        float timeLength = stop - start;
        int samplesLength = (int)(frequency * timeLength);
        AudioClip newClip = AudioClip.Create(clip.name + "-sub", samplesLength, 1, frequency, false);

        /* Create a temporary buffer for the samples */
        float[] data = new float[samplesLength];

        /* Get the data from the original clip */
        clip.GetData(data, (int)(frequency * start));

        /* Transfer the data to the new clip */
        newClip.SetData(data, 0);

        /* Return the sub clip */
        return newClip;
    }

    //-------------------------------------------------------- REALIZAR TIRADA --------------------------------------------------//

    //-------------------------------------------------------- SAVE --------------------------------------------------//

    public void Save()
    {
        SaveObject saveObject = new SaveObject
        {
            numTickets = tickets
        };
        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.Save(json);
    }

    //-------------------------------------------------------- SAVE --------------------------------------------------//

    //-------------------------------------------------------- LOAD --------------------------------------------------//

    public void Load()
    {
        string saveString = SaveSystem.Load();
        if (saveString != null)
        {
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);
            modificarNumTickets(saveObject.numTickets);
        }
        else
        {
            //No se puede cargar.
            Debug.Log("Error al cargar");
        }
    }

    //-------------------------------------------------------- LOAD --------------------------------------------------//


    private class SaveObject
    {
        public int numTickets;
    }
}
