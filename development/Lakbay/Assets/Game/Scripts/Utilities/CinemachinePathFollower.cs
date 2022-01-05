/*
 * Date Created: Wednesday, January 5, 2022 3:31 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Cinemachine.CinemachinePathBase;

namespace Utilities {
    [ExecuteInEditMode]
    public class CinemachinePathFollower : MonoBehaviour {
        public bool position = true;
        public bool rotation = true;
        public PositionUnits unit = PositionUnits.Normalized;
        public float value;
        public CinemachinePath path;
        public CinemachineSmoothPath smoothPath;

        public virtual void Update() {
            if(path) {
                if(position) {
                    transform.position =
                        path.EvaluatePositionAtUnit(value, unit);
                }
                if(rotation) {
                    transform.rotation =
                        path.EvaluateOrientationAtUnit(value, unit);
                }
            } else if(smoothPath) {
                if(position) {
                    transform.position =
                        smoothPath.EvaluatePositionAtUnit(value, unit);
                }
                if(rotation) {
                    transform.rotation =
                        smoothPath.EvaluateOrientationAtUnit(value, unit);
                }
            }
        }
    }
}