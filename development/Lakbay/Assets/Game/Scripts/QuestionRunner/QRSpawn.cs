/*
 * Date Created: Tuesday, November 23, 2021 7:33 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ph.CoDe_A.Lakbay.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    public class QRSpawn : Core.Spawn {
        public override bool OnSpawn(
            Spawner spawner, Transform[] locations, Transform location) {
            if(base.OnSpawn(spawner, locations, location)) {
                var occupied =
                    locations.Count((l) => l.GetComponentInChildren<QRSpawner>());
                if(occupied >= locations.Length) return false;
                return true;
            }

            return false;
        }
    }
}