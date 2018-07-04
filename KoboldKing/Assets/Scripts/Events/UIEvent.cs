using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Events
{
    public class UIEvent
    {
        /// <summary>
        /// This is called by somebody who has new dialogue to show, such that anybody who cares about new dialogue (perhaps the dialogue UI?) Can react accordingly.
        /// </summary>
        public static string SHOW_DIALOGUE = "SHOW_DIALOGUE";
        /// <summary>
        /// Called when the player chooses a response to a dialogue statement
        /// </summary>
        public static string DIALOGUE_RESPONSE = "DIALOGUE_RESPONSE";
    }
}
