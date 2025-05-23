using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
   [SerializeField] private KitchenObjectSO kitchenObjectSO;

   private IKitchenObjectParent kitchenObjectParent;
   public KitchenObjectSO GetKitchenObjectSO(){
      return kitchenObjectSO;
   }
   
   public void SetKithenObjectParent(IKitchenObjectParent kitchenObjectParent){
      if(this.kitchenObjectParent != null)
      {
         this.kitchenObjectParent.ClearKitchenObject();
      }

      this.kitchenObjectParent = kitchenObjectParent;

      if (kitchenObjectParent.HasKitchenObject()){
         Debug.Log("Already have Object");
      }
      kitchenObjectParent.SetKitchenObject(this);
      transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
      transform.localPosition = Vector3.zero;
   }
   public IKitchenObjectParent GetKitchenObjectParent(){
      return kitchenObjectParent;
   }
   public void DestroySelf(){
      if (kitchenObjectParent != null) {
         kitchenObjectParent.ClearKitchenObject(); // 💥 สำคัญมาก
      }
      Destroy(gameObject);
   }
   public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO,IKitchenObjectParent kitchenObjectParent){
      Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
      KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
      kitchenObject.SetKitchenObjectSO(kitchenObjectSO);
      kitchenObject.SetKithenObjectParent(kitchenObjectParent);
      return kitchenObject;
   }
   public void SetKitchenObjectSO(KitchenObjectSO newSO){
    kitchenObjectSO = newSO;
   }
   public bool TryGetPlate(out PlateKitchenObject plateKitchenObject){
      if(this is PlateKitchenObject){
         plateKitchenObject = this as PlateKitchenObject;
         return true;
      }else{
         plateKitchenObject = null;
         return false;
      }
   }

}
