using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    [Header("Settings: ")]
    public Color DefaultTextColor;
    public Color ActivatedTextColor;

    public AnimationCurve TextCurve;
    public AnimationCurve ImageCurve;

    [Header("References: ")]
    public Text Gold;
    public Text Health;
    public Text Level;
    public Image XPBar;

    Coroutine xpBarFillRoutine;
    Coroutine goldCoroutine;
    Coroutine healthCoroutine;
    Coroutine levelCoroutine;

    public void UpdateInterface(Player _player)
    {
        Health.text = "Health: " + _player.Health;
        Gold.text = "Coins: " + ConvertNumbers(_player.Gold);
        Level.text = "Level: " + ConvertNumbers(_player.Level);
        float xpFillValue = 1 / (_player.ExperienceCap * _player.Level) * _player.Experience;
        XPBar.fillAmount = xpFillValue;
    }

    public void UpdateHealth(Player _player)
    {
        Health.text = "Health: " + _player.Health;

        if (healthCoroutine != null)
            StopCoroutine(healthCoroutine);
        healthCoroutine = StartCoroutine(ILerpTextAmount(Health, Color.red, 1));
    }
    public void UpdateGold(Player _player)
    {
        Gold.text = "Gold: " + ConvertNumbers(_player.Gold);

        if (goldCoroutine != null)
            StopCoroutine(goldCoroutine);
        goldCoroutine = StartCoroutine(ILerpTextAmount(Gold, Color.yellow, 0.5f));
    }
    public void UpdateLevel(Player _player)
    {
        Level.text = "Level: " + ConvertNumbers(_player.Level);

        if (levelCoroutine != null)
            StopCoroutine(levelCoroutine);
        levelCoroutine = StartCoroutine(ILerpTextAmount(Level, Color.cyan, 0.5f));
    }
    public void UpdateExperience(Player _player)
    {
        float xpFillValue = 1 / (_player.ExperienceCap * _player.Level) * _player.Experience;
        //XPBar.fillAmount = xpFillValue < XPBar.fillAmount ? xpFillValue : XPBar.fillAmount;

        if (xpBarFillRoutine != null)
            StopCoroutine(xpBarFillRoutine);
        xpBarFillRoutine = StartCoroutine(ILerpImageFillAmount(XPBar.fillAmount, xpFillValue, XPBar, 1));
    }

    IEnumerator ILerpImageFillAmount(float a, float b, Image _image, float _duration)
    {
        float _tweenTimeKey = 0;

        while (_tweenTimeKey < 1)
        {
            _tweenTimeKey += Time.deltaTime / _duration;

            float _tweenTimeValue = ImageCurve.Evaluate(_tweenTimeKey);
            LerpImageFillAmount(a, b, _image, _tweenTimeValue);
            yield return null;
        }
    }

    void LerpImageFillAmount(float a, float b, Image _image, float _time)
    {
        if (b < a)
        {
            if (_time < 0.5f)
                _image.fillAmount = Mathf.Lerp(a, 1, _time * 2);
            else
                _image.fillAmount = Mathf.Lerp(0, b, -0.5f + _time);
        }
        else
            _image.fillAmount = Mathf.Lerp(a, b, _time);
    }

    IEnumerator ILerpTextAmount(Text _text, Color _color, float _duration)
    {
        float _tweenTimeKey = 0;

        Vector3 _scaleA = Vector3.one * 0.3f;
        Vector3 _scaleB = _scaleA * 1.5f;
        Color _colorA = _text.color;

        while (_tweenTimeKey < 1)
        {
            _tweenTimeKey += Time.deltaTime / _duration;

            float _tweenTimeValue = TextCurve.Evaluate(_tweenTimeKey);
            LerpTextAmount(_scaleA, _scaleB, DefaultTextColor, ActivatedTextColor, _text, _tweenTimeValue);
            yield return null;
        }

    }

    void LerpTextAmount(Vector3 _scaleA, Vector3 _scaleB, Color _colorA, Color _colorB, Text _text, float _time)
    {
        _text.gameObject.transform.localScale = Vector3.Lerp(_scaleA, _scaleB, _time);
        _text.color = Color.Lerp(_colorA, _colorB, _time);
    }

    public string ConvertNumbers(float _amount)
    {
        if(_amount < 1000)
        {
            return _amount.ToString();
        }

        else if (_amount > 1000 && _amount < 100000)
        {
            return _amount / 1000 + "K";
        }

        else
        {
            return _amount / 100000 + "M";
        }
    }
}
