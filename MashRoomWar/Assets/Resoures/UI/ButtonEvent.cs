using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using GameMode;
public class ButtonEvent: MonoBehaviour  
{
	public Text Console;
	[HideInInspector]
	public GameObject network_manager;
	void Start()
	{
		network_manager = GameObject.FindGameObjectWithTag ("NetWorkManager");
	}
	[RPC]
	public void JoinQuickGame()
	{
		network_manager.GetComponent<NetworkView> ().RPC ("JoinTheGame",RPCMode.Server,(int)Mode.MODE_QUICK,network_manager.GetComponent<NetWorkManager>().IP_PLAYER);
	}
	[RPC]
	public void JoinMulGame()
	{
		network_manager.GetComponent<NetworkView> ().RPC ("JoinTheGame",RPCMode.Server,(int)Mode.MODE_MUL_GAME,network_manager.GetComponent<NetWorkManager>().IP_PLAYER);
	}
	[RPC]
	public void JoinSingle()
	{
		network_manager.GetComponent<NetworkView> ().RPC ("JoinTheGame",RPCMode.Server,(int)Mode.MODE_SINGLE,network_manager.GetComponent<NetWorkManager>().IP_PLAYER);
	}
	public void Finish()
	{
		GameObject g = GameObject.FindGameObjectWithTag ("MainCamera");
		if (g.GetComponent<Juggle_Scene_Mode> ()) 
		{
			g.GetComponent<Juggle_Scene_Mode> ().UI_USE = false;
			g.GetComponent<Juggle_Scene_Mode> ().ViewSphere.SetActive (false);
			g.GetComponent<Juggle_Scene_Mode> ().SW.SetActive (false);
		}
	}
}
