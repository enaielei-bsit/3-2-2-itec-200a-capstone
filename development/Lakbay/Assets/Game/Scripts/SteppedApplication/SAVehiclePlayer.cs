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
    using Core;

    public enum SignalLight {
        Left = -1, None, Right
    }

    public enum GearBox {
        Reverse = -1, Neutral, Drive
    }

    [RequireComponent(typeof(VehicleController))]
    public class SAVehiclePlayer : SAPlayer {
        public bool debug = false;

        [Header("Level")]
        public SAVehicleInGameUI inGameUI;
        public GameOverUI gameOverUI;

        public GearBox currentGear {
            get {
                if(vehicle) {
                    int rgear = vehicle.powertrain.transmission.Gear;
                    if(rgear >= (int) GearBox.Drive)
                        return GearBox.Drive;
                    var gear = (GearBox) rgear;
                    return gear;
                }

                return GearBox.Neutral;
            }
        }
        public virtual SignalLight signalLight {
            get {
                if(!vehicle) return SignalLight.None;
                var lights = vehicle.effectsManager.lightsManager;
                if(lights.leftBlinkers.On) return SignalLight.Left;
                if(lights.rightBlinkers.On) return SignalLight.Right;
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
            // Accelerate(ndata.GetPressure());
            Accelerate(1.0f);
            printLog($"Accelerating: {vehicle.input.Throttle}");
        }

        public virtual void Brake(float value) {
            if(!vehicle) return;
            vehicle.input.Brakes = value;
        }

        public virtual void Brake(BaseEventData data) {
            var ndata = data as PointerEventData;
            // Brake(ndata.GetPressure());
            Brake(1.0f);
            printLog($"Braking: {vehicle.input.Brakes}");
        }

        public virtual void SetGear(GearBox gear) {
            if(!vehicle) return;
            vehicle.powertrain.transmission.ShiftInto((int) gear);
            // _currentGear = gear;
        }

        public virtual void SetGear(int gear) => SetGear((GearBox) gear);

        public virtual void SetIgnition(bool value) {
            if(vehicle) {
                if(value) {
                    vehicle.effectsManager.lightsManager.IsOn = true;
                    vehicle.powertrain.engine.Start();
                } else {
                    vehicle.effectsManager.lightsManager.IsOn = false;
                    vehicle.powertrain.engine.Stop();
                }
            }
        }

        public virtual void ToggleIgnition() {
            if(vehicle) {
                SetIgnition(!isEngineRunning);
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
            if(!vehicle || !isEngineRunning) return;
            var input = vehicle.input;

            if(light == SignalLight.Left) {
                input.LeftBlinker = true;
            } else if(light == SignalLight.Right) {
                input.RightBlinker = true;
            } else {
                if(signalLight == SignalLight.Left) {
                    input.LeftBlinker = true;
                } else if(signalLight == SignalLight.Right) {
                    input.RightBlinker = true;
                }
            }
        }

        public virtual void ToggleSignalLight(SignalLight light) {
            if(signalLight == light) {
                SetSignalLight(SignalLight.None);
            } else SetSignalLight(light);
        }

        public virtual void ToggleSignalLight(int light) =>
            ToggleSignalLight((SignalLight) light);

        public virtual void SetSignalLight(int light) =>
            SetSignalLight((SignalLight) light);

        public override void Update() {
            base.Update();

            if(vehicle.input.LeftBlinker) printLog("left");
            if(vehicle.input.RightBlinker) printLog("right");

            if(Debug.isDebugBuild && !Input.touchSupported && debug) {
                var kb = IInput.keyboard;
                if(kb.leftArrowKey.isPressed) {
                    Steer(-1.0f);
                } else if(kb.rightArrowKey.isPressed) {
                    Steer(1.0f);
                }

                if(kb.upArrowKey.isPressed) {
                    Accelerate(1.0f);
                } else Accelerate(0.0f);

                if(kb.downArrowKey.isPressed) {
                    Brake(1.0f);
                } else Brake(0.0f);

                if(kb.spaceKey.isPressed) {
                    Handbrake(1.0f);
                } else Handbrake(0.0f);

                if(kb.digit1Key.isPressed) {
                    SetGear(GearBox.Drive);
                } else if(kb.digit2Key.isPressed) {
                    SetGear(GearBox.Neutral);
                } else if(kb.digit3Key.isPressed) {
                    SetGear(GearBox.Reverse);
                }

                if(kb.zKey.wasPressedThisFrame) {
                    ToggleSignalLight(SignalLight.Left);
                } else if(kb.xKey.wasPressedThisFrame) {
                    ToggleSignalLight(SignalLight.Right);
                }

                if(kb.iKey.wasPressedThisFrame) {
                    ToggleIgnition();
                }
            }
        }

        public override void Build() {
            base.Build();
            if(vehicle) Reset();
        }

        public virtual void Reset() {
            SetGear(GearBox.Neutral);
            SetSignalLight(SignalLight.None);
            Handbrake(1.0f);
            SetIgnition(false);
        }
    }
}