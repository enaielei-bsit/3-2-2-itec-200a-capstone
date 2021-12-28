/*
 * Date Created: Tuesday, December 28, 2021 5:55 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using NaughtyAttributes;
using UnityEngine.EventSystems;

namespace Utilities {
    [RequireComponent(typeof(Collider))]
    public class Interactable : MonoBehaviour {
        // public virtual new Collider collider => GetComponent<Collider>();
        // protected int _mouseButtonPressed = -1;
        // public virtual int mouseButtonPressed => _mouseButtonPressed;
        // protected bool _held;
        // public virtual bool held => _held;
        // protected bool _hovered;
        // public virtual bool hovered => _hovered;
        // protected RaycastHit _hit;
        // public virtual RaycastHit hit => _hit;
        // protected Touch _touch;
        // public virtual Touch touch => _touch;

        // public new Camera camera;
        // public List<string> layers = new List<string>() {"Default"};
        // public UnityEvent<Interactable> onHover = new UnityEvent<Interactable>();
        // public UnityEvent<Interactable> onPress = new UnityEvent<Interactable>();
        // public UnityEvent<Interactable> onHold = new UnityEvent<Interactable>();
        // public UnityEvent<Interactable> onRelease = new UnityEvent<Interactable>();

        // public virtual void Update() {
        //     if(Input.touchSupported) {
        //         foreach(var touch in Input.touches) {
        //             var pos = touch.position;
        //             if(collider.IsHitFrom(
        //                 camera, pos, out var hit, layers: layers.ToArray()
        //             )) {
        //                 OnHover(hit, touch);
        //             }
        //         }
        //     } else {
        //         if(collider.IsHitFrom(
        //             camera, Input.mousePosition,
        //             out var hit, layers: layers.ToArray())) {
        //             OnHover(hit, default);
        //         }
        //     }

        //     if(held) {
        //         OnHold(hit, touch, mouseButtonPressed);
        //     }
        // }

        // public virtual void OnHover(RaycastHit hit, Touch touch) {
        //     if(Input.touchSupported) {
        //         EventSystem.current.RaycastAll(default)
        //         if(EventSystem.current.alreadySelecting)
        //             return;
        //     } else {
        //         if(EventSystem.current.IsPointerOverGameObject()) return;
        //     }
        //     _hovered = true;
        //     _hit = hit;
        //     _touch = touch;
        //     if(Input.touchSupported) {
        //         if(touch.phase == TouchPhase.Began && !held) {
        //             OnPress(hit, touch, -1);
        //         } else if(this.touch.phase == TouchPhase.Ended && held) {
        //             OnRelease(hit, touch, -1);
        //         }
        //     } else {
        //         var buttons = Enumerable.Range(0, 2);
        //         var buttonsDown = buttons.Select((i) =>
        //             Input.GetMouseButtonDown(i));
        //         int buttonDown = buttonsDown.ToList().IndexOf(true);
        //         var buttonsUp = buttons.Select((i) =>
        //             Input.GetMouseButtonUp(i));
        //         int buttonUp = buttonsUp.ToList().IndexOf(true);
        //         if(buttonDown != -1 && !held) {
        //             OnPress(hit, touch, buttonDown);
        //         } else if(buttonUp != -1 && buttonUp == mouseButtonPressed && held) {
        //             OnRelease(hit, touch, buttonUp);
        //         }
        //     }
        //     onHover?.Invoke(this);
        // }

        // public virtual void OnPress(RaycastHit hit, Touch touch, int mouseButton) {
        //     _held = true;
        //     _hit = hit;
        //     _touch = touch;
        //     _mouseButtonPressed = mouseButton;
        //     onPress?.Invoke(this);
        // }

        // public virtual void OnHold(RaycastHit hit, Touch touch, int mouseButton) {
        //     onHold?.Invoke(this);
        // }

        // public virtual void OnRelease(RaycastHit hit, Touch touch, int mouseButton) {
        //     _held = false;
        //     _hit = default;
        //     _touch = default;
        //     _mouseButtonPressed = -1;
        //     onRelease?.Invoke(this);
        // }

        // public virtual void Print(string text) => print(text);
    }
}