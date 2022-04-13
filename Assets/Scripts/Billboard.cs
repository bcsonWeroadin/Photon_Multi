using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    //[SerializeField] private Transform mainCam;
    public Transform mainCam;

    private void Start()
    {
        mainCam = Camera.main.transform;

        //mainCam = GameObject.Find("Main Camera")?.transform;
        //mainCam = GameObject.FindGameObjectWithTag("MainCamera")?.transform;

    }

    private void LateUpdate()
    {
        if(mainCam)
        {
            transform.LookAt(transform.position + mainCam.rotation * Vector3.forward,
                                 mainCam.rotation * Vector3.up);
        }

    }
}
