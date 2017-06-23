using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

		Transform tr_Player;
		float f_RotSpeed=3.0f,f_MoveSpeed = 4.0f;


		void Start () {

			tr_Player = GameObject.FindGameObjectWithTag ("Player").transform; }


		void Update () {
        //This is to look at the player object, and the second one is to go to the player object.
			transform.rotation = Quaternion.Slerp (transform.rotation , Quaternion.LookRotation (tr_Player.position - transform.position) , f_RotSpeed * Time.deltaTime);
			transform.position += transform.forward * f_MoveSpeed * Time.deltaTime;
		}
	}

