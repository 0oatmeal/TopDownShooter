using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private PlayerWeaponVisuals _weaponVisualController;
    
    private void Start()
    {
        _weaponVisualController = GetComponentInParent<PlayerWeaponVisuals>();
    }

    public void ReloadIsOver()
    {
        _weaponVisualController.MaximizeRigWeight();
    }

    public void ReturnRig()
    {
        _weaponVisualController.MaximizeRigWeight();
        _weaponVisualController.MaximizeLeftHandWeight();
    }

    public void WeaponGrabIsOver()
    {
        _weaponVisualController.SetBusyGrabbingWeapon(false);
    }
}
