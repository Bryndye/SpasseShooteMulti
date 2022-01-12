using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    public TextMeshProUGUI PlayerName, Message, KillerName;
    public Image Fond,Icon;

    [SerializeField] float TimeDisappear = 3.5f;
    float time = 0;

    private void OnEnable()
    {
        time = 0;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= TimeDisappear)
        {
            DisableMySelf();
        }
    }

    void DisableMySelf()
    {
        time = 0;
        gameObject.SetActive(false);
    }
}
