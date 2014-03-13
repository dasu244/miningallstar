
using UnityEngine;
using System.Collections;

public class Timmer : MonoBehaviour
{
		private float startTime;
		public static  float restSeconds;
		private int roundedRestSeconds;
		private int displaySeconds;
		private int displayMinutes;
		public string text;
		public int countDownSeconds = 10;
		public GUIStyle timerStyle, gameoverstyel1, gameoverstyel2, gameoverstyel3, gameoverstyel4, gameoverstyel5, scorevalstyle;
	bool stop = false;

		void  OnEnable ()
		{
				countDownSeconds = 60;
				startTime = Time.time;
		}

		void  OnGUI ()
		{

		if(GUI.Button(new Rect(20,40,80,20), " Restart ")) {
			Application.LoadLevel("Mining Allstar");
		}
		if (MenuHandler.isAllReady == true && !stop) {
		
						//GameObject.Find("Timer").renderer.enabled=true;
						float guiTime = Time.time - startTime;
						restSeconds = countDownSeconds - (guiTime);
						//this.gameObject.GetComponent<dfLabel>().Text=text;

						//display the timer
						roundedRestSeconds = Mathf.CeilToInt (restSeconds);
						displaySeconds = roundedRestSeconds % 60;
						displayMinutes = roundedRestSeconds / 60; 

						text = string.Format ("{00}", roundedRestSeconds); 
						GameObject.Find ("Timer_val").GetComponent<TextMesh> ().text = "" + text;
    	
						if (restSeconds <= 0) {
								restSeconds = 0;
								text = "to Game Over Screen";
								Time.timeScale = 0;

				//gameObject.transform.position = new Vector3(23,0,-10);
								
								GUI.Label (new Rect (Screen.width / 2 - 160, Screen.height / 2 - 240, Screen.width, Screen.height), "", gameoverstyel1);//green bg
								GUI.Label (new Rect (Screen.width / 2 - 152, Screen.height / 2 - 237, Screen.width - 17, Screen.height - 25), "", gameoverstyel2);//blue bg
								GUI.Label (new Rect (Screen.width / 2 - 157, Screen.height / 2 - 237, Screen.width - 15, Screen.height - 30), "", gameoverstyel3);//border
								GUI.Label (new Rect (Screen.width / 2 - 145, Screen.height / 2 - 233, Screen.width - 22, Screen.height - 35), "", gameoverstyel4);//blue border bg
								GUI.Label (new Rect (Screen.width / 2 - 130, Screen.height / 2 - 180, Screen.width - 140, Screen.height - 160), "", gameoverstyel5);//game over text
								GUI.Label (new Rect (Screen.width / 2 - 10, Screen.height / 2 - 105, Screen.width / 2 - 100, Screen.height / 2 - 200), "" + CurrentGame.CurrentScore.ToString (), scorevalstyle);//green bg
							stop = true;
			}
				}
				//  
				//		if(((int)restSeconds)>=0)
//		{
//			this.gameObject.SetActive(false);
//}
		}
}

//		1)Mecanim 
//		2)AI
//		3)Navigation Path finding
//		4)Camera movement
//		5)NGUI
//		6)Animation Control
//		7)Ragdoll
//		8)Good knowledge of physics
//		9)Audio
//		10)Humanoid avatars and avatar masks
