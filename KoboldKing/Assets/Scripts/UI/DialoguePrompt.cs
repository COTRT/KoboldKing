using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePrompt : MonoBehaviour
{
    private GameObject Player;

    public Text PromptCanvasText;
    public Canvas ChatPromptCanvas;
    public string PromptName;
    public string PromptText;
    public Canvas PromptClone;

    public Canvas ConversationBox;
    public Text ConversationCanvasText;
    public string CurrentConversationText;
    public Canvas ConversationClone;
    public int ConversationPageNumber = 0;
    public string[] AllConversationTexts;

    public bool CanStartCoroutine = true;

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
                    PromptClone.enabled = false;

                }
                else
                {
                    PromptClone.enabled = false;
                    ConversationCanvasText.text = CurrentConversationText;
                    ConversationClone = Instantiate(ConversationBox);
                    var bob = GameObject.Find("ConversationBox(Clone)");
                }
            }
        }
        if(ConversationClone.enabled == true)
        {
            while (CanStartCoroutine)
            {
                StartCoroutine("ConversationWait");
 
            }
            CurrentConversationText = AllConversationTexts[ConversationPageNumber];
        }
    }

   IEnumerator ConversationWait()
    {
        CanStartCoroutine = false;
        yield return new WaitForSeconds(2.0f);
        ConversationPageNumber = 1;
       yield return new WaitForSeconds(2.0f);
       ConversationPageNumber = 2;
    } 

}
