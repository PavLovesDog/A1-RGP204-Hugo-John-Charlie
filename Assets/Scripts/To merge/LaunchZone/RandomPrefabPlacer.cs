using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPrefabPlacer : MonoBehaviour
{
    public GameObject prefab;  // The prefab to be instantiated.
    public int minCount = 5;   // Minimum number of prefabs to be placed.
    public int maxCount = 10;  // Maximum number of prefabs to be placed.
    public Vector3 center;     // Center of the area where prefabs will be placed.
    public Vector3 size;       // Size of the area where prefabs will be placed.

    // Start is called before the first frame update
    void Start()
    {
        int count = Random.Range(minCount, maxCount + 1);  // Get a random count between minCount and maxCount.

        for (int i = 0; i < count; i++)
        {
            Vector3 position = center + new Vector3(Random.Range(-size.x / 2, size.x / 2),
                                                    Random.Range(-size.y / 2, size.y / 2),
                                                    Random.Range(-size.z / 2, size.z / 2));

            Instantiate(prefab, position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
