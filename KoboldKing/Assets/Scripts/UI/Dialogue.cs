using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour {
    private GameObject Player;
    public Canvas ChatPromptCanvas;
    public string ConversationName;

    void OnTriggerEnter (Collider other) {

        Player = GameObject.FindGameObjectWithTag("Player");

        if (other.gameObject.CompareTag("Player"))
        {
         
            Instantiate(ChatPromptCanvas);

            
        }

    }
}
