using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using pluck;

public class chat_control : MonoBehaviour 
{
	PluckManager pm;
	NetWorkManager nm;
	public int size;
	public int Initpos;
	public float pos_scale;
	public string contain;
	GameObject _IF;
	public Color col;
	public Vector2 velocity;
	public int _F;
	[HideInInspector]
	public bool Is_chat=false;
	void Start () 
	{
		pm = GameObject.FindGameObjectWithTag ("PluckManager").GetComponent<PluckManager>();
		nm = GameObject.FindGameObjectWithTag ("NetWorkManager").GetComponent<NetWorkManager> ();
		_IF = GameObject.FindGameObjectWithTag ("InputField");
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if (!pm||!_IF||!nm) 
		{
			pm = GameObject.FindGameObjectWithTag ("PluckManager").GetComponent<PluckManager> ();
			nm = GameObject.FindGameObjectWithTag ("NetWorkManager").GetComponent<NetWorkManager> ();
			_IF = GameObject.FindGameObjectWithTag ("InputField");
		}
		else
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				Is_chat=!Is_chat;
				if (Is_chat) 
				{
					_IF.GetComponent<InputFieldFadeInOut> ().FadeIn ();
				}
				else 
				{
					_IF.GetComponent<InputFieldFadeInOut> ().FadeOut ();	
				}
			}
			if(Input.GetKeyDown(KeyCode.Return))
			{
				string user_input = _IF.GetComponent<InputField> ().text;
				_IF.GetComponent<InputField> ().text = "";
				decoding (user_input);
				Pluck _pl = new Pluck (size,Initpos,pos_scale,contain,col,velocity,_F);
				pm.GetComponent<PluckManager> ().pluckqueue.Add (_pl);
				nm.GetComponent<NetworkView> ().RPC ("Init_Pluck",RPCMode.Others,size,Initpos,pos_scale,contain,col.r,col.g,col.b,col.a,velocity.x,velocity.y,_F,nm.GetComponent<NetWorkManager>().IP_PLAYER);
				contain = "";
			}
		}
	}
	void decoding(string s)
	{
		pos_scale = Random.Range (0.0f, 1.0f);
		if (s [0] != '<') 
		{
			contain = s;
		}
		else 
		{
			Queue<char> qc=new Queue<char>();
			int Right=0;
			for(int i=1;i<s.Length;i++)
			{
				if (s [i] != '>') 
				{
					qc.Enqueue (s [i]);
				}
				else 
				{
					Right = i;
					break;
				}
			}
			for(int i=Right+1;i<s.Length;i++)
			{
				contain += s [i];
			}
			while(qc.Count!=0)
			{
				char _c=qc.Dequeue ();
				if(_c=='#')
				{
					char Mark = qc.Dequeue ();
					char eq = qc.Dequeue ();
					int[] DataBuffer = new int[4];
					int Leg = 0;
					string StringBuffer="";
					bool _T = true;
					while(_T)
					{
						if(Leg==4)
						{
							break;
						}
						char charbuffer = qc.Dequeue ();
						if (charbuffer == ' '||charbuffer=='#')
						{
							DataBuffer [Leg] = int.Parse (StringBuffer);
							StringBuffer = "";
							Leg++;
							if(charbuffer=='#')
							{
								_T = false;
							}
						}
						else
						{
							StringBuffer += charbuffer;
						}
					}
					switch(Mark)
					{
					case 'C':
						col = new Color ((float)DataBuffer[0],(float)DataBuffer[1],(float)DataBuffer[2],(float)DataBuffer[3]);
						break;
					case 'F':
						break;
					case 'V':
						velocity = new Vector2 (DataBuffer[0],DataBuffer[1]);
						break;
					case 'S':
						size = DataBuffer [0];
						break;
					case 'D':
						Initpos = DataBuffer [0] + 2 * DataBuffer [1] + 3 * DataBuffer [2] + 4 * DataBuffer [3];
						break;
					case 'P':
						pos_scale = (float)DataBuffer [0] / 100;
						Debug.Log (pos_scale);
						break;
					default:
						break;
					}
				}
			}
		}
	}
}
