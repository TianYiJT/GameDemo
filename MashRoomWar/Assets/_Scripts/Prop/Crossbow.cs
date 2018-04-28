using UnityEngine;
using System.Collections;
using AU;
public class Crossbow : Prop_Prefab {

	// Use this for initialization
	public GameObject arrow;
	public float MAX_DISTANCE;
	public int MIN_COUNT_ONETIME;
	public int MAX_COUNT_ONETIME;
	bool Attack_SetUp;
	float SumAttackTime;
	public float velocity;
	GameObject nm;
	protected override void Start ()
	{
		Attack_SetUp = false;
		SumAttackTime = 0;
		nm = GameObject.FindGameObjectWithTag ("NetWorkManager");
		base.Start ();
	}
	protected override void Effect ()
	{
		base.Effect ();
		if (nm) 
		{
			if (Attack_SetUp) 
			{
				if (SumAttackTime >= 1.0f) 
				{
					int random = Random.Range (MIN_COUNT_ONETIME, MAX_COUNT_ONETIME);
					for (int i = 0; i < random; i++) 
					{
						GameObject _g = Instantiate (arrow, arrow.transform.position, arrow.transform.rotation) as GameObject;
						_g.SetActive (true);
						_g.transform.position += Alluse.RandomVectorToNormal (_g.transform.forward, 0, 2.0f, _g.transform.position);
						_g.GetComponent<Arrow> ().velocity = velocity * arrow.transform.forward.normalized;
						nm.GetComponent<NetworkView> ().RPC ("Init_arrow", RPCMode.Others, _g.transform.position, _g.transform.rotation, _g.GetComponent<Arrow> ().velocity, nm.GetComponent<NetWorkManager> ().IP_PLAYER);
					}
					SumAttackTime = 0;
				} 
				else
				{
					SumAttackTime += Time.deltaTime;
				}
			}
		}
		else
		{
			nm = GameObject.FindGameObjectWithTag ("NetWorkManager");
		}
	}
	protected override void Update ()
	{
		base.Update ();
		int _c = 0;
		Alluse.FanShape_Area_Colliders (this.transform.position, this.transform.forward, MAX_DISTANCE, -45, 45, ref _c,"Player");
		if (_c != 0) 
		{
			Attack_SetUp = true;
		}
		else 
		{
			Attack_SetUp = false;	
		}
	}
}
