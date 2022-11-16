using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameObjectPool : Singleton<GameObjectPool>
{
    [SerializeField] private ObjectCache caches;
    [SerializeField] private GameObject prefab;
    [SerializeField] private RectTransform parentCard;

    private GameObjectPool Pool;
    private List<Card> activeCards = new List<Card>();

    private Vector2 sizeDelta => prefab.GetComponent<RectTransform>().sizeDelta;
    private Positions[] positions;

    private int minIndex;
    private int maxIndex;

    public int MinIndex => minIndex;
    public int MaxIndex => maxIndex;

    public Hashtable activeCachedObjects;
    protected override void Setup()
    {
        Pool = this;
    }

    public void UpdateIndexses(int min, int max)
    {
        minIndex = min;
        maxIndex = max;
    }

    public void Initialize()
    {
        int amount = 0;
        minIndex = -1;
        maxIndex = 9;
        caches.Initialize();
        amount += caches.cacheSize;
        activeCachedObjects = new Hashtable(amount);
    }

    private void SetPositions()
    {
        positions = new Positions[JsonManager.Instance.Humans.Length];
        int count = JsonManager.Instance.Humans.Length;
        for (int i = count; i > 0; i--)
        {
            parentCard.sizeDelta = new Vector2(0, parentCard.sizeDelta.y + sizeDelta.y + 2f);
            positions[i - 1] = new Positions() { position = new Vector2(sizeDelta.x / 2f, parentCard.sizeDelta.y - sizeDelta.y / 2f) };

        }
    }

    public class Positions
    {
        public Vector2 position;
    }

    [Serializable]
    public class ObjectCache
    {

        public int cacheSize = 10;

        private int cacheIndex = 0;
        public Card[] objects;

        [HideInInspector]
        public void Initialize()
        {

            objects = new Card[cacheSize];
            for (var i = 0; i < cacheSize; i++)
            {
                objects[i] = MonoBehaviour.Instantiate(GameObjectPool.Instance.prefab, GameObjectPool.Instance.parentCard).GetComponent<Card>();
                objects[i].gameObject.SetActive(false);
            }
        }

        public GameObject GetNextObjectInCache()
        {
            GameObject obj = null;
            for (var i = 0; i < cacheSize; i++)
            {
                obj = objects[cacheIndex].gameObject;
                if (!obj.activeSelf)
                {
                    break;
                }
                cacheIndex = (cacheIndex + 1) % cacheSize;
            }
            if (obj.activeSelf)
            {
                Debug.LogWarning(GameObjectPool.Instance.prefab.name + obj);
            }
            cacheIndex = (cacheIndex + 1) % cacheSize;
            return obj;
        }
    }

    public GameObject Spawn(Human human, int index)
    {
        if (positions == null)
            SetPositions();

        ObjectCache cache = null;
        if (Pool != null)
        {
            if (prefab)
            {
                cache = Pool.caches;
            }
        }
        if (cache == null)
        {
            return GameObject.Instantiate(prefab, parentCard) as GameObject;
        }

        GameObject obj = cache.GetNextObjectInCache();
        Card card = obj.GetComponent<Card>();
        activeCards.Add(card);
        card.RectTr.anchoredPosition = positions[index].position;
        card.Setup(human);
        obj.SetActive(true);
        Pool.activeCachedObjects[obj] = true;
        return obj;
    }

    public Card GetCard()
    {
        var max = activeCards.Max(a => a.IDInt);
        var result = activeCards.FirstOrDefault(a => a.IDInt == max);
        activeCards.Remove(result);
        return result;
    }

    public void Unspawn(GameObject objectToDestroy)
    {
        if (Pool != null && Pool.activeCachedObjects.ContainsKey(objectToDestroy))
        {
            objectToDestroy.SetActive(false);
            Pool.activeCachedObjects[objectToDestroy] = false;
        }
        else
        {
            Unspawn(objectToDestroy);
        }
    }


}