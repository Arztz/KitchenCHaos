using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;
    private IHasProgress hasProgress;
    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if(hasProgress == null){
            Debug.LogError("Game Object: " + hasProgressGameObject +" doen't have IHasProgress");
        }
        hasProgress.OnProgressChanged += HasProgress_OnprogressChanged;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void HasProgress_OnprogressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;
        if(e.progressNormalized == 0f || e.progressNormalized == 1f){
            Hide();
        }else{
            Show();
        }
    }

    private void Show(){
        gameObject.SetActive(true);
    }
    private void Hide(){
        gameObject.SetActive(false);
    }
}
