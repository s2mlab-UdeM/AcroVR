using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    /*    public GameObject target;
        public bool isRotating = false;

        void Start()
        {
            target.SetActive(false);
        }

        void ShowTarget()
        {
            isRotating = true;
            target.SetActive(true);
        }

        void Update()
        {
            if(Input.anyKeyDown && !isRotating)
            {
                Invoke("ShowTarget", 0.6f);
            }

            if(isRotating)
            {
                target.transform.Rotate(Vector3.up * 20f * Time.deltaTime);
            }
        }*/

    private void Update()
    {
        transform.Rotate(Vector3.up * 20f * Time.deltaTime);
    }
}
