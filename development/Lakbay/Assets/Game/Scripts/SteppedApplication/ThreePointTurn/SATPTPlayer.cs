/*
 * Date Created: Tuesday, January 4, 2022 9:00 AM
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

namespace Ph.CoDe_A.Lakbay.SteppedApplication.ThreePointTurn {
    using Utilities;
    using Core;

    public class SATPTPlayer : SASteppedVehiclePlayer {
        public override void Proceed() {
            LoadScene(BuiltScene.Tailgating);
        }
    }
}