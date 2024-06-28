using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IceAndFireEffect", menuName = "Data/Item Effect/Ice and Fire")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private float xVelocity;

    public override void ExecuteEffect(Transform enemyPosition)
    {
        Player player = PlayerManager.instance.player;
        bool thirdAttack = player.PrimaryAttackState._comboCounter == 2;

        if (thirdAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, enemyPosition.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody>().velocity = new Vector3(xVelocity * player.FacingDirection, 0, 0);
            
            Destroy(newIceAndFire, 4);
        }

    }
}
