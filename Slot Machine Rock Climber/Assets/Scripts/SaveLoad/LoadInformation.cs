using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadInformation : MonoBehaviour {

    TicketsControl tickets;

    void Start ()
    {
        tickets = GameObject.FindGameObjectWithTag("ticketManager").GetComponent<TicketsControl>();
        tickets.Load();
    }
	
}
