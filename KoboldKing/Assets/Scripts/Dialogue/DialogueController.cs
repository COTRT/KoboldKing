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
    private Button[] responseButtons = new Button[0];
    private Button responseButtonPrefab;

    void Start()
    {
        dialogueParent = transform.GetChild(0);
        statement = dialogueParent.Find("Statement").GetChild(0).GetComponent<Text>();
        responses = dialogueParent.Find("Responses");
        dialogueParent.gameObject.SetActive(false);
        responseButtonPrefab = Resources.Load<Button>("UI/DialogueResponse");
        Messenger<Dialogue, GameObject>.AddListener(UIEvent.SHOW_DIALOGUE, ShowDialogue);
    }


    public void ShowDialogue(Dialogue dialogue, GameObject speaker)
    {
        foreach (var b in responseButtons) Destroy(b.gameObject);
        statement.text = dialogue.Statement;
        dialogueParent.gameObject.SetActive(true);
        if (dialogue.Action != null)
        {
            string[] actionSegments = dialogue.Action.Split(' ');
            string action = actionSegments[0];
            var arguments = actionSegments.Skip(1);
            Debug.Log("Attempting to perform command:  " + action);
            Messenger<string[],GameObject>.Broadcast(action, arguments.ToArray(), speaker,MessengerMode.DONT_REQUIRE_LISTENER);
        }
        if (dialogue.Responses != null)
        {
            responseButtons = dialogue.Responses.Select(kv =>
            {
                var rb = Instantiate(responseButtonPrefab, responses);
                rb.transform.GetChild(0).GetComponent<Text>().text = kv.Key;
                rb.onClick.AddListener(() =>
                {
                    Messenger<string>.Broadcast(UIEvent.DIALOGUE_RESPONSE, kv.Key, MessengerMode.DONT_REQUIRE_LISTENER);
                    Messenger<Dialogue,GameObject>.Broadcast(UIEvent.SHOW_DIALOGUE, kv.Value,speaker);
                });
                return rb;
            }).ToArray();
        }
        else
        {
            var rb = Instantiate(responseButtonPrefab, responses);
            rb.transform.GetChild(0).GetComponent<Text>().text = "Dismiss";
            rb.onClick.AddListener(() =>
            {
                dialogueParent.gameObject.SetActive(false);
            });
            responseButtons = new Button[]
            {
                rb
            };
        }
    }

}
