using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour
{

		CurrentGame CurrentGameInstance;
		MainGame mainGameInstance;
		MenuHandler menuHandlerInstance;
		bool StopChecking = false;
		int[] LeftXYVal, RightXYVal, UpXYVal, DownXYVal;
		GameObject leftGO, rightGO, upGO, downGO;
		public int chance = 0;
		public int OVal = 0;


		// Use this for initialization
		void Start ()
		{

				CurrentGameInstance = GameObject.Find ("GameController").GetComponent<CurrentGame> ();
				mainGameInstance = GameObject.Find ("GameController").GetComponent<MainGame> ();
				menuHandlerInstance = GameObject.Find ("Main Camera").GetComponent<MenuHandler> ();

				leftGO = mainGameInstance.GetObject (gameObject, MainGame.Direction.left);
				rightGO = mainGameInstance.GetObject (gameObject, MainGame.Direction.right);
				upGO = mainGameInstance.GetObject (gameObject, MainGame.Direction.up);
				downGO = mainGameInstance.GetObject (gameObject, MainGame.Direction.down);
				

				LeftXYVal = mainGameInstance.getXYVal (leftGO);
				RightXYVal = mainGameInstance.getXYVal (rightGO);
				DownXYVal = mainGameInstance.getXYVal (downGO);
				UpXYVal = mainGameInstance.getXYVal (upGO);

		}
	
		// Update is called once per frame
		void LateUpdate ()
		{
	
				if (!StopChecking) {
						if (leftGO != null) {
								//leftscore
								if (mainGameInstance.oreArray [LeftXYVal [0], LeftXYVal [1]] == 0) {
										StopChecking = true;
								}
						}
						if (rightGO != null) {
								//right score
								if (mainGameInstance.oreArray [RightXYVal [0], RightXYVal [1]] == 0) {
										StopChecking = true;
								}
						}
			
						if (downGO != null) {
								//down score
								if (mainGameInstance.oreArray [DownXYVal [0], DownXYVal [1]] == 0) {
										StopChecking = true;
								}
						}
			
						if (upGO != null) {
								//up score
								if (mainGameInstance.oreArray [UpXYVal [0], UpXYVal [1]] == 0) {
										StopChecking = true;
								}
						}
				}
		}

		void ChangeSprite ()/// yet to implement :*
		{
		
		}

		void KillSelf ()
		{
				Invoke ("killIt", 0.1f);
		}

		void killIt ()
		{
				if (gameObject.GetComponent<SpriteRenderer> ().enabled && gameObject.tag == "block") {

						gameObject.GetComponent<SpriteRenderer> ().enabled = false;
						gameObject.GetComponent<BoxCollider> ().enabled = false;
						CurrentGameInstance.AddScore (1);
						int x = Mathf.Abs ((int)gameObject.transform.position.x);
						int y = Mathf.Abs ((int)gameObject.transform.position.y);
						if (mainGameInstance.oreArray [x, y] == 50) {
								CurrentGameInstance.AddExplosion ();
								StartCoroutine (mainGameInstance.ExplosionRandom (this.gameObject, 0.7f));
						}
						mainGameInstance.oreArray [x, y] = 0;
				} else {
				}
		}

		void GenerateExplosion ()
		{

		}

		void Explode ()
		{
				Instantiate (mainGameInstance.prefabGO [8], gameObject.transform.position, Quaternion.identity);
		}




}
