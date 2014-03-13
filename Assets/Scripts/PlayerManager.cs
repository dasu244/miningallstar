using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	

		}

		void LateUpdate ()
		{

				if (!IsGrounded () && !animation.isPlaying) {
						Vector3 PosXYZ = gameObject.transform.position;
						PosXYZ.y = PosXYZ.y - 0.15f;
						gameObject.transform.position = PosXYZ;
				}
		
		}

		public bool IsGrounded ()
		{
				bool thisReady = false;
				Vector3 PosXYZ = gameObject.transform.position;
				thisReady = Physics.Raycast (gameObject.transform.position, -Vector3.up, 0.55f);
				return thisReady;
		}

		public void MovePlayer (Vector3 EndPos)
		{
				Vector3 StartPos = gameObject.transform.position;

				AnimationCurve curve1 = null, curve2 = null, curve3 = null;
				AnimationClip clip = null;
				curve1 = AnimationCurve.Linear (0, StartPos.x, 0.2f, EndPos.x);
				curve2 = AnimationCurve.Linear (0, StartPos.y, 0.2f, EndPos.y);
				curve3 = AnimationCurve.Linear (0, StartPos.z, 0.2f, EndPos.z);
		
				clip = new AnimationClip ();
				clip.SetCurve ("", typeof(Transform), "localPosition.x", curve1);
				clip.SetCurve ("", typeof(Transform), "localPosition.y", curve2);
				clip.SetCurve ("", typeof(Transform), "localPosition.z", curve3);
		
				if (gameObject.GetComponent ("Animation") == null) {
						gameObject.AddComponent ("Animation");
				}
				if (gameObject.animation.IsPlaying ("AnimationDemo")) {
						//TempGO.animation["AnimationDemo"].time = 0.5f ;
						gameObject.animation.Sample ();
						gameObject.animation.RemoveClip ("AnimationDemo");
				}
		
				gameObject.animation.AddClip (clip, "AnimationDemo");
				gameObject.animation ["AnimationDemo"].speed = 1f;
				gameObject.animation.Play ("AnimationDemo");
				//TempGO.animation.wrapMode=WrapMode.PingPong;
		}
}
