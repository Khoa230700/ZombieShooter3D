using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("Danh sách các Object")]
    public List<GameObject> trackedObjects;

    void Start()
    {
        Invoke(nameof(HideCursor), 0.1f);
    }

    void Update()
    {
        if (ShouldShowCursor())
        {
            ShowCursor();
        }
        else
        {
            HideCursor();
        }
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

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
