using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPool : MonoBehaviour
{
    private static RocketPool instance;

    public static RocketPool Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private Rigidbody rocketPrefab;

    [SerializeField]
    private int size;

    private List<Rigidbody> rockets;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            PrepareRocket();
        }
        else
            Destroy(gameObject);
    }

    private void PrepareRocket()
    {
        rockets = new List<Rigidbody>();
        for (int i = 0; i < size; i++)
            AddRocket();
    }

    public Rigidbody GetRocket()
    {
        if (rockets.Count == 0)
            AddRocket();
        return AllocateRocket();
    }

    public void ReleaseRocket(Rigidbody rocket)
    {
        rocket.gameObject.SetActive(false);
        rockets.Add(rocket);
    }

    private void AddRocket()
    {
        Rigidbody instance = Instantiate(rocketPrefab);
        instance.gameObject.SetActive(false);
        rockets.Add(instance);
    }

    private Rigidbody AllocateRocket()
    {
        Rigidbody rocket = rockets[rockets.Count - 1];
        rockets.RemoveAt(rockets.Count - 1);
        rocket.gameObject.SetActive(true);
        return rocket;
    }
}
