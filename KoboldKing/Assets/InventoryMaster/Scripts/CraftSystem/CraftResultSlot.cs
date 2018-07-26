using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftResultSlot : MonoBehaviour
{

    CraftSystem craftSystem;
    public int temp = 0;
    ItemSlot itemGameObject;
    //Inventory inventory;


    // Use this for initialization
    void Start()
    {
        //inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        craftSystem = transform.parent.GetComponent<CraftSystem>();

        itemGameObject = Instantiate(Resources.Load("Prefabs/Item") as GameObject).GetComponent<ItemSlot>();
        itemGameObject.transform.SetParent(this.gameObject.transform);
        itemGameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
        itemGameObject.GetComponent<DragItem>().enabled = false;
        itemGameObject.gameObject.SetActive(false);
        itemGameObject.transform.GetChild(1).GetComponent<Text>().enabled = true;
        itemGameObject.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector2(PlayerInventory.Instance.mainInventory.positionNumberX, PlayerInventory.Instance.mainInventory.positionNumberY);

    }

    // Update is called once per frame
    void Update()
    {
        if (craftSystem.possibleItems.Count != 0)
        {
            itemGameObject.Item = craftSystem.possibleItems[temp];
            itemGameObject.gameObject.SetActive(true);
        }
        else
            itemGameObject.gameObject.SetActive(false);

    }


}
