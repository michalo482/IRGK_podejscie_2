using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThunderStrikeEffect", menuName = "Data/Item Effect/Thunder Strike")]
public class ThunderStrike_Effect : ItemEffect
{

    [SerializeField] private GameObject thunderPrefab;
    public override void ExecuteEffect(Transform enemyPosition)
    {
        GameObject newThunderstrike = Instantiate(thunderPrefab, enemyPosition.position, Quaternion.identity);
        
        Destroy(newThunderstrike, 1);
    }
}
