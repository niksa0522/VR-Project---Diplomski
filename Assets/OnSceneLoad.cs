using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class OnSceneLoad : MonoBehaviour
{
    public UnityEvent OnLoad = new UnityEvent();

    private void Awake()
    {
        SceneManager.sceneLoaded += PlayEvent;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= PlayEvent;
    }

    private void PlayEvent(Scene scene, LoadSceneMode mode)
    {
        OnLoad.Invoke();
    }
}
