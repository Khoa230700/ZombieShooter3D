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
            Debug.LogWarning("Chưa gán GameObject cần kiểm tra!");
            return;
        }

        List<Component> targetComponents = new List<Component>(targetObject.GetComponents<Component>());
        List<Component> foundComponents = new List<Component>();

        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

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
                                Debug.Log($"Component {comp.GetType().Name} của {targetObject.name} được tham chiếu trong script {script.GetType().Name} của {obj.name}");
                            }
                        }
                    }
                }
            }
        }

        if (foundComponents.Count == 0)
        {
            Debug.Log($"Không tìm thấy script nào tham chiếu đến {targetObject.name}");
        }
    }
}
