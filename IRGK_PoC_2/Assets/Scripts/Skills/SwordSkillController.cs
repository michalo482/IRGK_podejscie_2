using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
   [SerializeField] private float returnSpeed = 12f;
   
   private Animator anim;
   private Rigidbody rb;
   private BoxCollider collider;
   private Player player;

   private bool canRotate = true;
   private bool isReturning = false;
   
   private void Awake()
   {
      anim = GetComponentInChildren<Animator>();
      rb = GetComponent<Rigidbody>();
      collider = GetComponent<BoxCollider>();
   }

   private void Update()
   {
      if (canRotate)
      {
         transform.right = rb.velocity;
      }

      if (isReturning)
      {
         transform.position = Vector2.MoveTowards(transform.position, SkillManager.instance.sword.dotsParent.position, returnSpeed * Time.deltaTime);
         if (Vector2.Distance(transform.position, SkillManager.instance.sword.dotsParent.position) < 1)
         {
            player.ClearSword();
         }
      }
   }

   public void SetUpSword(Vector2 dir, float mass, Player _player)
   {
      player = _player;
      
      rb.velocity = dir;
      rb.mass = mass;
   }

   public void ReturnSword()
   {
      rb.isKinematic = false;
      transform.parent = null;
      isReturning = true;
   }

   private void OnTriggerEnter(Collider other)
   {
      canRotate = false;
      collider.enabled = false;

      rb.isKinematic = true;
      rb.constraints = RigidbodyConstraints.FreezeAll;

      transform.parent = other.transform;
   }
}
