using UnityEngine;
using System.Collections;
public class PickUpItem : MonoBehaviour
{
    public Item item;
    private Inventory _inventory;
    private GameObject _player;
    // Use this for initialization

    void Start()
    {
        _inventory = PlayerInventory.Instance.mainInventory;
        _player = PlayerInventory.Instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float distance = Vector3.Distance(transform.position, _player.transform.position);

            if (distance <= 3)
            {
                if (_inventory.AddItemToInventory(item.ID, item.Quantity, false) != 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}