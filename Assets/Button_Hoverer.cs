using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Hoverer : MonoBehaviour
{
    RectTransform rt;
    Vector2 start;
    bool hovering;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        start = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        rt.localPosition = Vector3.Lerp(rt.localPosition, hovering ? start - Vector2.right * 64 : start, Time.deltaTime * 10);
    }

    public void Hovered(bool toggle)
    {
        hovering = toggle;
    }
}

public class Button_Hoverer2 : MonoBehaviour
{
    RectTransform rt;
    public Vector2 start;
    bool hovering;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        print(start);
    }

    // Update is called once per frame
    void Update()
    {
        rt.localPosition = Vector3.Lerp(rt.localPosition, hovering ? start - Vector2.right * 64 : start, Time.deltaTime * 10);
    }

    public void Hovered(bool toggle)
    {
        hovering = toggle;
    }
}
