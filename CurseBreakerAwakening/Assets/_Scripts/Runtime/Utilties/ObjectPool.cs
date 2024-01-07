using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private int _growAmount = 10;
    private T _objectToPool;

    private Queue<T> _availableObjects = new Queue<T>();

    public ObjectPool(T objectToPool, int growAmount)
    {
        this._growAmount = growAmount;
        this._objectToPool = objectToPool;
    }

    public void GrowPool()
    {
        for (int i = 0; i < _growAmount; i++)
        {
            var instance = GameObject.Instantiate(_objectToPool);
            AddToPool(instance);
        }
    }

    public void AddToPool(T instance)
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

    public T GetFromPool(Vector3 position, Quaternion rotation)
    {
        if (_availableObjects.Count == 0)
        {
            GrowPool();
        }

        var instance = _availableObjects.Dequeue();
        instance.gameObject.SetActive(true);
        instance.transform.SetPositionAndRotation(position, rotation);
        return instance;
    }
}
