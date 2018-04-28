using UnityEngine;
using System.Collections;
using AU;

public class Wind 
{
	float _r;
	Vector3 _pos;
	Vector3 _dir;
	Vector3 _force;
	GameObject _g;
	public Wind(Vector3 pos,Vector3 dir,float r,float force,GameObject g)
	{
		_pos = pos;
		_r = r;
		_force = dir.normalized * force;
		Vector3 _up = new Vector3 (0, Random.Range (0.0f, 1.5f),0);
		_force += _up;
		_dir = dir;
		_g = g;
	}
	public void Windupdate(Vector3 v)
	{
		_pos = v;
		Collider[] cols = Physics.OverlapCapsule (_pos, _pos + _dir, _r);
		for(int i=0;i<cols.Length;i++)
		{
			if (cols [i].gameObject != _g&&!(cols[i].gameObject.tag=="Plane")) 
			{
				Transform temp_transform = cols [i].transform;
				while(temp_transform.parent)
				{
					temp_transform = temp_transform.parent;
				}
				temp_transform.Translate (_force*Time.deltaTime);
				temp_transform.Translate (Alluse.RandomVector () * Time.deltaTime);
			}
		}
	}
}
