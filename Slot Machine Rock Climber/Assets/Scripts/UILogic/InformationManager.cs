using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InformationManager : MonoBehaviour {

    TicketsControl tickets;
    public GameObject lineasUI;
    public GameObject slotsNumberUI;
    public Text numTicketsPorLinea;
    public Text pricePerSpin;
    public Text lastAward;
    public Text numTickets;

    private int numTicketsI;
    private int numLineasActivas;
    private int ticketPorLinea;
    private int tickerPorTirada;
    private int lastAwardTickets;
    private bool autoPlay;
    private bool needChangeLines;
    private bool needChangeTicketsPorLinea;
    private bool needChangeTicketsPorTirada;

    void Start ()
    {
        tickets = GameObject.FindGameObjectWithTag("ticketManager").GetComponent<TicketsControl>();
        autoPlay = false;
    }
	
	void Update ()
    {
        numTicketsI = tickets.numTickets();
        numTickets.text = numTicketsI.ToString();

        //Comprobamos si hace falta modificar el numero de lineas a las que se esta jugando.
        needChangeLines = tickets.needChangeLines();
        if(needChangeLines) showNumLines();

        //Comprobamos si hace falta modificar el numero de tickets por linea a los que se esta jugando.
        needChangeTicketsPorLinea = tickets.needChangeTicketsPorLinea();
        if (needChangeTicketsPorLinea) showNumTicketsPorLinea();

        //Comprobamos si hace falta modificar el numero de tickets por tirada a los que se esta jugando.
        needChangeTicketsPorTirada = tickets.needChangeTicketsPorTirada();
        if(needChangeTicketsPorTirada) showPricePerSpin();

        //Mostramos la ultima puntuacion obtenida
        showLastAward();

        //Miramos el autoSpin;
        checkAutoSpin();
    }

    //Modifica el numero de lineas que se visualizan en la UI dependiendo de las lineas que tenga seleccionadas el jugador.
    private void showNumLines()
    {
        tickets.changeNumLine();
        numLineasActivas = tickets.numLinesActivas();
        string lineaActiva = numLineasActivas.ToString() + "A";
        string lineaInactiva = numLineasActivas.ToString() + "I";

        foreach (Transform child in lineasUI.transform)
        {
            child.gameObject.SetActive(true);
            if(((child.name.Contains("I") && child.name == lineaInactiva) || (child.name.Contains("A") && child.name != lineaActiva)) && child.name != "MoreLines")
            {
                child.gameObject.SetActive(false);
                int a = (int)"hello"[0];
            }
        }

        foreach (Transform child in slotsNumberUI.transform)
        {
            double firstValueCharacter = Char.GetNumericValue(child.name[0]);
            child.gameObject.SetActive(true);
            if(numLineasActivas < firstValueCharacter && child.name.Contains("A"))
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    //Muestra el numero de lineas que se juegan en la tirada.
    private void showNumTicketsPorLinea()
    {
        tickets.changeTicketsPorLinea();
        ticketPorLinea = tickets.numTicketPorLinea();
        numTicketsPorLinea.text = ticketPorLinea.ToString();
    }

    //Muestra el precio que cuesta la tirada.
    private void showPricePerSpin()
    {
        tickets.changeTicketsPorTirada();
        tickerPorTirada = tickets.precioPorTirada();
        pricePerSpin.text = tickerPorTirada.ToString();
    }

    //Muestra el ultimo premio obtenido.
    private void showLastAward()
    {
        lastAwardTickets = tickets.retornaRecompensaAct();
        lastAward.text = lastAwardTickets.ToString();
    }

    //Verifica si esta activado el modo AutoSpin.
    private void checkAutoSpin()
    {
        GameObject autoSpiner = GameObject.FindGameObjectWithTag("autoSpin");
        if (tickets.autoSpinIsActive() == false && autoPlay == false)
        {
            autoPlay = true;
            foreach (Transform children in autoSpiner.transform)
            {
                children.gameObject.SetActive(false);
            }
        }

        if (tickets.autoSpinIsActive() == true && autoPlay == true)
        {
            autoPlay = false;
            foreach (Transform children in autoSpiner.transform)
            {
                children.gameObject.SetActive(true);
            }
        }
    }
}
