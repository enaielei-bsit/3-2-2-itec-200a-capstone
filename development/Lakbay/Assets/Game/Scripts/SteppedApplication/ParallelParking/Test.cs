/*
 * Date Created: Thursday, December 23, 2021 11:32 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NWH.VehiclePhysics2;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.SteppedApplication.ParallelParking {
    using Utilities;

    public class Test : EventTrigger {
        public VehicleController vehicle;
        public bool handbrake = false;
        public bool upGear = false;
        public bool downGear = false;
        public bool accelerate = false;
        public bool brake = false;
        public bool clutch = false;

        public virtual void OnHandbrake() => handbrake = true;
        public virtual void OffHandbrake() => handbrake = false;
        public virtual void OnUpGear() => upGear = true;
        public virtual void OffUpGear() => upGear = false;
        public virtual void OnDownGear() => downGear = true;
        public virtual void OffDownGear() => downGear = false;
        public virtual void OnAccelerate() => accelerate = true;
        public virtual void OffAccelerate() => accelerate = false;
        public virtual void OnBrake() => brake = true;
        public virtual void OffBrake() => brake = false;
        public virtual void OnClutch() => clutch = true;
        public virtual void OffClutch() => clutch = false;

        public UnityEvent<BaseEventData> onPointerHold;

        public virtual void UpGear() {
            if(vehicle) {
                vehicle.input.ShiftUp = true;
            }
        }

        public virtual void DownGear() {
            if(vehicle) {
                vehicle.input.ShiftDown = true;
            }
        }

        public virtual void Handbrake() {
            if(!vehicle) return;
            vehicle.input.Handbrake = GetPressure();
        }

        public virtual void OnDrag(BaseEventData eventData) {
            print("Moving");
        }

        public virtual void Accelerate(BaseEventData datas) {
            var data = datas as PointerEventData;
            if(!vehicle) return;
            vehicle.input.Throttle = data.GetPressure();
        }
        public virtual void Accelerate() {
            if(!vehicle) return;
            vehicle.input.Throttle = GetPressure();
            // print("Accelerating...");
        }

        public virtual void Brake() {
            if(!vehicle) return;
            vehicle.input.Brakes = GetPressure();
        }

        public virtual void Clutch() {
            if(!vehicle) return;
            vehicle.input.Clutch = GetPressure();
        }

        public static float GetPressure() {
            if(Input.touchCount > 0) {
                var touch = Input.GetTouch(0);
                return touch.pressure;
            } else return 1.0f;
        }

        public virtual void FixedUpdate() {
            if(handbrake) Handbrake();
            if(upGear) UpGear();
            if(downGear) DownGear();
            if(accelerate) Accelerate();
            if(brake) Brake();
            if(clutch) Clutch();
        }
    }
}