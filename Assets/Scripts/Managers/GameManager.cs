using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Settings: ")]
    public int Gold;

    [Header("References: ")]
    public GameObject GameOverMenuWindow;
    public MainMenuManager MainMenuManager;

    void Awake()
    {
        Instance = Instance == null ? this : Instance;

        DataManager.LoadData();

        if (MainMenuManager != null)
            MainMenuManager.UpdateGoldText(Gold);
    }

    public void OnRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void OnMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        StartCoroutine(IOpenMenu(GameOverMenuWindow));
    }

    IEnumerator IOpenMenu(GameObject _window)
    {
        Time.timeScale = 0;

        _window.SetActive(true);

        yield return null;
    }

    IEnumerator ICloseMenu(GameObject _window)
    {
        _window.SetActive(true);

        Time.timeScale = 1;

        yield return null;
    }


    //    public void StartCoroutine(Coroutine _coroutine)
    //    {
    //        StartCoroutine(_coroutine);
    //    }
}

public class DataManager
{
    public static void SaveData()
    {
        BinaryFormatter _bf = new BinaryFormatter();
        FileStream _file = File.Create(Application.persistentDataPath + "/SaveFile.dat");

        Data _data = new Data();
        _data.Gold = GameManager.Instance.Gold;

        _bf.Serialize(_file, _data);
        _file.Close();
    }

    public static void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveFile.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SaveFile.dat", FileMode.Open);
            Data data = (Data)bf.Deserialize(file);
            file.Close();

            GameManager.Instance.Gold = data.Gold;
        }
    }
}

[System.Serializable]
public class Data
{
    //Todo: store weapons, level, experience, etc.
    public int Gold;
}

