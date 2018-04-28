using UnityEngine;
using System.Collections;

public class TranferDoor : Prop_Prefab 
{
	int MyIndex=0;
	static int count=0;
	static GameObject[] Tranferdoors=new GameObject[100];
	public float MAX_DISTANCE;
	protected override void Start ()
	{
		base.Start ();
		MyIndex = TranferDoor.count;
		TranferDoor.Tranferdoors [MyIndex] = this.gameObject;
		TranferDoor.count++;
	}
	protected override void Update ()
	{
		base.Update ();
	}
	protected override void Effect ()
	{
		base.Effect ();
		Collider[] cols = Physics.OverlapSphere (this.transform.position, MAX_DISTANCE);
		foreach(Collider col in cols)
		{
			if (col.tag == "Player") 
			{
				if (col.GetComponent<CharacterManager> ().IsMain&&Input.GetKeyDown(KeyCode.F)) 
				{
					Vector3 _pos = TranferDoor.Tranferdoors [(MyIndex + 1) % count].transform.position;
					col.transform.position = _pos;
					break;
				}
			}
		}
	}
}