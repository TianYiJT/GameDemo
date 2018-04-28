using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using pluck;
public class Pluck
{
	Rect MyRect;
	Vector2 velocity;
	Vector2 position;
	string contain;
	Color c;
	int size;
	int font;
	public static Font f1;
	public static Font f2;
	public static Font f3;
	public static Font f4;
	public Pluck(int Size,int InitPos,float _scale,string _contain,Color col,Vector2 velocity,int f)
	{
		this.velocity=velocity;
		Vector2 Init_pos;
		switch(InitPos)
		{
		case (int)PluckInitPosition.Bottom:
			Init_pos=new Vector2(Mathf.Lerp(10,Screen.width-10,_scale),10);
			break;
		case (int)PluckInitPosition.Top:
			Init_pos=new Vector2(Mathf.Lerp(10,Screen.width-10,_scale),Screen.height-10);
			break;
		case (int)PluckInitPosition.Left:
			Init_pos=new Vector2(10,Mathf.Lerp(10,Screen.height-10,_scale));
			break;
		case (int)PluckInitPosition.Right:
			Init_pos=new Vector2(Screen.width-10,Mathf.Lerp(10,Screen.height-10,_scale));
			break;
		default:
			Init_pos=new Vector2();
			break;
		}
		position=Init_pos;
		MyRect=new Rect(Init_pos,new Vector2(150,150));
		size=Size;
		contain=_contain;
		this.velocity=velocity;
		c=col;
		this.font=f;
	}
	public bool IsNotRender()
	{
		if(Vector2.SqrMagnitude(MyRect.center-position)>1.0f)
		{
			if(OutOfMaxRect(MyRect))
			{
				return true;
			}
		}
		return false;
	}
	bool OutOfMaxRect(Rect r)
	{
		if (r.yMin > Screen.height || r.yMax < 0 || r.xMax < 0 || r.xMin > Screen.width) 
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public void updateposition()
	{
		MyRect.center += velocity;
	}
	public void Render()
	{
		GUIStyle gs = new GUIStyle ();
		switch(font)
		{
		case (int)PluckFont.default_font:
			gs.font = f1;
			break;
		case (int)PluckFont.font_one:
			gs.font = f2;
			break;
		case (int)PluckFont.font_two:
			gs.font = f3;
			break;
		case (int)PluckFont.font_three:
			gs.font = f4;
			break;
		default:
			gs.font = f1;
			break;
		}
		gs.fontSize =size;

		GUI.backgroundColor=Color.clear;
		gs.normal.textColor = c;
		GUI.TextField(MyRect,contain,gs);
		Debug.Log (c);
	}
}
public class PluckManager : MonoBehaviour 
{

	[HideInInspector]
	public List<Pluck> pluckqueue=new List<Pluck>();
	//float timer;
	//public Material mat_std;
	//public GameObject TextPrefab;
	public  Font f1;
	public  Font f2;
	public  Font f3;
	public  Font f4;
	void Start()
	{
		Pluck.f1 = f1;
		Pluck.f2 = f2;
		Pluck.f3 = f3;
		Pluck.f4 = f4;
		//pluckqueue = new List<Pluck> ();
	}
	void OnGUI()
	{
		foreach(Pluck pl in pluckqueue)
		{
			pl.Render ();
		}
	}
	void Update()
	{
		//test
//		if (timer < 0.5f) 
//		{
//			timer += Time.deltaTime;
//		}
//		else 
//		{
//			timer = 0;
//			Pluck _pluck=new Pluck(30,(int)PluckInitPosition.Right,Random.Range(0.0f,1.0f),"billbill",
//				Color.red,new Vector2(-6,0),font);
//			pluckqueue.Add (_pluck);
//		}
		List<Pluck> BufferPluck=new List<Pluck>();
		foreach(Pluck pl in pluckqueue)
		{
			if (pl.IsNotRender ()) 
			{
				BufferPluck.Add (pl);
			}
		}
		foreach(Pluck pl in BufferPluck)
		{
			pluckqueue.Remove (pl);
		}
		BufferPluck.Clear ();
		foreach(Pluck pl in pluckqueue)
		{
			pl.updateposition ();
		}
	}
}
namespace pluck
{
	public enum PluckInitPosition:int
	{
		Left=1,
		Right=2,
		Bottom=3,
		Top=4
	}
	public enum PluckFont:int
	{
		default_font=0,
		font_one=1,
		font_two=2,
		font_three=3
	}
}
