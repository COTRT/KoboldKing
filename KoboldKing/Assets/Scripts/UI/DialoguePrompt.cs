using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePrompt : MonoBehaviour
{
    private GameObject Player;

    public Text PromptCanvasText;
    public Canvas ChatPromptCanvas;
    public string PromptnName;
    public string PromptText;
    public Canvas PromptClone;

    public Canvas ConversationBox;
    public Text ConversationCanvasText;
    public string ConversationText;
    public Canvas ConversationClone;
 


    void OnTriggerEnter(Collider other)
    {

        Player = GameObject.FindGameObjectWithTag("Player");

        if (other.gameObject.CompareTag("Player"))
        {
            if (GameObject.Find("NPC Says(Clone)"))
            {
                PromptClone.enabled = true;
            }
            else
            {
                PromptCanvasText.text = PromptText;
                PromptClone = Instantiate(ChatPromptCanvas);
                var bob = GameObject.Find("NPC Says(Clone)");
            }
        }

    }
    void OnTriggerExit(Collider other)
    {
        ConversationClone.enabled = false;
        //Instantiate(ChatPromptCanvas);
    }

    private void Update()
    {
        if (PromptClone.enabled == true)
        {

            if (Input.GetKeyDown(KeyCode.X))
            {
                if (GameObject.Find("ConversationBox(Clone)"))
                {
                    ConversationClone.enabled = true;
                }
                else
                {
                    PromptClone.enabled = false;
                    ConversationCanvasText.text = ConversationText;
                    ConversationClone = Instantiate(ConversationBox);
                    var bob = GameObject.Find("ConversationBox(Clone)");
                }
            }
        }
    }



}
