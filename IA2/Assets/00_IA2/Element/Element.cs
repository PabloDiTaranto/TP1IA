using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element
{
    public object elem;
    public Element(object elem)
    {
        this.elem = elem;
    }
    public T GetValue<T>()
    {
        Debug.Log("original"+elem);
        Debug.Log("cast"+(T)elem);
        return (T)elem;
    }
    public void SetValue<T>(T newElem)
    {
        elem = newElem;
    }

    public Element Clone()
    {
        return new Element(elem);
    }

}
