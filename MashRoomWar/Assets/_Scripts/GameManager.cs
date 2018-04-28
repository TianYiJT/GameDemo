using UnityEngine;
using System.Collections;
using GameMode;
using AU;
public class GameManager : MonoBehaviour
{
	[HideInInspector]
	public int _State;
	[HideInInspector]
	public bool Is_Main;
	public int mode;
	PropManager prm;
	NetWorkManager nm;
	[SerializeField]
	Vector3 MIN;
	[SerializeField]
	Vector3 MAX;
	public int all_round;
	int temp_round=0;
	bool canins=true;
	public int MIN_PROP;
	public int MAX_PROP;
	public float timer;
	// Use this for initialization
	void Start ()
	{
		Is_Main = false;
		_State = (int)GameState.PREPARE;
		prm = GameObject.FindGameObjectWithTag ("PropManager").GetComponent<PropManager>();
		nm = GameObject.FindGameObjectWithTag ("NetWorkManager").GetComponent<NetWorkManager> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (prm&&nm) 
		{
			switch (mode)
			{
			case (int)Mode.MODE_QUICK:
				Quick ();
				break;
			case (int)Mode.MODE_MUL_GAME:
				Debug.Log (Is_Main.ToString());
				if (nm.index_prepare == nm.temp_Index_prepare) 
				{
					Is_Main = true;
				}
				else 
				{
					Is_Main = false;	
				}
				Mul_Game ();
				break;
			default:
				break;
			}
		}
		else 
		{
			prm = GameObject.FindGameObjectWithTag ("PropManager").GetComponent<PropManager>();
			nm = GameObject.FindGameObjectWithTag ("NetWorkManager").GetComponent<NetWorkManager> ();
		}
	}
	void Quick()
	{
		switch((int)_State)
		{
		case (int)GameState.PREPARE:
			for(int i=0;i<nm.propcode.Length;i++)
			{
				prm.insprop(nm.propcode[i],nm.proppos[i],false);
			}
			_State = (int)GameState.ING;
			break;
		case (int)GameState.ING:
			StartCoroutine (Gaming(360.0f));
			break;
		case (int)GameState.COUNT:
			Count ();
			break;
		case (int)GameState.END:
			End ();
			break;
		}
	}
	void Mul_Game()
	{
		switch((int)_State)
		{
		case (int)GameState.PREPARE:
			if (nm.temp_Index_prepare == nm.Max_Index_prepare) 
			{
				_State = (int)GameState.ING;
				nm.temp_Index_prepare = 0;
			}
			if(Is_Main)
			{
				if (timer <= 40.0f)
				{
					timer += Time.deltaTime;
				}
				else
				{
					timer = 0;
					nm.temp_Index_prepare++;
					nm.GetComponent<NetworkView> ().RPC ("AddIndex_Prepare",RPCMode.Others,nm.IP_PLAYER);
				}
				if(canins)
				{
					prm.insprop_prafab_random (MIN_PROP,MAX_PROP);
					canins = false;
				}
			}
			break;
		case (int)GameState.ING:
			StartCoroutine (Gaming(600.0f));
			break;
		case (int)GameState.COUNT:
			Count ();
			break;
		case (int)GameState.END:
			End ();
			break;
		}
	}
	void Count()
	{
		
	}
	void End()
	{
		switch (mode) 
		{
		case (int)Mode.MODE_MUL_GAME:
			if (temp_round != all_round) 
			{
				_State = (int)GameState.PREPARE;
				temp_round++;
				canins = true;
			}
			else
			{
				
			}
			break;
		case (int)Mode.MODE_QUICK:
			break;
		}
	}
	IEnumerator Gaming(float time)
	{
		yield return new WaitForSeconds (time);
		_State = (int)GameState.COUNT;
	}
}
public enum GameState
{
	PREPARE=0,
	ING=1,
	COUNT=2,
	END=3
};
