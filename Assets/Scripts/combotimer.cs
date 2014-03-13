using UnityEngine;
using System.Collections;

public class combotimer : MonoBehaviour {
	//variables for combo timer
	public string text; 
	public static float countDownSeconds=10;
	// Use this for initialization
	public GameObject cdtimer,cmultiplier;
	void Start () {

		countDownSeconds=20;
		Debug.Log("in start"+countDownSeconds);
		InvokeRepeating("cobmotimef",1f,1f);
	}

	void cobmotimef()
	{

		if(MainGame.combo_Count>1)
		{
			countDownSeconds=countDownSeconds-1;
			Debug.Log("countDownSeconds"+countDownSeconds);
			cdtimer.GetComponent<TextMesh>().text="cdTimer:"+countDownSeconds.ToString();
		}
		if(countDownSeconds==0)
		{
			MainGame.combo_Count=0;
			cmultiplier.GetComponent<TextMesh>().text="Multiplier=:"+MainGame.combo_Count.ToString();
			countDownSeconds=20;
			cdtimer.GetComponent<TextMesh>().text="cdTimer:"+countDownSeconds.ToString();
			cdtimer.SetActive(false);
		}
	}

}
