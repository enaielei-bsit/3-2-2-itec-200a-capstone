/*
 * Date Created: Tuesday, December 28, 2021 6:02 AM
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

namespace Ph.CoDe_A.Lakbay.SteppedApplication {
    using TMPro;
    using Utilities;

    public class SAVehicleInGameUI : SAInGameUI {
        [Space]
        public SAVehiclePlayer player;

        [Header("Indicators")]
        public string speedFormat = "000";
        public TextMeshProUGUI speed;

        [Space]
        public Toggle ignitionSwitch;

        [Header("Gears")]
        public Toggle driveGear;
        public Toggle neutralGear;
        public Toggle reverseGear;

        public override void Update() {
            base.Update();
            if(player) {
                speed?.SetText(player.vehicle.Speed.ToString(speedFormat));
                SetGear(player.currentGear);
                SetIgnition(player.isEngineRunning);
            }
        }

        public virtual void SetGear(VehicleGear gear) {
            if(gear == VehicleGear.Drive && driveGear) driveGear.isOn = true;
            if(gear == VehicleGear.Neutral && neutralGear) neutralGear.isOn = true;
            if(gear == VehicleGear.Reverse && reverseGear) reverseGear.isOn = true;
        }

        public virtual void SetIgnition(bool value) {
            if(ignitionSwitch) ignitionSwitch.isOn = value;
        }

        // public virtual void SetGear(int gear) {
        //     VehicleGear rgear;
        //     if (gear >= (int) VehicleGear.Drive) rgear = VehicleGear.Drive;
        //     else rgear = (VehicleGear) gear;
        //     SetGear(rgear);
        // }
    }
}