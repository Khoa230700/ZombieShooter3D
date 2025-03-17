using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class ReferenceFinder : MonoBehaviour
{
    public GameObject targetObject;

    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("⚠️ Chưa gán GameObject cần kiểm tra!");
            return;
        }

        FindReferencesToTarget();
        FindAllAudioSourcesInScene();
    }

    // ✅ Tìm tất cả script đang tham chiếu đến targetObject (bao gồm cả Object bị Inactive)
    void FindReferencesToTarget()
    {
        List<Component> targetComponents = new List<Component>(targetObject.GetComponents<Component>());
        List<Component> foundComponents = new List<Component>();

        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (GameObject obj in allObjects)
        {
            MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour script in scripts)
            {
                if (script == null) continue;
                FieldInfo[] fields = script.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (FieldInfo field in fields)
                {
                    foreach (Component comp in targetComponents)
                    {
                        if (field.FieldType == comp.GetType())
                        {
                            object fieldValue = field.GetValue(script);

                            if (fieldValue as Object == comp)
                            {
                                foundComponents.Add(script);
                                Debug.Log($"🔎 Component {comp.GetType().Name} của {targetObject.name} được tham chiếu trong script {script.GetType().Name} của {obj.name}");
                            }
                        }
                    }
                }
            }
        }

        if (foundComponents.Count == 0)
        {
            Debug.Log($"⚠️ Không tìm thấy script nào tham chiếu đến {targetObject.name}");
        }
    }

    // ✅ Tìm tất cả GameObject có chứa AudioSource trong Scene (bao gồm cả Object bị Inactive)
    void FindAllAudioSourcesInScene()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        List<GameObject> objectsWithAudioSource = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.GetComponent<AudioSource>() != null)
            {
                objectsWithAudioSource.Add(obj);
                Debug.Log($"🎵 GameObject {obj.name} có chứa AudioSource.");
            }
        }

        if (objectsWithAudioSource.Count == 0)
        {
            Debug.Log("⚠️ Không có GameObject nào trong Scene chứa AudioSource.");
        }
    }
}
