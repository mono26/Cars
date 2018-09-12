using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public delegate void EventManager();
    public static event EventManager Events;


    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
