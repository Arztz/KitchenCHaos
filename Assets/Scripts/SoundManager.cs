using System.Collections;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {get; private set;}
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliverManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliverManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnDropSomething += BaseCounter_OnDropSomething;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash,trashCounter.transform.position);
   
    }

    private void BaseCounter_OnDropSomething(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop,baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        Player player = Player.Instance;
        PlaySound(audioClipRefsSO.objectPickup,player.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop,cuttingCounter.transform.position);
    }

    private void DeliverManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail,deliveryCounter.transform.position);
    }

    private void DeliverManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess,deliveryCounter.transform.position);
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f){
        PlaySound(audioClipArray[Random.Range(0,audioClipArray.Length)],position,volume);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 0.5f){
        AudioSource.PlayClipAtPoint(audioClip,position,volume);
    }
    public void PlayFootstepsSound(Vector3 position,float volume){
        PlaySound(audioClipRefsSO.footstep,position,volume);
    }
}
