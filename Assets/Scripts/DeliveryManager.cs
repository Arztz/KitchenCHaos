using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public static DeliveryManager Instance{get; private set;}
    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;

    private int waitingRecipeMax = 4;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if(spawnRecipeTimer <= 0f){
            spawnRecipeTimer = spawnRecipeTimerMax;

            if(waitingRecipeSOList.Count < waitingRecipeMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0,recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this,EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject){
        for(int i=0; i < waitingRecipeSOList.Count; i++){
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if(waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count){
                //Has same number og ingredients

                bool plateContentsMatchesRecipe = true;
                foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList){
                    //all ingredient in recipe
                    bool ingredientFound = false;
                    foreach(KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()){
                        //all ingredient in plate
                        print(plateKitchenObjectSO);
                        
                        if(plateKitchenObjectSO == recipeKitchenObjectSO){
                            //ingredient match
                            ingredientFound = true;
                            break;
                        }
                    }
                    if(!ingredientFound){
                        //This recipe was not found on plate
                        plateContentsMatchesRecipe = false;
                    }
                }
                if(plateContentsMatchesRecipe){
                    //Player dirivered correct recipe
                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this,EventArgs.Empty);
                    return;
                }
            }
        }

        //No match found
        //Player cannot plate
    }

    public List<RecipeSO> GetWaitingRecipeSOList(){
        return waitingRecipeSOList;
    }
}
