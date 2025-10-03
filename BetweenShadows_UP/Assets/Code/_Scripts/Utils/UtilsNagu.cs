using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UtilsNagu
{
    public static Vector3 GetCameraForwardNormalized(Transform cam)
    {
        Vector3 forward = cam.forward;
        forward.y = 0;
        return forward.normalized;
    }

    public static Vector3 GetCameraRightNormalized(Transform cam)
    {
        Vector3 right = cam.right;
        right.y = 0;
        return right.normalized;
    }
    
    public static void AddToList<T>(List<T> list, T obj)
    {
        if(!list.Contains(obj))
        {
            list.Add(obj);
        }
    }

    public static void RemoveFromList<T>(List<T> list, T obj)
    {
        if (list.Contains(obj))
        {
            list.Remove(obj);
        }
    }
    
    public static void RemoveAllInactive(ref List<GameObject> list)
    {
        List<GameObject> withoutInactive = new List<GameObject>();
        foreach (GameObject item in list)
        {
            if (item != null)
            {
                if (item.activeSelf)
                {
                    withoutInactive.Add(item);
                }
            }
        }
        list = withoutInactive;
    }

    public static bool DoListsMatch<T>(List<T> list1, List<T> list2)
    {
        var areListsEqual = true;

        if (list1.Count != list2.Count)
            return false;

        //list1.Sort(); // Sort list one
        //list2.Sort(); // Sort list two

        for (var i = 0; i < list1.Count; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(list2[i], list1[i]))
            {
                areListsEqual = false;
            }
        }

        return areListsEqual;
    }

    public static bool ListContainsAnotherListFromStart<T>(List<T> listaPrincipal, List<T> listaSecundaria)
    {
        if (listaSecundaria.Count > listaPrincipal.Count)
        {
            return false;
        }

        for (int i = 0; i < listaSecundaria.Count; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(listaPrincipal[i], listaSecundaria[i]))
            {
                return false;
            }
        }

        return true;
    }
    
    public static Vector3 GetRandomNavmeshPoint(Vector3 point, float area)
    {
        Vector3 randomDir = Random.insideUnitSphere * area;
        randomDir += point;
        NavMeshHit hit;
        Vector3 finalPos = Vector3.zero;
        if (NavMesh.SamplePosition(randomDir, out hit, area, 1))
        {
            finalPos = hit.position;
        }
        return finalPos;
    }
}
