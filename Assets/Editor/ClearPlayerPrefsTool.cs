#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class ClearPlayerPrefsTool
{
    [MenuItem("Tools/Clear All PlayerPrefs")]
    public static void ClearAllPlayerPrefs()
    {
        bool confirm = EditorUtility.DisplayDialog(
            "Clear All PlayerPrefs",
            "Are you sure you want to clear all PlayerPrefs?\nThis action cannot be undone.",
            "Yes",
            "No"
        );

        if (!confirm)
            return;

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("? ALL PLAYER PREFS CLEARED SUCCESSFULLY!");
    }
}
#endif