using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

//    GameObject player;
    public float dragSpeed = 2;
    private Vector3 dragOrigin;
    private Vector3 pos;

    void Start()
    {
//        player = GameObject.FindGameObjectWithTag("Player");
        //        pos = player.transform.Find("FirstViewPoint").transform.position;

//        player = ToolBox.GetInstance().GetManager<DrawManager>().girl1Hip;
    }

    void Update () {
//        if(ToolBox.GetInstance().GetManager<DrawManager>().girl1Hip != null)
//        {
//            transform.LookAt(ToolBox.GetInstance().GetManager<DrawManager>().girl1Hip.transform);
//            transform.Translate(new Vector3(0,0,player.transform.position.y));
//        }

        /*        if (Input.GetMouseButtonDown(2))
                {
                    dragOrigin = Input.mousePosition;
                    return;
                }

                if (!Input.GetMouseButton(2)) return;

                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

                transform.Translate(move, Space.World);*/
    }
}
