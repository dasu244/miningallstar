using UnityEngine;
using System.Collections;

public class MenuHandler : MonoBehaviour
{

		Vector3[] cameraPos = new Vector3[10];
		MainGame mainGameInstance ;
		GameObject objHit;
		public static bool isAllReady = false;
		public static int Coins = 0, Score = 0, Depth = 0; 
		//variables for combo
		public  int block_val = 0;
		public GameObject planet1, planet2, planet3;//level select screen objects
		public GameObject item_btn, suits_btn, items, suits;//guistore screen objects
		public GameObject trophies_btn, gallery_btn, trophies, gallery;//Achievement screen objects
		public bool itemflag = false;
		public int right_btn_count = 0, left_btn_count = 0;
		public GameObject hud;
		float timeDiff, StTime, EnTime;
		float imgclick_timediff, imgclick_sttime, imgclick_endtime;
		public GameObject playbtn, play_icon, settings_btn, store_btn, achievements_btn, right_btn, left_btn, back_btn;//,extit_btn


		// Use this for initialization
		void Awake ()
		{
				cameraPos [0] = new Vector3 (-15, 0, -10);//main camera posns
				cameraPos [1] = new Vector3 (-3, 0, -10);// game play posns
				cameraPos [2] = new Vector3 (7, 0, -10);//setting screen posns
				cameraPos [3] = new Vector3 (15, 0, -10);//level select screen posns
				cameraPos [4] = new Vector3 (23, 0, -10);//gameover screen posns
				cameraPos [5] = new Vector3 (31, 0, -10);//achievement 1 screen posns
				cameraPos [6] = new Vector3 (39, 0, -10);//achievement 2 screen posns
				cameraPos [7] = new Vector3 (47, 0, -10);//gui store screen posns
				mainGameInstance = GameObject.Find ("GameController").GetComponent<MainGame> ();
				mainGameInstance.enabled = false;
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

						if (Physics.Raycast (ray, out hit, 100)) {
								objHit = hit.collider.gameObject;
						}
				}
				if (Input.GetMouseButtonUp (0)) {
						if (Physics.Raycast (ray, out hit, 100)) {
								if (objHit.name == hit.collider.gameObject.name && hit.collider.gameObject.tag == "button") {
										HandleMenu (hit.collider.gameObject);
								} else {
								}
						}
				}
		}

		void HandleMenu (GameObject hitGO)
		{
		
				if (object.ReferenceEquals (hitGO, playbtn)) {
						mainGameInstance.MoveGO (gameObject, cameraPos [0], cameraPos [3]);
						back_btn.transform.position = new Vector3 (16.23009f, -4.8f, -1.288086f);
				} else if (object.ReferenceEquals (hitGO, play_icon)) {
						mainGameInstance.MoveGO (gameObject, cameraPos [0], cameraPos [1]);
						gameObject.GetComponent<Timmer> ().enabled = true;
						
						isAllReady = true;
						hud.renderer.enabled = true;
						GameObject.Find ("Timer_val").renderer.enabled = true;      
						//GameObject.Find ("c_d_Timer").renderer.enabled = true;
						GameObject.Find ("combo_multiplier").renderer.enabled = true;
						back_btn.transform.position = new Vector3 (16.23666f, -4.8f, -1.288086f);
						mainGameInstance.enabled = true;
						this.enabled = false;
						//mainGameInstance.spawnGrid ();

				} else if (object.ReferenceEquals (hitGO, settings_btn)) {
						mainGameInstance.MoveGO (gameObject, cameraPos [0], cameraPos [2]);
						back_btn.transform.position = new Vector3 (8.2f, -4.8f, -1.288086f);
				} else if (object.ReferenceEquals (hitGO, store_btn)) {
						mainGameInstance.MoveGO (gameObject, cameraPos [0], cameraPos [7]);
						back_btn.transform.position = new Vector3 (48.2f, -4.8f, -1.288086f);
				} else if (object.ReferenceEquals (hitGO, achievements_btn)) {
						mainGameInstance.MoveGO (gameObject, cameraPos [0], cameraPos [5]);
						back_btn.transform.position = new Vector3 (32.21f, -4.8f, -1.288086f);
				} else if (object.ReferenceEquals (hitGO, items)) {//store screen
						if (itemflag == false) {
								item_btn.renderer.material.mainTexture = Resources.Load ("gui_itemsDown")as Texture2D;
								items.SetActive (true);
								suits.SetActive (false);
						}
				} else if (object.ReferenceEquals (hitGO, suits)) {
						suits_btn.renderer.material.mainTexture = Resources.Load ("gui_suitsDown")as Texture2D;
						suits.SetActive (true);
						items.SetActive (false);
				} else if (object.ReferenceEquals (hitGO, trophies)) {//achievement screen
						trophies_btn.renderer.material.mainTexture = Resources.Load ("gui_trophiesDown")as Texture2D;
						trophies.SetActive (true);
						gallery.SetActive (false);
				} else if (object.ReferenceEquals (hitGO, gallery)) {//achievement screen
						gallery_btn.renderer.material.mainTexture = Resources.Load ("gui_galleryDown")as Texture2D;
						trophies.SetActive (false);
						gallery.SetActive (true);
				} else if (object.ReferenceEquals (hitGO, right_btn)) {//level select right button
						right_btn_count = right_btn_count + 1;
						levelSelectionUI (right_btn_count);
			
				} else if (object.ReferenceEquals (hitGO, left_btn)) {//level select right button
						left_btn_count = left_btn_count + 1;
						level_select_leftbutton (left_btn_count);
				} else if (object.ReferenceEquals (hitGO, back_btn)) {
						Debug.Log ("in back button");
						mainGameInstance.MoveGO (gameObject, cameraPos [2], cameraPos [0]);
				}
		
				/*else if(hitGO.name=="ExitButton")
			{
				Application.Quit();
			}*/
		
		
		}

		// UI section -------------------------------------
		void mainGameUI ()
		{

		}

		void levelSelectionUI (int rcount)
		{
				switch (rcount) {
				case 1:

						planet1.animation ["planet11"].speed = 1;
						planet2.animation ["planet21"].speed = 1;
						planet3.animation ["planet31"].speed = 1;
						planet2.animation.Play ("planet21");
						planet1.animation.Play ("planet11");
						planet3.animation.Play ("planet31");
						break;
				case 2:
						planet1.animation ["planet12"].speed = 1;
						planet2.animation ["planet22"].speed = 1;
						planet3.animation ["planet32"].speed = 1;
						planet1.animation.Play ("planet12");
						planet2.animation.Play ("planet22");
						planet3.animation.Play ("planet32");
						break;
				case 3:
						planet1.animation ["planet13"].speed = 1;
						planet2.animation ["planet23"].speed = 1;
						planet3.animation ["planet33"].speed = 1;
						planet1.animation.Play ("planet13");
						planet2.animation.Play ("planet23");
						planet3.animation.Play ("planet33");
						break;
			
				}
		
		
		}

		void level_select_leftbutton (int lcount)
		{
				switch (lcount) {
			
				case 1:
						Debug.Log ("lcount====" + lcount);
						planet1.animation ["planet11"].speed = -1;
						planet1.animation ["planet11"].time = planet1.animation ["planet11"].length;
						planet1.animation.Play ("planet11");
			
						planet2.animation ["planet21"].speed = -1;
						planet2.animation ["planet21"].time = planet2.animation ["planet21"].length;
						planet2.animation.Play ("planet21");
			
			
						planet3.animation ["planet31"].speed = -1;
						planet3.animation ["planet31"].time = planet3.animation ["planet31"].length;
						planet3.animation.Play ("planet31");
			
						break;
				case 2:
						Debug.Log ("lcount====" + lcount);
						planet1.animation ["planet12"].speed = -1;
						planet1.animation ["planet12"].time = planet1.animation ["planet12"].length;
						planet1.animation.Play ("planet12");
			
						planet2.animation ["planet22"].speed = -1;
						planet2.animation ["planet22"].time = planet2.animation ["planet22"].length;
						planet2.animation.Play ("planet22");
			
						planet3.animation ["planet32"].speed = -1;
						planet3.animation ["planet32"].time = planet3.animation ["planet32"].length;
						planet3.animation.Play ("planet32");
						break;
				case 3:
						Debug.Log ("lcount====" + lcount);
						planet1.animation ["planet13"].speed = -1;
						planet1.animation ["planet13"].time = planet1.animation ["planet13"].length;
						planet1.animation.Play ("planet13");
			
						planet2.animation ["planet23"].speed = -1;
						planet2.animation ["planet23"].time = planet2.animation ["planet23"].length;
						planet2.animation.Play ("planet23");
			
						planet3.animation ["planet33"].speed = -1;
						planet3.animation ["planet33"].time = planet3.animation ["planet33"].length;
						planet3.animation.Play ("planet33");
						break;
			
			
				}
		}

		void upgradeUI ()
		{

		}

		void settingsUI ()
		{

		}

		//UI section END --------------------------------------






		//code for combo logic
		void combo_logic (int combo_counter, int val)
		{
				//Debug.Log("combo count === "+combo_counter);
				int comboval = 0;
				comboval = val * combo_counter;
				//Score=Score+comboval;
				CurrentGame.CurrentScore = CurrentGame.CurrentScore + comboval;
				Debug.Log ("comboval==" + comboval + "CurrentGame.CurrentScore" + CurrentGame.CurrentScore);
				//Score += comboval; 

		}

		void OnGUI ()
		{
				if (isAllReady) {
						if (GUI.Button (new Rect (0, 50, 70, 30), "Restart")) {
								Time.timeScale = 1;
								isAllReady = false;
								Application.LoadLevel ("Mining Allstar");
								CurrentGame.CurrentScore = 0;
						}
				}
		}

		//GamePLay section END---------------------------


	public void MoveCamera(int start, int end)
	{
		mainGameInstance.MoveGO (gameObject, cameraPos [start], cameraPos [end]);
	}


}

