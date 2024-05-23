using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SwordSkill : Skill
{
    [Header("SwordSkill Info")] 
    [SerializeField] private GameObject swordPrefab;
    [FormerlySerializedAs("launchDirection")] [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private Transform handPosition;

    private Vector2 finalDirection;

    [Header("Aim Dots")] 
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] public Transform dotsParent;

    private GameObject[] dots;
    
    private Camera mainCamera;
    [SerializeField] private Transform playerTransform;

    private void Awake()
    {
        mainCamera = Camera.main;
        //playerTransform = player.transform;
    }


    protected override void Start()
    {
        base.Start();
        GenerateDots();
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDirection = new Vector2(AimDirection().normalized.x * launchForce.x,
                AimDirection().normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = PositionDots(i * spaceBetweenDots);
            }
        }
    }
    
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, handPosition.position, transform.rotation);
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();
        
        newSwordScript.SetUpSword(finalDirection, swordGravity, player);
        
        player.AssignNewSword(newSword);
        
        DotsActivation(false);
    }

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = playerTransform.position;
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction.normalized;
    }

    public void DotsActivation(bool isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, dotsParent.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector3 PositionDots(float t)
    {
        Vector2 aimDirection = AimDirection();
        Vector2 initialPosition = dotsParent.position;

        Vector3 position = (Vector3)initialPosition + new Vector3(aimDirection.x * launchForce.x,
            aimDirection.y * launchForce.y) * t + Physics.gravity * (0.17f * swordGravity * (t * t));
        return position;
    }
}
