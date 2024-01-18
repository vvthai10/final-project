using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyRotation : MonoBehaviour
{
    float degrees = 90;
    Vector3 to = new Vector3(0, 30, 0);
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(new Vector3(0, speed, 0) * Time.deltaTime);
    }
}
