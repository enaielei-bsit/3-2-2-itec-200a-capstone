/*
 * Date Created: Thursday, December 23, 2021 1:22 PM
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

namespace Ph.CoDe_A.Lakbay.SteppedApplication.ParallelParking {
    using Utilities;
    using Core;
    using EVP;

    [AddComponentMenu("SA Parallel Parking Player")]
    public class SAPPPlayer : SAVehicleParkingPlayer {
        public override void Proceed() {
            LoadScene(BuiltScene.PerpendicularParking);
        }
    }
}