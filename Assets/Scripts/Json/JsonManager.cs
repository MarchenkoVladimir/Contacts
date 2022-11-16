using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class JsonManager : Singleton<JsonManager>
{
    [SerializeField] private string nameJSON = "test_data";

   [SerializeField] private Human[] humans;
    public Human[] Humans => humans;

    private string json;

    protected override void Setup()
    {
        DontDestroyOnLoad(gameObject);
        json = DownloadJson();
    }

    public string DownloadJson()
    {
        string filePath = nameJSON.Replace(".json", "");

        TextAsset targetFile = Resources.Load<TextAsset>(filePath);
        return targetFile.text;
    }

    public void LoadDataJson()
    {
        string filePath = nameJSON.Replace(".json", "");

        TextAsset targetFile = Resources.Load<TextAsset>(filePath);

        string jsonString = FixJson(targetFile.text);
        humans = FromJson<Human>(jsonString);
    }

    string FixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return UnityEngine.JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
