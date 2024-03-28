using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    #region Singleton
    public static ObjectPooler Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [System.Serializable]

    // This class represent pool
    public class Pool
    {
        public string tag; 
        public GameObject prefab;
        public int size;
        public bool isConneted = false;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // create pools and store the referance
        foreach (Pool pool in pools) 
        {            
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++) 
            {
                GameObject obj = Instantiate(pool.prefab); 
                obj.SetActive(false);
                obj.transform.parent = gameObject.transform;
                obj.GetComponent<LineRenderer>().numCornerVertices = 0; // inline Smoothness
                obj.GetComponent<LineRenderer>().numCapVertices = 30;    // Corners Roundness
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    /// <summary>
    /// Spawn an item from the pool at given position and rotation.
    /// </summary>
    /// <param name="tag">Unique string that represent the pool</param>
    /// <param name="position">World position at which item should be spawned</param>
    /// <param name="rotation">The rotation with which item should be spawned</param>
    /// <returns>Spawned GameObject</returns>
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        // check for key
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError("Pool with tag "+ tag + " doesn't excist.");
            return null;
        }

        // If run Out of object Create new
        if(poolDictionary[tag].Count < 1)
        {
#if SHOW_DETAIL_LOGS
            print("Adding more"); 
#endif
            foreach (Pool pool in pools) 
            {
                if(pool.tag != tag)
                    continue;

                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++) 
                {
                    GameObject obj = Instantiate(pool.prefab); 
                    obj.SetActive(false);
                    obj.transform.parent = gameObject.transform;
                    poolDictionary[tag].Enqueue(obj);
                }
                break;
            }
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        return objectToSpawn;
    }
}
