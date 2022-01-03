/*
 * Date Created: Monday, January 3, 2022 3:56 PM
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

namespace Ph.CoDe_A.Lakbay.SteppedApplication.BackInAngleParking {
    public class SABIAPPlayer : SAVehicleParkingPlayer {
        public override void Proceed() {
            LoadScene();
        }
    }
}