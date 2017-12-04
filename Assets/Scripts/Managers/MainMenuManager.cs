using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    [Header("References: ")]
    public GameObject SecretWindow;
    public Text GoldText;

    public void OnStart()
    {
        SceneManager.LoadScene(1);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void UpdateGoldText(float _gold)
    {
        GoldText.text = "Current Gold: " + ConvertNumbers(_gold);
    }

    public void OnSecretButton()
    {
        if (GameManager.Instance.Gold >= 3000)
        {
            GameManager.Instance.Gold -= 3000;
            DataManager.SaveData();
            SecretWindow.SetActive(true);
        }
    }

    public void OnCloseSecretWindow()
    {
        SecretWindow.SetActive(false);
    }

    string ConvertNumbers(float _amount)
    {
        if (_amount < 1000)
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
