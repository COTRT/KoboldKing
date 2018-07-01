using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Assets.Scripts.GameGrind
{
    public class DialogueDatabase : MonoBehaviour
    {
        public static DialogueDatabase Instance { get; set; }
        //private List<Item> Items { get; set; }
        private List<GGDialogue> Dialogues { get; set; }
        //public List<BaseStat> nothing { get; set; }
        public string npc = "bob";
       



        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            BuildDatabase();
            GetConversation(npc);
        }
        private void BuildDatabase()
        {
            Dialogues = JsonConvert.DeserializeObject<List<GGDialogue>>(Resources.Load<TextAsset>("GameGrind/JSON/DialogueTest").ToString());
            //Debug.Log(Items[0].Stats[1].StatName + " level is " + Items[0].Stats[0].GetCalculatedStatValue());
            Debug.Log(Dialogues[0].Introduction);
        }

        public GGDialogue GetConversation(string Dialoguesv)
        {
            // TODO: Lamba expression with where clause could be faster?
            foreach (GGDialogue talk in Dialogues)
            {
                if (talk.Name == Dialoguesv)
                    return talk;
            }

            Debug.LogWarning("Couldn't find conversation " + Dialoguesv);
            return null;

        }
    }
}
