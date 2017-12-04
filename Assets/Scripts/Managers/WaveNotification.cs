using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaveNotification : MonoBehaviour
{
    [Header("Settings: ")]
    public float SlideDuration = 5;
    public float FadeDuration = 3;

    public AnimationCurve SlideCurve;
    public AnimationCurve FadeAlphaCurve;

    [Header("References: ")]
    public Image Overlay;
    public Image Banner;
    public Text Text;
    public CanvasGroup Buttons;

    float overlayAlphaTarget;
    int wave;

    void Awake()
    {
        overlayAlphaTarget = Overlay.color.a;
        Overlay.color = new Color(Overlay.color.r, Overlay.color.g, Overlay.color.b, 0);
        Banner.color = new Color(Banner.color.r, Banner.color.g, Banner.color.b, 0);
        Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, 0);
        //ActivateNotification(2);
    }    

    public void OnContinue()
    {
        Buttons.interactable = false;
        Time.timeScale = 1;
        StartCoroutine(INotifyContinue(wave, 0));
    }

    public void OnHome()
    {
        Buttons.interactable = false;
        GameManager.Instance.Gold += Player.Instance.Gold;
        DataManager.SaveData();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ActivateNotification(int _wave, float _delay)
    {
        wave = _wave;
        StartCoroutine(INotify(_wave, _delay));
    }

    IEnumerator INotify(int _wave, float _delay)
    {
        float _tweenTimeKey = 0;

        yield return new WaitForSeconds(_delay);

        //IMAGES
        while (_tweenTimeKey < 1)
        {
            _tweenTimeKey += Time.deltaTime / FadeDuration;

            float _tweenTime = FadeAlphaCurve.Evaluate(_tweenTimeKey);
            LerpImageAlpha(Overlay, 0, overlayAlphaTarget, _tweenTime);
            LerpImageAlpha(Banner, 0, 1, _tweenTime);

            yield return null;
        }        

        yield return new WaitForSeconds(1);

        //TEXT 1
        _tweenTimeKey = 0;
        Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, 1);
        Text.text = "Wave " + (_wave-1).ToString() + " Completed";

        while (_tweenTimeKey < 1)
        {
            _tweenTimeKey += Time.deltaTime / SlideDuration;

            float _tweenTime = SlideCurve.Evaluate(_tweenTimeKey);
            LerpText(Text, _tweenTime);

            yield return null;
        }

        yield return new WaitForSeconds(1);

        //CONTINUE?
        _tweenTimeKey = 0;
        Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, 1);
        Text.transform.localPosition = Vector3.zero;
        Text.text = "You can take home " + Player.Instance.Gold + " gold";
        Buttons.alpha = 0;
        while (_tweenTimeKey < 1)
        {
            _tweenTimeKey += Time.deltaTime / SlideDuration;

            float _tweenTime = FadeAlphaCurve.Evaluate(_tweenTimeKey);
            LerpTextAlpha(Text, 0, 1, _tweenTime);
            LerpCanvasGroupAlpha(Buttons, 0, 1, _tweenTimeKey);
            yield return null;
        }
        Buttons.interactable = true;
        Time.timeScale = 0;
    }

    IEnumerator INotifyContinue(int _wave, float _delay)
    {
        float _tweenTimeKey = 0;

        yield return new WaitForSeconds(_delay);

        _tweenTimeKey = 0;
        while (_tweenTimeKey < 1)
        {
            _tweenTimeKey += Time.deltaTime / FadeDuration;

            float _tweenTime = FadeAlphaCurve.Evaluate(_tweenTimeKey);
            LerpTextAlpha(Text, 1, 0, _tweenTime);
            LerpCanvasGroupAlpha(Buttons, 1, 0, _tweenTime);
            yield return null;
        }

        yield return new WaitForSeconds(1);

        //NEXT WAVE
        _tweenTimeKey = 0;
        Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, 1);
        Text.text = "Wave " + _wave;
        while (_tweenTimeKey < 1)
        {
            _tweenTimeKey += Time.deltaTime / SlideDuration;

            float _tweenTime = SlideCurve.Evaluate(_tweenTimeKey);
            LerpText(Text, _tweenTime);

            yield return null;
        }
        Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, 0);

        yield return new WaitForSeconds(0.5f);

        //IMAGES
        _tweenTimeKey = 0;
        while (_tweenTimeKey < 1)
        {
            _tweenTimeKey += Time.deltaTime / FadeDuration;

            float _tweenTime = FadeAlphaCurve.Evaluate(_tweenTimeKey);
            LerpImageAlpha(Overlay, overlayAlphaTarget, 0, _tweenTime);
            LerpImageAlpha(Banner, 1, 0, _tweenTime);

            yield return null;
        }

        yield return new WaitForSeconds(3);

        WaveManager.Instance.BeginWave();
    }

    public void LerpText(Text _text, float _time)
    {
        Debug.Log("SLiding");

        Vector3 _StartPos = new Vector3(-1000, 0, 0);
        Vector3 _TargetPos = new Vector3(1000, 0, 0);

        _text.transform.localPosition = Vector3.Lerp(_StartPos, _TargetPos, _time);

        //_text.transform.localPosition = new Vector3(0 + (Mathf.Tan(Time.time - 3) * 5), 0, 0);

                   //Creates awesome "slide in" effect
                   //transform.position = new Vector3
                   //        (m_OriginalX + ((float)Mathf.Tan(Time.time - m_Delay) * m_FloatStrength),
                   //        transform.position.y,
                   //        transform.position.z);
    }

    public void LerpImageAlpha(Image _image, float _alphaA, float _alphaB, float _time)
    {
        Color _col = _image.color;
        float _alpha = Mathf.Lerp(_alphaA, _alphaB, _time);

        _image.color = new Color(_col.r, _col.g, _col.b, _alpha);
    }

    public void LerpTextAlpha(Text _image, float _alphaA, float _alphaB, float _time)
    {
        Color _col = _image.color;
        float _alpha = Mathf.Lerp(_alphaA, _alphaB, _time);

        _image.color = new Color(_col.r, _col.g, _col.b, _alpha);
    }

    public void LerpCanvasGroupAlpha(CanvasGroup _canvasGroup, float _alphaA, float _alphaB, float _time)
    {
        float _alpha = Mathf.Lerp(_alphaA, _alphaB, _time);

        _canvasGroup.alpha = _alpha;
    }
}
