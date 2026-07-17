using System.Collections; using System.Collections.Generic;
using UnityEngine; using UnityEngine.UI; using System.IO;
using SFB;

public class FileManager : MonoBehaviour {
    public Transform Points;

    public void Save() {
        List<PointPackage> pointList = new List<PointPackage>();
        foreach (Transform point in Points) {
            PointLogic pl;
            if (point.TryGetComponent<PointLogic>(out pl)) {
                string lQ = pl.q;
                string lW = pl.w;
                string lE = pl.e;
                pointList.Add(new PointPackage(lQ, lW, lE));
            }
        }
        string json = JsonUtility.ToJson(new SerializationWrapper<PointPackage>(pointList), true);
        string paths = StandaloneFileBrowser.SaveFilePanel("Save test", "", "points", "json");
        if (!string.IsNullOrEmpty(paths)) {
            File.WriteAllText(paths, json); Debug.Log("Saved at: " + paths);
        } else {
            Debug.LogWarning("Save cancelled or no path selected");
        }
    }
    public void Load() {
        var paths = StandaloneFileBrowser.OpenFilePanel("Load test", "", "json", false);
        if (paths != null && paths.Length > 0) {
            string jsonContent = File.ReadAllText(paths[0]);
            SerializationWrapper<PointPackage> wrapper = JsonUtility.FromJson<SerializationWrapper<PointPackage>>(jsonContent);
            foreach (var pointData in wrapper.list) {
                Debug.Log($"q = {pointData.q}, w = {pointData.w}, e = {pointData.e}");
            }
        }
    }
}

[System.Serializable]
public class PointPackage {
    public string q;
    public string w;
    public string e;

    public PointPackage(string q, string w, string e) {
        this.q = q;
        this.w = w;
        this.e = e;
    }
}

[System.Serializable]
public class SerializationWrapper<T> { public List<T> list; public SerializationWrapper(List<T> list) { this.list = list; } }