using UnityEngine;
using System.Collections;

public class WindHole :Prop_Prefab 
{
	//WindZone _wz;
	public float _R;
	public float  MAX_VELOCITY;
	Wind w;
	public float _H;
	protected override void Start ()
	{
		base.Start ();
		//_wz.mode = WindZoneMode.Spherical;
		this.transform.Rotate(new Vector3(0,Random.Range(0,360),0));
		w=new Wind(this.transform.position,this.transform.forward.normalized*_H,_R,MAX_VELOCITY,this.gameObject);
	}
	protected override void Update ()
	{
		base.Update ();
	}
	protected override void Effect ()
	{
		base.Effect ();
		w.Windupdate (this.transform.position);
	}
}
