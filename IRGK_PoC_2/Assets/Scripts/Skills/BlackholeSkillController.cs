using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;
    
    
    public float maxSize;
    public float growSpeed;
    public float shrinkSpeed;
    private bool canGrow = true;
    private bool canShrink;
    private float blackholeTimer;

    private bool canCreateHotKeys = true;
    private bool canAttack;
    private int amountOfAttacks = 3;
    private float cloneAttackCooldown = 0.3f;
    private float cloneAttackTimer;
    private bool playerCanDisapear = true;
    

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKeys = new List<GameObject>();

    public bool playerCanExitState { get; private set; }

    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeTimer)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeTimer;
    }
    
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;
            if (targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackholeAbility();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }
        
        CloneAttackLogic();
        
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale =
                Vector3.Lerp(transform.localScale, new Vector3(-1, -1, 0), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0)
        {
            return;
        }
        DestroyHotKyes();
        canAttack = true;
        canCreateHotKeys = false;
        if (playerCanDisapear)
        {
            playerCanDisapear = false;
            PlayerManager.instance.player.MakeTransparent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && canAttack && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = Random.Range(0, targets.Count);
            float xoffset;
            if (Random.Range(0, 100) > 50)
            {
                xoffset = 2f;
            }
            else
            {
                xoffset = -2f;
            }
            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xoffset, 0));
            amountOfAttacks--;
            //Debug.Log(amountOfAttacks);
            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackholeAbility", 1f);
            }
        }
    }

    private void FinishBlackholeAbility()
    {
        DestroyHotKyes();
        playerCanExitState = true;
        //PlayerManager.instance.player.ExitBlackhole();
        canShrink = true;
        canAttack = false;
    }

    private void DestroyHotKyes()
    {
        if (createdHotKeys.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < createdHotKeys.Count; i++)
        {
            Destroy(createdHotKeys[i]);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().FreezeTime(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().FreezeTime(true);

            CreateHotKey(other);
        }
    }

    private void CreateHotKey(Collider other)
    {

        if (keyCodeList.Count <= 0)
        {
            return;
        }

        if (!canCreateHotKeys)
        {
            return;
        }
        GameObject newHotKey = Instantiate(hotKeyPrefab, other.transform.position + new Vector3(0, 1, 0.5f),
            Quaternion.Euler(-90, 0, 0));
        createdHotKeys.Add(newHotKey);

        KeyCode chosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(chosenKey);

        BlackholeHotkeyController newHotKeyScript = newHotKey.GetComponent<BlackholeHotkeyController>();
            
        newHotKeyScript.SetupHotKey(chosenKey, other.transform, this);
    }
    
    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
