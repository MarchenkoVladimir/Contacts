using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runtime : MonoBehaviour
{
    private JsonManager manager;
    void Start()
    {
        manager = JsonManager.Instance;
        manager.LoadDataJson();

        GameObjectPool.Instance.Initialize();
        
        for (int i = 0; i < 10; i++)
        {
            GameObjectPool.Instance.Spawn(manager.Humans[i], i);
        }
    }

}
