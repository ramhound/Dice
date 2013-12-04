using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// This class is a collection of static functions designed to help with everyday use
/// </summary>
public class SOS : MonoBehaviour {
    #region SOSException
    /// <summary>
    /// Exception handler for the SOS class
    /// </summary>
    class SOSExcpetion : Exception {
        public SOSExcpetion() : base() { }
        public SOSExcpetion(string reason) : base(reason) { }
    }
    #endregion

    #region variables
    private static SOS instance;
    private static Transform _prototype;  //Transform to hold the prototypes
    private static Transform _objectPool; //pool for prefabrication of prototypes
    private static Dictionary<string, List<GameObject>> pooledObjects;
    #endregion

    #region startup
    private void Awake() {
        instance = this;
        pooledObjects = new Dictionary<string, List<GameObject>>();

        _prototype = transform;
        //for(int i = 0; i < _prototype.childCount; i++)
        //    _prototype.GetChild(i).gameObject.SetActive(false);

        _objectPool = _prototype.FindByName("Object Pool");
        if(_objectPool == null) {
            _objectPool = new GameObject("Object Pool").transform;
            _objectPool.parent = _prototype;
        }

        gameObject.SetActive(false);
    }
    #endregion

    #region input and mouse
    private void Update() {
        //nothing to see here move along...
    }
    #endregion

    #region prototype handling
    /// <summary>
    /// Returns the Prototypes object in the scene
    /// </summary>
    public static Transform prototype {
        get {
            if(_prototype == null) {
                _prototype = instance.transform;
                if(_prototype == null) throw new SOSExcpetion("There is not a prototypes object in the scene");
            } return _prototype;
        }
    }

    public static Transform objectPool {
        get {
            if(_objectPool == null) {
                _objectPool = new GameObject("Object Pool").transform;
                _objectPool.parent = _prototype;
            } return _objectPool;
        }
        set { _objectPool = value; }
    }

    /// <summary>
    /// Finds the requested prototype in the prototypes object and activates it in the scene
    /// </summary>
    /// <param name="name"></param>
    /// <param name="deepSearch"></param>
    /// <returns></returns>
    private static GameObject[] GetPrototypeObject(string name, int count, bool active, bool deepSearch, bool addToPool) {
        bool poolFound = false;
        char[] seperator = new char[] { '/' };
        string[] parentNames = name.Split(seperator, StringSplitOptions.None);
        Transform parent = prototype;

        if(parentNames.Length == 0) throw new SOSExcpetion("No prototype by that name found: " + name);
        poolFound = (pooledObjects.ContainsKey(parentNames[parentNames.Length - 1]) && 
            pooledObjects[parentNames[parentNames.Length - 1]].Count > 0);
        if(poolFound) {
            //since the object is in a pool, remove it from the Dictionary and detach its parent
            GameObject go = pooledObjects[parentNames[parentNames.Length - 1]][0];
            pooledObjects[parentNames[parentNames.Length - 1]].Remove(go);
            if(pooledObjects[parentNames[parentNames.Length - 1]].Count <= 0)
                pooledObjects.Remove(parentNames[parentNames.Length - 1]);
            go.transform.parent = null;
            go.SetActive(true);
            return new GameObject[] { go };
        } else {
            for(int i = 0; i < parentNames.Length; i++) {
                if(deepSearch) parent = parent.FindByNameRecursively(parentNames[i]);
                else parent = parent.FindByName(parentNames[i]);

                if(parent == null) throw new SOSExcpetion("No prototype by that name found: " + parentNames[i]);
            }

            GameObject[] prefabs = new GameObject[count];
            for(int i = 0; i < count; i++) {
                prefabs[i] = Instantiate(parent.gameObject) as GameObject;
                if(addToPool && !active) prefabs[i].transform.parent = objectPool;
                prefabs[i].SetActive(active);
            }
            if(addToPool) pooledObjects.Add(parentNames[parentNames.Length - 1], prefabs.ToList());
            return prefabs;
        }
    }

    /// <summary>
    /// Creates an object in the scene from a prototype (excludes grandchildren)
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject CreateObject(string name) {
        return CreateObject(name, false);
    }

    /// <summary>
    /// Creates an object in the scene form a prototype (if true, includes grandchildren)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="deepSearch"></param>
    /// <returns></returns>
    public static GameObject CreateObject(string name, bool deepSearch) {
        return GetPrototypeObject(name, 1, true, false, false)[0];
    }

    /// <summary>
    /// Creates an object in the scene form a prototype at the specified location (excludes grandchildren)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static GameObject CreateObjectAt(string name, Vector2 position) {
        GameObject go = CreateObject(name);
        if(go != null) go.transform.position = new Vector3(position.x, position.y, go.transform.position.z);
        return go;
    }

    /// <summary>
    /// Creates an object in the scene form a prototype at the specified location (if true, excludes grandchildren)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static GameObject CreateObjectAt(string name, Vector2 position, bool deepSearch) {
        GameObject go = CreateObject(name, deepSearch);
        if(go != null) go.transform.position = new Vector3(position.x, position.y, go.transform.position.z);
        return go;
    }

    /// <summary>
    /// Creates an object in the scene form a prototype and returns type T (excludes grandchildren)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T CreateObject<T>(string name) where T : MonoBehaviour {
        return CreateObject<T>(name, false).GetComponent<T>();
    }

    /// <summary>
    /// Creates an object in the scene form a prototype, sets its position, and returns type T (excludes grandchildren)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T CreateObjectAt<T>(string name, Vector2 position) where T : MonoBehaviour {
        GameObject go = CreateObjectAt(name, position);
        return go.GetComponent<T>();
    }

    /// <summary>
    /// Creates an object in the scene form a prototype and returns type T (if true, includes grandchildren)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T CreateObject<T>(string name, bool deepSearch) where T : MonoBehaviour {
        return GetPrototypeObject(name, 1, true, deepSearch, false)[0].GetComponent<T>();
    }

    /// <summary>
    /// Creates an object in the scene form a prototype, sets its position, and returns type T (if true, includes grandchildren)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T CreateObjectAt<T>(string name, Vector2 position, bool deepSearch) where T : MonoBehaviour {
        GameObject go = CreateObjectAt(name, position, deepSearch);
        return go.GetComponent<T>();
    }

    /// <summary>
    /// Creates a specified number of non-active prototype objects in the scene for faster creation later (excludes grandchildren)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static GameObject[] PreFabricate(string name, int count) {
        return PreFabricate(name, count, false);
    }

    /// <summary>
    /// Creates a specified number of "active" prototype objects in the scene for faster creation later (excludes grandchildren)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static GameObject[] PreFabricate(string name, int count, bool active) {
        return PreFabricate(name, count, active, false);
    }

    /// <summary>
    /// Creates a specified number of "active" prototype objects in the scene for faster creation later (if true, includes grandchildren)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static GameObject[] PreFabricate(string name, int count, bool active, bool deepSearch) {
        return GetPrototypeObject(name, count, active, deepSearch, true);
    }

    /// <summary>
    /// Clears the objectPool cache
    /// </summary>
    public static void CleanPool() {
        pooledObjects.Clear();
    }

    /// <summary>
    /// <para>Wrapper for Unity native Destroy(GameObject)</para>
    /// <para>(I eventually want destroyed objects to go into a pool)</para>
    /// </summary>
    /// <param name="obj"></param>
    public static void Destroy(GameObject obj) {
        //reset all scripts attached somehow and add it to a pool for reuse later on
        Destroy(obj);//destroy for now
    }

    //private void OnDestroy() {
    //    Destroy(_objectPool);
    //}
    #endregion

    #region delegate execution
    /// <summary>
    /// Call the passed function in x time
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="method"></param>
    public static void Execute(float delay, Action method) {
        Execute(delay, method, null, null);
    }

    /// <summary>
    /// Call the passed function in x time, with a onStart and onEnd delegate
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="method"></param>
    /// <param name="onStart"></param>
    /// <param name="onEnd"></param>
    public static void Execute(float delay, Action method, Action onStart, Action onEnd) {
        instance.StartCoroutine(instance._Execute(delay, method, onStart, onEnd));
    }

    /// <summary>
    /// Internal execution of the delegates
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="method"></param>
    /// <param name="startCall"></param>
    /// <param name="endCall"></param>
    /// <returns></returns>
    private IEnumerator _Execute(float delay, Action method, Action startCall, Action endCall) {
        yield return new WaitForSeconds(delay);
        if(startCall != null)
            startCall();
        method();
        if(endCall != null)
            endCall();
    }
    #endregion
}