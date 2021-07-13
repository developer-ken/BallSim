using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform TransformToFollow;
    public float CameraHeight;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(TransformToFollow.position.x, TransformToFollow.position.y + CameraHeight, TransformToFollow.position.z);
    }
}
