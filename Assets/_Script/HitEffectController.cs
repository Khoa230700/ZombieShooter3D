using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HitEffectController : MonoBehaviour
{
    public RawImage imageBodyshot;
    public RawImage imageHeadshot;

    void Start()
    {
        if (imageBodyshot != null) imageBodyshot.gameObject.SetActive(false);
        if (imageHeadshot != null) imageHeadshot.gameObject.SetActive(false);
    }

    public void ShowHitEffect(string hitType)
    {
        if (hitType == "Bodyshot" && imageBodyshot != null)
        {
            StartCoroutine(ShowImageTemporarily(imageBodyshot));
        }
        else if (hitType == "Headshot" && imageHeadshot != null)
        {
            StartCoroutine(ShowImageTemporarily(imageHeadshot));
        }
    }

    private IEnumerator ShowImageTemporarily(RawImage image)
    {
        image.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        image.gameObject.SetActive(false);
    }
}
