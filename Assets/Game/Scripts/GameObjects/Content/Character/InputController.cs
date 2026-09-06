using Game;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private GameObject _character;

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        Vector3 _moveDirection = Vector3.zero;
        
        if (Input.GetKey(KeyCode.D))
            _moveDirection.x = 1;
        else if(Input.GetKey(KeyCode.A))
            _moveDirection.x = -1;
        
        if (Input.GetKey(KeyCode.Space))
            _moveDirection.y = 1;

        if (_moveDirection != Vector3.zero)
            _character.GetComponent<MoveRequest>().Move(_moveDirection);

        if (Input.GetKeyDown(KeyCode.Space))
            _character.GetComponent<JumpRequest>().JumpUp();
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
            _character.GetComponent<PushRequest>().Push();
        
        if (Input.GetKeyDown(KeyCode.Mouse1))
            _character.GetComponent<FlipRequest>().Flip();
    }
}
