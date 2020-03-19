using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightLeg : MonoBehaviour
{
    private bool mouseLeftButtonON = false;
    float currentEulerAngles;

    void Start()
    {
        currentEulerAngles = transform.eulerAngles.x;
    }

    void Update()
    {
        if (mouseLeftButtonON)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            currentEulerAngles += pos.x*10f;
            transform.localEulerAngles = new Vector3(currentEulerAngles, 0f, 0f);
        }
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseLeftButtonON = true;
        }
    }

    void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0) && mouseLeftButtonON)
        {
            mouseLeftButtonON = false;
        }
    }
}
