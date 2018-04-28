using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;
public class BananaSkin:Prop_Prefab 
{
	struct BuffForGameObject
	{
		public GameObject _g;
		public float _time;
		public BuffForGameObject(GameObject g,float time)
		{
			_g=g;
			_time=time;
		}
	};
	public float EffectTime;
	public float EffectRadius;
	public string Tag_Player;
	List<BuffForGameObject> GameObjectLists;
	protected override void Start ()
	{
		base.Start ();
		GameObjectLists = new List<BuffForGameObject> ();
	}
	protected override void Effect ()
	{
		Collider[]	_cs = (Physics.OverlapSphere (this.transform.position, EffectRadius));
		foreach(Collider _c in _cs)
		{
			if(_c.tag==Tag_Player)
			{
				bool bool_add = true;
				foreach(BuffForGameObject _b in GameObjectLists)
				{
					if(_b._g==_c.gameObject)
					{
						bool_add = false;
						break;
					}
				}
				if(bool_add)
				{
					BuffForGameObject _bgo = new BuffForGameObject (_c.gameObject, 3.0f);
					GameObjectLists.Add (_bgo);
				}
				break;
			}
		}
		base.Effect ();
		List<int> _indexs=new List<int>();
		for(int i=0;i<GameObjectLists.Count;i++)
		{
			BuffForGameObject _b= GameObjectLists [i];
			_b._time -= Time.deltaTime;
			GameObjectLists [i] = _b;
			if(_b._time<=0)
			{
				_indexs.Add(i);
			}
		}
		foreach(int _i in _indexs)
		{
			GameObjectLists.RemoveAt (_i);
		}
		foreach(BuffForGameObject _b in GameObjectLists)
		{
			_b._g.GetComponent<CharacterController> ().Move (_b._g.transform.forward*Time.deltaTime);
			_b._g.transform.Rotate (new Vector3(0,360,0)*Time.deltaTime);
			if(_b._g.GetComponent<CharacterManager>().IsMain)
			{
				_b._g.GetComponent<Input_Control> ().Main_Camera.gameObject.AddComponent<MotionBlur> ();
			}
		}
	}	
	protected override void Update ()
	{
		base.Update ();
	}

}
