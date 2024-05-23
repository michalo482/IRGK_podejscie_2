using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Info")] 
    [SerializeField] private bool canAttack;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration = 1.5f;

    public void CreateClone(Transform clonePosition)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetUpClone(clonePosition, cloneDuration, canAttack);
    }
}
