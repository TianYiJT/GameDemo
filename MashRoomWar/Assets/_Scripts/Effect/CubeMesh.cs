using UnityEngine;
using System.Collections;

public class CubeMesh : MonoBehaviour 
{
	public GameObject[] prefabs;
	int temp_face=0;
	public Material _select;
	Material[] _normal;
	public Material[] prefab_mats;
	// Use this for initialization
	void Start () 
	{
		_normal = new Material[prefabs.Length];
		for(int i=0;i<prefabs.Length;i++)
		{
			_normal[i] = prefabs [i].GetComponent<MeshRenderer> ().material;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	public void Tranform_Face(int next_face)
	{
		prefabs [temp_face].GetComponent<MeshRenderer> ().material = _normal[temp_face];
		//prefabs [temp_face].GetComponent<MeshRenderer> ().material.color = Color.white;
		prefabs [next_face].GetComponent<MeshRenderer> ().material = _select;
		prefabs [next_face].GetComponent<MeshRenderer> ().material.SetColor ("Main_Color",Color.green);
		temp_face = next_face;
	}
	public void Tranform_Mat(Material mat)
	{
		prefabs [temp_face].GetComponent<MeshRenderer> ().material = mat;
		_normal [temp_face] = mat;
	}
	public void End_Face()
	{
		prefabs [temp_face].GetComponent<MeshRenderer> ().material = _normal[temp_face];
		//prefabs [temp_face].GetComponent<MeshRenderer> ().material.color = Color.white;
	}
	public void Give_A_Mat(int[] mats)
	{
		for(int i=0;i<mats.Length;i++)
		{
			if(mats[i]>=0)
				prefabs [i].GetComponent<MeshRenderer> ().material = prefab_mats[mats[i]];
		}
	}
}
