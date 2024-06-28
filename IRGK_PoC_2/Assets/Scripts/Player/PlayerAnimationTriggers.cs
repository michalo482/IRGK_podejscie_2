using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider[] colliders = Physics.OverlapSphere(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats target = hit.GetComponent<EnemyStats>();

                if (target != null)
                {
                    player.Stats.DoDamage(target);
                }

                //Inventory.instance.GetEquipment(EquipmentType.Weapon).ExecuteItemEffect(target.transform);
                
                ItemData_Equipment itemDataEquipment = Inventory.instance.GetEquipment(EquipmentType.Weapon);
                if (itemDataEquipment != null)
                {
                    itemDataEquipment.ExecuteItemEffect(target.transform);
                }
                
                //hit.GetComponent<Enemy>().Damage();
                //hit.GetComponent<CharacterStats>().TakeDamage(player.Stats.damage.GetValue());
                //Debug.Log(player.Stats.damage.GetValue());
            }
        }
    }
    
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
