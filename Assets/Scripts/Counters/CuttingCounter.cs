using System;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingCounter : BaseCounter , IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler  OnCut;
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;
    public override void Interact(Player player)
    {
        if(!HasKitchenObject()){
            // no kitchen object
            if(player.HasKitchenObject() && HasReciprWithInput(player.GetKitchenObject().GetKitchenObjectSO())){
                //player carry object
                player.GetKitchenObject().SetKithenObjectParent(this);
                cuttingProgress = 0;

            }else
            {
                //player notcarry object
            }
        }else{
            if(!player.HasKitchenObject()){
                GetKitchenObject().SetKithenObjectParent(player);
            }else{
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player carry plate
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())){
                        GetKitchenObject().DestroySelf();
                    }
                }
            }   
        }
    
    }
    public override void InteractAlternate(Player player){
        if(HasKitchenObject() && HasReciprWithInput(GetKitchenObject().GetKitchenObjectSO())){
            //cut object
            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
            if(cuttingProgress >= cuttingRecipeSO.cuttingProgressMax){
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO,this); 
            }
        }
    }

    private bool HasReciprWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO!=null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO){
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if(cuttingRecipeSO!=null){
            return cuttingRecipeSO.output;
        }else{
            return null;}
    }
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO){
        foreach(CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if(inputKitchenObjectSO == cuttingRecipeSO.input ){
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
