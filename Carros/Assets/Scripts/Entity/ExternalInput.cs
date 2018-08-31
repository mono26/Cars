using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalInput : MonoBehaviour
{
    [Header("Editor debugging")]
    [SerializeField]
    protected Vector3 movement;
    public Vector3 Movement { get { return movement; } }
}
