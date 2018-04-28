using UnityEngine;
using System.Collections;

public class Camera_Control : MonoBehaviour {

	// Use this for initialization
	public Material mat;
	[HideInInspector]
	public float Size;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnPostRender()
	{
		GL.Clear (false,false,Color.white);
		mat.color = Color.red;
		mat.SetPass (0);
		AttackCenter.DrawCenter ();
		mat.SetPass (0);
		AttackCenter.DrawCircle (Size);
		GL.Flush ();
	}
}
