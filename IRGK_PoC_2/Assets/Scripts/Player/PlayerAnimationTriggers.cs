using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player Player => GetComponentInParent<Player>();
    
    private void AnimationTrigger()
    {
        Player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider[] colliders = Physics.OverlapSphere(Player.attackCheck.position, Player.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }
}
