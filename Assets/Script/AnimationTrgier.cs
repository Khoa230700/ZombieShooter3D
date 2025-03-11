using UnityEngine;
[System.Serializable]
public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] Zombie zumbie;
    private void Awake()
    {
        zumbie = FindObjectOfType<Zombie>();
    }
    public void OnAnimationTriggered()
    {
        zumbie.Animationtrigger();
    }
    public void AnimationEnd()
    {
        zumbie.animationEnd();
    }
}
