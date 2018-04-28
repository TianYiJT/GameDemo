using UnityEngine;
using System.Collections;

public class Count_Point : MonoBehaviour
{
	[HideInInspector]
	public GameObject[] players;
	[HideInInspector]
	public int[] goals;
	public GameObject[] goal_point;
	public float goal_interval;
	float timer;
	public float MAX_DISTANCE;
	public float per_hurting_goal;
	void Start () 
	{
		players = GameObject.FindGameObjectsWithTag ("Player");
		for(int i=0;i<players.Length;i++)
		{
			players [i].GetComponent<CharacterManager> ().goal_index = i;
		}
		goals = new int[players.Length];
	}

	void Update () 
	{
		if (timer > goal_interval) 
		{
			for (int i = 0; i < players.Length; i++)
			{
				int goal_temp=0;
				for (int j = 0; j < goal_point.Length; j++) 
				{
					goal_temp += (int)(MAX_DISTANCE/(players [i].transform.position - goal_point [j].transform.position).magnitude);
					goal_temp = (int)Mathf.Clamp (0, MAX_DISTANCE, goal_temp);
				}
				goal_temp /= goal_point.Length;
				players [i].GetComponent<CharacterManager> ().normal_goal += goal_temp;
			}
		}
		else 
		{
			timer += Time.deltaTime;	
		}
	}
	public void Count_Point_End()
	{
		for(int i=0;i<players.Length;i++)
		{
			goals [i] = (int)(players [i].GetComponent<CharacterManager> ().hurting * per_hurting_goal + players [i].GetComponent<CharacterManager> ().normal_goal);
		}
	}
}
