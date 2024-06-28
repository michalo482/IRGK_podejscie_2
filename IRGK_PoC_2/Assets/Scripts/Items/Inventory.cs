using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour
{
    
    public static Inventory instance;

    public List<ItemData> startingEquipment;
    
    [FormerlySerializedAs("inventoryItems")] public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    [Header("Inventory UI")] 
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;

    private UI_ItemSlot[] _inventoryItemSlots;
    private UI_ItemSlot[] _stashItemSlots;
    private UI_EquipmentSlot[] _equipmentSlots;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        
        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        _inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        _stashItemSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        _equipmentSlots = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        for (int i = 0; i < startingEquipment.Count; i++)
        {
            AddItem(startingEquipment[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipItem(ItemData item)
    {
        ItemData_Equipment newEquipment = item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);
        ItemData_Equipment oldEquipment = null; 

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> stuff in equipmentDictionary)
        {
            if (stuff.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = stuff.Key;    
            }
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }
        
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();
        
        RemoveItem(item);
        
        UpdateUISlot();
    }

    public void UnequipItem(ItemData_Equipment itemToDelete)
    {
        if (equipmentDictionary.TryGetValue(itemToDelete, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToDelete);
            itemToDelete.RemoveModifiers();
        }
    }

    private void UpdateUISlot()
    {
        for (int i = 0; i < _equipmentSlots.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> stuff in equipmentDictionary)
            {
                if (stuff.Key.equipmentType == _equipmentSlots[i].slotType)
                {
                    _equipmentSlots[i].UpdateSlot(stuff.Value);    
                }
            }
        }
        
        for (int i = 0; i < _inventoryItemSlots.Length; i++)
        {
            _inventoryItemSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < _stashItemSlots.Length; i++)
        {
            _stashItemSlots[i].CleanUpSlot();
        }
        
        
        for (int i = 0; i < inventory.Count; i++)
        {
            _inventoryItemSlots[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            _stashItemSlots[i].UpdateSlot(stash[i]);
        }
    }

    public void AddItem(ItemData item)
    {

        if (item.itemType == ItemType.Equipment)
        {
            AddToInventory(item);
        }
        else if (item.itemType == ItemType.Material)
        {
            AddToStash(item);
        }
        
        UpdateUISlot();
    }

    private void AddToStash(ItemData item)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            stash.Add(newItem);
            stashDictionary.Add(item, newItem);
        }
    }

    private void AddToInventory(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            inventory.Add(newItem);
            inventoryDictionary.Add(item, newItem);
        }
    }

    public void RemoveItem(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(item);
            }
            else
            {
                value.RemoveStack();
            }
        }
        
        if (stashDictionary.TryGetValue(item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }
        
        UpdateUISlot();
    }

    public bool CanBeCrafted(ItemData_Equipment itemToCraft, List<InventoryItem> requiredMaterial)
    {

        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        
        for (int i = 0; i < requiredMaterial.Count; i++)
        {
            if (stashDictionary.TryGetValue(requiredMaterial[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < requiredMaterial[i].stackSize)
                {
                    Debug.Log("nie mam materialow");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);    
                }
            }
            else
            {
                Debug.Log("nie mam materialow");
                return false;
            }
        }

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
            Debug.Log("usunalem " + materialsToRemove[i].data.name);
        }
        
        AddItem(itemToCraft);
        Debug.Log("zrobilem " + itemToCraft.name);
        
        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemData_Equipment GetEquipment(EquipmentType type)
    {
        ItemData_Equipment equippedItemData = null;
        
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> stuff in equipmentDictionary)
        {
            if (stuff.Key.equipmentType == type)
            {
                equippedItemData = stuff.Key;
            }
        }

        return equippedItemData;
    }
}
