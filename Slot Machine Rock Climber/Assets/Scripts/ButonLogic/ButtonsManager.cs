using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsManager : MonoBehaviour {

    TicketsControl tickets;
    Slots slots;

    public AudioSource pressStart;
    public AudioSource betAndDouble;
    public AudioSource incNumLines;
    public AudioSource autoSpin;

    // Use this for initialization
    void Start ()
    {
        tickets = GameObject.FindGameObjectWithTag("ticketManager").GetComponent<TicketsControl>();
        slots = GameObject.FindGameObjectWithTag("slotManager").GetComponent<Slots>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void incNumLineas()
    {
        if(slots.changeInformation == true)
        {
            tickets.incNumLineas();
            tickets.changeNumLine();
            tickets.changeTicketsPorTirada();
            incNumLines.Play();
        }
    }

    public void maxNumTicketsPorLinea()
    {
        if (slots.changeInformation == true)
        {
            tickets.maxTicketsPorLinea();
            tickets.changeTicketsPorLinea();
            tickets.changeTicketsPorTirada();
            betAndDouble.Play();
        }
    }

    public void doblarNumTicketsPorLinea()
    {
        if (slots.changeInformation == true)
        {
            tickets.incTicketsPorLinea();
            tickets.changeTicketsPorLinea();
            tickets.changeTicketsPorTirada();
            betAndDouble.Play();
        }
    }

    public void decrementarNumTicketsPorLinea()
    {
        if (slots.changeInformation == true)
        {
            tickets.decTicketsPorLinea();
            tickets.changeTicketsPorLinea();
            tickets.changeTicketsPorTirada();
            betAndDouble.Play();
        }
    }

    public void pressStartBtn()
    {
        tickets.modificarRealizarTiradaBtn();
        pressStart.Play();
    }

    public void pressAutoSpin()
    {
        tickets.changeAutoSpin();
        autoSpin.Play();
    }

    public void startScene()
    {
        SceneManager.LoadScene("IntroScene");
    }

    public void gameScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void closeGame()
    {
        Application.Quit();
    }
}
