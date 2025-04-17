using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;
    private void Start()
    {
        timerImage.fillAmount = 1;
    }
    private void Update()
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        { timerImage.fillAmount = 1; }
        else
        {
            timerImage.fillAmount = KitchenGameManager.Instance.GetGamePlayTimerNormalized();
        }
    }
}
