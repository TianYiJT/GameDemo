﻿using UnityEngine;
using System.Collections;

public class JustRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate (new Vector3(0,1,0));
	}
}