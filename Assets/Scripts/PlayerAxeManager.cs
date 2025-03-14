using System;
using Content.Scripts.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class PlayerAxeManager : MonoBehaviour
{
    private int _currentAxeCount = 1;
    private int _maxAxeCount = 1;
    public bool HasAxe => _currentAxeCount > 0;

    private bool _throwQueued = false;

    [SerializeField] private InputActionReference swingAction;
    [SerializeField] private InputActionReference throwAction;
    [SerializeField] private InputActionReference recallAction;

    private Animator _animator;
    private LauncherComponent _launcher;
    [SerializeField] private HealthChangeBoxComponent healthChangeBox;
    [SerializeField] private InteractorComponent interactor;

    [SerializeField] private GameObject axeMesh;

    [SerializeField] private VisualEffectAsset gib;

    [SerializeField] private AudioClip swingSound;
    [SerializeField] private AudioClip throwSound;
    
    [SerializeField] private Transform head;

    
    [SerializeField] private AudioClip fleshSound;
    [SerializeField] private AudioClip boneSound;
    [SerializeField] private AudioClip metalSound;
    
    private void Start()
    {
        _launcher = GetComponent<LauncherComponent>();
        _animator = GetComponent<Animator>();

        DeactivateDamage();
    }

    public void ActivateDamage()
    {
        healthChangeBox.enabled = true;
        interactor.ActivateInteractable();

        AudioManager.Instance.SpawnSound(swingSound);
        
        if (Physics.Raycast(head.position, head.forward, out RaycastHit hit, 2f,
                LayerMask.GetMask("Geometry"))){
            Debug.Log(hit.collider.gameObject.tag);
            switch (hit.collider.gameObject.tag){
                case "bone":
                    AudioManager.Instance.SpawnSound(boneSound);
                    break;
                case "metal":
                    AudioManager.Instance.SpawnSound(metalSound);
                    break;
                case "film":
                    AudioManager.Instance.SpawnSound(fleshSound);
                    break;
                default:
                    AudioManager.Instance.SpawnSound(fleshSound);
                    VFXManager.Instance.SpawnVFX(gib, 1.0f, hit.point, Quaternion.identity);
                    break;
            }
        }
    }

    public void DeactivateDamage()
    {
        healthChangeBox.enabled = false;
        interactor.DeactivateInteractable();
    }

    public void ChangeAxeCount(int delta)
    {
        _currentAxeCount += delta;

        if (_currentAxeCount < 0)
            _currentAxeCount = 0;

        if (_currentAxeCount > _maxAxeCount)
            _currentAxeCount = _maxAxeCount;

        SetActiveAxe(_currentAxeCount != 0);
    }

    private void SetActiveAxe(bool active)
    {
        axeMesh.SetActive(active);
    }

    public void SetAxeTier(int newTier)
    {
        healthChangeBox.tier = newTier;
    }

    public void SetAxeProjectile(ProjectileData newData)
    {
        _launcher.projectileData = newData;
    }

    private void OnEnable()
    {
        swingAction.action.Enable();
        throwAction.action.Enable();
        recallAction.action.Enable();

        swingAction.action.performed += OnSwingPerformed;

        throwAction.action.performed += OnThrowPerformed;

        recallAction.action.started += OnRecallPerformed;
    }

    private void OnSwingPerformed(InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.paused)
            return;

        if (!HasAxe)
            return;

        // Debug.Log("Swing performed");
        _animator.Play("Swing");
    }

    private void OnThrowPrepared(InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.paused)
            return;
        if (HasAxe){
            _throwQueued = true;
            // Debug.Log("Throw prepared");
        }
    }

    private void OnThrowReleased(InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.paused)
            return;
        if (_throwQueued){
            _throwQueued = false;
            // Debug.Log("Axe Thrown.");
            AudioManager.Instance.SpawnSound(throwSound);
            LaunchParameters launchParameters = new LaunchParameters(Camera.main.transform.forward, 5f);
            _launcher.Launch(launchParameters);
            ChangeAxeCount(-1);
        }
    }

    private void OnRecallPerformed(InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.paused)
            return;
        // Debug.Log("Recall performed");
    }


    private void OnDisable()
    {
        swingAction.action.Disable();
        throwAction.action.Disable();
        recallAction.action.Disable();

        swingAction.action.performed -= OnSwingPerformed;

        throwAction.action.performed -= OnThrowPerformed;

        recallAction.action.started -= OnRecallPerformed;
    }

    private void OnThrowPerformed(InputAction.CallbackContext obj)
    {
        OnThrowPrepared(obj);
        OnThrowReleased(obj);
    }
}