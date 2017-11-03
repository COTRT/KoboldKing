using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePrompt : MonoBehaviour {
    private GameObject Player;

    public Text Text;
    public Canvas ChatPromptCanvas;
    public string ConversatioPromptnName;
    public string ConversationPromptText;
    public Canvas PromptClone;
    public Canvas ConversationBox;
    public Canvas ConversationClone;



    void OnTriggerEnter (Collider other) {

        Player = GameObject.FindGameObjectWithTag("Player");

        if (other.gameObject.CompareTag("Player"))
        {
            if (GameObject.Find("NPC Says")) {
                PromptClone.enabled = true;
            }
            else
            {
                Text.text = ConversationPromptText;
                PromptClone = Instantiate(ChatPromptCanvas);
                var bob = GameObject.Find("NPC Says");
            }
        }

    }
    public void OnTriggerExit(Collider other)
    {
        PromptClone.enabled = false;

    }

}
