using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

public enum GrabType
{
    SideGrab,
    BackGrab,
}

public class PlayerWeaponVisuals : MonoBehaviour
{
    private Animator _anim;
    private bool _isGrabbingWeapon;
    
    [SerializeField] private Transform[] gunTransforms;
    
    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform sniperRifle;

    private Transform _currentGun;

    [Header("Rig")]
    [SerializeField] private float rigWeightIncreaseRate;
    private Rig _rig;
    private bool _shouldIncreaseRigWeight;
    
    [Header("Left hand IK")]
    [SerializeField] private float leftHandIKWeightIncreaseRate;
    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    private bool _shouldIncreaseLeftHandIKWeight;


    private void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _rig = GetComponentInChildren<Rig>();

        SwitchOnGun(pistol);
    }

    private void Update()
    {
        CheckWeaponSwitch();

        if (Input.GetKeyDown(KeyCode.R) && _isGrabbingWeapon == false)
        {
            _anim.SetTrigger("Reload");
            ReduceRigWeight();
        }

        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }

    private void UpdateLeftHandIKWeight()
    {
        if (_shouldIncreaseLeftHandIKWeight)
        {
            leftHandIK.weight += leftHandIKWeightIncreaseRate * Time.deltaTime;
            if (leftHandIK.weight >= 1)
            {
                _shouldIncreaseLeftHandIKWeight = false;
            }
        }
    }

    private void UpdateRigWeight()
    {
        if (_shouldIncreaseRigWeight)
        {
            _rig.weight += rigWeightIncreaseRate * Time.deltaTime;
            if (_rig.weight >= 1)
            {
                _shouldIncreaseRigWeight = false;
            }
        }
    }

    private void ReduceRigWeight()
    {
        _rig.weight = 0.15f;
    }

    private void PlayWeaponGrabAnimation(GrabType grabType)
    {
        leftHandIK.weight = 0;
        ReduceRigWeight();
        _anim.SetFloat("WeaponGrabType", (float)grabType);
        _anim.SetTrigger("Grab");

        SetBusyGrabbingWeapon(true);
    }

    public void SetBusyGrabbingWeapon(bool isBusy)
    {
        _isGrabbingWeapon = isBusy;
        _anim.SetBool("BusyGrabbingWeapon", _isGrabbingWeapon);
    }

    public void MaximizeRigWeight() => _shouldIncreaseRigWeight = true;
    public void MaximizeLeftHandWeight() => _shouldIncreaseLeftHandIKWeight = true;

    private void SwitchOnGun(Transform gunTransform)
    {
        SwitchOffGuns();
        gunTransform.gameObject.SetActive(true);
        _currentGun = gunTransform;
        AttachLeftHand();
    }

    private void SwitchOffGuns()
    {
        for (int i = 0; i < gunTransforms.Length; i++)
        {
            gunTransforms[i].gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand()
    {
        Transform targetTransform = _currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;

        leftHandTarget.localPosition = targetTransform.localPosition;
        leftHandTarget.localRotation = targetTransform.localRotation;
    }

    private void SwtichAnimatorLayer(int layerIndex)
    {
        for (int i = 0; i < _anim.layerCount; i++)
        {
            _anim.SetLayerWeight(i, 0);
        }
        
        _anim.SetLayerWeight(layerIndex, 1);
    }
    
    private void CheckWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOnGun(pistol);
            SwtichAnimatorLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOnGun(revolver);
            SwtichAnimatorLayer(1);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOnGun(autoRifle);
            SwtichAnimatorLayer(1);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOnGun(shotgun);
            SwtichAnimatorLayer(2);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOnGun(sniperRifle);
            SwtichAnimatorLayer(3);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
    }
}
