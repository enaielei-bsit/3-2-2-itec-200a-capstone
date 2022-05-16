/*
 * Date Created: Tuesday, December 28, 2021 6:02 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using UnityEngine;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.SteppedApplication
{
    using System.Collections.Generic;
    using TMPro;

    public class SAVehicleInGameUI : SAInGameUI
    {
        [Space]
        public SAVehiclePlayer player;

        [Header("Indicators")]
        public string speedFormat = "000";
        public TextMeshProUGUI speed;

        [Space]
        public Toggle ignitionSwitch;

        [Space]
        public Slider handbrake;

        [Header("Pedals")]
        public Image acceleratorHighlight;
        public Image brakeHighlight;
        public Image clutchHighlight;
        public CanvasGroup clutch;

        [Header("Gears")]
        public Toggle fiveGear;
        public Toggle fourGear;
        public Toggle threeGear;
        public Toggle twoGear;
        public Toggle driveGear;
        public Toggle neutralGear;
        public Toggle reverseGear;

        [Header("Lights")]
        public Toggle leftSignalLight;
        public Toggle rightSignalLight;

        public override void Update()
        {
            base.Update();
            if (player)
            {
                var manualGears = new List<Toggle> {twoGear, threeGear, fourGear, fiveGear};
                if(Session.transmission == Core.Transmission.Automatic) {
                    var txt = driveGear.GetComponentInChildren<TextMeshProUGUI>();
                    txt?.SetText("D");

                    manualGears.ForEach((e) => {
                        var cg = e?.GetComponentInChildren<CanvasGroup>();
                        if(cg) {
                            cg.alpha = 0.0f;
                            cg.interactable = false;
                        }
                    });

                    if(clutch) {
                        clutch.alpha = 0.0f;
                        clutch.interactable = false;
                    }
                } else {
                    var txt = driveGear.GetComponentInChildren<TextMeshProUGUI>();
                    txt?.SetText("1");

                    manualGears.ForEach((e) => {
                        var cg = e?.GetComponentInChildren<CanvasGroup>();
                        if(cg) {
                            cg.alpha = 1.0f;
                            cg.interactable = true;
                        }
                    });

                    if(clutch) {
                        clutch.alpha = 1.0f;
                        clutch.interactable = true;
                    }
                }

                speed?.SetText(player.vehicle.Speed.ToString(speedFormat));
                SetGear(player.currentGear);
                SetIgnition(player.isEngineRunning);
                SetSignalLight(player.signalLight);
                Handbrake(player.vehicle.input.Handbrake);
                Accelerate(player.vehicle.input.Throttle);
                Brake(player.vehicle.input.Brakes);
                Clutch(player.vehicle.input.Clutch);
            }
        }

        public virtual void SetGear(GearBox gear)
        {
            if (gear == GearBox.Five && fiveGear)
                fiveGear.SetIsOnWithoutNotify(true);
            if (gear == GearBox.Four && fourGear)
                fourGear.SetIsOnWithoutNotify(true);
            if (gear == GearBox.Three && threeGear)
                threeGear.SetIsOnWithoutNotify(true);
            if (gear == GearBox.Two && twoGear)
                twoGear.SetIsOnWithoutNotify(true);
            if (gear == GearBox.Drive && driveGear)
                driveGear.SetIsOnWithoutNotify(true);
            if (gear == GearBox.Neutral && neutralGear)
                neutralGear.SetIsOnWithoutNotify(true);
            if (gear == GearBox.Reverse && reverseGear)
                reverseGear.SetIsOnWithoutNotify(true);
        }

        public virtual void SetIgnition(bool value)
        {
            if (ignitionSwitch) ignitionSwitch.SetIsOnWithoutNotify(value);
        }

        // public virtual void SetGear(int gear) {
        //     VehicleGear rgear;
        //     if (gear >= (int) VehicleGear.Drive) rgear = VehicleGear.Drive;
        //     else rgear = (VehicleGear) gear;
        //     SetGear(rgear);
        // }

        public virtual void SetSignalLight(SignalLight light)
        {
            var ll = leftSignalLight;
            var rl = rightSignalLight;

            if (ll) ll.SetIsOnWithoutNotify(false);
            if (rl) rl.SetIsOnWithoutNotify(false);

            if (light == SignalLight.Left)
            {
                if (ll) ll.SetIsOnWithoutNotify(true);
            }
            else if (light == SignalLight.Right)
            {
                if (rl) rl.SetIsOnWithoutNotify(true);
            }
        }

        public virtual void Handbrake(float value)
        {
            if (handbrake) handbrake.SetValueWithoutNotify(value);
        }

        public virtual void Accelerate(float value)
        {
            var hl = acceleratorHighlight;
            hl?.gameObject.SetActive(value > 0.0f);
        }

        public virtual void Brake(float value)
        {
            var hl = brakeHighlight;
            hl?.gameObject.SetActive(value > 0.0f);
        }

        public virtual void Clutch(float value) {
            var hl = clutchHighlight;
            hl?.gameObject.SetActive(value > 0.0f);
        }
    }
}