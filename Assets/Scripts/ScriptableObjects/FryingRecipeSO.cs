using UnityEngine;
using UnityEngine.PlayerLoop;
[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float fryTimerMax;
}
