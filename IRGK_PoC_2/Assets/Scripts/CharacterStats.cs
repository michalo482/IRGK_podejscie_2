using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFx _fx;
    
    [Header("Major Stats")]
    public Stat strength;  // damage and crit power
    public Stat agility;   // evasion and crit chance
    public Stat intellect; // magic damage and magic res
    public Stat vitality;  // hp and armor
    
    [Header("Off Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;
    
    
    [Header("Def Stats")]
    public Stat maxHp;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic Stats")] 
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

    [SerializeField] private float ailmentsDuration = 4f;

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;
    
    
    private float igniteDamageTick = 0.3f;
    private float igniteDamageTimer;
    private int igniteDamage;

    public bool isDead { get; private set; }
    
    
    public int currentHp;

    public System.Action onHpChange;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHp = GetMaxHp();
        _fx = GetComponent<EntityFx>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        
        igniteDamageTimer -= Time.deltaTime;
        if (ignitedTimer < 0)
        {
            isIgnited = false;
        }

        if (chilledTimer < 0)
        {
            isChilled = false;
        }

        if (shockedTimer < 0)
        {
            isShocked = false;
        }

        ApplyIgniteDamage();
    }

    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0 && isIgnited)
        {
            //Debug.Log("ogieeeen " + igniteDamage);
            DecreaseHpBy(igniteDamage);
            if (currentHp < 0 && !isDead)
            {
                //isIgnited = false;
                Die();
            }
            igniteDamageTimer = igniteDamageTick;
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (EvasionCheck(_targetStats))
        {
            return;
        }

        
        int totalDamage = damage.GetValue() + strength.GetValue();
        if (CritCheck())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }
        totalDamage = ArmorCheck(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(_targetStats);
    }

    private int ArmorCheck(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue();
        }
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool CritCheck()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) <= totalCritChance)
        {
            return true;
        }

        return false;
    }

    private bool EvasionCheck(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            totalEvasion += 20;
        }

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intellect.GetValue();
        totalMagicalDamage = ResistanceCheck(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            return;
        }

        AtemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }

    private void AtemptToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage,
        int _lightningDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < 0.33f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < 0.33f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < 0.33f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));
        }
        
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private int ResistanceCheck(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intellect.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked)
        {
            return;
        }

        if (_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;
            _fx.IgniteFxFor(ailmentsDuration);
        }

        if (_chill)
        {
            isChilled = _chill;
            chilledTimer = ailmentsDuration;
            float slowPercentage = 0.3f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            _fx.ChillFxFor(ailmentsDuration);
        }

        if (_shock)
        {
            isShocked = _shock;
            shockedTimer = ailmentsDuration;
            _fx.ShockFxFor(ailmentsDuration);
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    
    public virtual void TakeDamage(int _damage)
    {
        DecreaseHpBy(_damage);
        GetComponent<Entity>().DamageEffects();
        _fx.StartCoroutine("FlashFx");
        if (currentHp <= 0 && !isDead)
        {
            Die();
        }
    }

    protected virtual void DecreaseHpBy(int _damage)
    {
        currentHp -= _damage;
        if (onHpChange != null)
            onHpChange();
    }

    protected virtual void Die()
    {
        isDead = true;
    }

    private int CalculateCritDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHp()
    {
        return maxHp.GetValue() + (vitality.GetValue() * 5);
    }
}
