using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopUpType { Bubble, ReverseBubble, DecreasingBubble }

/// <summary>
/// Working as in-world data-objects holder.
/// </summary>
public class BubblesPool : MonoBehaviour
{
    private PlayerController _player;

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


    public void Init(PlayerController player)
    {
        _player = player;
        ClearAll();
        FillPool();
    }

    /// <summary>
    /// Initial filling of the pool
    /// </summary>
    private void FillPool()
    {
        // go through the list of prefabs
        for (int i = 0; i < _prefabs.Count; i++)
        {
            // if the prefab field is empty, we ignore it and do not add it to the database.
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

            // at the same time we fill in the prefab dictionary
            _prefabsDictionary[_prefabs[i].type] = _prefabs[i].prefab;
        }
    }

    /// <summary>
    /// Create exemplar of object
    /// </summary>
    /// <param name="prefab">Prefab of object variant</param>
    /// <returns>Spawned object base class</returns>
    private PopUpObject InstantiateObject(PopUpObject prefab)
    {
        PopUpObject obj = Instantiate(prefab);

        if (obj.gameObject.activeSelf) obj.gameObject.SetActive(false);
        obj.transform.position = _poolDefaultPoint;
        obj.transform.SetParent(transform);

        if (_hidePrefabsInHierarchy) obj.gameObject.hideFlags = HideFlags.HideInHierarchy;

        return obj;
    }

    /// <summary>
    /// Spawn object of Type. </br>
    /// Also this is the only one point where Init() of object is called.
    /// </summary>
    /// <param name="type">Type of object to spawn</param>
    /// <returns>Spawned object base class</returns>
    public PopUpObject Spawn(PopUpType type)
    {
        PopUpObject _object = FindAndSpawn(type);
        _object.Init();

        if (type == PopUpType.Bubble) _object.GenerateSize(_player.GetBubbleSizeX());
        return _object;
    }

    /// <summary>
    /// Spawn object of Type in selected position.
    /// </summary>
    /// <param name="type">Type of object to spawn</param>
    /// <param name="position">Position to spawn</param>
    /// <returns>Spawned object base class</returns>
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

    /// <summary>
    /// Find object of Type in dictionary, or create new if no free objects.
    /// </summary>
    /// <param name="type">Type of object to spawn</param>
    /// <returns>Spawned object base class</returns>
    public PopUpObject FindAndSpawn(PopUpType type)
    {
        List<PopUpObject> _internalList;
        PopUpObject obj;
        bool listExisting = false;

        // looking in the pool for a list of objects of the type we need
        if (_pool.TryGetValue(type, out _internalList))
        {
            listExisting = true;

            // looking for a free object
            for (int i = 0; i < _internalList.Count; i++)
            {
                if (_internalList[i].gameObject.activeSelf) continue;

                // found a suitable one - spawn and exit the function
                _internalList[i].gameObject.SetActive(true);
                if (!_activeObjects.Contains(_internalList[i])) _activeObjects.Add(_internalList[i]);
                return _internalList[i];
            }
        }

        /* 
         * If the pool does not contain the required list, or there are no free objects in the list,
         * we try to find the desired one in the prefab dictionary, create a list (if necessary) and do everything that is needed.
         */
        if (_prefabsDictionary.TryGetValue(type, out obj))
        {
            PopUpObject _prefab = InstantiateObject(obj);
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
            // throw error and return null
            if (listExisting) 
                Debug.LogError("Can't find prefab of type <color=red><b>\"" + type + "\"</b></color> in prefab dictionary!");
            else
                Debug.LogError("Can't find prefab type <color=red><b>\"" + type + "\"</b></color> anywhere!");

            return null;
        }
    }

    /// <summary>
    /// Get active objects data
    /// </summary>
    /// <returns>List of active objects or count of this list</returns>
    public List<PopUpObject> GetActiveObjects()
    {
        return _activeObjects;
    }
    public int GetActiveObjectsCount()
    {
        return _activeObjects.Count;
    }
    /// <summary>
    /// Return object to pool
    /// </summary>
    /// <param name="obj">Object to return</param>
    public void Return(PopUpObject obj)
    {
        if (obj.gameObject.activeSelf) obj.gameObject.SetActive(false);
        if (obj.transform.parent != transform) obj.transform.SetParent(transform);
        if (_activeObjects.Contains(obj)) _activeObjects.Remove(obj);
    }

    /// <summary>
    /// Clear All lists in pool.
    /// </summary>
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

    /// <summary>
    /// Clear selected list
    /// </summary>
    /// <param name="list">List to clear</param>
    private void ClearPopUpList(List<PopUpObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i].gameObject);
        }
        list.Clear();
    }

}
