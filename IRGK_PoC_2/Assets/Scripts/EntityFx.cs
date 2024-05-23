using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFx : MonoBehaviour
{
    private SkinnedMeshRenderer smr;
    [Header("Flash Fx")] 
    [SerializeField] private Material hitMat;
    private Material orginalMat;

    private void Start()
    {
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        orginalMat = smr.material;
    }

    private IEnumerator FlashFx()
    {
        smr.material = hitMat;

        yield return new WaitForSeconds(.15f);

        smr.material = orginalMat;
    }

    private void RedColorBlink()
    {
        if (smr.material.color != Color.white)
        {
            smr.material.color = Color.white;
        }
        else
        {
            smr.material.color = Color.red;
        }
    }

    private void CancelRedBlink()
    {
        CancelInvoke();
        smr.material.color = Color.white;
    }
}
