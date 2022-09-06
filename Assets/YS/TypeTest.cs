using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassA
{
    public virtual int GetTypeHashCode()
    {
        return GetType().GetHashCode();
    }
}

public class ClassAB : ClassA
{

}

public class TypeTest : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        ClassA a = new ClassA();
        ClassAB ab = new ClassAB();
        ClassA aInB = new ClassAB();

        Debug.Log("Type Hash");
        Debug.Log(a.GetTypeHashCode());
        Debug.Log(ab.GetTypeHashCode());
        Debug.Log(aInB.GetTypeHashCode());

        Debug.Log("Object Hash");
        Debug.Log(a.GetHashCode());
        Debug.Log(ab.GetHashCode());
        Debug.Log(aInB.GetHashCode());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
