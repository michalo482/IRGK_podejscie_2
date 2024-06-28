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

    [Header("Ailment colors")] 
    [SerializeField] private Color[] chillColors;
    [SerializeField] private Color[] igniteColors;
    [SerializeField] private Color[] shockColors;

    private void Start()
    {
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        orginalMat = smr.material;
    }

    private IEnumerator FlashFx()
    {
        smr.material = hitMat;
        Color currentColor = smr.material.color;

        smr.material.color = Color.white;
        yield return new WaitForSeconds(.15f);
        smr.material.color = currentColor;

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

    public void IgniteFxFor(float seconds)
    {
        InvokeRepeating(nameof(IgniteColorsFx), 0, 0.3f);
        Invoke(nameof(CancelRedBlink), seconds);
    }
    
    public void ChillFxFor(float seconds)
    {
        InvokeRepeating(nameof(ChillColorsFx), 0, 0.1f);
        Invoke(nameof(CancelRedBlink), seconds);
    }
    
    public void ShockFxFor(float seconds)
    {
        InvokeRepeating(nameof(ShockColorsFx), 0, 0.3f);
        Invoke(nameof(CancelRedBlink), seconds);
    }
    private void IgniteColorsFx()
    {
        if (smr.material.color != igniteColors[0])
        {
            smr.material.color = igniteColors[0];
        }
        else
        {
            smr.material.color = igniteColors[1];
        }
    }
    
    private void ShockColorsFx()
    {
        if (smr.material.color != shockColors[0])
        {
            smr.material.color = shockColors[0];
        }
        else
        {
            smr.material.color = shockColors[1];
        }
    }
    
    private void ChillColorsFx()
    {
        if (smr.material.color != chillColors[0])
        {
            smr.material.color = chillColors[0];
        }
        else
        {
            smr.material.color = chillColors[1];
        }
    }

}
