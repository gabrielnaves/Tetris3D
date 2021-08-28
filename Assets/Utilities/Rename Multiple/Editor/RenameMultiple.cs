using System.IO;
using UnityEngine;
using UnityEditor;

public class RenameMultiple : EditorWindow {

    [MenuItem("Assets/Rename Multiple &r")]
    static public void Init() {
        var window = GetWindowWithRect<RenameMultiple>(new Rect(0, 0, 300, 60), true, "Rename Multiple", true);
        window.minSize = new Vector2(300f, 62f);
    }

    public string pattern = "";
    public string replacement = "";

    void OnGUI() {
        var labelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 50;
        pattern = EditorGUILayout.TextField("Replace", pattern);
        replacement = EditorGUILayout.TextField("With", replacement);
        if (GUILayout.Button("Rename") && !string.IsNullOrEmpty(pattern))
            Rename();
        EditorGUIUtility.labelWidth = labelWidth;
    }

    void Rename() {
        foreach (var obj in Selection.objects) {
            if (ObjectIsNotScript(obj)) {
                string path = AssetDatabase.GetAssetPath(obj);
                if (IsGameObject(obj))
                    ApplyRenamingToGameObject(obj);
                else if (AssetDatabase.IsValidFolder(path))
                    ApplyRenamingToFilesInFolder(path);
                else if (!string.IsNullOrEmpty(path))
                    RenameAssetAt(path);
            }
        }
        AssetDatabase.Refresh();
    }

    bool ObjectIsNotScript(Object obj) => obj.GetType() != typeof(MonoScript);
    bool IsGameObject(Object obj) => obj.GetType() == typeof(GameObject);

    void ApplyRenamingToGameObject(Object obj) {
        string targetName = obj.name.Replace(pattern, replacement);
        Undo.RecordObject(obj, $"Renamed object {obj.name} to {targetName}");
        obj.name = targetName;
    }

    void ApplyRenamingToFilesInFolder(string folderPath) {
        var files = Directory.GetFiles(folderPath);
        foreach (var file in files) {
            if (!file.Contains(".meta")) {
                string path = file.Replace('\\', '/');
                RenameAssetAt(path);
            }
        }
    }

    void RenameAssetAt(string path) {
        string pathPrefix = ExtractPathPrefixFrom(path);
        string fileExtension = ExtractFileExtensionFrom(path);
        string fileName = path.Substring(0, path.Length - fileExtension.Length).Substring(pathPrefix.Length);
        string targetName = fileName.Replace(pattern, replacement);
        string targetPath = pathPrefix + targetName + fileExtension;
        File.Move(path, targetPath);
        File.Move(path + ".meta", targetPath + ".meta");
    }

    string ExtractPathPrefixFrom(string path) {
        string[] substrings = path.Split('/');
        string result = "";
        for (int i = 0; i < substrings.Length - 1; ++i)
            result += substrings[i] + "/";
        return result;
    }

    string ExtractFileExtensionFrom(string path) {
        string[] substrings = path.Split('.');
        return "." + substrings[substrings.Length - 1];
    }
}
