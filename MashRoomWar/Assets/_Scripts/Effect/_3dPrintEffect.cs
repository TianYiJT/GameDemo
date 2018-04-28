using UnityEngine;
using System.Collections;

public class _3dPrintEffect : MonoBehaviour {

	// Use this for initialization
	float MIN_HEIGHT;
	float MAX_HEIGHT;
	float temp;
	float timer;
	Material[] mat;
	void Start ()
	{
		mat = new Material[1];
		if (GetComponent<MeshRenderer> ()) 
		{
			MIN_HEIGHT = GetComponent<MeshRenderer> ().bounds.min.y;
			MAX_HEIGHT = GetComponent<MeshRenderer> ().bounds.max.y;
		}
		temp = MIN_HEIGHT;
		if (GetComponent<MeshRenderer> ())
			mat = GetComponent<MeshRenderer> ().materials;
		foreach(Material m in mat)
		{
			m.SetFloat ("_ConstructY", temp);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;  
		if (timer > 0.1f)  
		{  
			temp += 0.2f;
			foreach(Material m in mat)
			{
				m.SetFloat ("_ConstructY", temp);
			}
			timer = 0;
		}
		if(temp>=MAX_HEIGHT)
		{
			Destroy (this);
		}
	}
}
