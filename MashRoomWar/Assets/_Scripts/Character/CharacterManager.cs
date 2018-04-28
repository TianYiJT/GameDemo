using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CharacterManager : MonoBehaviour 
{
	public int Life;
	[HideInInspector]
	public int TempLife;
	CharacterController cc;
	public GameObject UpBody;
	[HideInInspector]
	public bool IsMain;
	GameObject Text_TempLife;
	GameObject Text_AllLife;
	GameObject nm;
	[HideInInspector]
	public int goal_index;
	[HideInInspector]
	public int hurting;
	[HideInInspector]
	public int normal_goal;
	void Start () 
	{
		cc = GetComponent<CharacterController> ();
		nm = GameObject.FindGameObjectWithTag ("NetWorkManager");
		Text_AllLife = GameObject.FindWithTag ("AllLife");
		Text_TempLife = GameObject.FindWithTag ("TempLife");
		TempLife = Life;
		if(IsMain)
		{
			if(Text_AllLife)
				Text_TempLife.GetComponent<Text>().text = TempLife.ToString();
			if(Text_TempLife)
				Text_AllLife.GetComponent<Text>().text = Life.ToString();
		}
	}
	void Update () 
	{
		if(IsMain)
		{
			nm.GetComponent<NetworkView> ().RPC ("Data_Sync",RPCMode.Others,TempLife,this.transform.position,this.transform.rotation,nm.GetComponent<NetWorkManager>().IP_PLAYER);
		}
		if (!Text_AllLife) 
		{
			Text_AllLife = GameObject.FindWithTag ("AllLife");
			if(Text_AllLife)
				Text_AllLife.GetComponent<Text>().text = Life.ToString();
		}
		if (!Text_TempLife) 
		{
			Text_TempLife = GameObject.FindWithTag ("TempLife");
			if(Text_TempLife)
				Text_TempLife.GetComponent<Text>().text = TempLife.ToString();
		}
		if(TempLife<=0)
		{
			Die ();
		}
		if(cc.collisionFlags==CollisionFlags.None)
		{
			cc.Move (new Vector3(0,-1,0)*Time.deltaTime);
		}
	}
	public void Behurt(int power)
	{
		hurting -= power;
		if(IsMain)
		{
			TempLife -= power;
			Text_TempLife.GetComponent<Text>().text = TempLife.ToString();
			Text_AllLife.GetComponent<Text>().text = Life.ToString();
		}
	}
	void Die()
	{
		if(IsMain)
		{
			nm.GetComponent<NetworkView> ().RPC ("Data_Sync",RPCMode.Others,TempLife,this.transform.position,this.transform.rotation,hurting,nm.GetComponent<NetWorkManager>().IP_PLAYER);
		}
		GameObject.Destroy (this.gameObject);	
	}
	//public void  
}
