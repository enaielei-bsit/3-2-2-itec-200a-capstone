/*
 * Date Created: Monday, November 22, 2021 11:45 AM
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
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    using Utilities;

    public class Travel : Core.DirectionalMovement {
        public float speed {
            set {
                xAxis.speed = value;
                yAxis.speed = value;
                zAxis.speed = value;
            }
        }

        public override void Update() {
            base.Update();
            bool spaced = IInput.keyboard.spaceKey.wasPressedThisFrame;

            if(spaced) Perform(!performing);
        }
    }
}