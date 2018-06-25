using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Assets.Scripts.GameGrind
{
    class DialogueTester
    {
        public enum Dialogues { Name, Quest }
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Dialogues Talk { get; set; }

    }
    [Newtonsoft.Json.JsonConstructor]
    public DialogueTest(string _Name, string _Introduction, string _Response, string _Conclusion, Talk _Quest)
    {
        this.Name = _Name;
        this.Introduction = _Introduction;
        this.Response = _Response;
        this.Conclusion = _Conclusion;
        this.Quest = _Quest;
    }
}
