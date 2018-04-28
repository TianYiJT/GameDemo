using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AU
{
	public static class Alluse
	{
		public static Collider[] FanShape_Area_Colliders(Vector3 _v,Vector3 _f,float _dis,float angle_min,float angle_max,ref int Count,string tag)
		{
			Collider[] cols = Physics.OverlapSphere (_v, _dis);
			if(cols.Length==0)
			{
				return cols;
			}
			Collider[] real_col = new Collider[30];
			for(int i=0;i<cols.Length;i++)
			{
				if(cols[i].tag!=tag)
				{
					continue;
				}
				Vector3 coltopos = cols [i].transform.position - _v;
				Vector3 cross = Vector3.Cross (coltopos, _f);
				float _sin = cross.magnitude / (coltopos.magnitude * _f.magnitude);
				if(_sin>=Mathf.Sin(angle_min)&&_sin<=Mathf.Sin(angle_max)&&Vector3.Dot(_f,coltopos)>0)
				{
					real_col [Count] = cols [i];
					Count++;
				}
			}
			return real_col;
		}
		public static Vector3 RandomVectorToNormal(Vector3 _v,float MIN,float MAX,Vector3 _pos)
		{
			float rX=0,rY=0,rZ=0;
			float randomF = Random.Range (MIN, MAX);
			if (_v.x != 0 && _v.y != 0 && _v.z != 0) 
			{
				rX = Random.Range (-1,1);
				rY = Random.Range (-1, 1);
				rZ = (-rX * _v.x - rY * _v.y) / _v.z;
			}
			if(_v.x == 0 && _v.y != 0 && _v.z != 0)
			{
				rX = Random.Range (-1,1);
				rY = Random.Range (-1, 1);
				rZ = (-rX * _v.x - rY * _v.y) / _v.z;
			}
			if(_v.x != 0 && _v.y == 0 && _v.z != 0)
			{
				rX = Random.Range (-1,1);
				rY = Random.Range (-1, 1);
				rZ = (-rX * _v.x - rY * _v.y) / _v.z;
			}
			if(_v.x != 0 && _v.y != 0 && _v.z == 0)
			{
				rX = Random.Range (-1,1);
				rZ = Random.Range (-1, 1);
				rY = (-rX * _v.x - rZ * _v.z) / _v.y;
			}
			if(_v.x == 0 && _v.y == 0 && _v.z != 0)
			{
				rX = Random.Range (-1,1);
				rY = Random.Range (-1, 1);
				rZ = (-rX * _v.x - rY * _v.y) / _v.z;
			}
			if(_v.x == 0 && _v.y != 0 && _v.z == 0)
			{
				rX = Random.Range (-1,1);
				rZ = Random.Range (-1, 1);
				rY = (-rX * _v.x - rZ * _v.z) / _v.y;
			}
			if(_v.x != 0 && _v.y == 0 && _v.z == 0)
			{
				rY = Random.Range (-1,1);
				rZ = Random.Range (-1, 1);
				rX = (-rY * _v.y - rZ * _v.z) / _v.x;
			}
			Vector3 vec3 = new Vector3 (rX, rY, rZ);
			return vec3.normalized * randomF;
		}
		public static Collider[] Cylinder_Colliders(Vector3 pos1,Vector3 pos2,float _r,ref int COUNT)
		{
			Vector3 _dir = pos1 - pos2;
			float _max = _dir.magnitude;
			_dir.Normalize ();
			Collider[] cols = Physics.OverlapCapsule (pos1, pos2, _r);
			//Debug.Log (cols.Length+"overlapcapsule");
			Collider[] real_col = new Collider[100];
			foreach(Collider _c in cols)
			{
				Vector3 _v =	_c.transform.position - pos1;
				Vector3 v_proj = Vector3.Dot (_v, _dir) * _dir;
				float _my = v_proj.x / _dir.x;
				if ((_my / _max) > 0 && (_my / _max) < 1) 
				{
					real_col [COUNT] = _c;
					COUNT++;
				}
			}
			return real_col;
		}
		public static Vector3 RandomVector()
		{
			float rX = Random.Range (-1.0f, 1.0f);
			float rY = Random.Range (-1.0f, 1.0f);
			float rZ = Random.Range (-1.0f,1.0f);
			Vector3 v = new Vector3 (rX, rY, rZ);
			return v.normalized;
		}
	}
};
	


