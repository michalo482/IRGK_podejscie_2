//using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CloneSkillController : MonoBehaviour
{
    [SerializeField] private float colorLoosingSpeed;
    private Animator anim;
    private float cloneTimer;

    private SkinnedMeshRenderer smr;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 0.8f;

    private Transform closestEnemy;

    private void Awake()
    {
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            Color color = smr.material.color;
            color.a = Mathf.Max(0, color.a - (Time.deltaTime * colorLoosingSpeed));
            smr.material.color = color;
            if (smr.material.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetUpClone(Transform newTransform, float cloneDuration, bool canAttack)
    {
        if (canAttack)
        {
            anim.SetInteger("AttackNumber", 1);
        }
        transform.position = newTransform.position;
        //transform.rotation = newTransform.rotation;
        FaceClosestTarget();
        cloneTimer = cloneDuration;
        
       
    }
    
    private void AnimationTrigger()
    {
        cloneTimer = -0.1f;
    }

    private void AttackTrigger()
    {
        Collider[] colliders = Physics.OverlapSphere(attackCheck.position, attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }

    private void FaceClosestTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 25);
        float closestDistance = Mathf.Infinity;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = hit.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            if (transform.position.x < closestEnemy.transform.position.x)
            {
                transform.Rotate(0,180,0);
            }
        }
    }
    
}
