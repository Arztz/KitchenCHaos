using UnityEngine;
using UnityEngine.PlayerLoop;
[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{

    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public int cuttingProgressMax;
}
