using UnityEngine;
using System.Collections;

public class Input_Control_test : MonoBehaviour
{

	// Use this for initialization
	public GameObject gobj;
	void Start () 
	{
		Instantiate (gobj,gobj.transform.position,gobj.transform.rotation);
	}
	[RPC]
	void Update () 
	{
		
	}
}
