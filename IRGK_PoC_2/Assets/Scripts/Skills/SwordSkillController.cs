
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
   private float returnSpeed = 12f;
   
   private Animator anim;
   private Rigidbody rb;
   private BoxCollider boxCollider;
   private Player player;

   private bool canRotate = true;
   private bool isReturning = false;

   private float freezeTimeDuration;

   [Header("Pierce Info")] 
   [SerializeField] private int amountOfPierces;

   [Header("Bounce Info")]
   private float bounceSpeed = 20f;
   private bool isBouncing;
   private int amountOfBounces;
   private List<Transform> enemyTargets;
   private int targetIndex;

   [Header("Spin Info")] 
   private float maxTravelDistance;
   private float spinDuration;
   private float spinTimer;
   private bool wasStopped;
   private bool isSpinning;

   private float hitTimer;
   private float hitCooldown;

   private float spinDirection;
   
   private void Awake()
   {
      anim = GetComponentInChildren<Animator>();
      rb = GetComponent<Rigidbody>();
      boxCollider = GetComponent<BoxCollider>();
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

      BounceLogic();
      SpinLogic();
   }

   private void SpinLogic()
   {
      if (isSpinning)
      {
         if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
         {
            StopWhenSpinning();
         }

         if (wasStopped)
         {
            transform.position = Vector3.MoveTowards(transform.position,
               new Vector3(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);
            
            spinTimer -= Time.deltaTime;
            
            if (spinTimer < 0)
            {
               isReturning = true;
               isSpinning = false;
            }

            hitTimer -= Time.deltaTime;
            
            if (hitTimer < 0)
            {
               hitTimer = hitCooldown;
               Collider[] colliders = Physics.OverlapSphere(transform.position, 1);
               foreach (var hit in colliders)
               {
                  if (hit.GetComponent<Enemy>() != null)
                  {
                     SwordDamage(hit.GetComponent<Enemy>());
                  }
               }
            }
         }
      }
   }

   private void StopWhenSpinning()
   {
      wasStopped = true;
      rb.constraints = RigidbodyConstraints.FreezePosition;
      spinTimer = spinDuration;
   }

   private void BounceLogic()
   {
      if (isBouncing && enemyTargets.Count > 0)
      {
         transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bounceSpeed * Time.deltaTime);
         if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < 0.1f)
         {
            SwordDamage(enemyTargets[targetIndex].GetComponent<Enemy>());
            targetIndex++;
            amountOfBounces--;

            if (amountOfBounces <= 0)
            {
               isBouncing = false;
               isReturning = true;
            }

            if (targetIndex >= enemyTargets.Count)
            {
               targetIndex = 0;
            }
         }
      }
   }

   private void DestroySword()
   {
      Destroy(gameObject);
   }

   public void SetUpSword(Vector2 dir, float mass, Player _player, float _freezeTimeDuration, float _returnSpeed)
   {
      player = _player;
      freezeTimeDuration = _freezeTimeDuration;
      returnSpeed = _returnSpeed;
      rb.velocity = dir;
      rb.mass = mass;
      
      spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);
      
      Invoke("DestroySword", 7f);
   }

   public void SetupBounce(bool isBouncy, int amountOfBounces, float _bounceSpeed)
   {
      this.isBouncing = isBouncy;
      this.amountOfBounces = amountOfBounces;
      bounceSpeed = _bounceSpeed;
      enemyTargets = new List<Transform>();
   }

   public void SetupPierce(int _amountOfPierces)
   {
      this.amountOfPierces = _amountOfPierces;
   }

   public void SetupSpinning(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
   {
      isSpinning = _isSpinning;
      maxTravelDistance = _maxTravelDistance;
      spinDuration = _spinDuration;
      hitCooldown = _hitCooldown;
   }

   public void ReturnSword()
   {
      rb.constraints = RigidbodyConstraints.FreezeAll;
      //rb.isKinematic = false;
      transform.parent = null;
      isReturning = true;
   }

   private void OnTriggerEnter(Collider other)
   {
      if (isReturning)
      {
         return;
      }

      if (other.GetComponent<Enemy>() != null)
      {
         Enemy enemy = other.GetComponent<Enemy>();
         SwordDamage(enemy);
      }
      
      other.GetComponent<Enemy>()?.DamageEffects();

      SetupTargetsForBounce(other);
      Stuck(other);
   }

   private void SwordDamage(Enemy enemy)
   {
      player.Stats.DoDamage(enemy.GetComponent<CharacterStats>());
      enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);

      ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

      if (equipedAmulet != null)
      {
         equipedAmulet.ExecuteItemEffect(enemy.transform);
      }
   }

   private void SetupTargetsForBounce(Collider other)
   {
      if (other.GetComponent<Enemy>() != null)
      {
         if (isBouncing && enemyTargets.Count <= 0)
         {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 20);
            foreach (var hit in colliders)
            {
               if (hit.GetComponent<Enemy>() != null)
               {
                  enemyTargets.Add(hit.transform);
               }
            }
         }
      }
   }

   private void Stuck(Collider other)
   {
      if (amountOfPierces > 0 && other.GetComponent<Enemy>() != null)
      {
         amountOfPierces--;
         return;
      }

      if (isSpinning)
      {
         StopWhenSpinning();
         return;
      }
      canRotate = false;
      boxCollider.enabled = false;

      rb.isKinematic = true;
      rb.constraints = RigidbodyConstraints.FreezeAll;
      if (isBouncing && enemyTargets.Count > 0)
      {
         return;
      }
      transform.parent = other.transform;
   }
}
