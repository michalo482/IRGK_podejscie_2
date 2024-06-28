using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackholeHotkeyController : MonoBehaviour
{

    private MeshRenderer mr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform enemTransform;
    private BlackholeSkillController blackhole;

    public void SetupHotKey(KeyCode _myHotKey, Transform _myEnemy, BlackholeSkillController _myBlackhole)
    {
        mr = GetComponent<MeshRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        enemTransform = _myEnemy;
        blackhole = _myBlackhole;
        
        myHotKey = _myHotKey;
        myText.text = myHotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            //Debug.Log("Hotkej to " + myHotKey);
            blackhole.AddEnemyToList(enemTransform);
            
            myText.color = Color.clear;
            mr.material.color = Color.clear;
        }
    }
}
