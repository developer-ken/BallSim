using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetWhenOutbounded : MonoBehaviour
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

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(Ball))
        {
            Ball.transform.position = new Vector3(0, 0.125f, 0);
            NetworkAPIHandler.SendMsg("OB", "BallFallOutFromArea");
            Debug.LogWarning("OB - Ball fall out of pannel! Reset location.");
        }
    }
}
