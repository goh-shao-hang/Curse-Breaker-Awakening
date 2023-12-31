using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private int _growAmount = 10;
    [SerializeField] private T objectToPool;

    private Queue<T> _availableObjects = new Queue<T>();

    private void Awake()
    {
        GrowPool();
    }

    private void GrowPool()
    {
        for (int i = 0; i < _growAmount; i++)
        {
            var instance = Instantiate(objectToPool);
            instance.transform.SetParent(transform);
            ReturnToPool(instance);
        }
    }

    public void ReturnToPool(T instance)
    {
        instance.gameObject.SetActive(false);
        _availableObjects.Enqueue(instance);
    }

    public T GetFromPool()
    {
        if (_availableObjects.Count == 0)
        {
            GrowPool();
        }

        var instance = _availableObjects.Dequeue();
        instance.gameObject.SetActive(true);
        return instance;
    }
}
