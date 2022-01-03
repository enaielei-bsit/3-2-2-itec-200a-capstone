/*
 * Date Created: Sunday, January 2, 2022 8:28 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.SteppedApplication.PerpendicularParking {
    using Core;

    [AddComponentMenu("SA Perpendicular Parking Player")]
    public class SAPPPlayer : SAVehicleParkingPlayer {
        public override void Proceed() {
            LoadScene(BuiltScene.BackInAngleParking);
        }
    }
}