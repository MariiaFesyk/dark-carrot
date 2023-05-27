using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/SaveSettings")]
public class SaveSystem : ScriptableObject {
    [SerializeField] private string filename; 
    [SerializeField] private string directory = "C:/Users/user/Desktop/Git/External/Save";//Application.persistentDataPath;
    private string filepath => $"{directory}/{filename}_{profile}.json";
    [System.NonSerialized] private Dictionary<string, string> database = new();
    [System.NonSerialized] private uint profile = 0;

    void OnEnable(){
        LoadProfile();
    }
    void OnDisable(){
        SaveProfile();
    }

    private void LoadProfile(){
        if(!File.Exists(filepath)) return;
        string content = File.ReadAllText(filepath);
        if(content != null) database = JsonUtility.FromJson<Dictionary<string, string>>(content);
    }
    private void SaveProfile(){
        string content = JsonUtility.ToJson(database, false);
        File.WriteAllText(filepath, content);
    }

    public void Save<T>(string key, T value){
        string content = JsonUtility.ToJson(value);
        database.Add(key, content);
    }
    public T Load<T>(string key){
        if(!database.TryGetValue(key, out var value)) return default(T);
        return JsonUtility.FromJson<T>(value);
    }
}
