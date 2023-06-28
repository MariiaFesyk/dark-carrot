using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
    [SerializeField] private string startingScene;

    public void Play(){
        gameObject.SetActive(false);
        Loader.LoadScene(startingScene);
    }
    public void Exit(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
