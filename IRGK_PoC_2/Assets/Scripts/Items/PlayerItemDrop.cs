using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player drop")] 
    [SerializeField] private float chanceToDropEquipmentOnDeath;
    [SerializeField] private float chanceToDropMaterialsOnDeath;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;
        
        List<InventoryItem> currentEq = inventory.GetEquipmentList();
        List<InventoryItem> itemsToUnequipped = new List<InventoryItem>();

        List<InventoryItem> currentStash = inventory.GetStashList();
        List<InventoryItem> materialsToLoose = new List<InventoryItem>();
        
        
        foreach (InventoryItem item in currentEq)
        {
            if (Random.Range(0, 100) <= chanceToDropEquipmentOnDeath)
            {
                DropItem(item.data);
                itemsToUnequipped.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequipped.Count; i++)
        {
            inventory.UnequipItem(itemsToUnequipped[i].data as ItemData_Equipment);
            //inventory.RemoveItem(itemsToUnequipped[i].data);
        }

        foreach (InventoryItem item in currentStash)
        {
            if (Random.Range(0, 100) <= chanceToDropMaterialsOnDeath)
            {
                DropItem(item.data);
                materialsToLoose.Add(item);
            }
        }

        for (int i = 0; i < materialsToLoose.Count; i++)
        {
            inventory.RemoveItem(materialsToLoose[i].data);
        }
        
    }
}
