using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engear : MobileObject
{

	// Use this for initialization
	public void Start () {
		
	}

	// Update is called once per frame
	public void Update ()
    {
        ApplyMovement();
        ApplyLayerInteraction();

        if (Input.GetAxis("Horizontal") < 0) XSpeed = -10;
        else if (Input.GetAxis("Horizontal") > 0) XSpeed = 10;
        if (Input.GetAxis("Vertical") < 0) YSpeed = -10;
        else if (Input.GetAxis("Vertical") > 0) YSpeed = 10;

    }
}
