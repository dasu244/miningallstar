using UnityEngine;
using System.Collections;

public class CurrentGame : MonoBehaviour
{

		//do not forget to add in gameover method
		public static int CurrentScore = 0;
		int CurrentDepth = 0, CurrentBlocks = 0, ComboScore = 0;
		int CurrentChargeBar = 0, NumberOfExplosions = 0;
		public int ChargeBarLenght = 0, TotalChargeBars = 0;
		TextMesh ScoreText, DepthText;
		MainGame mainGameInstance;
		float Xval;
		GameObject ProgressBar, BarCount;

		// Use this for initialization
		void Start ()
		{
				TotalChargeBars = 0;
				ChargeBarLenght = 2;
				ScoreText = GameObject.Find ("Score").GetComponent<TextMesh> ();
				DepthText = GameObject.Find ("Depth").GetComponent<TextMesh> ();
				mainGameInstance = GameObject.Find ("GameController").GetComponent<MainGame> ();

				ProgressBar = GameObject.Find ("ProgressBar");
				BarCount = GameObject.Find ("TotalBar");
				Xval = ProgressBar.transform.localScale.y;
		}
	
		// Update is called once per frame
		void Update ()
		{

	
		}

		public void AddScore (int addScore)
		{
				CurrentScore += addScore; 
				ScoreText.text = "" + CurrentScore;
				if (CurrentDepth < Mathf.Abs ((int)mainGameInstance.mainPlayer.transform.position.y)) {
						CurrentDepth = Mathf.Abs ((int)mainGameInstance.mainPlayer.transform.position.y);
				}
				DepthText.text = "" + CurrentDepth;
				FillChargeBar (1);
		}

		public void FillChargeBar (int addCharge)
		{
				CurrentChargeBar += addCharge;
				Xval = Xval + 0.2f;
				//Debug.Log(CurrentChargeBar+">="+ChargeBarLenght+":"+Xval);
				if (CurrentChargeBar >= 10) {
						Debug.Log ("Charge bar is full");
						TotalChargeBars++;
						CurrentChargeBar = 0;
						Xval = 0;

				} else {
				}

				ProgressBar.transform.localScale = new Vector3 (1f, Xval, 1f);
				BarCount.GetComponent<TextMesh> ().text = "" + TotalChargeBars;

		}

		public void ChargeBarUsed ()
		{
				TotalChargeBars--;
				BarCount.GetComponent<TextMesh> ().text = "" + TotalChargeBars;
		}

		public void AddExplosion ()
		{
				NumberOfExplosions += 1;
		}

		public void GameOver ()
		{
				CurrentScore = 0;
				CurrentDepth = 0;
				CurrentBlocks = 0;
				ComboScore = 0;
				TotalChargeBars = 0;
				CurrentChargeBar = 0;
				NumberOfExplosions = 0;
				//ChargeBarLenght=2;

		}



}
