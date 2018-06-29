using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogueManager : MonoBehaviour {

    private Camera mainCamera;
	// Use this for initialization
	void Start () {
       mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonUp("EnterDialogue"))
        {
            RaycastHit hit;
            Ray raycast = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(raycast,out hit))
            {
                IConverser converser = hit.transform.gameObject.GetComponent<IConverser>();
                if (converser != null)
                {
                    Debug.Log(converser.GetDialogue().Statement);
                }
            }
        }
	}
}
