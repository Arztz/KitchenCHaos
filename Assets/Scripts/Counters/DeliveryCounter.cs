using System.Runtime.CompilerServices;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{


    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public static DeliveryCounter Instance {get; private set;}

    private void Awake(){
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Interact(Player player){
        if(player.HasKitchenObject()){
            if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)){
                // player.GetKitchenObject().SetKithenObjectParent(this);
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                player.GetKitchenObject().DestroySelf();
            }
        }
    }

}
