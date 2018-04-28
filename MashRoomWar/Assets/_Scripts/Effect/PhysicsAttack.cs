using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class PhysicsAttack : MonoBehaviour 
{

	Rigidbody rb;
	float PerAttackNeedPower=100.0f;
	bool canattack=true;
	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.tag=="face"||other.tag=="body"||other.tag=="Eyes"||other.tag=="Nose"&&canattack)
		{
			float mass = rb.mass;
			float velocity =rb.velocity.magnitude;
			float power = mass * Mathf.Pow (velocity, 2);
			int attack = (int)(power / PerAttackNeedPower);
			attack = Mathf.Clamp (attack,0,other.GetComponentInParent<CharacterManager>().Life);
			other.GetComponentInParent<CharacterManager> ().Behurt (attack);
			StartCoroutine (CannotAttack());
		}
	}
	// Update is called once per frame
	void Update () 
	{
	
	}
	IEnumerator CannotAttack()
	{
		canattack = false;
		yield return new WaitForSeconds (3.0f);
		canattack = true;
	}
}
