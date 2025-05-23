using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
public class Player: MonoBehaviour,IKitchenObjectParent
{
    public static Player Instance{ get; private set;}
    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSeletedCounterChangedEventArgs> OnSeletedCounterChanged;
    public class OnSeletedCounterChangedEventArgs: EventArgs{
        public BaseCounter selectedCounter;
    }
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 11f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;

    [SerializeField] private Transform kitchenObjectHoldPoint;
    private bool isWalking = false;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if(Instance != null) {
            Debug.LogError("There is more than one player");
        }
        Instance = this;
    }
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction; 
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction; 
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e){
        if(!KitchenGameManager.Instance.IsGamePlaying()){ return;}
        if(selectedCounter != null){
            selectedCounter.Interact(this);
        }
    }
    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e){
        if(!KitchenGameManager.Instance.IsGamePlaying()){ return;}
        if(selectedCounter != null){
            selectedCounter.InteractAlternate(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }
    public bool IsWalking(){
        return isWalking;
    }
    private void HandleInteractions(){
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x,0f,inputVector.y);
        if( moveDir != Vector3.zero){
            lastInteractDir = moveDir;
        }
        float interactDistance = 2f;
        if(Physics.Raycast(transform.position,lastInteractDir, out RaycastHit raycastHit,interactDistance,counterLayerMask)){
            if(raycastHit.transform.TryGetComponent(out BaseCounter BaseCounter)){
                // BaseCounter.Interact();
                
                if(BaseCounter != selectedCounter){
                    SetSelectedCounter(BaseCounter);
                }
            }else{
                SetSelectedCounter(null);

            }

        }else{
            SetSelectedCounter(null);
        }
    }
    private void HandleMovement(){
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x,0f,inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .8f;
        float playerHeight = 2f;
        bool canMove= !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDir,moveDistance);
        // canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDir,moveDistance);

        if(!canMove){
            Vector3 moveDirX = new Vector3(moveDir.x,0,0).normalized;
            canMove= moveDir.x != 0 && !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDirX,moveDistance);
            
            if(canMove)
            {
                moveDir = moveDirX;
            }else {
                Vector3 moveDirZ = new Vector3(0,0,moveDir.z).normalized;
                canMove= moveDir.z != 0 &&  !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDirZ,moveDistance);
                if(canMove){
                    moveDir = moveDirZ;
                }else{
                    //cannot move
                }
            }
        }
    
        if(canMove){
            transform.position += moveDir * moveDistance;
        }
        
        isWalking = moveDir != Vector3.zero;
        if(isWalking){
        transform.forward = Vector3.Slerp(transform.forward,moveDir,Time.deltaTime*rotateSpeed);
        }

    }

    private void SetSelectedCounter(BaseCounter selectedCounter){
        this.selectedCounter = selectedCounter;
        OnSeletedCounterChanged?.Invoke(this,new OnSeletedCounterChangedEventArgs{
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform(){
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject){
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null){
            OnPickedSomething?.Invoke(this,EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject(){
        return kitchenObject;
    }

    public void ClearKitchenObject(){
        kitchenObject = null;
    }

    public bool HasKitchenObject(){
        return kitchenObject != null;
    }
}
