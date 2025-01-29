using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.eulerAngles += new Vector3(10, 5, 3) * .1f;
        transform.Rotate(new Vector3(5, 10, 12) * .01f);
    }
}
