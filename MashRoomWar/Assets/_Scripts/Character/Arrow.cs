using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
public class Arrow : MonoBehaviour 
{
	[HideInInspector]
	public Vector3 velocity;
	[HideInInspector]
	public int id_name;
	Rigidbody rb;
	public int Arrow_Power;
	public float MAX_VELOCITY;
	//bool IsHurted=false;
	NetWorkManager nm;
	GameObject g;
	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
		rb.velocity = velocity;
		Invoke ("DestroyThisArrow",8.0f);
		nm = GameObject.FindGameObjectWithTag ("NetWorkManager").GetComponent<NetWorkManager>();
		GameObject[] gs=GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject go in gs) 
		{
			if (go.GetComponent<Input_Control> ()) 
			{
				g = go;
			}
		} 
	}
	void OnTriggerEnter(Collider other)
	{
		int hurt = (int)(rb.velocity.magnitude / MAX_VELOCITY * Arrow_Power);
		if(other.tag=="face")
		{
			other.GetComponentInParent<CharacterManager> ().Behurt (hurt*2);
			add_hurt (hurt*2);
		}
		if(other.tag=="body")
		{
			other.GetComponentInParent<CharacterManager> ().Behurt (hurt);
			add_hurt (hurt);
		}
		if(other.tag=="Nose")
		{
			other.GetComponentInParent<CharacterManager> ().Behurt (hurt*3);
			add_hurt (hurt*3);
		}
		DestroyThisArrow ();
	}
	// Update is called once per frame
	void Update () 
	{
		if(rb.velocity.magnitude<=0.2f)
		{
			DestroyThisArrow ();	
		}
	}
	void DestroyThisArrow()
	{
		StartCoroutine (waitDie ());
	}
	IEnumerator waitDie()
	{
		yield return new WaitForSeconds (0.2f);
		Destroy (this.gameObject);
	}
	void add_hurt(int hurt)
	{
		if(nm.IP_PLAYER==id_name)
		{
			g.GetComponent<CharacterManager> ().hurting += hurt;
		}
	}
}
