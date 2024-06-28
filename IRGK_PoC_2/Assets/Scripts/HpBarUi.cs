using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarUi : MonoBehaviour
{
    private Entity _entity;
    private CharacterStats _stats;
    private RectTransform _rectTransform;
    private Slider _slider;
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _entity = GetComponentInParent<Entity>();
        _slider = GetComponentInChildren<Slider>();
        _stats = GetComponentInParent<CharacterStats>();

        _entity.onFlipped += FlipUi;
        _stats.onHpChange += UpdateHpUi;
        
        UpdateHpUi();
    }

    private void UpdateHpUi()
    {
        _slider.maxValue = _stats.GetMaxHp();
        _slider.value = _stats.currentHp;
    }

    private void FlipUi()
    {
        _rectTransform.Rotate(0, 180, 0);
    }

    private void OnDisable()
    {
        _entity.onFlipped -= FlipUi;
        _stats.onHpChange -= UpdateHpUi;
    }
}
