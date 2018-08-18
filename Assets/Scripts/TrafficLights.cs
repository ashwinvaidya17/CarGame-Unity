using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLights : MonoBehaviour {
    public GameObject GreenLight;
    public GameObject RedLight;
    public GameObject SignalZone;
    public int currentState; // 0 green, 1 red
	// Use this for initialization
	void Start () {
        currentState = 0;
        //change state on every timer
        InvokeRepeating("changeState",0f, Random.value*100);
	}
	
    void changeState()
    {
        if(currentState == 0)
        {
            RedLight.SetActive(true);
            GreenLight.SetActive(false);
            currentState = 1;
        }
        else
        {
            RedLight.SetActive(false);
            GreenLight.SetActive(true);
            currentState = 0;
        }

    }

	// Update is called once per frame
	void Update () {
		
	}
}
