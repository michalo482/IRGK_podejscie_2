using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemText;

    public InventoryItem item;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateSlot(InventoryItem newItem)
    {
        item = newItem;
        
        _itemImage.color = Color.white;
        if (item != null)
        {
            _itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                _itemText.text = item.stackSize.ToString();
            }
            else
            {
                _itemText.text = "";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CleanUpSlot()
    {
        item = null;
        _itemImage.sprite = null;
        _itemImage.color = Color.clear;
        _itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }
        
        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(item.data);
        }
    }
}
