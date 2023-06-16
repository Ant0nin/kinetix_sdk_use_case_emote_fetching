using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using Kinetix.UI;
using Kinetix;
using TMPro;
using UnityEngine.Assertions;

public class CustomCharacterController : MonoBehaviour
{
    public ExampleCharacterController Character;
    public ExampleCharacterCamera CharacterCamera;
    public TMP_Text _txtActionKeyMessage;
    private EmoteShopArea _currentShopArea;

    private const string MouseXInput = "Mouse X";
    private const string MouseYInput = "Mouse Y";
    private const string MouseScrollInput = "Mouse ScrollWheel";
    private const string HorizontalInput = "Horizontal";
    private const string VerticalInput = "Vertical";

    private void Start()
    {
        Assert.IsNotNull(Character);
        Assert.IsNotNull(CharacterCamera);
        Assert.IsNotNull(_txtActionKeyMessage);

        _txtActionKeyMessage.enabled = false;
        _currentShopArea = null;
        Cursor.lockState = CursorLockMode.Locked;

        // Tell camera to follow transform
        CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);

        // Ignore the character's collider(s) for camera obstruction checks
        CharacterCamera.IgnoredColliders.Clear();
        CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
    }

    public void OnEnterEmoteShopArea(EmoteShopArea area) {
        _currentShopArea = area;
    }
    public void OnExitEmoteShopArea(EmoteShopArea area) {
        Assert.AreEqual(_currentShopArea, area);
        if(_currentShopArea.IsShopUiOpen()) {
            _currentShopArea.CloseShopUi();
        }
        _currentShopArea = null;
        _txtActionKeyMessage.enabled = false;
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //}

        bool uiShopOnTop = _currentShopArea && _currentShopArea.IsShopUiOpen();

        if(!uiShopOnTop)
        {
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                if (KinetixUI.IsShown) {
                    KinetixUI.HideAll();
                }
                else {
                    KinetixUI.Show();
                }
            }
        }

        if(!KinetixUI.IsShown)
        {
            if (_currentShopArea)
            {
                if (_currentShopArea.IsShopUiOpen())
                {
                    _txtActionKeyMessage.enabled = false;
                    if (Input.GetKey(KeyCode.Escape)) {
                        _currentShopArea.CloseShopUi();
                    }
                }
                else {
                    _txtActionKeyMessage.enabled = true;
                    if (Input.GetKey(KeyCode.E)) {
                        _currentShopArea.OpenShopUi();
                    }
                }
            }
        }
        else {
            _txtActionKeyMessage.enabled = false;
        }

        if (KinetixUI.IsShown || uiShopOnTop) {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else {
            Cursor.lockState = CursorLockMode.Locked;
        }

        HandleCharacterInput();
    }

    private void LateUpdate()
    {
        // Handle rotating the camera along with physics movers
        if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
        {
            CharacterCamera.PlanarDirection = Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * CharacterCamera.PlanarDirection;
            CharacterCamera.PlanarDirection = Vector3.ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
        }

        HandleCameraInput();
    }

    private void HandleCameraInput()
    {
        // Create the look input vector for the camera
        float mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);
        float mouseLookAxisRight = Input.GetAxisRaw(MouseXInput);
        Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

        // Prevent moving the camera while the cursor isn't locked
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            lookInputVector = Vector3.zero;
        }

        // Input for zooming the camera (disabled in WebGL because it can cause problems)
        float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
    scrollInput = 0f;
#endif

        // Apply inputs to the camera
        CharacterCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

        // Handle toggling zoom level
        if (Input.GetMouseButtonDown(1))
        {
            CharacterCamera.TargetDistance = (CharacterCamera.TargetDistance == 0f) ? CharacterCamera.DefaultDistance : 0f;
        }
    }

    private void HandleCharacterInput()
    {
        PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

        // Build the CharacterInputs struct
        characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
        characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);
        characterInputs.CameraRotation = CharacterCamera.Transform.rotation;
        characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Space);
        characterInputs.CrouchDown = Input.GetKeyDown(KeyCode.C);
        characterInputs.CrouchUp = Input.GetKeyUp(KeyCode.C);

        // Apply inputs to character
        Character.SetInputs(ref characterInputs);
    }
}
