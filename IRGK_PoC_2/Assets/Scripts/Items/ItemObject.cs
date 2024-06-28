using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ItemData itemData;
    [SerializeField] private Vector2 velocity;

   

    private void SetupVisuals()
    {
        if (itemData == null)
        {
            return;
        }
        
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "ItemObject" + itemData.itemName;
    }


    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;
        
        SetupVisuals();
        
    }

    public void PickUpItem()
    {
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
