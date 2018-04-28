using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using GameMode;
public class ServerManager : MonoBehaviour 
{
	[SerializeField]
	int Port;
	string Path;
	Dictionary<string,string> dic=new Dictionary<string, string>();
	XmlDocument xml_document;
	Queue<int> Wait_Queue_ModeQuick;
	Queue<int> Wait_Queue_Mode_Mul_Game;
	public int MAX_COUNT_GAMEQuick;
	public int MAX_COUNT_GAME_MUL_GAME;
	public int MIN_COUNT_GAME_MUL_GAME;
	public GameObject postion_transform;
	public Vector3 MIN;
	public Vector3 MAX;
	public int MIN_PROP;
	public int MAX_PROP;
	public int MAX_PROP_INDEX;
	void Awake()
	{
		Path=Application.dataPath+"/DATA_BASE.xml";
		Wait_Queue_ModeQuick = new Queue<int> ();
		Wait_Queue_Mode_Mul_Game = new Queue<int> ();
	}
	Vector3 Init_postion()
	{
		float randomX = Random.Range (-8.0f,8.0f);
		float randomZ = Random.Range (-8.0f,8.0f);
		return new Vector3 (postion_transform.transform.position.x + randomX, postion_transform.transform.position.y, postion_transform.transform.position.z + randomZ);
	}
	void Start () 
	{
		xml_document = new XmlDocument ();
		if (File.Exists (Path)) 
		{
			xml_document.Load (Path);
			Load_XML (Path);
		} 
		else
		{
			xml_document=Create_XML (Path);	
		}
		if(Network.peerType==NetworkPeerType.Disconnected)
		{
			var error=Network.InitializeServer (12,Port,false);
			switch (error) 
			{
			case NetworkConnectionError.NoError:
				Debug.Log ("Server Working");
				break;
			default:
				break;
			}
		}
	}
	XmlDocument Create_XML(string Path)
	{
		XmlDocument xmldoc = new XmlDocument ();
		XmlElement Top_ELEMENT = xmldoc.CreateElement ("GAMER");
		xmldoc.AppendChild (Top_ELEMENT);
		xmldoc.Save (Path);
		return xmldoc;
	}
	void Load_XML(string Path)
	{
		XmlDocument xmldoc = new XmlDocument ();
		xmldoc.Load (Path);
		XmlNodeList	list_node=xmldoc.SelectSingleNode ("GAMER").ChildNodes;
		foreach(XmlElement xe in list_node)
		{
			string user = xe.InnerText;
			string password = xe.GetAttribute ("password");
			dic.Add (user,password);
		}
	}
	void Update () 
	{
		if (Wait_Queue_ModeQuick.Count >= MAX_COUNT_GAMEQuick) 
		{
			int[] LOCAL_ARRAY_BUFFER_IP = new int[MAX_COUNT_GAMEQuick];
			Vector3[] LOCAL_ARRAY_BUFFER_POSTION = new Vector3[MAX_COUNT_GAMEQuick];
			for(int i=0;i<MAX_COUNT_GAMEQuick;i++)
			{
				LOCAL_ARRAY_BUFFER_IP [i] = Wait_Queue_ModeQuick.Dequeue ();
				LOCAL_ARRAY_BUFFER_POSTION [i] = Init_postion ();
			}
			int Random_Count = Random.Range (MIN_PROP, MAX_PROP);
			int[] LOCAL_ARRAY_BUFFER_PROP_INDEX=new int[Random_Count];
			Vector3[] LOCAL_ARRAY_BUFFER_PROP_POSITION = new Vector3[Random_Count];
			for(int i=0;i<Random_Count;i++)
			{
				int temp_Random = Random.Range (0,MAX_PROP_INDEX);
				temp_Random = Mathf.Clamp (temp_Random,0,MAX_PROP_INDEX-1);
				LOCAL_ARRAY_BUFFER_PROP_INDEX [i] = temp_Random;
				Vector3 v = new Vector3 (Random.Range(MIN.x,MAX.x),Random.Range(MIN.y,MAX.y),Random.Range(MIN.z,MAX.z));
				LOCAL_ARRAY_BUFFER_PROP_POSITION [i] = v;
			}
			GetComponent<NetworkView> ().RPC ("Init_Game",RPCMode.Others,LOCAL_ARRAY_BUFFER_IP,LOCAL_ARRAY_BUFFER_POSTION,LOCAL_ARRAY_BUFFER_PROP_INDEX,LOCAL_ARRAY_BUFFER_PROP_POSITION,(int)Mode.MODE_QUICK);
		}
		if(Wait_Queue_Mode_Mul_Game.Count>=MIN_COUNT_GAME_MUL_GAME)
		{
			StartCoroutine ("WaitForOtherGamer");
		}
	}
	[RPC]
	void  Login(string account,string password,int Virtual_IP)
	{
		if (dic.ContainsKey (account) && dic [account] == password)
		{
			GetComponent<NetworkView> ().RPC ("LoginSucceed", RPCMode.Others, Virtual_IP);
		}
		else
		{
			GetComponent<NetworkView> ().RPC ("LoginFailed", RPCMode.Others, Virtual_IP);
		}
	}
	[RPC]
	void JoinTheGame(int Mode_Which,int Virtual_IP)
	{
		switch(Mode_Which)
		{
		case (int)Mode.MODE_QUICK:
			if(!Wait_Queue_Mode_Mul_Game.Contains(Virtual_IP))
				Wait_Queue_ModeQuick.Enqueue (Virtual_IP);
			break;
		case (int)Mode.MODE_MUL_GAME:
			if (!Wait_Queue_ModeQuick.Contains (Virtual_IP))
				Wait_Queue_Mode_Mul_Game.Enqueue (Virtual_IP);
			break;
		case (int)Mode.MODE_SINGLE:
			int[] LOCAL_ARRAY_BUFFER_IP = new int[1];
			Vector3[] LOCAL_ARRAY_BUFFER_POSTION = new Vector3[1];
			LOCAL_ARRAY_BUFFER_IP[0] = Virtual_IP;
			LOCAL_ARRAY_BUFFER_POSTION[0] = Init_postion ();
			GetComponent<NetworkView> ().RPC ("Init_Game",RPCMode.Others,LOCAL_ARRAY_BUFFER_IP,LOCAL_ARRAY_BUFFER_POSTION,(int)Mode.MODE_SINGLE);
			break;
		}
	}
	[RPC]
	void  Register(string account,string password,int Virtual_IP,NetworkMessageInfo info)
	{
		if (!dic.ContainsKey (account)) 
		{
			XmlElement xe = xml_document.CreateElement ("USERMESSAGE");
			xe.InnerText = account;
			xe.SetAttribute ("password",password);
			xml_document.FirstChild.AppendChild (xe);
			xml_document.Save (Path);
			dic.Add (account, password);
			GetComponent<NetworkView> ().RPC ("RegisterSucceed", RPCMode.Others, Virtual_IP);
		} 
		else
		{
			GetComponent<NetworkView> ().RPC ("RegisterFailed", RPCMode.Others, Virtual_IP);
		}
	}
	[RPC]
	void InputLife(int Life,int Virtual_IP)
	{
		
	}
	[RPC]
	void LoginSucceed(int ip){}
	[RPC]
	void LoginFailed(int ip){}
	[RPC]
	void RegisterSucceed(int ip){}
	[RPC]
	void RegisterFailed(int ip){}
	[RPC]
	void Init_Game(int[] ip_all,Vector3[] pos_all,int[] prop_code,Vector3[] prop_pos,int Mode_Which){}
	[RPC]
	void Move(Vector3 v,int ip){}
	[RPC]
	void view_move(Vector3 v,int ip){}
	[RPC]
	void view_move_up_down(Vector3 v,int ip){}
	[RPC]
	void Init_arrow(Vector3 pos,Vector3 velocity,int ip){}
	[RPC]
	void Init_Pluck(int Size,int InitPos,float _scale,
		string _contain,float r,float g,float b,
		float a,float vx,float vy,int f,int ip){}
	[RPC]
	void Data_Sync(int life,Vector3 v,Vector3 r,int ip){}
	IEnumerator WaitForOtherGamer()
	{
		yield return new WaitForSeconds (15.0f);
		int GamerCount = Mathf.Clamp (Wait_Queue_Mode_Mul_Game.Count,MIN_COUNT_GAME_MUL_GAME,MAX_COUNT_GAME_MUL_GAME);
		int[] LOCAL_ARRAY_BUFFER_IP = new int[GamerCount];
		Vector3[] LOCAL_ARRAY_BUFFER_POSTION = new Vector3[GamerCount];
		for(int i=0;i<GamerCount;i++)
		{
			LOCAL_ARRAY_BUFFER_IP[i] = Wait_Queue_Mode_Mul_Game.Dequeue ();
			LOCAL_ARRAY_BUFFER_POSTION[i] = Init_postion ();
		}
		int[] ARRAY_DEFAULT = new int[1];
		Vector3[] ARRAY_DEFAULT_V = new Vector3[1];
		GetComponent<NetworkView> ().RPC ("Init_Game",RPCMode.Others,LOCAL_ARRAY_BUFFER_IP,LOCAL_ARRAY_BUFFER_POSTION,ARRAY_DEFAULT,ARRAY_DEFAULT_V,(int)Mode.MODE_MUL_GAME);
	}
}
