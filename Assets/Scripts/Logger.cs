using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Logger : MonoBehaviour {
    public Text DisplayText;
    int collisionCars = 0, collisionPedestrians = 0, collisionOther = 0;
    int OffroadCounter=0;
    int NoOfTrafficSignalsBroken = 0;
    float speed;
    Vector3 lastPosition;
    bool isOffroad = false;
    string text;
    List<GameObject> CollidableObjects = new List<GameObject>();
    List<GameObject> Cars = new List<GameObject>();
    public GameObject[] Checkpoints = new GameObject[10];
    int numberOfCars, numberOfCollidableObjects, checkpointsCrossed;


    private void FixedUpdate()
    {
        speed = (transform.position - lastPosition).magnitude /Time.deltaTime;
        lastPosition = transform.position;
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down*10, Color.green);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10))
            if (hit.collider.gameObject.tag != "Road" && isOffroad == false)
            {
                OffroadCounter++;
                isOffroad = true;
            }
            else if (hit.collider.gameObject.tag == "Road" && isOffroad == true)
            {
                isOffroad = false;
            }
    }
    // Use this for initialization
    void Start () {
        checkpointsCrossed = 0;
        string path = "./log.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("NoOfTrafficSignalsBroken, collisionCars, numberOfCars, collisionPedestrians, collisionOther, numberOfCollidableObjects, OffroadCounter,checkpointsCrossed\n");
        writer.Close();
        InvokeRepeating("UpdateGUI", 0f, 1.0f);
        numberOfCollidableObjects = GameObject.FindGameObjectsWithTag("Other").Length;
        numberOfCars = GameObject.FindGameObjectsWithTag("Cars").Length;
	}

    private void Update()
    {
        text = "Speed: " + speed +
                        "\nTraffic Lights Broken: " + NoOfTrafficSignalsBroken
                        + "\nCollision(Cars): " + collisionCars +"/"+ numberOfCars
                        + "\nCollision(Pedestrian): " + collisionPedestrians
                        + "\nCollision(Other): " + collisionOther + "/" + numberOfCollidableObjects
                        + "\nOffroad count: " + OffroadCounter
                        + "\nCheckpoints Crossed: "+ checkpointsCrossed+"/10";
        DisplayText.text = text;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Other")
        {
           if (CollidableObjects.Contains(other.gameObject))
                return;
            CollidableObjects.Add(other.gameObject);
            collisionOther++;
        }
        else if (other.gameObject.tag == "Pedestrian")
        {
            collisionPedestrians++;
        }
        else if (other.gameObject.tag == "Cars")
        {
            if (Cars.Contains(other.gameObject))
                return;
            Cars.Add(other.gameObject);
            collisionCars++;
        }
        else if(other.gameObject.tag == "Checkpoint")
        {
            checkpointsCrossed++;
            other.gameObject.SetActive(false);
            if(checkpointsCrossed == 10)
            {
                StartCoroutine(Completed());
            }
            else
            {
                Checkpoints[checkpointsCrossed].SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "TrafficLight")
        {
            if (other.gameObject.GetComponentInParent<TrafficLights>().currentState == 1) //red
                NoOfTrafficSignalsBroken++;
        }
        
    }

    void UpdateGUI()
    {
        string path = "./log.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(NoOfTrafficSignalsBroken + "," + collisionCars + "," + numberOfCars + "," + collisionPedestrians + "," + collisionOther + "," + numberOfCollidableObjects + "," + OffroadCounter + "," + checkpointsCrossed+ "\n");
        writer.Close();

    }
    IEnumerator Completed()
    {
        DisplayText.text = "Thank you for playing!\n Game shutting down in 5 seconds\n";
        DisplayText = null; //Not a good way to disable update but will work for now
        yield return new WaitForSeconds(5);
        Application.Quit();
    }
}
