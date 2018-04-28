using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading;
using System.IO;
public struct GData
{
	public int _Index;
	public int[] _mat;
	public bool visible;
}

public enum Cube:int
{
	front=0,
	back=1,
	top=2,
	bottom=3,
	left=4,
	right=5
}

public class Juggle_Scene_Mode : MonoBehaviour
{

	float MaxX,MinX,MaxY,MinY;
	public GameObject plane;
	public GameObject[] prefabs;
	public Material[] prefab_mats;
	public int Tempprefab_mat;
	int TempIndex=0;
	int tempface=0;
	public int MaxCount_X;
	public int MaxCount_Y;
	float juggle_height;
	float Height;
	float X_Interval;
	float Y_Interval;
	public Material mat;
	public Camera Main_Camera;
	List<GData>[,] Scene_Data;
	float temp_x;
	float temp_y;
	GameObject Temp;
	public float velocity;
	public float mouse;
	public Material default_Mat;
	public InputField _IF;
	public Button _b;
 	GameObject lastselect;
	GameObject lastdelete;
	GameObject realGameObject;
	public Material select_delete_mat;
	bool validIns = true;
	[HideInInspector]
	public bool UI_USE=false;
	public GameObject SW;
	public GameObject ViewSphere;
	Load_Mode lm;
	string _path;
	// Use this for initialization
	void Start () 
	{
		Scene_Data = new List<GData>[MaxCount_X, MaxCount_Y];
		for(int i=0;i<MaxCount_X;i++)
		{
			for (int j = 0; j < MaxCount_Y; j++) 
			{
				Scene_Data[i,j]=new List<GData>();
			}
		}
		MaxX = plane.GetComponent<MeshRenderer> ().bounds.max.x;
		MaxY = plane.GetComponent<MeshRenderer> ().bounds.max.z;
		MinX = plane.GetComponent<MeshRenderer> ().bounds.min.x;
		MinY = plane.GetComponent<MeshRenderer> ().bounds.min.z;
		Height = plane.transform.position.y;
		X_Interval = (MaxX - MinX) / MaxCount_X;
		Y_Interval = (MaxY - MinY) / MaxCount_Y;
		Transform_TempPrefab (TempIndex);
		juggle_height = Temp.GetComponent<BoxCollider> ().bounds.max.x - Temp.GetComponent<BoxCollider> ().bounds.min.x;
		lm = this.GetComponent<Load_Mode> ();
	}
	void OnPostRender()
	{
		GL.Clear (false,false,Color.white);
		mat.color = Color.green;
		mat.SetPass (0);
		DrawLine ();
		mat.SetPass (0);
		if(!Input.GetKey(KeyCode.LeftAlt))
			DrawQuad (temp_x,temp_y);
		GL.Flush ();
		//Material mta = new Material ();

	}
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.F2))
		{
			_IF.gameObject.SetActive (!_IF.gameObject.activeSelf);
			_b.gameObject.SetActive (!_b.gameObject.activeSelf);
			UI_USE = !UI_USE;
		}
		if(Input.GetKeyDown(KeyCode.M))
		{
			SW.SetActive (true);
			ViewSphere.SetActive (true);
			UI_USE = true;
		}
		if (!UI_USE) 
		{
			if(Input.GetKeyDown(KeyCode.F1))
			{
				GameObject[] gs = GameObject.FindGameObjectsWithTag ("Cube_Six");
				for(int i=0;i<gs.Length;i++)
				{
					Destroy (gs [i]);
				}
				lm._loadpath = _path;
				lm.Load ();
				Scene_Data = lm._datascene;
			}
			if(Input.GetKeyDown(KeyCode.F3))
			{
				Scene_Mode_IO.Save_A_Mode (_path,Scene_Data,MaxCount_X,MaxCount_Y);
			}
			default_Mat.SetColor ("Main_Color", new Color (0.0f, 0.2f, 0.6f, 0.5f));
			if (lastselect) 
			{
				lastselect.GetComponent<CubeMesh> ().End_Face ();
				lastselect = null;
			}
			if (lastdelete)
			{
				Destroy (lastdelete);
				if (realGameObject)
					realGameObject.SetActive (true);
				realGameObject = null;
			}
			if (Input.GetKey (KeyCode.W)) 
			{
				Vector3 _ForWard = new Vector3 (Main_Camera.transform.forward.x, 0, Main_Camera.transform.forward.z);
				Main_Camera.transform.position += _ForWard.normalized * velocity * Time.deltaTime;
			}
			if (Input.GetKey (KeyCode.A)) 
			{
				Vector3 _Right = new Vector3 (Main_Camera.transform.right.x, 0, Main_Camera.transform.right.z);
				Main_Camera.transform.position += -_Right.normalized * velocity * Time.deltaTime;
			}
			if (Input.GetKey (KeyCode.S)) 
			{
				Vector3 _ForWard = new Vector3 (Main_Camera.transform.forward.x, 0, Main_Camera.transform.forward.z);
				Main_Camera.transform.position += -_ForWard.normalized * velocity * Time.deltaTime;
			}
			if (Input.GetKey (KeyCode.D))
			{
				Vector3 _Right = new Vector3 (Main_Camera.transform.right.x, 0, Main_Camera.transform.right.z);
				Main_Camera.transform.position += _Right.normalized * velocity * Time.deltaTime;
			}
			// test
			if (Input.GetKeyDown (KeyCode.T) && !Input.GetKey (KeyCode.LeftAlt))
			{
				TempIndex = (++TempIndex) % prefabs.Length;
				Transform_TempPrefab (TempIndex);
			}
			float view_Right_Left = Input.GetAxis ("Mouse X");
			Main_Camera.transform.Rotate (new Vector3 (0, view_Right_Left, 0), Space.World);
			float view_up_down = Input.GetAxis ("Mouse Y");
			Main_Camera.transform.Rotate (new Vector3 (-view_up_down, 0, 0), Space.Self);
			Vector3 mouse_position = Input.mousePosition;
			Ray World_Position = Main_Camera.ScreenPointToRay (mouse_position);
			if (!Input.GetKey (KeyCode.LeftAlt) && !Input.GetKey (KeyCode.LeftControl) && !Input.GetKey (KeyCode.RightAlt))
			{	
				float scroll = Input.GetAxis ("Mouse ScrollWheel");
				Main_Camera.transform.position += new Vector3 (0, scroll, 0) * 5;
				Vector3 _cross = crosspoint (World_Position);
				RaycastHit hit = new RaycastHit ();
				if (Physics.Raycast (World_Position, out hit, 100.0f)) 
				{
					if (hit.collider.gameObject.tag != "Plane") 
					{
						_cross = hit.collider.gameObject.transform.position;
					}
				}
				temp_x = (_cross.x - MinX) / (MaxX - MinX) * (float)MaxCount_X;
				temp_y = (_cross.z - MinY) / (MaxY - MinY) * (float)MaxCount_Y;
				temp_x = Mathf.Clamp (temp_x, 0, MaxCount_X - 1);
				temp_y = Mathf.Clamp (temp_y, 0, MaxCount_Y - 1);
				if (!Temp || !validIns)
					Transform_TempPrefab (TempIndex);
				validIns = true;
				if (Scene_Data [(int)temp_x, (int)temp_y].Count > 0) 
				{
					if (Scene_Data [(int)temp_x, (int)temp_y] [Scene_Data [(int)temp_x, (int)temp_y].Count - 1]._Index != 0) 
					{
						foreach (MeshRenderer _mr in Temp.GetComponentsInChildren<MeshRenderer>()) 
						{
							Material[] mat_new = new Material[_mr.materials.Length];
							for (int i = 0; i < mat_new.Length; i++)
							{
								mat_new [i] = select_delete_mat;
							}
							_mr.materials = mat_new;
						}
						validIns = false;
					}
				} 
				float Y_Cross = Height + (Scene_Data [(int)temp_x, (int)temp_y].Count + 1) * (juggle_height);
				Vector3 TempCenter = new Vector3 (MinX + ((int)(temp_x) + 0.5f) * X_Interval, Y_Cross, MinY + ((int)(temp_y) + 0.5f) * Y_Interval);
				if (Input.GetMouseButtonDown (0) && validIns) 
				{
					GameObject _g = Instantiate (prefabs [TempIndex], TempCenter, Quaternion.identity) as GameObject;
					_g.transform.localScale = Temp.transform.localScale;
					GData _gd = new GData ();
					_gd._Index = TempIndex;
					_gd._mat = new int[6];
					_gd.visible = true;
					for (int i = 0; i < 6; i++) 
					{
						_gd._mat [i] = -1;
					}
					Scene_Data [(int)temp_x, (int)temp_y].Add (_gd);
				}
				Temp.transform.position = TempCenter;
			} 
			else if (!Input.GetKey (KeyCode.LeftControl) && !Input.GetKey (KeyCode.RightAlt)) 
			{
				
				if (Temp)
					Destroy (Temp);
				RaycastHit hit = new RaycastHit ();
				if (Physics.Raycast (World_Position, out hit, 100.0f)) 
				{
					//Debug.Log (hit.collider.gameObject.tag);
					if (hit.collider.gameObject.tag == "Cube_Six") 
					{
					
//					//float scroll = Input.GetAxis ("Mouse ScrollWheel");
//					//Debug.Log (scroll.ToString());
//					//tempface = (int)(tempface + scroll * 10) % 6;
//					//if(tempface<0)
//					{
//						tempface = -(-tempface) % 6 + 6;
////				}
						//Debug.Log (hit.normal.normalized.ToString ());
						if (hit.normal.normalized == Vector3.back) {
							tempface = (int)Cube.back;
						} else if (hit.normal.normalized == Vector3.forward) {
							tempface = (int)Cube.front;
						} else if (hit.normal.normalized == Vector3.left) {
							tempface = (int)Cube.left;
						} else if (hit.normal.normalized == Vector3.right) {
							tempface = (int)Cube.right;
						} else if (hit.normal.normalized == Vector3.up) {
							tempface = (int)Cube.top;
						} else if (hit.normal.normalized == Vector3.down) {
							tempface = (int)Cube.bottom;
						}
						hit.collider.gameObject.GetComponent<CubeMesh> ().Tranform_Face (tempface);
					
						if (Input.GetMouseButtonDown (0)) 
						{
							hit.collider.gameObject.GetComponent<CubeMesh> ().Tranform_Mat (prefab_mats [Tempprefab_mat]);
							Vector3 pos_temp = hit.collider.gameObject.transform.position;
							int xtemp = (int)((pos_temp.x - MinX) / X_Interval);
							int ytemp = (int)((pos_temp.z - MinY) / Y_Interval);
							float height = ((pos_temp.y - Height) / juggle_height) - 1;
							height += 0.00001f;
							Scene_Data [xtemp, ytemp] [(int)height]._mat [tempface] = Tempprefab_mat;
						}
						lastselect = hit.collider.gameObject;
					}
					else if (hit.collider.gameObject.tag == "Cube_five") 
					{
					
					}
				}
			} 
			else if (!Input.GetKey (KeyCode.RightAlt)) 
			{
				if (Temp)
					Destroy (Temp);
				RaycastHit hit = new RaycastHit ();
				if (Physics.Raycast (World_Position, out hit, 100.0f)) 
				{
					if (hit.collider.gameObject.tag != "Plane") 
					{
						Vector3 pos_temp = hit.collider.gameObject.transform.position;
						int xtemp = (int)((pos_temp.x - MinX) / X_Interval);
						int ytemp = (int)((pos_temp.z - MinY) / Y_Interval);
						int height = (int)(((pos_temp.y - Height) / juggle_height) - 1 + 0.00001f);
						if (height == Scene_Data [xtemp, ytemp].Count - 1) 
						{
							GameObject g1 = Instantiate (hit.collider.gameObject, hit.collider.gameObject.transform.position, Quaternion.identity) as GameObject;
							MeshRenderer[] mrs = g1.GetComponentsInChildren<MeshRenderer> ();
							foreach (MeshRenderer mr in mrs) 
							{
								Material[] mat_new = new Material[mr.materials.Length];
								for (int i = 0; i < mat_new.Length; i++) 
								{
									mat_new [i] = select_delete_mat;
								}
								mr.materials = mat_new;
							}
							if (Input.GetMouseButtonDown (0)) 
							{
								Scene_Data [xtemp, ytemp].RemoveAt (height);
								Destroy (hit.collider.gameObject);
							}
							lastdelete = g1;
							realGameObject = hit.collider.gameObject;
							realGameObject.SetActive (false);
						}
					}
				}
			} 
			else 
			{
				if (Temp)
					Destroy (Temp);
				RaycastHit hit = new RaycastHit ();
				if (Physics.Raycast (World_Position, out hit, 100.0f))
				{
					if (hit.collider.tag == "Cube_Six" && Input.GetMouseButtonDown (0))
					{
						int X_Index = (int)((hit.collider.transform.position.x - MinX) / X_Interval);
						int Y_Index = (int)((hit.collider.transform.position.z - MinY) / Y_Interval);
						int H_Index = (int)(((hit.collider.transform.position.y - Height) / juggle_height) - 1 + 0.00001f);
						Debug.Log (H_Index.ToString ());
						//Scene_Data [X_Index, Y_Index] [H_Index].visible = false;
						GData gd = Scene_Data [X_Index, Y_Index] [H_Index];
						gd.visible = false;
						Scene_Data [X_Index, Y_Index] [H_Index] = gd;
						hit.collider.gameObject.SetActive (false);
					} 
					else if (hit.collider.tag == "Cube_Six")
					{
						Material mat_temp = default_Mat;
						mat_temp.SetColor ("Main_Color", Color.gray);
						GameObject g1 = Instantiate (hit.collider.gameObject, hit.collider.gameObject.transform.position, Quaternion.identity) as GameObject;
						MeshRenderer[] mrs = g1.GetComponentsInChildren<MeshRenderer> ();
						foreach (MeshRenderer mr in mrs) 
						{
							Material[] mat_new = new Material[mr.materials.Length];
							for (int i = 0; i < mat_new.Length; i++) 
							{
								mat_new [i] = mat_temp;
							}
							mr.materials = mat_new;
						}
						lastdelete = g1;
						realGameObject = hit.collider.gameObject;
						realGameObject.SetActive (false);
					}
				}
			}
		}
	} 	
	void DrawQuad(float _x,float _y)
	{
		GL.PushMatrix ();
		GL.Begin (GL.QUADS);
		GL.Vertex3 (MinX+((int)(_x))*X_Interval,Height*(Scene_Data[(int)_x,(int)_y].Count+1),MinY+((int)(_y))*Y_Interval);
		GL.Vertex3 (MinX+((int)(_x))*X_Interval,Height*(Scene_Data[(int)_x,(int)_y].Count+1),MinY+((int)(_y+1))*Y_Interval);
		GL.Vertex3 (MinX+((int)(_x+1))*X_Interval,Height*(Scene_Data[(int)_x,(int)_y].Count+1),MinY+((int)(_y+1))*Y_Interval);
		GL.Vertex3 (MinX+((int)(_x+1))*X_Interval,Height*(Scene_Data[(int)_x,(int)_y].Count+1),MinY+((int)(_y))*Y_Interval);
		GL.End ();
		GL.PopMatrix ();
	}
	Vector3 crosspoint(Ray r)
	{
		float _H = Height - r.origin.y;
		float _scale = _H / r.direction.y;
		return r.origin + r.direction * _scale;
	}
	void DrawLine()
	{
		GL.PushMatrix ();
		GL.Begin (GL.LINES);
		for(int i=0;i<=MaxCount_X;i++)
		{
			GL.Vertex3 (MinX+i*(X_Interval),Height,MinY);
			GL.Vertex3 (MinX+i*(X_Interval),Height,MaxY);
		}
		for(int i=0;i<=MaxCount_Y;i++)
		{
			GL.Vertex3 (MinX,Height,MinY+i*(Y_Interval));
			GL.Vertex3 (MaxX,Height,MinY+i*(Y_Interval));
		}
		GL.End();
		GL.PopMatrix ();
	}
	void Transform_TempPrefab(int _index)
	{
		if(Temp)
			Destroy (Temp);
		Temp = Instantiate (prefabs[_index],transform.position,Quaternion.identity) as GameObject;
		float _tempsize = Temp.GetComponent<BoxCollider> ().bounds.max.y - Temp.GetComponent<BoxCollider> ().bounds.min.y;
		float _scale = X_Interval / _tempsize;
		Temp.transform.localScale *= _scale;
		foreach (MeshRenderer _mr in Temp.GetComponentsInChildren<MeshRenderer>()) 
		{
			Material[] mat_new = new Material[_mr.materials.Length];
			for(int i=0;i<mat_new.Length;i++)
			{
				mat_new [i] = default_Mat;
			}
			_mr.materials = mat_new;
		}
	}
	public void confirm()
	{
		_path=_IF.text;
		//Scene_Mode_IO.Save_A_Mode (out_path,Scene_Data,MaxCount_X,MaxCount_Y);
	}
}
