using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopUpType { Bubble, ReverseBubble, DecreasingBubble }

public class BubblesPool : MonoBehaviour
{

    [Header("Pool Settings")]
    [SerializeField]
    private bool _hidePrefabsInHierarchy = false;
    [SerializeField]
    private Vector3 _poolDefaultPoint = new Vector3(10000, 10000, 10000);

    [Header("Prefabs Data")]
    [SerializeField]
    public List<PrefabPoolData> _prefabs = new List<PrefabPoolData>();

    private Dictionary<PopUpType, PopUpObject> _prefabsDictionary = new Dictionary<PopUpType, PopUpObject>();
    private Dictionary<PopUpType, List<PopUpObject>> _pool = new Dictionary<PopUpType, List<PopUpObject>>();
    private List<PopUpObject> _activeObjects = new List<PopUpObject>();


    public void Init()
    {
        ClearAll();
        FillPool();
    }

    private void FillPool()
    {
        // проходимся по списку префабов
        for (int i = 0; i < _prefabs.Count; i++)
        {
            // если поле префаба пустое - игнорируем и в базу не вносим.
            if (_prefabs[i].prefab == null) continue;

            List<PopUpObject> _internalList;
            if (!_pool.TryGetValue(_prefabs[i].type, out _internalList))
            {
                _internalList = new List<PopUpObject>();
            }

            for (int i2 = 0; i2 < _prefabs[i].poolCount; i2++)
            {
                _internalList.Add(InstantiateObject(_prefabs[i].prefab));
            }

            _pool[_prefabs[i].type] = _internalList;

            // заодно заполняем словарь префабов
            _prefabsDictionary[_prefabs[i].type] = _prefabs[i].prefab;
        }
    }

    private PopUpObject InstantiateObject(PopUpObject prefab)
    {
        PopUpObject obj = Instantiate(prefab);

        if (obj.gameObject.activeSelf) obj.gameObject.SetActive(false);
        obj.transform.position = _poolDefaultPoint;
        obj.transform.SetParent(transform);

        if (_hidePrefabsInHierarchy) obj.gameObject.hideFlags = HideFlags.HideInHierarchy;

        return obj;
    }

    public PopUpObject Spawn(PopUpType type, Vector3 position)
    {
        PopUpObject obj = Spawn(type);

        if (obj == null)
        {
            Debug.LogError("Exception at <b>Spawn in Position<b> method!");
            return null;
        }

        obj.transform.position = position;
        return obj;
    }

    public PopUpObject Spawn(PopUpType type)
    {
        List<PopUpObject> _internalList;
        PopUpObject obj;
        bool listExisting = false;

        // ищем в пуле список объектов нужного нам типа
        if (_pool.TryGetValue(type, out _internalList))
        {
            listExisting = true;

            // ищем свободный объект
            for (int i = 0; i < _internalList.Count; i++)
            {
                if (_internalList[i].gameObject.activeSelf) continue;

                // нашли подходящий - спавним и выходим из функции
                _internalList[i].Init();
                _internalList[i].gameObject.SetActive(true);
                if (!_activeObjects.Contains(_internalList[i])) _activeObjects.Add(_internalList[i]);
                return _internalList[i];
            }
        }

        // Если в пуле нет нужного списка, либо в списке нет свободных объектов - пробуем найти в словаре префабов нужный, создать список(при необходимости) и выполнить все что нужно.
        if (_prefabsDictionary.TryGetValue(type, out obj))
        {
            PopUpObject _prefab = InstantiateObject(obj);
            _prefab.Init();
            _prefab.gameObject.SetActive(true);

            if (listExisting)
            {
                _internalList.Add(_prefab);
            }
            else
            {
                _internalList = new List<PopUpObject>();
                _internalList.Add(_prefab);
                _pool[type] = _internalList;
            }
            if (!_activeObjects.Contains(_prefab)) _activeObjects.Add(_prefab);

            return _prefab;
        }
        else
        {
            // иначе даем ошибку и возвращаем ноль
            if (listExisting) 
                Debug.LogError("Can't find prefab of type <color=red><b>\"" + type + "\"</b></color> in prefab dictionary!");
            else
                Debug.LogError("Can't find prefab type <color=red><b>\"" + type + "\"</b></color> anywhere!");

            return null;
        }
    }

    public void Return(PopUpObject obj)
    {
        if (obj.gameObject.activeSelf) obj.gameObject.SetActive(false);
        if (obj.transform.parent != transform) obj.transform.SetParent(transform);
        if (_activeObjects.Contains(obj)) _activeObjects.Remove(obj);
    }

    public List<PopUpObject> GetActiveObjects()
    {
        return _activeObjects;
    }
    public int GetActiveObjectsCount()
    {
        return _activeObjects.Count;
    }

    private void ClearAll()
    {
        foreach (PopUpType type in System.Enum.GetValues(typeof(PopUpType)))
        {
            if (_pool.TryGetValue(type, out List<PopUpObject> val))
            {
                ClearPopUpList(val);
            }
        }
        _pool.Clear();
        _prefabsDictionary.Clear();

        ClearPopUpList(_activeObjects);
    }

    private void ClearPopUpList(List<PopUpObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i].gameObject);
        }
        list.Clear();
    }

}
