using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Load_Mode : MonoBehaviour
{
	public string _loadpath;
	public GameObject plane;
	public GameObject[] prefabs;
	//public Material[] prefab_mats;
	float MINX;
	float MAXX;
	float MINY;
	float MAXY;
	float X_INTERVAL;
	float Y_INTERVAL;
	float juggle_height;
	[HideInInspector]
	public List<GData>[,] _datascene;
	public bool Is_START_LOAD;
	//Vector3 local_scale;
	// Use this for initialization
	void Start () 
	{
		if (Is_START_LOAD) 
		{
			Load ();
		}	
	}
	void Init()
	{
		GameObject init_g = Instantiate (prefabs[0],transform.position,Quaternion.identity) as GameObject;
		float _tempsize = init_g.GetComponent<BoxCollider> ().bounds.max.y - init_g.GetComponent<BoxCollider> ().bounds.min.y;
		float _scale = X_INTERVAL / _tempsize;
		init_g.transform.localScale *= _scale;
		juggle_height=init_g.GetComponent<BoxCollider> ().bounds.max.x - init_g.GetComponent<BoxCollider> ().bounds.min.x;
		Destroy (init_g);
	}
	public void Load()
	{
		int _countx=0, _county=0;
		_datascene = Scene_Mode_IO.Load_A_Mode (_loadpath,ref _countx,ref _county);
		MINX = plane.GetComponent<MeshRenderer> ().bounds.min.x;
		MAXX = plane.GetComponent<MeshRenderer> ().bounds.max.x;
		MINY = plane.GetComponent<MeshRenderer> ().bounds.min.z;
		MAXY = plane.GetComponent<MeshRenderer> ().bounds.max.z;
		X_INTERVAL = (MAXX - MINX) / (float)(_countx);
		Y_INTERVAL = (MAXY - MINY) / (float)(_county);
		Init ();
		for(int i=0;i<_countx;i++)
		{
			for(int j=0;j<_county;j++)
			{
				int count=0;
				foreach(GData _gd in _datascene[i,j])
				{
					if (!_gd.visible) 
					{
						count++;
						continue;
					}
					GameObject _prefab = prefabs [_gd._Index];
					Vector3 temp = Vector3.zero;
					temp.x=MINX+(i+0.5f)*X_INTERVAL;
					temp.z = MINY + (j + 0.5f) * Y_INTERVAL;
					temp.y = plane.transform.position.y+(count+1)*juggle_height;
					count++;
					GameObject _g=Instantiate (_prefab,temp,Quaternion.identity) as GameObject;

					if (_gd._Index == 0) 
					{
						_g.GetComponent<CubeMesh> ().Give_A_Mat (_gd._mat);
					}
					float _tempsize = _g.GetComponent<BoxCollider> ().bounds.max.y - _g.GetComponent<BoxCollider> ().bounds.min.y;
					float _scale = X_INTERVAL / _tempsize;
					_g.transform.localScale*=_scale;
				}
			}
		}
	}
	// Update is called once per frame
	void Update () 
	{
	
	}
}
