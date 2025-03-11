using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button _btnPlay;
    public string sceneName;

    void Start()
    {
        _btnPlay.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // Tạo hiệu ứng khí
        StartCoroutine(TransitionEffect());
    }

    private IEnumerator TransitionEffect()
    {
        // Thêm hiệu ứng, ví dụ: thu nhỏ nút
        Vector3 originalScale = _btnPlay.transform.localScale;
        Vector3 targetScale = originalScale * 0.9f;

        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            _btnPlay.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _btnPlay.transform.localScale = targetScale;
        SceneManager.LoadScene(sceneName);
    }
}
