using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelsCarTeleport : MonoBehaviour
{
    public GameObject tunelIn;
    public GameObject tunelOut;

    public float carRotation;

    public float xAxis;
    public float zAxis;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == tunelIn)
        {
            transform.position = tunelOut.transform.position;
            transform.Translate(xAxis, 0f, zAxis);
            transform.Rotate(0f, carRotation, 0f);

            PlayerPrefs.SetInt("TP", 1);
        }
    }

}
