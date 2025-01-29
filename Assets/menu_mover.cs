using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu_mover : MonoBehaviour
{
    int x = 648;
    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector2.Lerp(transform.localPosition, new Vector2(960 + (active ? 0 : x), -540), Time.deltaTime * 10);
    }
}
