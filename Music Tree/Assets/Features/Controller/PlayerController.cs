using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, ICharacterSignals
{
    [Header("References")]
    public PlayerControllerInput playerController;
    private CharacterController _characterController;
    private Camera _camera;
    
    [Header("Player Properties")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runSpeed = 10f;
    
    [SerializeField] private float toGround = 3f;
    [SerializeField] private float jumpSpeed = 10f;
    
    [SerializeField] private float smoothLooking = 100f;
    private Vector2 _smoothLook = Vector2.zero;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float minLookAngle = -60f;
    [SerializeField] private float maxLookAngle = 60f;

    private bool _isCrouch;
    private bool _doingCrouch;
    [SerializeField] private float height = 4f;
    [SerializeField] private float crouchHeight = 2f;
    private float _cameraPosRatio;
    private Vector3 _center = Vector3.zero;
    private Vector3 _crouchCenter;
    [SerializeField] private float timeToCrouch = 0.5f;
    [SerializeField] private float crouchSpeed = 2f;
    private List<float> _heightList = new();
    private List<Vector3> _cameraPosList = new();
    private List<Vector3> _centerList = new();
    
    //bearbeiten ICharacterSignals
    private Subject<Vector3> _moved = new();
    private ReactiveProperty<bool> _isRunning;
    [SerializeField] private float strideLength = 2.5f;

    public float StrideLength => strideLength;
    public ReactiveProperty<bool> IsRunning => _isRunning;
    public IObservable<Vector3> Moved => _moved;
    public IObservable<Unit> Landed { get; }
    public IObservable<Unit> Jumped { get; }
    public IObservable<Unit> Stepped { get; }
    
    //singleton
    private static PlayerController _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        _characterController = GetComponent<CharacterController>();
        _camera = GetComponentInChildren<Camera>();
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _cameraPosRatio = _camera.transform.localPosition.y / height;
        _crouchCenter = new Vector3(0f, (crouchHeight - height) / 2, 0f);

        _moved.AddTo(this);
        _isRunning = new ReactiveProperty<bool>(false);
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        bool isGrounded;
        bool isJump;
        float jump;
        playerController.move.Subscribe(movDir =>
        {
            //Berechnung von Jump
            isGrounded = _characterController.isGrounded;
            isJump = playerController.jump.Value;
            if (isGrounded && isJump)
            {
                jump = jumpSpeed;
            } else if (isGrounded)
            {
                jump = -1f;
            }
            else
            {
                var velocity = _characterController.velocity;
                jump = velocity.y + Physics.gravity.y * Time.deltaTime * toGround;
                var jumpMotion = new Vector3(velocity.x, jump, velocity.z) * Time.deltaTime;
                _characterController.Move(jumpMotion);
                return;
            }
            
            //Berechnung von Bewegung
            float moveSpeed;
            if (!_isCrouch && !_doingCrouch)
            {
                moveSpeed = playerController.run.Value ? runSpeed : walkSpeed;
            }
            else
            {
                moveSpeed = crouchSpeed;
            }
            movDir *= moveSpeed;
            var localMove = new Vector3(movDir.x, jump, movDir.y);
            var worldMove = transform.TransformVector(localMove);
            var motion = worldMove * Time.deltaTime;
            _characterController.Move(motion);
            
            //senden CameraBob Signal
            var tempIsRunning = false;
            if (isGrounded && !_isCrouch)
            {
                _moved.OnNext(_characterController.velocity * Time.deltaTime);
                if (_characterController.velocity.magnitude > 0) 
                    tempIsRunning = playerController.run.Value;
            }
            _isRunning.Value = tempIsRunning;
        }).AddTo(this);

        playerController.crouch.Where(signal => signal)
            .Subscribe(_ =>
            {
                if (!_doingCrouch) StartCoroutine(Crouch());
            }).AddTo(this);
        _camera.transform.localRotation = Quaternion.identity;
        playerController.look.Where(vector => vector != Vector2.zero)
            .Subscribe(rawLook =>
        {
            //smooth Rotation
            var interpolatation = smoothLooking * Time.deltaTime;
            _smoothLook = new Vector2(Mathf.Lerp(_smoothLook.x, rawLook.x, interpolatation),
                Mathf.Lerp(_smoothLook.y, rawLook.y, interpolatation));
            
            //waagerecht
            var horizontalLook = rotationSpeed * _smoothLook.x * Vector3.up * Time.deltaTime;
            transform.localRotation *= Quaternion.Euler(horizontalLook);
            
            //senkrecht
            var verticalLook = rotationSpeed * _smoothLook.y * Vector3.left * Time.deltaTime;
            //clamp rotation
            var newQ = _camera.transform.localRotation * Quaternion.Euler(verticalLook);
            var euler = newQ.eulerAngles;
            if (euler.y == 180) return;
            if (euler.x > 180) euler.x -= 360;
            euler.x = Mathf.Clamp(euler.x, -maxLookAngle, -minLookAngle);
            _camera.transform.localRotation = Quaternion.Euler(euler);
        }).AddTo(this);
    }

    private IEnumerator Crouch()
    {
        _doingCrouch = true;
        float elaspedTime = 0f;
        float targetHeight = _isCrouch ? height : crouchHeight;
        float currentHeight = _characterController.height;
        Vector3 currentCenter = _characterController.center;
        Vector3 targetCenter = _isCrouch ? _center : _crouchCenter;
        
        _heightList.Add(currentHeight);
        _cameraPosList.Add(_camera.transform.localPosition);
        _centerList.Add(currentCenter);
        
        while (elaspedTime < timeToCrouch)
        {
            //vermeiden, dass man beim Aufstehen ein Hindernis begegnet. Falls behindert, zuruecksetzen
            if (_isCrouch && Physics.Raycast(_camera.transform.position, Vector3.up, 2f))
            {
                var total = _heightList.Count;
                while (total > 0)
                {
                    _characterController.height = _heightList[total - 1];
                    _camera.transform.localPosition = _cameraPosList[total - 1];
                    _characterController.center = _centerList[total - 1];
                    total--;
                    yield return null;
                }

                _heightList = new List<float>();
                _cameraPosList = new List<Vector3>();
                _centerList = new List<Vector3>();
                
                _doingCrouch = false;
                yield break;
            }
            
            //kontinuierlich sneaken
            float crouchPercentage = elaspedTime / timeToCrouch;
            _characterController.height = Mathf.Lerp(currentHeight, targetHeight, crouchPercentage);
            _camera.transform.localPosition = new Vector3(0f, _cameraPosRatio * _characterController.height + _characterController.center.y, 0f);
            _characterController.center = Vector3.Lerp(currentCenter, targetCenter, crouchPercentage);
            
            _heightList.Add(_characterController.height);
            _cameraPosList.Add(_camera.transform.localPosition);
            _centerList.Add(_characterController.center);
            
            elaspedTime += Time.deltaTime;
            yield return null;
        }
        
        //Resultat
        _isCrouch = !_isCrouch;
        _characterController.height = targetHeight;
        _camera.transform.localPosition = new Vector3(0f, _cameraPosRatio * targetHeight + _characterController.center.y, 0f);
        _characterController.center = targetCenter;
        
        _heightList = new List<float>();
        _cameraPosList = new List<Vector3>();
        _centerList = new List<Vector3>();
        
        _doingCrouch = false;
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Platform"))
        {
            SceneManager.LoadScene(hit.gameObject.name);
        } else if (hit.transform.CompareTag("Music Wall"))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}