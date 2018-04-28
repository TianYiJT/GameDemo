using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputFieldFadeInOut : MonoBehaviour 
{

	// Use this for initialization
	public InputField IF;
	bool IsFadingIn;
	bool IsFadingOut;
	Color col;
	float timer;
	void Start ()
	{
		IsFadingIn = false;
		IsFadingOut = false;
		col = IF.image.color;
		IF.image.color = Color.clear;
	}
		
	// Update is called once per frame
	void Update () 
	{
		if(IsFadingIn)
		{
			timer += Time.deltaTime;
			IF.image.color = Color.Lerp (Color.clear,col,2*timer);
			StartCoroutine (Fade());
		}
		if(IsFadingOut)
		{
			timer += Time.deltaTime;
			IF.image.color = Color.Lerp (col,Color.clear,2*timer);
			StartCoroutine (Fade());
		}
	}
	public void FadeIn()
	{
		IsFadingIn = true;
	}
	public void FadeOut()
	{
		IsFadingOut = true;
	}
	IEnumerator Fade()
	{
		yield return new WaitForSeconds (0.5f);
		IsFadingIn = false;
		IsFadingOut = false;
		timer = 0;
	}
}
