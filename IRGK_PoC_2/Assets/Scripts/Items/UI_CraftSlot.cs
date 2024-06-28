using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    private void OnValidate()
    {
        UpdateSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment craftData = item.data as ItemData_Equipment;

        Inventory.instance.CanBeCrafted(craftData, craftData.craftingMaterials);
        
    }
}
