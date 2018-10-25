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
	}
}
