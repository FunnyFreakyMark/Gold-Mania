using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    public enum WaveState { Starting, OnGoing, Ending }
    public enum EnemyTypes { Default, Large, Small }

    public List<Wave> Waves = new List<Wave>();

    [Space]
    public WaveState CurrentWaveState;
    public int CurrentWave;

    public List<GameObject> CurrentEnemies = new List<GameObject>();

    [Header("References: ")]
    public GameObject DefaultEnemyPrefab;
    public GameObject LargeEnemyPrefab;
    public GameObject SmallEnemyPrefab;
    public WaveNotification WaveNotification;

    void Awake()
    {
        Instance = Instance == null ? this : Instance;
        InitializeGroups();
        InitializeWaves();
    }

    void Start()
    {
        StartCoroutine(IBeginWave(Waves[CurrentWave], 3));
    }

    public void BeginWave()
    {
        GenerateNewWave();
        StartCoroutine(IBeginWave(Waves[CurrentWave],0));
    }

    void GenerateNewWave()
    {
        Wave _wave = new Wave();

        for (int i = 0; i < 1*CurrentWave; i++)
        {
            _wave.Groups.Add(GroupType1);

        }
        for (int i = 0; i < 1 * CurrentWave; i++)
        {
            _wave.Groups.Add(GroupType2);
        }
        for (int i = 0; i < 1 * CurrentWave; i++)
        {
            _wave.Groups.Add(GroupType3);
        }

        Waves.Add(_wave);
    }

    IEnumerator IBeginWave(Wave _wave, float _delay)
    {
        GameObject _waveParent = new GameObject("Wave" + CurrentWave);

        yield return new WaitForSeconds(_delay);

        //Groups
        foreach (var _group in _wave.Groups)
        {
            GameObject _groupParent = new GameObject("Group" + _wave.Groups.IndexOf(_group));
            _groupParent.transform.parent = _waveParent.transform;

            //Enemies
            foreach (var _enemy in _group.EnemyTypes)
            {
                switch (_enemy)
                {
                    default:
                        SpawnEnemy(_enemy, DefaultEnemyPrefab, _groupParent.transform);
                        break;
                    case EnemyTypes.Large:
                        SpawnEnemy(_enemy, LargeEnemyPrefab, _groupParent.transform);
                        break;
                    case EnemyTypes.Small:
                        SpawnEnemy(_enemy, SmallEnemyPrefab, _groupParent.transform);
                        break;
                }
                yield return null;
            }
            yield return new WaitForSeconds(_group.Delay);
        }

        yield return null;
    }


    void SpawnEnemy(EnemyTypes _type, GameObject _prefab, Transform _parent)
    {
        GameObject _enemyObj = Instantiate(_prefab, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0), _prefab.transform.rotation, _parent);
        _enemyObj.name = _prefab.name;
        CurrentEnemies.Add(_enemyObj);

        switch (_type)
        {
            default:
                _enemyObj.AddComponent<DefaultEnemy>();
                break;
            case EnemyTypes.Large:
                _enemyObj.AddComponent<LargeEnemy>();
                break;
            case EnemyTypes.Small:
                _enemyObj.AddComponent<SmallEnemy>();
                break;
        }
    }

    public void RemoveEnemy(Enemy _enemyTarget)
    {
        foreach (var _enemy in CurrentEnemies)
        {
            if (_enemy == _enemyTarget.gameObject)
            {
                CurrentEnemies.Remove(_enemy);
                CheckEnemyAmount();
                break;
            }
        }

        Destroy(_enemyTarget.gameObject);
    }

    void CheckEnemyAmount()
    {
        Debug.Log("CHECKING");
        if (CurrentEnemies.Count <= 0)
        {
            StopAllCoroutines();
            CurrentWave++;
            WaveNotification.ActivateNotification(CurrentWave + 1, 3);
        }
    }

    Group GroupType1 = new Group();
    Group GroupType2 = new Group();
    Group GroupType3 = new Group();
    void InitializeGroups()
    {
        EnemyTypes _default = EnemyTypes.Default;
        EnemyTypes _large = EnemyTypes.Large;
        EnemyTypes _small = EnemyTypes.Small;

        GroupType1.Name = "Group1";
        for (int i = 0; i < 3; i++)
        {
            GroupType1.EnemyTypes.Add(_default);
        }

        GroupType2.Name = "Group2";
        for (int i = 0; i < 3; i++)
        {
            GroupType2.EnemyTypes.Add(_default);
        }
        for (int i = 0; i < 10; i++)
        {
            GroupType2.EnemyTypes.Add(_small);
        }

        GroupType3.Name = "Group3";
        for (int i = 0; i < 3; i++)
        {
            GroupType3.EnemyTypes.Add(_default);
        }
        for (int i = 0; i < 10; i++)
        {
            GroupType3.EnemyTypes.Add(_small);
        }
        GroupType3.EnemyTypes.Add(_large);
    }
    void InitializeWaves()
    {
        Wave _wave = new Wave();
        _wave.Groups.Add(GroupType1);
        Waves.Add(_wave);

        Wave _wave1 = new Wave();
        _wave1.Groups.Add(GroupType1);
        _wave1.Groups.Add(GroupType2);
        Waves.Add(_wave1);

        Wave _wave2 = new Wave();
        _wave2.Groups.Add(GroupType1);
        _wave2.Groups.Add(GroupType2);
        _wave2.Groups.Add(GroupType1);
        Waves.Add(_wave2);

        Wave _wave3 = new Wave();
        _wave3.Groups.Add(GroupType1);
        _wave3.Groups.Add(GroupType2);
        _wave3.Groups.Add(GroupType3);
        _wave3.Groups.Add(GroupType2);
        _wave3.Groups.Add(GroupType1);
        Waves.Add(_wave3);
    }
}

[System.Serializable]
public class Wave
{
    public string Name = "New Wave";
    public List<Group> Groups = new List<Group>();
}

[System.Serializable]
public class Group
{
    public string Name = "New Group";
    public float Delay;
    public List<WaveManager.EnemyTypes> EnemyTypes = new List<WaveManager.EnemyTypes>();
}
