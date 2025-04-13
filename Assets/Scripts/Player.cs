using UnityEngine;
using UnityEngine.AI;
public class Player: MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 11f;
    [SerializeField] private GameInput gameInput;
    private bool isWalking = false;
    private void Update(){
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x,0f,inputVector.y);
        isWalking = moveDir != Vector3.zero;
        transform.position += moveDir *moveSpeed* Time.deltaTime;
        transform.forward = Vector3.Slerp(transform.forward,moveDir,Time.deltaTime*rotateSpeed);
    }

    public bool IsWalking(){
        return isWalking;
    }
}
