using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public enum State { Alive, Dead }

    [Header("Settings: ")]
    public State CurrentState = State.Alive;
    public float Health = 100;
    public int Gold;
    public int Level = 1;
    public float Experience;
    public float ExperienceCap = 10;
    public GameObject GameObject;
    [Range(10,100)]
    public int SlowAmount;
    float MovementMultiplier = 1;

    [Header("Shooting: ")]
    public LayerMask HitMask;
    public LineRenderer AimLine;
    public Weapon CurrentWeapon;

    [Header("References: ")]
    public CinemachineVirtualCamera VirtualCamera;
    public GameObject ObjectPrefab;
    public InterfaceManager UI;


    void Awake()
    {
        Instance = Instance == null ? this : Instance;

        UI.UpdateInterface(this);

        LaserGun _laserGun = new LaserGun();
        _laserGun.InitliazeSettings("Awesome Lasergun of the gods");
        CurrentWeapon = _laserGun;

        GameObject = Instantiate(ObjectPrefab);
        GameObject.tag = "Player";
        VirtualCamera.m_Follow = GameObject.transform;

        AimLine = GameObject.GetComponent<LineRenderer>();
    }

    void Update()
    {
        HandleUserInput();
    }

    void HandleUserInput()
    {
        #region Movement
        GameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * MovementMultiplier;

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            MovementMultiplier = 7.5f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            MovementMultiplier = 5;
        }

        float _mouseScrollY = Input.mouseScrollDelta.y /5;

        if (_mouseScrollY < 0)
            VirtualCamera.m_Lens.OrthographicSize = VirtualCamera.m_Lens.OrthographicSize < 12 ? VirtualCamera.m_Lens.OrthographicSize - _mouseScrollY : 12;

        if (_mouseScrollY > 0)
            VirtualCamera.m_Lens.OrthographicSize = VirtualCamera.m_Lens.OrthographicSize > 2 ? VirtualCamera.m_Lens.OrthographicSize - _mouseScrollY : 2;

        #endregion

        #region Shooting
        if (Input.GetMouseButton(0))
        {
            CurrentWeapon.HoldDownMouse(AimLine, GameObject);
        }
        if(Input.GetMouseButtonUp(0))
        {
            CurrentWeapon.MouseUp(AimLine, GameObject);
        }
        if(Input.GetMouseButtonDown(0))
        {
            CurrentWeapon.MouseDown(AimLine, GameObject);
        }
        #endregion
    }

    void AddXP(float _amount)
    {
        Experience += _amount;
        if (Experience >= ExperienceCap * Level)
        {
            Experience = Experience - ExperienceCap * Level;
            Level++;
            UI.UpdateLevel(this);
        }
        UI.UpdateExperience(this);
    }
    void AddGold(int _amount)
    {
        Gold += _amount;
        UI.UpdateGold(this);
    }

    public void TakeDamage(float _damage)
    {
        Health = Health - _damage > 0 ? Health - _damage : 0;
        UI.UpdateHealth(this);

        if (Health <= 0)
            Die();
    }

    void Die()
    {
        GameManager.Instance.GameOver();
    }

    public void GainReward(int _gold, float _experience)
    {
        AddGold(_gold);
        AddXP(_experience);
    }
}
