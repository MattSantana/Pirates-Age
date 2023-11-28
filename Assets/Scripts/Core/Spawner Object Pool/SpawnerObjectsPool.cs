using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerObjectsPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string poolTag;
        public GameObject enemyType;
        public int poolTotalSize;
    }

    #region Singleton
    public static SpawnerObjectsPool Instance;

    private void Awake() 
    {
        Instance = this;
    }
    #endregion
    
    [SerializeField] private List<Pool> pools;
    [SerializeField] private Dictionary<string, Queue<GameObject>> poolDictionary;
    

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
    
        foreach ( Pool enemyPools in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for( int i = 0; i < enemyPools.poolTotalSize; i++)
            {
                GameObject enemyInstance =Instantiate(enemyPools.enemyType);
                enemyInstance.SetActive(false);
                objectPool.Enqueue(enemyInstance);
            }
            poolDictionary.Add(enemyPools.poolTag, objectPool);
        }
    }

    public void SpawnFromPool(string tag, Vector3 position,  Quaternion rotation)
    {
        if(poolDictionary.ContainsKey(tag))
        {
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            IPooledObject[] pooledObjects = objectToSpawn.GetComponents<IPooledObject>();

            if(pooledObjects != null)
            {
                foreach (IPooledObject pooledObjectToRest in pooledObjects)
                {
                    pooledObjectToRest.OnObjectSpawn();
                }
            }
            else
            {
                Debug.Log("Não achei o pooledInterface");
            }

            poolDictionary[tag].Enqueue(objectToSpawn);
        }
    }

}
