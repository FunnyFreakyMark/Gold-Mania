using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<Weapon> WeaponList = new List<Weapon>();

    void Awake()
    {

    }

    void FillWeaponList()
    {
        LaserGun _laserGun = new LaserGun();
        _laserGun.Name = "Awesome Lasergun of the gods";
        WeaponList.Add(_laserGun);
    }
}

public class Weapon
{
    public LayerMask RayMask;

    public string Name;
    public float Damage;
    public float ChargeTime;
    public int FireRate;

    public virtual void HoldDownMouse(LineRenderer _aimLine, GameObject _playerObject)
    {

    }

    public virtual void MouseUp(LineRenderer _aimLine, GameObject _playerObject)
    {

    }

    public virtual void MouseDown(LineRenderer _aimLine, GameObject _playerObject)
    {

    }
}

public class LaserGun : Weapon
{
    float ChargedDamage = 40;
    Color LineColor;

    public void InitliazeSettings(string _name)
    {
        Name = _name;
        Damage = 100;
        ChargeTime = 5;
        FireRate = 0;
        LineColor = Color.cyan;
    }

    public override void HoldDownMouse(LineRenderer _aimLine, GameObject _playerObject)
    {
        _aimLine.enabled = true;
        Vector2 _rayTarget = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 _rayOrigin = _playerObject.transform.position;

        RaycastHit2D[] _hits = Physics2D.RaycastAll(_rayOrigin, _rayTarget - _rayOrigin, Mathf.Infinity);
        RaycastHit2D _hit = new RaycastHit2D();

        foreach (var h in _hits)
        {
            if (h.collider.tag != "Player")
                _hit = h;
        }

        _aimLine.SetPosition(0, _rayOrigin);
        _aimLine.SetPosition(1, _hit.collider != null ? _hit.point : (_rayTarget - _rayOrigin) * Mathf.Infinity);

        _aimLine.widthMultiplier = _aimLine.widthMultiplier > 0.01f ? _aimLine.widthMultiplier - Time.deltaTime / ChargeTime : 0.01f;
        ChargedDamage = ChargedDamage < Damage ? ChargedDamage + Time.deltaTime * 50 : Damage;
    }

    public override void MouseUp(LineRenderer _aimLine, GameObject _playerObject)
    {
        if (_aimLine.widthMultiplier == 0.01f)
        {
            Vector2 _rayTarget = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            Vector2 _rayOrigin = _playerObject.transform.position;

            RaycastHit2D[] _hits = Physics2D.RaycastAll(_rayOrigin, _rayTarget - _rayOrigin, Mathf.Infinity);
            RaycastHit2D _hit = new RaycastHit2D();

            foreach (var h in _hits)
            {
                if (h.collider != null && h.collider.tag != "Player")
                {
                    _hit = h;

                    if (h.collider.GetComponent<Enemy>() != null)
                    {
                        Enemy _enemy = h.collider.GetComponent<Enemy>();
                        _enemy.TakeDamage(ChargedDamage);
                    }
                }
            }

            GameManager.Instance.StartCoroutine(ILaserEffect(_rayOrigin, _hit.point));

        }
        _aimLine.enabled = false;
    }

    public override void MouseDown(LineRenderer _aimLine, GameObject _playerObject)
    {
        ChargedDamage = 40;
        _aimLine.enabled = true;
        _aimLine.startColor = LineColor;
        _aimLine.endColor = LineColor;
        _aimLine.widthMultiplier = 0.2f;
    }

    IEnumerator ILaserEffect(Vector2 _origin, Vector2 _target)
    {
        GameObject _effectObject = new GameObject("Laser Line Effect");
        LineRenderer _effectLine = _effectObject.AddComponent<LineRenderer>();
        _effectLine.material = new Material(Shader.Find("Sprites/Default"));
        _effectLine.sortingLayerName = "ForegroundEffect";

        _effectLine.positionCount = 2;
        _effectLine.SetPosition(0, _origin);
        _effectLine.SetPosition(1, _target);
        _effectLine.widthMultiplier = 0.2f;

        float _pulseTime = 0.05f;

        _effectLine.startColor = Color.red;
        _effectLine.endColor = Color.red;
        _effectLine.widthMultiplier = 0.2f;
        yield return new WaitForSeconds(_pulseTime);
        _effectLine.startColor = Color.white;
        _effectLine.endColor = Color.white;
        _effectLine.widthMultiplier = 0.3f;
        yield return new WaitForSeconds(_pulseTime);
        _effectLine.startColor = Color.red;
        _effectLine.endColor = Color.red;
        _effectLine.widthMultiplier = 0.2f;
        yield return new WaitForSeconds(_pulseTime);
        _effectLine.startColor = Color.white;
        _effectLine.endColor = Color.white;
        _effectLine.widthMultiplier = 0.3f;
        yield return new WaitForSeconds(_pulseTime);
        _effectLine.startColor = Color.red;
        _effectLine.endColor = Color.red;
        _effectLine.widthMultiplier = 0.2f;
        yield return new WaitForSeconds(_pulseTime);
        _effectLine.startColor = Color.white;
        _effectLine.endColor = Color.white;
        _effectLine.widthMultiplier = 0.3f;
        yield return new WaitForSeconds(_pulseTime);
        _effectLine.startColor = Color.red;
        _effectLine.endColor = Color.red;
        _effectLine.widthMultiplier = 0.2f;
        yield return new WaitForSeconds(_pulseTime);
        _effectLine.startColor = Color.white;
        _effectLine.endColor = Color.white;
        _effectLine.widthMultiplier = 0.3f;
        yield return new WaitForSeconds(_pulseTime);

        Object.Destroy(_effectObject);

        yield return null;
    }
}
