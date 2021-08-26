///
/// Simple pooling for Unity.
///   Author: Martin "quill18" Glaude (quill18@quill18.com)
///   Latest Version: https://gist.github.com/quill18/5a7cfffae68892621267
///   License: CC0 (http://creativecommons.org/publicdomain/zero/1.0/)
///   UPDATES:
///     2015-04-16: Changed Pool to use a Stack generic.
///
/// Usage:
///
///   There's no need to do any special setup of any kind.
///
///   Instead of calling Instantiate(), use this:
///       SimplePool.Spawn(somePrefab, somePosition, someRotation);
///
///   Instead of destroying an object, use this:
///       SimplePool.Despawn(myGameObject);
///
///   If desired, you can preload the pool with a number of instances:
///       SimplePool.Preload(somePrefab, 20);
///
/// Remember that Awake and Start will only ever be called on the first instantiation
/// and that member variables won't be reset automatically.  You should reset your
/// object yourself after calling Spawn().  (i.e. You'll have to do things like set
/// the object's HPs to max, reset animation states, etc...)


using System.Collections.Generic;
using UnityEngine;


public static class SimplePool
{
    // You can avoid resizing of the Stack's internal data by
    // setting this to a number equal to or greater to what you
    // expect most of your pool sizes to be.
    // Note, you can also use Preload() to set the initial size
    // of a pool -- this can be handy if only some of your pools
    // are going to be exceptionally large (for example, your bullets.)
    const int DEFAULT_POOL_SIZE = 3;

    const string MASTER_CONTAINER_NAME = "[Simple Pool Master Container]";
    const string CONTAINER_SUFFIX = " [Pool]";


    static Dictionary<GameObject, Pool> pools;
    static Dictionary<Pool, GameObject> containers;
    static GameObject masterContainer;


    /// <summary>
    /// Spawns a copy of the specified prefab (instantiating one if required).
    /// </summary>
    static public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent = null) {
        Initialize(prefab);
        return pools[prefab].Spawn(pos, rot, parent);
    }

    /// <summary>
    /// Despawns the specified gameobject back into its pool (destroys it if it can't be returned).
    /// </summary>
    static public void Despawn(GameObject obj, bool setParent = true) {
        PoolMember pm = obj.GetComponent<PoolMember>();
        if (!pm) {
            Debug.Log($"Object { obj.name } wasn't spawned from a pool. Destroying it instead.");
            GameObject.Destroy(obj);
        }
        else try {
            pm.myPool.Despawn(obj, setParent);
        }
        catch (System.NullReferenceException) {
            Debug.Log($"Object { obj.name } is an invalid pool member; it was probably cloned from another object. Destroying it instead.");
            GameObject.Destroy(obj);
        }
    }

    /// <summary>
    /// If you want to preload a few copies of an object at the start
    /// of a scene, you can use this. Really not needed unless you're
    /// going to go from zero instances to 100+ very quickly.
    /// Could technically be optimized more, but in practice the
    /// Spawn/Despawn sequence is going to be pretty darn quick and
    /// this avoids code duplication.
    /// </summary>
    static public void Preload(GameObject prefab, int qty = 1) {
        Initialize(prefab, qty);

        // Make an array to grab the objects we're about to pre-spawn.
        GameObject[] obs = new GameObject[qty];
        for (int i = 0; i < qty; i++) {
            obs[i] = Spawn(prefab, Vector3.zero, Quaternion.identity);
        }

        // Now despawn them all.
        for (int i = 0; i < qty; i++) {
            Despawn(obs[i]);
        }
    }

    /// <summary>
    /// Initializes a dictionary for the given prefab
    /// </summary>
    static void Initialize(GameObject prefab = null, int qty = DEFAULT_POOL_SIZE) {
        if (!masterContainer) {
            masterContainer = new GameObject(MASTER_CONTAINER_NAME);
            masterContainer.isStatic = true;
        }

        pools ??= new Dictionary<GameObject, Pool>();
        containers ??= new Dictionary<Pool, GameObject>();

        if (prefab) {
            if (!pools.ContainsKey(prefab) || pools[prefab] == null) {
                pools[prefab] = new Pool(prefab, qty);
                containers[pools[prefab]] = new GameObject(prefab.name + CONTAINER_SUFFIX);
                containers[pools[prefab]].transform.SetParent(masterContainer.transform);
            }
            else if (!containers.ContainsKey(pools[prefab]) || containers[pools[prefab]] == null) {
                containers[pools[prefab]] = new GameObject(prefab.name + CONTAINER_SUFFIX);
                containers[pools[prefab]].transform.SetParent(masterContainer.transform);
            }
        }
    }


    /// <summary>
    /// The Pool class represents the pool for a particular prefab.
    /// </summary>
    class Pool
    {
        int nextId = 0;

        readonly Stack<GameObject> inactive;
        readonly GameObject prefab;

        public Pool(GameObject prefab, int initialQty) {
            this.prefab = prefab;
            inactive = new Stack<GameObject>(initialQty);
        }

        public GameObject Spawn(Vector3 pos, Quaternion rot, Transform parent) {
            GameObject obj;
            if (inactive.Count == 0) {
                // Empty pool, instantiating new object instead
                obj = Object.Instantiate(prefab, pos, rot);
                obj.name = $"{ prefab.name } ({ nextId++ })";

                // Add a PoolMember component so we know what pool
                // we belong to.
                obj.AddComponent<PoolMember>().myPool = this;
            }
            else {
                // Grab the last object in the inactive array
                obj = inactive.Pop();

                if (!obj) {
                    // The inactive object we expected to find no longer exists.
                    // The most likely causes are:
                    //   - Someone calling Destroy() on our object
                    //   - A scene change (which will destroy all our objects).
                    //     NOTE: This could be prevented with a DontDestroyOnLoad
                    //     if you really don't want this.
                    // No worries -- we'll just try the next one in our sequence.
                    return Spawn(pos, rot, parent);
                }
            }

            obj.transform.position = pos;
            obj.transform.rotation = rot;
            if (parent)
                obj.transform.SetParent(parent);
            else
                obj.transform.SetParent(containers[this].transform);
            obj.SetActive(true);
            return obj;
        }

        // Return an object to the inactive pool.
        public void Despawn(GameObject obj, bool setParent) {
            obj.SetActive(false);
            if (setParent)
                obj.transform.SetParent(containers[this].transform);
            inactive.Push(obj);
        }
    }


    /// <summary>
    /// Added to freshly instantiated objects, so we can link back
    /// to the correct pool on despawn.
    /// </summary>
    class PoolMember : MonoBehaviour
    {
        public Pool myPool;
    }
}
