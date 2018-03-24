using Assets.Scripts.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class DialogueManager : ManagerBase
    {
        private Text dialogueBoxText;
        private GameObject dialoguePanel;
        private EventHandler onDialogueDismissed;

        public override void Startup(DataService dataService)
        {
            dialogueBoxText = GameObject.FindGameObjectWithTag("DialogueText").GetComponent<Text>();
            dialoguePanel = dialogueBoxText.transform.parent.gameObject;
            dialoguePanel.SetActive(false);
        }

        public bool ShowDialogue(Dialogue dialogue,EventHandler onDialogueDismissed = null)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                return false;
            }
            dialogueBoxText.text = dialogue.Statement;
            dialoguePanel.SetActive(true);
            this.onDialogueDismissed = onDialogueDismissed;
            return true;
        }

        // Update is called once per frame
        void Update()
        {
            if (dialoguePanel.activeInHierarchy && Input.GetKeyUp(KeyCode.X)) //Static "X" control (not Input.GetButtonUp), because this UI is quite temporary.
            {
                if (onDialogueDismissed != null)
                {
                    onDialogueDismissed.Invoke(this, null);
                }
                dialoguePanel.SetActive(false);
                onDialogueDismissed = null;
            }
        }
    }
}

