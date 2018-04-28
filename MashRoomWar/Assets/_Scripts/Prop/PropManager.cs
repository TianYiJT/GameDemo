using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public struct Sync_Data
{
	public GameObject _g;
	public int _code;
	public bool Is_Main;
	public Sync_Data(GameObject g,int code,bool is_main)
	{
		_g = g;
		_code = code;
		Is_Main = is_main;
	}
};
public class PropManager : MonoBehaviour
{
	// Use this for initialization
	public GameObject[] GameObjects;
	public GameObject[] GameObjects_Active;
	[HideInInspector]
	public GameObject nm;
	int code;
	public GameObject pos;
	public float r;
	void Start () 
	{
		code = 0;
		nm = GameObject.FindGameObjectWithTag ("NetWorkManager");
	}	
	// Update is called once per frame
	void Update ()
	{

	}
	public void insprop_prafab_random(int min,int max)
    {
		int random_count = Random.Range (min, max);
		for(int i=0;i<random_count;i++)
		{
			int random_index = Random.Range (0,GameObjects.Length-1);
			float rX = Random.Range (-r, r);
			float rY = Random.Range (-r, r);
			float rZ = Random.Range (-r, r);
			Vector3 vec3_random = new Vector3 (rX,rY,rZ);
			Vector3 vec3_real = new Vector3 (vec3_random.x+pos.transform.position.x,vec3_random.y+pos.transform.transform.position.y,vec3_random.z+pos.transform.position.z);
			Instantiate (GameObjects[random_index],vec3_real,Quaternion.identity);
		}
    }
	public void insprop(int _code,Vector3 _pos,bool Is_Main)
	{
		Instantiate(GameObjects_Active[_code],_pos,Quaternion.identity);
	}
}
