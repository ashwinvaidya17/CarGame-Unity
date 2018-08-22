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
    int numberOfCars, numberOfCollidableObjects;
    public int LapCount = 1;
    public GameObject water;
    public GameObject barricades;


    private void FixedUpdate()
    {
        speed = (transform.position - lastPosition).magnitude /Time.deltaTime;
        lastPosition = transform.position;
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down*10, Color.green);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10))
            if (hit.collider.gameObject.tag != "Road" && isOffroad == false)
            {
                isOffroad = true;
            }
            else if (hit.collider.gameObject.tag == "Road" && isOffroad == true)
            {
                isOffroad = false;
            }
    }
    // Use this for initialization
    void Start () {
        string path = Application.dataPath + "/log_" + LapCount + ".csv";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Speed,collisionCars,collisionPedestrians,collisionOther,isOffRoad\n");
        writer.Close();
        InvokeRepeating("UpdateGUI", 0f, 1.0f);
        numberOfCollidableObjects = GameObject.FindGameObjectsWithTag("Other").Length;
        numberOfCars = GameObject.FindGameObjectsWithTag("Cars").Length;
        
	}

    private void Update()
    {
        text = "Speed: " + speed
                        + "\nCollision(Cars): " + collisionCars
                        + "\nCollision(Pedestrian): " + collisionPedestrians
                        + "\nCollision(Other): " + collisionOther
                        + "\n Is Offroad: " + isOffroad
                        + "\n Lap Count " + LapCount + "/4";
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
        else if(other.gameObject.tag == "Finish")
        {
            Debug.Log("In Finish");
            LapCount++;
            if(LapCount<=4)
            {
                ResetVehicle();
            }
            else
            {
                StartCoroutine(Completed());
            }
        }
    }


    void UpdateGUI()
    {
        string path = Application.dataPath + "/log_"+LapCount+".csv";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(speed+"," + collisionCars +   "," + collisionPedestrians + "," + collisionOther + "," + isOffroad + "\n");
        writer.Close();

    }
    IEnumerator Completed()
    {
        DisplayText.text = "Thank you for playing!\n Game shutting down in 5 seconds\n";
        DisplayText = null; //Not a good way to disable update but will work for now
        yield return new WaitForSeconds(5);
        Application.Quit();
    }

    void ResetVehicle()
    {
        this.transform.root.GetComponent<Rigidbody>().transform.position = new Vector3(-111.61f, 0.17f, -138.3f);
        this.transform.root.GetComponent<Rigidbody>().transform.eulerAngles = new Vector3(0, 90, 0);
        this.transform.root.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        switch (LapCount)
        {
            case 2: //without baricades
                barricades.SetActive(false);
                break;
            case 3: //with baricades and rain
                barricades.SetActive(true);
                water.SetActive(true);
                this.transform.root.GetComponent<Rigidbody>().angularDrag = 0;
                this.transform.root.GetComponent<Rigidbody>().drag = 0;
                break;
            case 4: //without barricades and rain
                barricades.SetActive(false);
                water.SetActive(true);
                this.transform.root.GetComponent<Rigidbody>().angularDrag = 0;
                this.transform.root.GetComponent<Rigidbody>().drag = 0;
                break;
        }
    }
}
