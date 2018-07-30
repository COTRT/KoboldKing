using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class CraftSystem : MonoBehaviour
{

    [SerializeField]
    public int finalSlotPositionX;
    [SerializeField]
    public int finalSlotPositionY;
    [SerializeField]
    public int leftArrowPositionX;
    [SerializeField]
    public int leftArrowPositionY;
    [SerializeField]
    public int rightArrowPositionX;
    [SerializeField]
    public int rightArrowPositionY;
    [SerializeField]
    public int leftArrowRotation;
    [SerializeField]
    public int rightArrowRotation;

    public Image leftArrowImage;
    public Image rightArrowImage;
    public Image finalSlotImage;

    //List<CraftSlot> slots = new List<CraftSlot>();
    public List<ItemSlot> itemsInCraftSystem = new List<ItemSlot>();
    BlueprintDatabase blueprintDatabase;
    public List<Item> possibleItems = new List<Item>();

    Transform slots;
    Transform resultSlot;
    Transform leftArrow;
    Transform rightArrow;
    RectTransform leftRect;
    RectTransform rightRect;
    RectTransform resultRect;


    //PlayerScript PlayerstatsScript;

    public void Start()  
    {
        blueprintDatabase = (BlueprintDatabase)Resources.Load("BlueprintDatabase");
        //playerStatsScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        slots = transform.GetChild(1);
        resultSlot = transform.GetChild(3);
        leftArrow = transform.GetChild(4);
        rightArrow = transform.GetChild(5);
        leftRect = leftArrow.GetComponent<RectTransform>();
        rightRect = rightArrow.GetComponent<RectTransform>();
        resultRect = resultSlot.GetComponent<RectTransform>();
        finalSlotImage = resultSlot.GetComponent<Image>();
        leftArrowImage = leftArrow.GetComponent<Image>();
        Image image = rightArrow.GetComponent<Image>();
    }

    void Update()
    {
        ListWithItem();
    }


    public void BackToInventory()
    {
        foreach (ItemOnObject ccitem in itemsInCraftSystem)
        {
            PlayerInventory.Instance.mainInventory.AddItemToInventory(ccitem.Item.ID, ccitem.Item.Quantity);
            Destroy(ccitem.gameObject);
        }

        itemsInCraftSystem.Clear();
    }



    public void ListWithItem()
    {
        itemsInCraftSystem.Clear();
        possibleItems.Clear();

        for (int i = 0; i < slots.childCount; i++)
        {
            Transform trans = slots.GetChild(i);
            if (trans.childCount != 0)
            {
                itemsInCraftSystem.Add(trans.GetChild(0).GetComponent<ItemSlot>());
            }
        }

        foreach (var blueprint in blueprintDatabase.blueprints)
        {
            var ccItemIds = itemsInCraftSystem.Select(c => c.Item.ID).ToArray();
            if (blueprint.ingredients.All(ingredient => ccItemIds.Contains(ingredient)))
            {
                Item item = blueprint.finalItem;
                item.Quantity = blueprint.amountOfFinalItem;
                possibleItems.Add(item);
            }
        }

    }

    public bool RemoveItemIngredients(Item item)
    {
        var blueprint = blueprintDatabase.blueprints.Find(b => b.finalItem == item);
        var ingredientIndices = blueprint.ingredients.Select(
            (ingredient,indice) => new
            {
                inventoryIndex = itemsInCraftSystem.FindIndex(ccitem =>
                    ccitem.Item.ID == ingredient),
                ingredientIndex = indice
            }); //Search the items in the crafting system for all ingredients of Blueprint.
        if (ingredientIndices.Any(i => i.inventoryIndex == -1)) return false; //Kick out if any ingredient is missing
        foreach (var indexPair in ingredientIndices)
        {
            var ingItem = itemsInCraftSystem[indexPair.inventoryIndex];
            var blueprintAmount = blueprint.amount[indexPair.ingredientIndex]; //BAD, FIX
            if (ingItem.Item.Quantity > blueprintAmount)
            {
                ingItem.Item.Quantity -= blueprint.amount[indexPair.ingredientIndex]; //I dissaprove of this system so greatly
                //Actually, let me make a "TODO:  Change Blueprint Ingredient System" out of this.
            }
            else if (ingItem.Item.Quantity == blueprintAmount)
            {
                Destroy(ingItem.gameObject);
                itemsInCraftSystem.RemoveAt(indexPair.inventoryIndex);
            }
            else
            {
                return false;
            }
        }
        ListWithItem();
        return true;
    }

#if UNITY_EDITOR //Editor functions
    public void MirrorLeftArrowToRight()
    {
        rightArrowImage.sprite = leftArrowImage.sprite;
        rightArrowImage.color = leftArrowImage.color;
        rightArrowImage.material = leftArrowImage.material;
        rightArrowImage.type = leftArrowImage.type;
        rightArrowImage.fillCenter = leftArrowImage.fillCenter;
    }


    public void SetArrowSettings()
    {
        leftRect.localPosition = new Vector3(leftArrowPositionX, leftArrowPositionY, 0);
        rightRect.localPosition = new Vector3(rightArrowPositionX, rightArrowPositionY, 0);

        leftRect.eulerAngles = new Vector3(0, 0, leftArrowRotation);
        rightRect.eulerAngles = new Vector3(0, 0, rightArrowRotation);
    }

    public void SetPositionFinalSlot()
    {
        resultRect.localPosition = new Vector3(finalSlotPositionX, finalSlotPositionY, 0);
    }

    public int GetSizeX()
    {
        return (int)GetComponent<RectTransform>().sizeDelta.x;
    }

    public int GetSizeY()
    { 
        return (int)GetComponent<RectTransform>().sizeDelta.y;
    }
#endif
}
