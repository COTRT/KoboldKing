using UnityEngine;

namespace Assets.Scripts.ItemX
{
    [System.Serializable]
    public class Icon : MonoBehaviour
    {
        [SerializeField] private Sprite _image;
        [SerializeField] private string _toolTip;
        [SerializeField] private string _description;

        public Sprite Image
        {
            get { return _image;}
            set { _image = value; }
        }

        public string ToolTip
        {
            get { return _toolTip;}
            set { _toolTip = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

    }
}
