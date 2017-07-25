using UnityEngine;

namespace Assets.Scripts.Item
{
    [System.Serializable]
    public class Icon : MonoBehaviour
    {
        [SerializeField] private Sprite _image;
        [SerializeField] private string _toolTip;

        public Sprite Image()
        {
            return null;
        }

        public string ToolTip
        {
            get { return _toolTip;}
            set { _toolTip = value; }
        }


    }
}
