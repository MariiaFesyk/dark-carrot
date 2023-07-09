using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Loader : MonoBehaviour {
    private static List<AsyncOperation> pending = new();
    private static UnityAction OnLoadingScene;

    public static void LoadScene(string key){
        for(int i = 0; i < SceneManager.sceneCount; i++)
            if(SceneManager.GetSceneAt(i).name == key) return;
        pending.Add(SceneManager.LoadSceneAsync(key, LoadSceneMode.Additive));
        pending = pending.FindAll(pending => !pending.isDone);
        OnLoadingScene?.Invoke();
    }
    public static void UnloadScene(string key){
        pending.Add(SceneManager.UnloadSceneAsync(key));
        pending = pending.FindAll(pending => !pending.isDone);
        OnLoadingScene?.Invoke();
    }

    [SerializeField] private GameObject indicator;
    void Awake(){
        OnLoadingScene += () => {
            if(!indicator.activeSelf)
                StartCoroutine(UpdateLoadingProgress());
        };
    }

    private IEnumerator UpdateLoadingProgress(){
        indicator.SetActive(true);
        while(pending.Count > 0){
            float total = 0f;
            for(int i = pending.Count - 1; i >= 0; i--)
                if(pending[i].isDone) pending.RemoveAt(i);
                else total += pending[i].progress;
            total /= pending.Count;
            yield return null;
        }
        indicator.SetActive(false);
    }
}
