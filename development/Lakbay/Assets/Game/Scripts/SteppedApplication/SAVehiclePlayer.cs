/*
 * Date Created: Tuesday, December 28, 2021 6:24 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NWH.VehiclePhysics2;
using NWH.VehiclePhysics2.Powertrain;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.SteppedApplication {
    using Utilities;

    public enum SignalLight {
        Left = -1, None, Right
    }

    public enum VehicleGear {
        Reverse = -1, Neutral, Drive
    }

    [RequireComponent(typeof(VehicleController))]
    public class SAVehiclePlayer : SAPlayer {
        public VehicleGear currentGear {
            get {
                if(vehicle) {
                    int rgear = vehicle.powertrain.transmission.Gear;
                    if(rgear >= (int) VehicleGear.Drive)
                        return VehicleGear.Drive;
                    var gear = (VehicleGear) rgear;
                    return gear;
                }

                return VehicleGear.Neutral;
            }
        }
        public SignalLight signalLight {
            get {
                if(!vehicle) return SignalLight.None;
                var input = vehicle.input;
                if(input.LeftBlinker) return SignalLight.Left;
                if(input.RightBlinker) return SignalLight.Right;

                return SignalLight.None;
            }
        }
        public bool isEngineRunning {
            get {
                if(!vehicle) return false;
                return vehicle.powertrain.engine.IsRunning;
            }
        }
        public virtual VehicleController vehicle =>
            GetComponent<VehicleController>();

        public virtual void Accelerate(float value) {
            if(!vehicle) return;
            // int currentGear = vehicle.powertrain.transmission.Gear;
            // if(currentGear != (int) this.currentGear) {
            //     SetGear(this.currentGear);
            // }
            vehicle.input.Throttle = value;
        }

        public virtual void Accelerate(BaseEventData data) {
            var ndata = data as PointerEventData;
            Accelerate(ndata.GetPressure());
            printLog($"Accelerating: {vehicle.input.Throttle}");
        }

        public virtual void Brake(float value) {
            if(!vehicle) return;
            vehicle.input.Brakes = value;
        }

        public virtual void Brake(BaseEventData data) {
            var ndata = data as PointerEventData;
            Brake(ndata.GetPressure());
            printLog($"Braking: {vehicle.input.Brakes}");
        }

        public virtual void SetGear(VehicleGear gear) {
            if(!vehicle) return;
            vehicle.powertrain.transmission.ShiftInto((int) gear);
            // _currentGear = gear;
        }

        public virtual void SetGear(int gear) => SetGear((VehicleGear) gear);

        public virtual void ToggleIgnition(bool value) {
            if(vehicle) {
                if(value) vehicle.powertrain.engine.Start();
                else vehicle.powertrain.engine.Stop();
            }
        }

        public virtual void ToggleIgnition() {
            if(vehicle) {
                ToggleIgnition(!vehicle.input.EngineStartStop);
            }
        }

        public virtual void Steer(float value) {
            if(vehicle) {
                vehicle.input.Steering = value;
            }
        }

        public virtual void Handbrake(float value) {
            if(!vehicle) return;
            vehicle.input.Handbrake = value;
        }

        public virtual void Handbrake(BaseEventData data) {
            var ndata = data as PointerEventData;
            Handbrake(ndata.GetPressure());
            printLog($"Handbraking: {vehicle.input.Handbrake}");
        }

        public virtual void SetSignalLight(SignalLight light) {
            if(!vehicle) return;
            var input = vehicle.input;
            input.LeftBlinker = false;
            input.RightBlinker = false;
            if(light == SignalLight.Left) input.LeftBlinker = true;
            if(light == SignalLight.Right) input.RightBlinker = true;
        }

        public virtual void ToggleSignalLight(SignalLight light) {
            if(signalLight == light) {
                SetSignalLight(SignalLight.None);
            } else SetSignalLight(light);
        }

        public virtual void ToggleSignalLight(int light) =>
            ToggleSignalLight((SignalLight) light);

        // public virtual void ToggleLeftSignalLight(bool value) =>
        //     SetSignalLight(value ? SignalLight.Left : SignalLight.None);

        // public virtual void ToggleRightSignalLight(bool value) =>
        //     SetSignalLight(value ? SignalLight.Right : SignalLight.None);

        public virtual void SetSignalLight(int light) =>
            SetSignalLight((SignalLight) light);

        public override void Update() {
            base.Update();
        }
    }
}