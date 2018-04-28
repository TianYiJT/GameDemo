using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectPanel : MonoBehaviour 
{
	public int MAX_COUNT;
	public Button[] buttons;
	ScrollRect _sr;
	float Last_Value;
	float AllOffset;
	// Use this for initialization
	protected virtual void Start () 
	{
		_sr = this.gameObject.GetComponent<ScrollRect> ();
		for (int i = 0; i < MAX_COUNT; i++) 
		{
			GameObject _g = GameObject.Find (i.ToString());
			buttons [i].onClick.AddListener (delegate()
			{
					this.Button_Event(_g);
			});
			buttons [i].transform.SetParent (_sr.viewport);
		}
		Last_Value = _sr.verticalScrollbar.value;
		AllOffset = buttons [0].transform.position.y - buttons [MAX_COUNT - 1].transform.position.y;
	}
	// Update is called once per frame
	protected virtual void Update ()
	{
		Rect sr_r = _sr.GetComponent<ScrollRect> ().viewport.rect;
		float Temp_Value = _sr.verticalScrollbar.value;
		float offset = (Temp_Value - Last_Value)*AllOffset;
		for(int i=0;i<MAX_COUNT;i++)
		{
			Vector3 bpos = buttons [i].transform.position;
			buttons [i].transform.position = new Vector3 (bpos.x,bpos.y-offset,bpos.z);
		}
		Last_Value = Temp_Value;
		for(int i=0;i<MAX_COUNT;i++)
		{
			Rect br = buttons [i].GetComponent<RectTransform> ().rect;
			if (br.min.y > sr_r.max.y || br.max.y < sr_r.min.y) 
			{
				buttons [i].enabled = false;
			}
			else 
			{
				buttons [i].enabled = true;
			}
		}
	}
	protected virtual void Button_Event(GameObject _g)
	{
		
	}
}
