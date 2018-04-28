using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Input_Control : MonoBehaviour
{

	// Use this for initialization
	NetWorkManager network_manager;
	public CharacterController character_controller;
	float move_velocity;
	public float _quick;
	public float _low;
	public float mouse_sensitility;
	Vector2 mousepostion_Last;
	public Camera Main_Camera;
	float timer_count_addvelocity;
	float max_velocity;
	float normal_velocity;
	public float MAX_ARROW_VELOCITY;
	public float MIN_ARROW_VELOCITY;
	float timer_arrow_power;
	public float MAX_ARROW_TIME;
	float temp_attack_angle;
	public GameObject arrow;
	public GameObject arrow_Init_postion;
	bool SumTimerOfAttack;
	Image _Cursor;
	PropManager prm;
	GameManager _gm;
	GameObject picking;
    public GameObject picking_position;
	void Start () 
	{				
		network_manager = GameObject.FindGameObjectWithTag ("NetWorkManager").GetComponent<NetWorkManager>();
		prm = GameObject.FindGameObjectWithTag ("PropManager").GetComponent<PropManager>();
		_gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		max_velocity = _low * 3.5f;
		normal_velocity = _low;
		SumTimerOfAttack = false;
		Cursor.visible = false;
		move_velocity = _low;
	}
	[RPC]
	void Update () 
	{
		if (_gm&&prm&&network_manager) 
		{
			if (Input.GetKeyDown (KeyCode.LeftAlt))
			{
				Cursor.visible = !Cursor.visible;
			}
			switch(_gm._State)
			{
			case (int)GameState.PREPARE:
				move_velocity = _quick;
				Prepare_Input ();
				break;
			case (int)GameState.ING:
				move_velocity = _low;
				Ing_Input ();
				break;
			default:
				break;
			}
		}
		else 
		{
			network_manager = GameObject.FindGameObjectWithTag ("NetWorkManager").GetComponent<NetWorkManager>();
			prm = GameObject.FindGameObjectWithTag ("PropManager").GetComponent<PropManager>();
			_gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		}
	}
	void attack(float power)
	{
		float attackpower = (power / MAX_ARROW_TIME) * (MAX_ARROW_VELOCITY - MIN_ARROW_VELOCITY) + MIN_ARROW_VELOCITY;
		Vector3 attack_vector = Main_Camera.transform.forward;
		attack_vector.Normalize ();
		attack_vector *= attackpower;
		GameObject g=Instantiate (arrow,arrow_Init_postion.transform.position,arrow_Init_postion.transform.rotation) as GameObject;
		g.GetComponent<Arrow> ().velocity = attack_vector;
		g.GetComponent<Arrow> ().id_name = network_manager.IP_PLAYER;
		network_manager.GetComponent<NetworkView> ().RPC ("Init_arrow",RPCMode.Others,arrow_Init_postion.transform.position,arrow_Init_postion.transform.rotation,attack_vector,network_manager.IP_PLAYER);
	}
	void Prepare_Input()
	{
		if(_gm.Is_Main)
		{
			Walk_Input ();
			View_Input ();
			if (!picking)
			{
				Collider[] cs = Physics.OverlapSphere (this.transform.position, 2.0f);
				bool _can = false;
				foreach (Collider _c in cs) 
				{
					if (_can)
					{
						break;
					}
					if (_c.tag == "Prop_Prefab")
					{
						Vector3 PropToGamer = (_c.transform.position - this.transform.position).normalized;
						Vector3 GamerForward = this.transform.forward.normalized;
						PropToGamer = new Vector3 (PropToGamer.x, 0, PropToGamer.z);
						GamerForward = new Vector3 (GamerForward.x, 0, GamerForward.z);
						float cos_angle = Vector3.Dot (PropToGamer,GamerForward);
						if (cos_angle >= Mathf.Cos (0.25f * Mathf.PI)&&Input.GetKeyDown(KeyCode.C)) 
						{
							picking = _c.gameObject;
							picking.transform.position = picking_position.transform.position;
							picking.transform.SetParent (this.transform);
							_can = true;
						}
					}
				}
			}
			else 
			{
				bool _can = false;
				Vector3 v = Vector3.zero;
				// not already
				v=picking.transform.position;
				if(Input.GetKeyDown(KeyCode.F))
				{
					prm.insprop (picking.GetComponent<Prop_Prefab>()._code,v,true);
					network_manager.GetComponent<NetworkView> ().RPC ("InitAProp",RPCMode.Others,picking.GetComponent<Prop_Prefab>()._code,v,network_manager.IP_PLAYER);
					Destroy (picking);
				}
			}
		}
	}
	void Ing_Input()
	{
		if (picking) 
		{
			Destroy (picking);
		}
		if (!GetComponent<chat_control> ().Is_chat) 
		{
			Main_Camera.GetComponent<Camera_Control> ().Size = 0;
			if (SumTimerOfAttack) 
			{ 
				timer_arrow_power += Time.deltaTime;
				Main_Camera.GetComponent<Camera_Control> ().Size = (MAX_ARROW_TIME - Mathf.Clamp (timer_arrow_power, 0, MAX_ARROW_TIME)) / MAX_ARROW_TIME;
			}
			if (Input.GetMouseButtonDown (0)) 
			{
				SumTimerOfAttack = true;		
			}
			if (Input.GetMouseButtonUp (0))
			{
				timer_arrow_power = Mathf.Clamp (timer_arrow_power, 0, MAX_ARROW_TIME);
				attack (timer_arrow_power);
				SumTimerOfAttack = false;
				timer_arrow_power = 0;
			}
			Walk_Input ();
			View_Input ();
			if (Input.GetKeyDown (KeyCode.End)) 
			{
				Application.Quit ();
			}
		}
	}
	void Walk_Input()
	{
		if (Input.GetKey (KeyCode.W)) 
		{
			timer_count_addvelocity += Time.deltaTime;
			if (timer_count_addvelocity >= 3.0f) 
			{
				move_velocity = _low*3.5f;
			}
			character_controller.Move (move_velocity * Time.deltaTime * this.transform.forward.normalized);
			network_manager.GetComponent<NetworkView> ().RPC ("Move", RPCMode.Others, move_velocity * this.transform.forward.normalized, network_manager.IP_PLAYER);
		} 
		else 
		{
			timer_count_addvelocity = 0;
		}
		if (Input.GetKey (KeyCode.S))
		{
			character_controller.Move (-move_velocity * Time.deltaTime * this.transform.forward.normalized);
			network_manager.GetComponent<NetworkView> ().RPC ("Move", RPCMode.Others, -move_velocity * this.transform.forward.normalized, network_manager.IP_PLAYER);
		}
		if (Input.GetKey (KeyCode.A)) 
		{
			character_controller.Move (-move_velocity * Time.deltaTime * this.transform.right.normalized);
			network_manager.GetComponent<NetworkView> ().RPC ("Move", RPCMode.Others, -move_velocity * this.transform.right.normalized, network_manager.IP_PLAYER);
		}
		if (Input.GetKey (KeyCode.D)) 
		{
			character_controller.Move (move_velocity * Time.deltaTime * this.transform.right.normalized);
			network_manager.GetComponent<NetworkView> ().RPC ("Move", RPCMode.Others, move_velocity * this.transform.right.normalized, network_manager.IP_PLAYER);
		}
	}
	void View_Input()
	{
		Vector2 View = new Vector2 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"));
		this.transform.Rotate (new Vector3 (0, View.x * mouse_sensitility, 0));
		network_manager.GetComponent<NetworkView> ().RPC ("view_move", RPCMode.Others, new Vector3 (0, View.x * mouse_sensitility, 0), network_manager.IP_PLAYER);
		if (-View.y < 0 && temp_attack_angle >= -15) 
		{
			Main_Camera.transform.Rotate (-View.y * mouse_sensitility, 0, 0);
			network_manager.GetComponent<NetworkView> ().RPC ("view_move_up_down", RPCMode.Others, new Vector3 (-View.y * mouse_sensitility, 0, 0), network_manager.IP_PLAYER);
			temp_attack_angle += -View.y;
		}
		if (-View.y > 0 && temp_attack_angle <= 15)
		{
			Main_Camera.transform.Rotate (-View.y * mouse_sensitility, 0, 0);
			network_manager.GetComponent<NetworkView> ().RPC ("view_move_up_down", RPCMode.Others, new Vector3 (-View.y * mouse_sensitility, 0, 0), network_manager.IP_PLAYER);
			temp_attack_angle += -View.y;
		}
	}
}
