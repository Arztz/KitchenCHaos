using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpwanedAmount = 0;
    private int platesSpwanedMax = 4;
    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if(spawnPlateTimer > spawnPlateTimerMax){
            spawnPlateTimer = 0f;
            if(platesSpwanedAmount < platesSpwanedMax)
            {
                platesSpwanedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public override void Interact(Player player){
      if(!player.HasKitchenObject()){
        if(platesSpwanedAmount > 0){
            platesSpwanedAmount--;
            KitchenObject.SpawnKitchenObject(plateKitchenObjectSO,player);
            OnPlateRemoved?.Invoke(this,EventArgs.Empty);
        }
      }
    }
}
