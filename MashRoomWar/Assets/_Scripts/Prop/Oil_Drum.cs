using UnityEngine;
using System.Collections;
using AU;

public class Oil_Drum : Prop_Prefab 
{
	public float r;
	public float explosion;
	public float r_Force;
	protected override void Start ()
	{
		base.Start ();
	}
	void OnTriggerEnter(Collider _c)
	{
		if(_c.tag=="Arrow")
		{
			Boom ();
			Destroy (this.gameObject);
		}
	}
	protected override void Update ()
	{
		base.Update ();
	}
	protected override void Effect ()
	{
		base.Effect ();
	}
	void Boom()
	{
		Collider[] cols = Physics.OverlapSphere (this.transform.position, r);
		foreach(Collider c in cols)
		{
			if(c.gameObject.GetComponent<Rigidbody>())
			{
				c.gameObject.GetComponent<Rigidbody> ().AddExplosionForce (explosion, this.transform.position, r_Force);
			}
			else if(c.tag=="Player")
			{
				c.gameObject.GetComponent<CharacterManager> ().Behurt (150);
			}
		}
	}
	void OnDestroy()
	{
		
	}
}
