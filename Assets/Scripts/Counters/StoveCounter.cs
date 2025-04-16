using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class StoveCounter : BaseCounter,IHasProgress
{
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;    
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs : EventArgs{
        public State state;
    }
    public enum State{
        Idle,
        Frying,
        Fried,
        Burned,
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private State state;
    private float fryingTimer;

    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {
        if(HasKitchenObject()){
            switch(state){
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = (float)fryingTimer / fryingRecipeSO.fryTimerMax
                    });
                    if(fryingTimer > fryingRecipeSO.fryTimerMax){
                        //Fried
                        GetKitchenObject().DestroySelf();
                        
                        KitchenObject newObj = KitchenObject.SpawnKitchenObject(fryingRecipeSO.output,this);
                        SetKitchenObject(newObj);
                        state =State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(fryingRecipeSO.output);
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                        });

                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = (float)burningTimer / burningRecipeSO.burnTimerMax
                    });
                    if(burningRecipeSO != null){
                        if(burningTimer > burningRecipeSO.burnTimerMax){
                            //Fried
                            GetKitchenObject().DestroySelf();
                            KitchenObject.SpawnKitchenObject(burningRecipeSO.output,this);
                            state =State.Burned;
                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                            });
                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                                progressNormalized = 0f
                            });
                    }}
                    break;
                case State.Burned:
                    break;
            }

        }
    }

    public override void Interact(Player player)
    {
        if(!HasKitchenObject()){
            // no kitchen object
            if(player.HasKitchenObject()){
                KitchenObjectSO inPlayerHandedSO = player.GetKitchenObject().GetKitchenObjectSO();
                player.GetKitchenObject().SetKithenObjectParent(this);
                if(HasRecipeWithOutput(inPlayerHandedSO)) {
                    fryingRecipeSO = GetFryingRecipeSOWithOutput(GetKitchenObject().GetKitchenObjectSO());
                    state = State.Fried;
                    
                }else if(HasRecipeWithInput(inPlayerHandedSO)){
                    
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    state = State.Frying;
                }
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                        });
                fryingTimer = 0f;
                burningTimer = 0f;
            }else
            {
                //player notcarry object
            }
        }else{
            if(!player.HasKitchenObject()){
                GetKitchenObject().SetKithenObjectParent(player);
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                        });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized = 0f
                });
            }else{
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player carry plate
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())){
                        GetKitchenObject().DestroySelf();
                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                                    state = state
                                });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                            progressNormalized = 0f
                        });
                    }
                }
            }   
        }
    }
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO!=null;
    }
    private bool HasRecipeWithOutput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithOutput(inputKitchenObjectSO);
        return fryingRecipeSO!=null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO){
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if(fryingRecipeSO!=null){
            return fryingRecipeSO.output;
        }else{
            return null;}
    }
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO){
        foreach(FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if(inputKitchenObjectSO == fryingRecipeSO.input ){
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private FryingRecipeSO GetFryingRecipeSOWithOutput(KitchenObjectSO inputKitchenObjectSO){
        foreach(FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if(inputKitchenObjectSO == fryingRecipeSO.output ){
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO){
        var cookedSO = GetKitchenObject().GetKitchenObjectSO();
        foreach(BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if(inputKitchenObjectSO == burningRecipeSO.input ){

                return burningRecipeSO;
            }
        }
        return null;
    }
}
