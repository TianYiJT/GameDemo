using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GameMode;
public class NetWorkManager : MonoBehaviour{
	public string IP;
	public int port;
	public Text Console;
	[HideInInspector]
	public int  IP_PLAYER=-2;
	public InputField UserAccount;
	public InputField UserPassWord;
	public GameObject prefab_character;
	public GameObject prefab_Main_character;
	NetWorkManager network_manager;
	Dictionary<int,GameObject> Other_Player;
	public GameObject arrow;
	PluckManager pm;
	PropManager prm;
	[HideInInspector]
	public int[] propcode;
	[HideInInspector]
	public Vector3[] proppos;
	bool Is_Enter_Game=false;
	[HideInInspector]
	public int temp_Index_prepare;
	[HideInInspector]
	public int Max_Index_prepare;
	[HideInInspector]
	public int index_prepare=-1;
	void Start ()
	{
		DontDestroyOnLoad (this);
		network_manager = this;
		Other_Player = new Dictionary<int, GameObject> ();
		if (Network.peerType == NetworkPeerType.Disconnected) 
		{
				
			var error = Network.Connect (IP, port);
			switch (error) 
			{
			case NetworkConnectionError.NoError:
				IP_PLAYER = Random.Range (int.MinValue,int.MaxValue);
				break;
			default:
				Debug.Log (error.ToString());
				break;
			}
		}
	}
	void Update () 
	{
		Debug.Log (IP_PLAYER);
		if(Is_Enter_Game)
		{
			if (!pm||!prm) 
			{
				pm = GameObject.FindGameObjectWithTag ("PluckManager").GetComponent<PluckManager> ();
				prm = GameObject.FindGameObjectWithTag ("PropManager").GetComponent<PropManager> ();
			}
			else
			{
				Is_Enter_Game = false;
			}	
		}
	}
	[RPC]
	public void RegisterUI()
	{
		Console.text="Register";
		if (Network.peerType == NetworkPeerType.Client)
			network_manager.GetComponent<NetworkView> ().RPC ("Register", RPCMode.Server, UserAccount.text,UserPassWord.text,IP_PLAYER);
		else
			Debug.LogError ("DisConnect");
	}
	[RPC]
	public void LoginUI()
	{
		Console.text="Login";
		if (Network.peerType == NetworkPeerType.Client)
			network_manager.GetComponent<NetworkView> ().RPC ("Login", RPCMode.Server, UserAccount.text,UserPassWord.text,IP_PLAYER);
		else
			Debug.LogError ("DisConnect");
	}
	[RPC]
	void LoginSucceed(int ip)
	{
		if (IP_PLAYER == ip) 
		{
			Console.text = "Login succeed";
			SceneManager.LoadScene ("_Scene/GameModeSelect");
		}
	}
	[RPC]
	void LoginFailed(int ip)
	{
		if(IP_PLAYER==ip)
			Console.text="Login Failed";
	}
	[RPC]
	void RegisterSucceed(int ip)
	{
		if(IP_PLAYER==ip)
			Console.text="Register succeed";
	}
	[RPC]
	void RegisterFailed(int ip)
	{
		if(IP_PLAYER==ip)
			Console.text="Register Failed";
	}
	[RPC]
	void Move(Vector3 v,int ip)
	{
		if(Other_Player.ContainsKey(ip))
			Other_Player [ip].GetComponent<CharacterController> ().Move (v*Time.deltaTime);
	}
	[RPC]
	void view_move(Vector3 v,int ip)
	{
		if(Other_Player.ContainsKey(ip))
			Other_Player [ip].transform.Rotate (v);
	}
	[RPC]
	void view_move_up_down(Vector3 v,int ip)
	{
		if(Other_Player.ContainsKey(ip))
		{
			Other_Player [ip].GetComponent<CharacterManager> ().UpBody.transform.Rotate (v);
		}
	}
	[RPC]
	void Instantiate_Object(int prefab_index)
	{
		
	}

	[RPC]
	[RPC]
	void Data_Sync(int life,Vector3 v,Quaternion r,int hurting,int ip)	
	{
		if(Other_Player.ContainsKey(ip))
		{
			Other_Player [ip].GetComponent<CharacterManager> ().TempLife = life;
			Other_Player [ip].transform.position = v;
			Other_Player [ip].transform.rotation = r;
			Other_Player [ip].GetComponent<CharacterManager> ().hurting = hurting;
		}
	}
	[RPC]
	void Init_arrow(Vector3 pos,Quaternion q,Vector3 velocity,int ip)
	{
		if(Other_Player.ContainsKey(ip))
		{
			//float temp_attack_angle = Mathf.Atan (velocity.y/(new Vector2(velocity.x,velocity.z).magnitude));
			GameObject g = Instantiate (arrow,pos,q) as GameObject;
			g.GetComponent<Arrow> ().velocity = velocity;
			g.GetComponent<Arrow> ().id_name = ip;
		}
	}
	[RPC]
	void Init_Pluck(int Size,int InitPos,float _scale,
		string _contain,float r,float g,float b,
		float a,float vx,float vy,int f,int ip)
	{
		if(Other_Player.ContainsKey(ip))
		{
			Pluck _pl = new Pluck (Size,InitPos,_scale,_contain,new Color(r,g,b,a),new Vector2(vx,vy),f);
			pm.pluckqueue.Add (_pl);
		}
	}
	[RPC]
	void Init_Game(int[] ip_all,Vector3[] pos_all,int[] prop_code,Vector3[] prop_pos,int Mode_Which)
	{
		foreach (int ip in ip_all) 
		{
			if (ip != IP_PLAYER) 
			{
				continue;
			}
			else 
			{
				switch(Mode_Which)
				{
				case (int)Mode.MODE_QUICK:
					propcode = prop_code;
					proppos = prop_pos;
					SceneManager.LoadScene ("_Scene/Quick");
					Is_Enter_Game = true;
					break;
				case (int)Mode.MODE_MUL_GAME:
					Is_Enter_Game = true;
					temp_Index_prepare = 0;
					Max_Index_prepare = ip_all.Length;
					SceneManager.LoadScene ("_Scene/Mul_Game");
					break;
				case (int)Mode.MODE_SINGLE:
					SceneManager.LoadScene ("_Scene/test");
					break;
				}
				for(int i=0;i<ip_all.Length;i++)
				{
					if (ip_all [i] == IP_PLAYER) 
					{
						index_prepare = i;
						GameObject Main_Player = Instantiate (prefab_Main_character, pos_all [i], Quaternion.identity) as GameObject;
						Main_Player.GetComponent<CharacterManager> ().IsMain = true;
						DontDestroyOnLoad (Main_Player);
					} 
					else 
					{
						GameObject Player = Instantiate (prefab_character, pos_all [i], Quaternion.identity) as GameObject;
						Player.GetComponent<CharacterManager> ().IsMain = false;
						DontDestroyOnLoad (Player);
						Other_Player.Add (ip_all [i], Player);
					}
				}
				break;
			}
		}
	}
	[RPC]
	void InitAProp(int propcode,Vector3 _v,int ip)
	{
		if (Other_Player.ContainsKey (ip)) 
		{
			prm.insprop (propcode,_v,false);
		}
	}
	[RPC]
	void AddIndex_Prepare(int ip)
	{
		if (Other_Player.ContainsKey (ip))
		{
			temp_Index_prepare++;
			Debug.Log ("flags");
			//temp_Index_prepare = (temp_Index_prepare) % Max_Index_prepare;
		}
	}
	[RPC]
	void  Login(string account,string password,int info){}
	[RPC]
	void Register(string account,string password,int info){}
	[RPC]
	void JoinTheGame(int Mode_Which,int Virtual_IP){}
}
