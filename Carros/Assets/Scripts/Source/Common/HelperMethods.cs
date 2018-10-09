// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections.Generic;
using UnityEngine;

public struct RayData
{
    private LayerMask targetLayer;
    private Vector3 initialPoint;
    private Vector3 direction;
    private float range;

    public LayerMask GetTargetLayer { get { return targetLayer; } }
    public Vector3 GetInitialPoint { get { return initialPoint; } }
    public Vector3 GetDirection { get { return direction; } }
    public float GetRange { get { return range; } }

    public RayData(LayerMask _targetLayer, Vector3 _initialPoint, Vector3 _direction, float _range)
    {
        targetLayer = _targetLayer;
        initialPoint = _initialPoint;
        direction = _direction;
        range = _range;
        return;
    }
}

public static class HelperMethods
{
    public static T GetComponentAtMinimumDistanceInColection<T>(List<T> _collection, Transform _collectionOwner) where T : Component
    {
        T nearestElement = null;
        if (_collection.Count > 0)
        {
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
        }
        return nearestElement;
    }

    public static List<T> ClearInactiveComponentsInCollection<T>(List<T> _collection) where T : Component
    {
        if (_collection.Count > 0)
        {
            for (int i = 0; i < _collection.Count; i++)
            {
                T element = _collection[i];
                if (element != null && !element.gameObject.activeInHierarchy)
                {
                    _collection.RemoveAt(i);
                    continue;
                }
            }
        }
        return _collection;
    }

    public static void DebugMessageWithTimeStamp(string _customMessage)
    {
        Debug.Log(Time.timeSinceLevelLoad + " " + _customMessage);
        return;
    }

    public static void DebugMessageWithTimeStamp(object _elementToDebug, string _customMessage = "")
    {
        Debug.Log(Time.timeSinceLevelLoad + " " + _customMessage + " " + _elementToDebug.ToString());
        return;
    }

    /// <summary>
    /// Cast a Ray and check for any GameObject hit. Can return null if no object is hit.
    /// </summary>
    /// <param name="_parameters"></param>
    /// <returns></returns>
    public static GameObject GetFirstGameObjectHitByRay(RayData _parameters)
    {
        GameObject objectHitByRay = null;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        bool hitsObject = Physics.Raycast(
            _parameters.GetInitialPoint, 
            _parameters.GetDirection, 
            out hit, 
            _parameters.GetRange, 
            _parameters.GetTargetLayer
            );
        if (hitsObject) {
            objectHitByRay = hit.collider.gameObject;
        }
        return objectHitByRay;
    }

    /// <summary>
    /// Cast a Ray and check for any GameObject hit. Can return null if no object is hit.
    /// </summary>
    /// <param name="_parameters"></param>
    /// <returns></returns>
    public static RaycastHit GetFirstHitInformation(RayData _parameters)
    {
        RaycastHit hitToReturn;
        // Does the ray intersect any objects excluding the player layer
        bool hitsObject = Physics.Raycast(
            _parameters.GetInitialPoint,
            _parameters.GetDirection,
            out hitToReturn,
            _parameters.GetRange,
            _parameters.GetTargetLayer
            );
        return hitToReturn;
    }
}
