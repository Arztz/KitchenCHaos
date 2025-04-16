using UnityEngine;

public class ClearCounter : BaseCounter
{


    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Interact(Player player){

        if(!HasKitchenObject()){
            // no kitchen object
            if(player.HasKitchenObject()){
                //player carry object
                player.GetKitchenObject().SetKithenObjectParent(this);
            }else
            {
                //player notcarry object
            }
        }else{
            if(!player.HasKitchenObject()){
                GetKitchenObject().SetKithenObjectParent(player);
            }else{
                //player carry something
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player carry plate
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())){
                        GetKitchenObject().DestroySelf();
                    }
                }else {
                    //player not carry plate
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject)){
                        //counter has plate
                        if(plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())){
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }   
        }
    }

}
