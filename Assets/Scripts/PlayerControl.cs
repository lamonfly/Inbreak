using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [Header("Player control values")]
    public float rotationSpeed = 5f;
    public float rotationFriction = 1f;
    public float rotationSmoothness = 1f;

    [Header("Pause menu")]
    public GameObject pauseMenu;

    private float inputValue;
    private float prevMousePos;
    private Controls controls = null;
    private bool isPaused = false;

    delegate void onPlay();

    private void Awake()
    {
        controls = new Controls();
        controls.Player.Pause.performed += ctx => PauseGame();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void Start()
    {
        prevMousePos = controls.Player.RotateMouse.ReadValue<float>();

        StartCoroutine(throwBall());
    }

    private void OnDisable()
    {
        controls.Player.Disable();   
    }

    private void Update()
    {
        // Given button 0 to 1 input
        inputValue += controls.Player.Rotate.ReadValue<float>() * rotationSpeed * rotationFriction;

        // Given mouse input
        if (controls.Player.RotateMouse.triggered)
        {
            float mouseChange = controls.Player.RotateMouse.ReadValue<float>() - prevMousePos;
            mouseChange = (mouseChange + 1) / 2;
            mouseChange = Mathf.Clamp(mouseChange, -1, 1);
            inputValue += mouseChange * rotationSpeed * rotationFriction;
            prevMousePos = controls.Player.RotateMouse.ReadValue<float>();
        }

        // Rotate at input speed
        Quaternion QuaternionTo = Quaternion.Euler(0, 0, inputValue);
        // Smooth rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, QuaternionTo, Time.deltaTime * rotationSmoothness);
    }

    // Throw ball into play
    private IEnumerator throwBall()
    {
        yield return new WaitUntil(() => controls.Player.Rotate.triggered || controls.Player.RotateMouse.triggered);
        GameController.Instance.initBall();
    }

    // Pause/Unpause game
    private void PauseGame()
    {
        if (isPaused)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }
}
