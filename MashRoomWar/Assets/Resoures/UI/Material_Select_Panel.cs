using UnityEngine;
using System.Collections;

public class Material_Select_Panel : SelectPanel 
{
	Juggle_Scene_Mode JSM;
	public GameObject _g;
	protected override void Start ()
	{
		base.Start ();
		JSM = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Juggle_Scene_Mode>();
	}
	protected override void Update ()
	{
		base.Update ();
	}
	protected override void Button_Event (GameObject g)
	{
		int _i = int.Parse (g.name);
		JSM.Tempprefab_mat = _i;
		Debug.Log (_i);
		_g.GetComponent<MeshRenderer> ().material = JSM.prefab_mats [_i];
		base.Button_Event (_g);
	}
}
