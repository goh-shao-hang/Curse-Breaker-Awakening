using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class ModuleManager : MonoBehaviour
{
    public readonly Dictionary<Type, Module> Modules = new Dictionary<Type, Module>();

    private void Awake()
    {
        Module[] modules = GetComponents<Module>();
        foreach (Module module in modules)
        {
            if (!Modules.ContainsKey(module.ModuleType))
            {
                Modules.Add(module.ModuleType, module);
            }
            else
            {
                Debug.LogError($"More than one instances of Module {module.ModuleType} exist in {transform.parent.name}!");
            }
        }
    }

    public T GetModule<T>() where T : Module
    {
        if (Modules.ContainsKey(typeof(T)))
        {
            return Modules[typeof(T)] as T;
        }
        else
        {
            return null;
        }
    }

    /*public Module GetModule(Type type)
    {
        if (Modules.ContainsKey(type))
        {
            return Modules[type];
        }
        else
        {
            return null;
        }
    }*/
}
