using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class MainGame : MonoBehaviour
{
		SmoothFollow SFInstance ;
		static int noRows = 100;
		static int noCols = 7;
		int KeyVal = 0;
		public GameObject mainPlayer;
		public GameObject[] prefabGO = new GameObject[10]; //oreblockes array used for instantiate
		public GameObject[,] mArray = new GameObject[noCols, noRows];// all OreBlocks stored
		public int[,] oreArray = new int[noCols, noRows];//represents type of ore 
		public GameObject[] oreTypes = new GameObject[6];
		string[] OreTypeNames;
		int[] Chances;
		int[] OreVal;
		protected Animator animator;
		public GameObject cdtimer;
		public enum AvatarAnim
		{
				idle,
				drill,
				walk
		}
		;

		public enum Direction
		{
				right,
				left,
				up,
				down
		}
		;

		GameObject objHit;
		public  static int combo_Count = 0;
		Vector3 TouchDownPos, TouchUpPos;
		float timeDiff, StTime;
		float imgclick_timediff, imgclick_sttime, imgclick_endtime;
		public  int block_val = 0;
		public GameObject cmultiplier;

		//script instance
		CurrentGame currGameInstance;
		MenuHandler menuHandlerInstance;

		//A* variables 
		bool canGO = false;
		List<GameObject> openList = new List<GameObject> ();
		List<GameObject> closedList = new List<GameObject> ();

		// Use this for initialization
		void Awake ()
		{
		
				OreTypeNames = new string[6] {
					"Soil",
					"LowDense",
					"MediumDense",
					"HighDense",
					"UnBreak",
					"Explosion"
				};
				Chances = new int[6]{100,10,7,3,7,5};
				OreVal = new int[6]{1,2,3,5,20,50};
				oreTypes = new GameObject[6];

				Debug.Log ("ore lenght  : " + OreTypeNames.Length);
				for (int i = 0; i < OreTypeNames.Length; i++) {
						GameObject TempGO;
						TempGO = Resources.Load (OreTypeNames [i], typeof(GameObject)) as GameObject;
						TempGO.GetComponent<SelfDestroy> ().chance = Chances [i];   //chance
						TempGO.GetComponent<SelfDestroy> ().OVal = OreVal [i];
						oreTypes [i] = TempGO;
						//oreTypes[i].GetComponent<SelfDestroy>().oreTypeVal = oreVal[i];
				}

				SFInstance = GameObject.Find ("Main Camera").GetComponent<SmoothFollow> ();
				currGameInstance = GameObject.Find ("GameController").GetComponent<CurrentGame> ();
				menuHandlerInstance = GameObject.Find ("Main Camera").GetComponent<MenuHandler> ();

		}

		public void InitializeVariables ()
		{
				animator = mainPlayer.transform.Find ("Global_CTRL").GetComponent<Animator> ();
		}

		void  OnEnable ()
		{
				spawnGrid ();
		}
	
		// Use this for initialization
		void Start ()
		{




		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.Escape)) {
						Application.Quit ();
				}
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		
				if (Input.GetMouseButtonDown (0)) {
						imgclick_sttime = Time.time;
						imgclick_timediff = imgclick_sttime - imgclick_endtime;
			
			
						StTime = Time.time;
						if (Physics.Raycast (ray, out hit, 100)) {
								TouchDownPos = Input.mousePosition;
								objHit = hit.collider.gameObject;
						}

				}
		
		
				if (Input.GetMouseButtonUp (0)) {

						timeDiff = Time.time - StTime;
						imgclick_endtime = Time.time;

						if (Physics.Raycast (ray, out hit, 100)) {
								if (timeDiff < 0.5f) {
										TouchUpPos = Input.mousePosition;
										if (objHit.name == hit.collider.gameObject.name && (hit.collider.gameObject.tag == "block" || hit.collider.gameObject.tag == "unbreak")) {
												HandleGame (hit.collider.gameObject);
										} else if (SwipeDirection (TouchDownPos, TouchUpPos) != 0) {
												if (!mainPlayer.animation.isPlaying) {
														UseChargeBar (mainPlayer, SwipeDirection (TouchDownPos, TouchUpPos));
												}
										} else {
										}
								}

						}
				}
		}

		public void spawnGrid ()
		{
				// find sum of all blocktype chances
				int sum_block_chances = 0;
				foreach (GameObject block in oreTypes)
						sum_block_chances += block.GetComponent<SelfDestroy> ().chance;

				for (int y = 0; y <= mArray.GetUpperBound(1); y++) {
						for (int x = 0; x <= mArray.GetUpperBound(0); x++) {
								if (y == 0) {
										if (x == noCols / 2) {
												//player
												mainPlayer = Instantiate (prefabGO [0], new Vector3 (-x, -y + 1, 0), Quaternion.identity)as GameObject;
												mainPlayer.name = "Hero";
												//mainPlayer.transform.rotation=Quaternion.Euler(0,180,0);
												SFInstance.target = mainPlayer.transform;
												oreArray [x, y] = 0;
												InitializeVariables ();
										} else {
												//empty
												GameObject InitGO;
												InitGO = Instantiate (prefabGO [1], new Vector3 (-x, -y, 0), Quaternion.identity)as GameObject;
												InitGO.name = "" + KeyVal;// remove this if not needed
												mArray [x, y] = InitGO;
												KeyVal++;
												oreArray [x, y] = 0;
										}
								} else {
						
						
										// choose a number within our bounds
										int choose = UnityEngine.Random.Range (0, sum_block_chances);
								
										// find corresponding blocktype
										int blocktype = 0;

										while (choose > oreTypes[blocktype].GetComponent<SelfDestroy>().chance)
												choose -= oreTypes [blocktype++].GetComponent<SelfDestroy> ().chance;
								
										// instantiate
										SpawnBlocks (x, y, blocktype);
								}
						}
				}
		}
				
		public void BlockLogic (GameObject TappedBlock, int count) //Tapped
		{
				if (Time.timeScale != 0) {
						Vector3 posXYZ = new Vector3 (0.1f, -1f, 0f);
						Vector3 minusposXYZ = new Vector3 (-0.1f, -1f, 0f);
						
						//increase combo-Counter
						if (imgclick_timediff <= 1) {
								combo_Count = combo_Count + 1;
						} 
						// break it update every thing
						int x = Mathf.Abs ((int)TappedBlock.transform.position.x);
						int y = Mathf.Abs ((int)TappedBlock.transform.position.y);
				
						if ((mainPlayer.transform.position.x - mArray [x, y].transform.position.x) == 0) {
						} else if ((mainPlayer.transform.position.x - mArray [x, y].transform.position.x) > 0) {
								//flip=true;
								mainPlayer.transform.Find ("Global_CTRL").GetComponent<Puppet2D_GlobalControl> ().flip = true;
								mainPlayer.transform.Find ("Global_CTRL").transform.localPosition = posXYZ;
						} else {
								mainPlayer.transform.Find ("Global_CTRL").GetComponent<Puppet2D_GlobalControl> ().flip = false;
								mainPlayer.transform.Find ("Global_CTRL").transform.localPosition = minusposXYZ;
								//flip=false;
						}

						//AstarAlgorithm (mainPlayer, TappedBlock);

						mainPlayer.GetComponent<PlayerManager> ().MovePlayer (TappedBlock.transform.position);
						TappedBlock.SendMessage ("KillSelf");

						//code for combo-----
						block_val = 1;//mainGameInstance.oreArray[x,y];
						if (count > 1) {
								cdtimer.SetActive (true);
								combo_logic (count, block_val);
						}
						cmultiplier.GetComponent<TextMesh> ().text = "Multiplier=:" + combo_Count.ToString ();
				}
		}

		//code for combo logic
		void combo_logic (int combo_counter, int val)
		{
				int comboval = 0;
				comboval = val * combo_counter;
				currGameInstance.AddScore (comboval);
		}

		void SpawnBlocks (int xPos, int yPos, int i)
		{
				GameObject InitGO;
				InitGO = Instantiate (oreTypes [i], new Vector3 (-xPos, -yPos, 0), Quaternion.identity)as GameObject;
				InitGO.name = "" + KeyVal;// remove this if not needed
				mArray [xPos, yPos] = InitGO;
				oreArray [xPos, yPos] = InitGO.GetComponent<SelfDestroy> ().OVal;
				KeyVal++;
		}

		void HandleGame (GameObject hitGO)
		{
				if (hitGO.tag == "block" && canBreakOre (hitGO)) {
						BlockLogic (hitGO, combo_Count);
				} else if (hitGO.tag == "unbreak") {
				}
		}

		public bool canBreakOre (GameObject gameObj)
		{

				if (gameObj == null)
						return false;

				int x = Mathf.Abs ((int)gameObj.transform.position.x);
				int y = Mathf.Abs ((int)gameObj.transform.position.y);

				if (x < mArray.GetUpperBound (0) && oreArray [x + 1, y] == 0) {
						return true;
				}	
				if (x > 0 && oreArray [x - 1, y] == 0) {
						return true;
				}

				if (y < mArray.GetUpperBound (1) && oreArray [x, y + 1] == 0) {
						return true;
				}	
				if (y > 0 && oreArray [x, y - 1] == 0) {
						return true;
				}

				return false;
		}

		public int[] getXYVal (GameObject tempGO)
		{
				int[] xyVal = new int[5];
				if (tempGO != null) {
						// break it update every thing
						int x1 = Mathf.Abs ((int)tempGO.transform.position.x);
						int y1 = Mathf.Abs ((int)tempGO.transform.position.y);
						xyVal [0] = x1;
						xyVal [1] = y1;
						return xyVal;
				} else {

				}
				return null;

		}

		public IEnumerator ExplosionRandom (GameObject center, float radius)
		{
				Collider[] hitColliders = Physics.OverlapSphere (center.transform.position, radius);
				int i = 0;
				while (i < hitColliders.Length) {
			
						if (hitColliders [i].tag != "unbreak") {
								hitColliders [i].SendMessage ("Explode");
								hitColliders [i].SendMessage ("KillSelf");
						}
						i++;
				}
		
				yield return new WaitForSeconds (0.5f);
		
				hitColliders = Physics.OverlapSphere (GetObject (center, Direction.down).transform.position, radius);
				i = 0;
				while (i < hitColliders.Length) {
						if (hitColliders [i].tag != "unbreak") {
								hitColliders [i].SendMessage ("Explode");
								hitColliders [i].SendMessage ("KillSelf");
						}
						i++;
				}
		
		}



		//Right=1,Left=2,Up=3,Down=4
		public void UseChargeBar (GameObject PlayerGO, int Direction)
		{
				int[] xyVal = getXYVal (PlayerGO);
				List<GameObject> BreakThisBlocks = new List<GameObject> ();
				BreakThisBlocks = BlocksToBreak (mArray [xyVal [0], xyVal [1]], Direction);
				if (BreakThisBlocks.Count > 1) {
						currGameInstance.ChargeBarUsed ();
						foreach (var item in BreakThisBlocks) {
								mainPlayer.GetComponent<PlayerManager> ().MovePlayer (item.transform.position);
								item.SendMessage ("KillSelf");
						}
				}
		}

		List<GameObject> BlocksToBreak (GameObject PlayerGO, int Dir)
		{
				List<GameObject> destroyGO = new List<GameObject> ();
				GameObject previous = PlayerGO;
				destroyGO.Add (PlayerGO);
		
				if (currGameInstance.TotalChargeBars > 0) {
						for (int i = 1; i <= currGameInstance.ChargeBarLenght; i++) {
								int x = (int)Mathf.Abs (previous.transform.position.x) + ((Dir == 1) ? -1 : (Dir == 2) ? 1 : 0);
								int y = (int)Mathf.Abs (previous.transform.position.y) + ((Dir == 3) ? -1 : (Dir == 4) ? 1 : 0);
				
								if (x < 0 || x > mArray.GetUpperBound (0))
										break;
								if (y < 0 || y > mArray.GetUpperBound (1))
										break;
								if (previous.tag == "unbreak")
										break;
				
								previous = mArray [x, y];
								if (previous.tag == "block")
										destroyGO.Add (previous);
						}
				}

				return destroyGO;
		}

		public GameObject GetObject (GameObject PlayerGO, Direction Dir)
		{
				if (PlayerGO == null)
						return null;

				GameObject returnGO = null;
				int x = (int)Mathf.Abs (PlayerGO.transform.position.x) + ((Dir == Direction.right) ? -1 : (Dir == Direction.left) ? 1 : 0);
				int y = (int)Mathf.Abs (PlayerGO.transform.position.y) + ((Dir == Direction.up) ? -1 : (Dir == Direction.down) ? 1 : 0);
				
				if (x < 0 || x > mArray.GetUpperBound (0))
						return returnGO;
				if (y < 0 || y > mArray.GetUpperBound (1))
						return returnGO;

				returnGO = mArray [x, y];
				return returnGO;
		}
		
		public void MoveGO (GameObject TempGO, Vector3 StartPos, Vector3 EndPos)
		{
				AnimationCurve curve1 = null, curve2 = null, curve3 = null;
				AnimationClip clip = null;
				curve1 = AnimationCurve.Linear (0, StartPos.x, 0.2f, EndPos.x);
				curve2 = AnimationCurve.Linear (0, StartPos.y, 0.2f, EndPos.y);
				curve3 = AnimationCurve.Linear (0, StartPos.z, 0.2f, EndPos.z);
		
				clip = new AnimationClip ();
				clip.SetCurve ("", typeof(Transform), "localPosition.x", curve1);
				clip.SetCurve ("", typeof(Transform), "localPosition.y", curve2);
				clip.SetCurve ("", typeof(Transform), "localPosition.z", curve3);
		
				if (TempGO.GetComponent ("Animation") == null) {
						TempGO.AddComponent ("Animation");
				}
				if (TempGO.animation.IsPlaying ("AnimationDemo")) {
						//TempGO.animation["AnimationDemo"].time = 0.5f ;
						TempGO.animation.Sample ();
						TempGO.animation.RemoveClip ("AnimationDemo");
				}
		
				TempGO.animation.AddClip (clip, "AnimationDemo");
				TempGO.animation ["AnimationDemo"].speed = 1f;
				TempGO.animation.Play ("AnimationDemo");
				//TempGO.animation.wrapMode=WrapMode.PingPong;
		}
	
		int SwipeDirection (Vector3 startPos, Vector3 EndPos)
		{
				int Offset = 150, value = 0;
				Vector3 currentSwipe = startPos - EndPos;
		
				//swipe right = 1
				if (currentSwipe.x < 0 && currentSwipe.y > -Offset && currentSwipe.y < Offset) {
						return 1;
				}
				//swipe left = 2
				else if (currentSwipe.x > 0 && currentSwipe.y > -Offset && currentSwipe.y < Offset) {
						return 2;
				}
				//swipe up = 3
				else if (currentSwipe.y < 0 && currentSwipe.x > -Offset && currentSwipe.x < Offset) {
						return 3;
				}
				//swipe down = 4	
				if (currentSwipe.y > 0 && currentSwipe.x > -Offset && currentSwipe.x < Offset) {
						return 4;
				}
		
				return value;
		}

		//************** A* path

		public void AstarAlgorithm (GameObject player, GameObject destOre)
		{

				openList.Clear ();
				closedList.Clear ();
				
				int[] XYval = getXYVal (destOre);
				
				oreArray [XYval [0], XYval [1]] = 0;
				
				//destOre.SendMessage ("KillSelf");

				GameObject currSqr;
		
				List<GameObject> AdjSqrsCheck = new List<GameObject> ();

				XYval = getXYVal (mainPlayer);

				openList.Add (mArray [XYval [0], XYval [1]]);

				openList.Add (MinScoreGameObj (mArray [XYval [0], XYval [1]], destOre));
		
				do {
						foreach (var item in openList) {
								Debug.Log ("Path is found  #$#$#$# :" + item.name);
						}

						//currSqr = openList [getIndexOfGameObj (mainPlayer, destOre)];
						currSqr = openList [openList.Count - 1];
						Debug.Log ("Path is found  #$#$#$# :" + (openList.Count - 1));
						closedList.Add (currSqr);
						openList.Remove (currSqr);

						if (closedList.Contains (destOre)) {		
								canGO = true;
								Debug.Log ("Path is found");
								break;				
						}
			
						//AdjSqrsCheck = AdjSqrs(currSqr);

						AdjSqrsCheck.Add (MinScoreGameObj (currSqr, destOre));
			
						foreach (var aSqr in AdjSqrsCheck) {

								if (closedList.Contains (aSqr)) {
										continue;
								}
								//GameObject GOMin = minScoreGameObj(player,destOre,currSqr);
								if (!openList.Contains (aSqr)) {						
										openList.Add (aSqr);
								} else {
								}

						}
		
				} while(openList.Count!=0);

				foreach (var item in closedList) {
						Debug.Log ("Closed list cntains ^#%#^%%^%^ : " + item);
				}

				if (canGO) {
						canGO = false;
						StartCoroutine (AnimatePlayer ());
				}

		}

		IEnumerator AnimatePlayer ()
		{
				for (int i = 0; i < closedList.Count; i++) {
			
						mainPlayer.GetComponent<PlayerManager> ().MovePlayer (closedList [i].transform.position);
						//MoveGO (mainPlayer, mainPlayer.transform.position, closedList [i].transform.position);
						yield return new WaitForSeconds (0.2f);
						//mainPlayer.transform.position = closedList [i].transform.position;
				}
				//mainPlayer.name = closedList[closedList.Count-1].name;
				closedList [closedList.Count - 1].SendMessage ("KillSelf");
				//closedList[closedList.Count-1].GetComponent<MeshRenderer>().enabled = false;
		}
	
		GameObject MinScoreGameObj (GameObject Center, GameObject destOre)
		{
				if (Center == null)
						return null;
		
		
		
				int[] XYVal, CenterXY, DestXY;
				GameObject leftGO, rightGO, upGO, downGO;
				leftGO = GetObject (Center, Direction.left);
				rightGO = GetObject (Center, Direction.right);
				upGO = GetObject (Center, Direction.up);
				downGO = GetObject (Center, Direction.down);
		
				CenterXY = getXYVal (Center);
				DestXY = getXYVal (destOre);
		
				GameObject returnGameObj = null;
				int leftScore = 1000, rightScore = 1000, upScore = 1000, downScore = 1000;
		
				if (leftGO != null) {
						//leftscore
						XYVal = getXYVal (leftGO);
						if (oreArray [XYVal [0], XYVal [1]] == 0) {
								leftScore = (int)((CalculateABSDiff (CenterXY [0], XYVal [0]) + CalculateABSDiff (CenterXY [1], XYVal [1])) + (CalculateABSDiff (XYVal [0], DestXY [0]) + CalculateABSDiff (XYVal [1], DestXY [1])));
						}
				}
				if (rightGO != null) {
						//right score
						XYVal = getXYVal (rightGO);
						if (oreArray [XYVal [0], XYVal [1]] == 0) {
								rightScore = (int)((CalculateABSDiff (CenterXY [0], XYVal [0]) + CalculateABSDiff (CenterXY [1], XYVal [1])) + (CalculateABSDiff (XYVal [0], DestXY [0]) + CalculateABSDiff (XYVal [1], DestXY [1])));
						}
				}
		
				if (downGO != null) {
						//down score
						XYVal = getXYVal (downGO);
						if (oreArray [XYVal [0], XYVal [1]] == 0) {
								downScore = (int)((CalculateABSDiff (CenterXY [0], XYVal [0]) + CalculateABSDiff (CenterXY [1], XYVal [1])) + (CalculateABSDiff (XYVal [0], DestXY [0]) + CalculateABSDiff (XYVal [1], DestXY [1])));
						}
				}
		
				if (upGO != null) {
						//up score
						XYVal = getXYVal (upGO);
						if (oreArray [XYVal [0], XYVal [1]] == 0) {
								upScore = (int)((CalculateABSDiff (CenterXY [0], XYVal [0]) + CalculateABSDiff (CenterXY [1], XYVal [1])) + (CalculateABSDiff (XYVal [0], DestXY [0]) + CalculateABSDiff (XYVal [1], DestXY [1])));
						}
				}
		
				//(int right,int left,int up,int down)
				int score = checkMinScore (rightScore, leftScore, upScore, downScore);

				switch (score) {
				case 1:
						return rightGO;

				case 2:
						return leftGO;
			
				case 3:
						return upGO;

				case 4:
						return downGO;
				}

				return returnGameObj;
		}
	
		float CalculateABSDiff (float a, float b)
		{
				return Mathf.Abs (a - b);
		}
	
		int getIndexOfGameObj (GameObject player, GameObject destOre)
		{
				int minScore = 100;
				int index = 0, LowestNumber = 0;
				List<int> fScore = new List<int> ();
		
				foreach (var item in openList) {
						fScore.Add (getScore (player, destOre, item));
				}
		
				minScore = fScore [0];
				for (index = 0; index < fScore.Count; index++) {
						LowestNumber = fScore [index];
						if (minScore > LowestNumber) {
								//				Debug.Log("LowestNumber :"+LowestNumber+"  minScore : "+minScore);
								minScore = LowestNumber;
						}
				}
		
				return (int)Mathf.Abs (fScore.IndexOf (minScore));
		}
	
		int getScore (GameObject player, GameObject destOre, GameObject currGameObj)
		{
				if (player == null || destOre == null || currGameObj == null)
						return 0;
		
				int[] playerXY = getXYVal (player);
				int[] destOreXY = getXYVal (destOre);
				int[] currXY = getXYVal (currGameObj);
		
		
				return (int)((CalculateABSDiff (playerXY [0], currXY [0]) + CalculateABSDiff (playerXY [1], currXY [1])) + (CalculateABSDiff (currXY [0], destOreXY [0]) + CalculateABSDiff (currXY [1], destOreXY [1])));
		}
	
		List<GameObject> AdjSqrs (GameObject currSqr)
		{		
				List<GameObject> adjSqrs = new List<GameObject> ();
		
				int[] XYVal;
				GameObject leftGO, rightGO, upGO, downGO;
				leftGO = GetObject (currSqr, Direction.left);
				rightGO = GetObject (currSqr, Direction.right);
				upGO = GetObject (currSqr, Direction.up);
				downGO = GetObject (currSqr, Direction.down);
		
				if (leftGO != null) {
						//leftscore
						XYVal = getXYVal (leftGO);
						if (oreArray [XYVal [0], XYVal [1]] == 0) {
								adjSqrs.Add (leftGO);
						}
				}
				if (rightGO != null) {
						//right score
						XYVal = getXYVal (rightGO);
						if (oreArray [XYVal [0], XYVal [1]] == 0) {
								adjSqrs.Add (rightGO);
						}
				}
		
				if (downGO != null) {
						//down score
						XYVal = getXYVal (downGO);
						if (oreArray [XYVal [0], XYVal [1]] == 0) {
								adjSqrs.Add (downGO);
						}
				}
		
				if (upGO != null) {
						//up score
						XYVal = getXYVal (upGO);
						if (oreArray [XYVal [0], XYVal [1]] == 0) {
								adjSqrs.Add (upGO);
						}
				}
		
				return adjSqrs;
		
		}

		int checkMinScore (int right, int left, int up, int down)
		{
				//((right < left) ? )
				//scoreArray 
				//this is not the correct way please change das (workonit)
				if (right < left) {
						if (right < up) {
								if (right < down) {
										//return right
										return 1;
								} else {
										//return down
										return 4;
								}
						} else {
								if (up < down) {
										//return up
										return 3;
								} else {
										//retutn down
										return 4;
								}
						}
				} else {
						if (left < up) {
								if (left < down) {
										//return left
										return 2;
								} else {
										//return down
										return 4;
								}
						} else {
								if (up < down) {
										//return up
										return 3;
								} else {
										//retutn down
										return 4;
								}
						}
				}
		
		}
		// A* star finish 

		// player animation

		public void PlayerAnimation (AvatarAnim anim)
		{
		
				switch (anim) {
				case AvatarAnim.idle:
						animator.SetBool ("walk", false);
						animator.SetBool ("drill", false);
						break;
				case AvatarAnim.walk:
						animator.SetBool ("walk", true);
						animator.SetBool ("drill", false);
						break;
				case AvatarAnim.drill:
						animator.SetBool ("walk", false);
						animator.SetBool ("drill", true);
						break;
			
			
				}
		
		}

		//player animation finish
}
