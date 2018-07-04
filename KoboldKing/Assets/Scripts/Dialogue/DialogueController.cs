using Assets.Scripts.Events;
using System.Collections;

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    private Transform dialogueParent;
    private Text statement;
    private Transform responses;
    private IEnumerable<Button> responseButtons = new Button[0];
    private Button responseButtonPrefab;
    private bool dismissing = false;

    void Start()
    {
        dialogueParent = transform.GetChild(0);
        statement = dialogueParent.Find("Statement").GetChild(0).GetComponent<Text>();
        responses = dialogueParent.Find("Responses");
        dialogueParent.gameObject.SetActive(false);
        responseButtonPrefab = Resources.Load<Button>("UI/DialogueResponse");
        Messenger<Dialogue>.AddListener(UIEvent.SHOW_DIALOGUE, ShowDialogue);
    }


    public void ShowDialogue(Dialogue dialogue)
    {
        foreach (var b in responseButtons) Destroy(b.gameObject);
        if (dismissing)
        {
            dialogueParent.gameObject.SetActive(false);
        }
        statement.text = dialogue.Statement;
        dialogueParent.gameObject.SetActive(true);
        if (dialogue.Responses != null)
        {
            dismissing = false;
            responseButtons = dialogue.Responses.Select(kv =>
            {
                var rb = Instantiate(responseButtonPrefab, responses);
                rb.transform.GetChild(0).GetComponent<Text>().text = kv.Key;
                rb.onClick.AddListener(() =>
                {
                    Messenger<string>.Broadcast(UIEvent.DIALOGUE_RESPONSE, kv.Key, MessengerMode.DONT_REQUIRE_LISTENER);
                    Messenger<Dialogue>.Broadcast(UIEvent.SHOW_DIALOGUE, kv.Value);
                });
                return rb;
            });
        }
        else
        {
            dismissing = true;
            var rb = Instantiate(responseButtonPrefab);
            rb.transform.GetChild(0).GetComponent<Text>().text = "Dismiss";
            responseButtons = new Button[]
            {
                rb
            };
        }
    }

}
