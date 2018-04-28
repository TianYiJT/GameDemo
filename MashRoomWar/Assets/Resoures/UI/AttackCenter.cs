using UnityEngine;
using System.Collections;

public static class AttackCenter
{
	static float line_length=Screen.width/20;
	static float line_interval=Screen.width/60;
	static float MAX_SIZE=0.05f;
	static int slice=40;
	public static void DrawCenter()
	{ 
		GL.PushMatrix(); 
		GL.LoadOrtho();  
		GL.Begin (GL.LINES);
		GL.Vertex(new Vector3(0.498f,0.5f,0.0f));
		GL.Vertex(new Vector3(0.502f,0.5f,0.0f));
		GL.End ();
		GL.Begin (GL.LINES);
		GL.Vertex (new Vector3(0.45f,0.5f,0.0f));
		GL.Vertex (new Vector3(0.485f,0.5f,0.0f));
		GL.End ();
		GL.Begin (GL.LINES);
		GL.Vertex (new Vector3(0.515f,0.5f,0.0f));
		GL.Vertex (new Vector3 (0.55f, 0.5f, 0.0f));
		GL.End ();
		GL.PopMatrix ();
	}
	public static void DrawCircle(float Size)
	{
		GL.PushMatrix(); 
		GL.LoadOrtho();  
		float Temp_Size = Size * MAX_SIZE;
		for(int i=0;i<slice;i++)
		{
			float _i = 2 * Mathf.PI / slice * (float)i;
			float _i1 = 2 * Mathf.PI / slice * (float)(i+1);
			float pos_x1 = 0.5f + Mathf.Cos (_i) * Temp_Size;
			float pos_y1 = 0.5f + Mathf.Sin (_i) * Temp_Size*(float)Screen.width / (float)Screen.height;
			float pos_x2 = 0.5f + Mathf.Cos (_i1) * Temp_Size;
			float pos_y2 = 0.5f + Mathf.Sin (_i1) * Temp_Size*(float)Screen.width / (float)Screen.height;
			GL.Begin (GL.LINES);
			GL.Vertex (new Vector3 (pos_x1, pos_y1, 0.0f));
			GL.Vertex (new Vector3 (pos_x2, pos_y2, 0.0f));
			GL.End ();
		}
		GL.PopMatrix ();
	}
}
