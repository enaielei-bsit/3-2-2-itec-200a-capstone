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

        [Header("Gears")]
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
                speed?.SetText(player.vehicle.Speed.ToString(speedFormat));
                SetGear(player.currentGear);
                SetIgnition(player.isEngineRunning);
                SetSignalLight(player.signalLight);
                Handbrake(player.vehicle.input.Handbrake);
                Accelerate(player.vehicle.input.Throttle);
                Brake(player.vehicle.input.Brakes);
            }
        }

        public virtual void SetGear(GearBox gear)
        {
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
    }
}