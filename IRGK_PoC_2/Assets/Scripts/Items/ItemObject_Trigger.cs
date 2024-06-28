using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{

    private ItemObject MyItem => GetComponentInParent<ItemObject>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            if (other.GetComponent<CharacterStats>().isDead)
            {
               return; 
            }
            MyItem.PickUpItem();
        }
    }
}
