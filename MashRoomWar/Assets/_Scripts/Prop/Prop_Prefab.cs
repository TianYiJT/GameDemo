using UnityEngine;
using System.Collections;

public class Prop_Prefab : MonoBehaviour 
{
	public  bool Is_Active;
	public  bool Is_Select;
	public int _code; 
	public bool Is_Should_Sync_Data;
	PropManager prm;
	GameManager _gm;
	protected virtual void Start () 
	{
		_gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager>();
	}
	public void ActiveProp(string s1)
	{
		Is_Active = true;
		Is_Select = false;
	}
	public void UnActive()
	{
		Is_Active = false;
	}
	public void SelectProp()
	{
		Is_Select = true;
	}
	protected virtual void Effect()
	{
		
	}  
	protected virtual void Update () 
	{
		if (_gm) 
		{
			RaycastHit hit;
			Vector3 MIN = this.GetComponent<Collider> ().bounds.min;
			Ray ray = new Ray (MIN, new Vector3 (0, -0.3f, 0));
			if (Physics.Raycast (ray, out hit))
			{
				this.transform.Translate (new Vector3 (0, -5.0f, 0) * Time.deltaTime);
			} 
			else 
			{
				if (!Is_Select && !Is_Active)
				{
					this.transform.Rotate (new Vector3 (0, 1, 0));
					if (_gm._State != (int)GameState.PREPARE) 
					{
						Destroy (this.gameObject);
					}
				}
				if (Is_Active && _gm._State == (int)GameState.ING) 
				{
					Effect ();
				}
			}
		}
		else 
		{
			_gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager>();
		}
	}
}
