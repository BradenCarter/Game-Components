using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectDisplay<in T>
{
    void Set(T obj);
}
