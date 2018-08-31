using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions
{
    public static T GetElementAtMinimumDistanceInColection<T>(List<T> _collection, Transform _collectionOwner) where T : Component 
    {
        if (_collection.Count.Equals(0)) { return null; }

        T nearestElement = null;
        float minimumDistance = float.MaxValue;
        for (int i = 0; i < _collection.Count; i++)
        {
            T elementToCompareTo = _collection[i];
            float distanceToCompareTo = Vector3.Distance(_collectionOwner.position, elementToCompareTo.transform.position);
            if (distanceToCompareTo < minimumDistance)
            {
                minimumDistance = distanceToCompareTo;
                nearestElement = elementToCompareTo;
            }
        }

        return nearestElement;
    }

    public static void ClearInactiveElementsInCollection<T>(ref List<T> _collection) where T : Component
    {
        if (_collection.Count.Equals(0)) { return; }

        for (int i = 0; i < _collection.Count; i++)
        {
            T element = _collection[i];
            if (element != null && !element.gameObject.activeInHierarchy)
            {
                _collection.RemoveAt(i);
                continue;
            }
        }

        return;
    }
}
