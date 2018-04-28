using UnityEngine;
using System.Collections;

public class Magma : Prop_Prefab
{
	public float Interval;
	float Temp;
	public float effectTime;
	bool NotEffect;
	public float MAX_DISTANCE;
	protected override void Start ()
	{
		NotEffect = true;
		base.Start ();
	}
	protected override void Update ()
	{
		base.Update ();
	}
	protected override void Effect ()
	{
		base.Effect ();
		if (Temp <= Interval&&NotEffect) 
		{
			Temp += Time.deltaTime;
		}
		else 
		{
			NotEffect = false;
			Temp = 0;
			StartCoroutine (IE_Effect ());
		}
		if (!NotEffect)
		{
			Collider[] cols = Physics.OverlapSphere (this.transform.position, MAX_DISTANCE);
			foreach(Collider col in cols)
			{
				if(col.tag=="Player")
				{
					col.GetComponent<CharacterManager> ().Behurt (200);
					break;
				}
			}
		}
	}
	IEnumerator IE_Effect()
	{
		yield return new WaitForSeconds (effectTime);
		NotEffect = true;
	}
}
