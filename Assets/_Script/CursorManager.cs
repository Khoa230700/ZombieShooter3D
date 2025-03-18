using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("Danh sách các Object")]
    public List<GameObject> trackedObjects;

    void Awake()
    {
        LockCursor();
    }

    void Update()
    {
        if (ShouldShowCursor())
        {
            UnlockCursor();
        }
        else if (Cursor.lockState != CursorLockMode.Locked)
        {
            LockCursor();
        }
        Debug.Log($"Cursor.lockState: {Cursor.lockState}, Cursor.visible: {Cursor.visible}");
    }

    private bool ShouldShowCursor()
    {
        foreach (var obj in trackedObjects)
        {
            if (obj != null && obj.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
