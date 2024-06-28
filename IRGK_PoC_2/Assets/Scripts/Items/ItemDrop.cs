using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int amountOfDrops;
    [SerializeField] private ItemData[] possibleDrops;
    public List<ItemData> dropList = new List<ItemData>();
    
    [SerializeField] private GameObject dropPrefab;
    //[SerializeField] private ItemData itemData;

    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrops.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrops[i].dropChance)
            {
                dropList.Add(possibleDrops[i]);
            }
        }

        if (dropList.Count > 0)
        {
            for (int i = 0; i < amountOfDrops; i++)
            {
                if (dropList.Count <= 0)
                {
                    return;
                }
                ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];
                dropList.Remove(randomItem);
                DropItem(randomItem);
            }
        }
    }
    
    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-2, 2), Random.Range(3, 5));
        
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
