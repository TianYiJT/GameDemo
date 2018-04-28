using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Collections.Generic;

public static class Scene_Mode_IO
{
	static Dictionary<string,string> All_Mode;
	public static void Save_A_Mode(string path,List<GData>[,] mode_scene,int count_x,int count_y)
	{
		if (File.Exists (path)) 
		{
			
		}
		else 
		{
			File.Create (path);	
		}
		XmlDocument xmldoc = new XmlDocument ();
		XmlElement xe_fir = xmldoc.CreateElement ("Scene");
		xmldoc.AppendChild (xe_fir);
		for(int i=0;i<count_x;i++)
		{
			for(int j=0;j<count_y;j++)
			{
				string _name = "position";
				XmlElement xe_pos = xmldoc.CreateElement (_name);
				xe_pos.SetAttribute ("x", i.ToString ());
				xe_pos.SetAttribute ("y",j.ToString());
				int index=0;
				foreach(GData _gd in mode_scene[i,j])
				{
					index++;
					XmlElement xe_GameObject = xmldoc.CreateElement ("GameObject");
					xe_GameObject.InnerText = _gd._Index.ToString ();
					Debug.Log (_gd.visible.ToString());
					xe_GameObject.SetAttribute ("Visible",_gd.visible.ToString());
					if (_gd._Index == 0) 
					{
						xe_GameObject.SetAttribute ("frontMat",_gd._mat[(int)Cube.front].ToString());
						xe_GameObject.SetAttribute ("backMat", _gd._mat [(int)Cube.back].ToString ());
						xe_GameObject.SetAttribute ("leftMat",_gd._mat[(int)Cube.left].ToString());
						xe_GameObject.SetAttribute ("rightMat",_gd._mat[(int)Cube.right].ToString());
						xe_GameObject.SetAttribute ("topMat", _gd._mat [(int)Cube.top].ToString ());
						xe_GameObject.SetAttribute ("downMat",_gd._mat[(int)Cube.bottom].ToString());
					}
					xe_pos.AppendChild (xe_GameObject);
				}
				xe_fir.AppendChild (xe_pos);
			}

		}
		xmldoc.Save (path);
	}
	public static List<GData>[,] Load_A_Mode(string _path,ref int MAX_X, ref int MAX_Y)
	{
		string path = _path;
		if (File.Exists (path)) 
		{
			XmlDocument xmldoc = new XmlDocument ();
			xmldoc.Load (path);
			int X_count, Y_count;
			XmlElement xe = (XmlElement)xmldoc.FirstChild.LastChild;
			X_count = int.Parse (xe.GetAttribute ("x"));
			Y_count = int.Parse (xe.GetAttribute("y"));

			List<GData>[,] mode_return = new List<GData>[X_count + 1, Y_count + 1];
			for(int i=0;i<X_count + 1;i++)
			{
				for (int j = 0; j < Y_count + 1; j++) 
				{
					mode_return[i,j]=new List<GData>();
				}
			}
			MAX_X = X_count + 1;
			MAX_Y = Y_count + 1;
			XmlNodeList xnl = xmldoc.SelectSingleNode ("Scene").ChildNodes;
			foreach(XmlElement xe1 in xnl)
			{
				int temp_x = int.Parse (xe1.GetAttribute("x"));
				int temp_y = int.Parse (xe1.GetAttribute ("y"));
				//Debug.Log (temp_x+" "+temp_y);
				XmlNodeList GameObject_List = xe1.ChildNodes;
				foreach(XmlElement xe2 in GameObject_List)
				{
					GData _gd;
					_gd._Index = int.Parse (xe2.InnerText);
					_gd.visible = bool.Parse (xe2.GetAttribute ("Visible"));
					_gd._mat = new int[6];
					if(_gd._Index==0)
					{
						_gd._mat [(int)Cube.front] = int.Parse (xe2.GetAttribute ("frontMat"));
						_gd._mat [(int)Cube.back] = int.Parse (xe2.GetAttribute("backMat"));
						_gd._mat [(int)Cube.left] = int.Parse (xe2.GetAttribute("leftMat"));
						_gd._mat [(int)Cube.right] = int.Parse (xe2.GetAttribute ("rightMat"));
						_gd._mat [(int)Cube.top] = int.Parse (xe2.GetAttribute("topMat"));
						_gd._mat [(int)Cube.bottom] = int.Parse (xe2.GetAttribute("downMat"));
					}
					mode_return [temp_x, temp_y].Add (_gd);
				}
			}
			return mode_return;
		}
		else
		{
			Debug.LogError ("None exist");
			return new List<GData>[1, 1];
		}
	}
}
