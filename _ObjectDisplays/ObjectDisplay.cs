using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectDisplay<T, H> : MonoBehaviour where H : IObjectDisplay<T>
{
    private H[] objDisplays;

    public T item { get; protected set; }

    public event Action<T> OnObjectSet;

    private List<H> updatedDisplays = new List<H>();

    private void Awake()
    {
        GetDisplays();
    }

    public void UpdateInfoDisplays()
    {
        updatedDisplays.Clear();
        GetDisplays();
        for (int i = 0; i < objDisplays.Length; i++)
        {
            if (updatedDisplays.Contains(objDisplays[i]) == false)
            {
                objDisplays[i].Set(item);
                updatedDisplays.Add(objDisplays[i]);
            }
        }
    }

    private void GetDisplays()
    {
        if (objDisplays == null)
            objDisplays = GetComponentsInChildren<H>(true);
    }

    public virtual void Set(T item)
    {
        this.item = item;
        OnObjectSet?.Invoke(item);
        UpdateInfoDisplays();
    }
}
