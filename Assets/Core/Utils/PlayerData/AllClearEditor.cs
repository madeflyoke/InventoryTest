#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Core.Utils.PlayerData
{
    public static class AllClearEditor
    {
        [MenuItem("Tools/Clear All Saved Data")]
        private static void ClearAllData()
        {
            // Delete all JSON files
            string dataPath = Application.persistentDataPath;
            if (Directory.Exists(dataPath))
            {
                foreach (string file in Directory.GetFiles(dataPath))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Failed to delete {file}: {e}");
                    }
                }
                Debug.Log("Deleted all saved JSON files.");
            }

            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            Debug.Log("All saved data (JSON + PlayerPrefs) cleared!");
        }
    }
}
#endif