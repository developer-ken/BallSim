using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideTrigger : MonoBehaviour
{
    public GameObject Ball;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(Ball))
        {
            NetworkAPIHandler.SendMsg("EN", gameObject.name);
            Debug.Log(Time.time + "Enter " + gameObject.name);
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    float Distance2D(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt(((a.x - b.x) * (a.x - b.x)) + ((a.z - b.z) * (a.z - b.z)));
    }

    bool inn=false;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.Equals(Ball))
        {
            var dist = Distance2D(other.transform.position, gameObject.transform.position);
            //Debug.Log(Time.time + "DIST " + dist);
            if (dist < 0.025)
            {
                if (!inn)
                {
                    NetworkAPIHandler.SendMsg("IN", gameObject.name);
                    Debug.Log(Time.time + "In " + gameObject.name);
                    gameObject.GetComponent<Renderer>().material.color = Color.green;
                    inn = true;
                }
            }
            else
            {
                if (inn)
                {
                    gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                    NetworkAPIHandler.SendMsg("LV", gameObject.name);
                    Debug.Log(Time.time + "Leave " + gameObject.name);
                    inn = false;
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(Ball))
        {
            NetworkAPIHandler.SendMsg("EX", gameObject.name);
            Debug.Log(Time.time + "Exit " + gameObject.name);
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
