using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputFieldNavigator : MonoBehaviour
{
    public InputField[] inputFields;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MoveToNextInputField();
        }
    }

    private void MoveToNextInputField()
    {
        int currentIndex = -1;

        for (int i = 0; i < inputFields.Length; i++)
        {
            if (EventSystem.current.currentSelectedGameObject == inputFields[i].gameObject)
            {
                currentIndex = i;
                break;
            }
        }

        if (currentIndex != -1)
        {
            int nextIndex = (currentIndex + 1) % inputFields.Length; // Quay lại đầu khi đến ô cuối
            SetFocusOnInputField(nextIndex);
        }
    }

    private void SetFocusOnInputField(int index)
    {
        inputFields[index].Select();
        inputFields[index].ActivateInputField();
    }
}
