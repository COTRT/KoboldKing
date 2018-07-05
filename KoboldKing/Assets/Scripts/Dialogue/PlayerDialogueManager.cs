using Assets.Scripts.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogueManager : MonoBehaviour
{
    private Camera mainCamera;
    // Use this for initialization
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Q))
        {
            RaycastHit hit;
            Ray raycast = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(raycast, out hit))
            {
                Converser converser = hit.transform.gameObject.GetComponent<Converser>();
                if (converser != null)
                {
                    Messenger<Dialogue>.Broadcast(UIEvent.SHOW_DIALOGUE, converser.GetDialogue());
                }
                else
                {
                    Debug.LogWarning("Found no converser on the target object");
                }
            }
        }
    }
}
