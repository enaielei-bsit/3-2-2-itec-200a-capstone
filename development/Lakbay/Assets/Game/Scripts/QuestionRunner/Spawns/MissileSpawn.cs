/*
 * Date Created: Tuesday, November 23, 2021 7:28 AM
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

namespace Ph.CoDe_A.Lakbay.QuestionRunner.Spawns {
    public class MissileSpawn : SkillSpawn<Buffs.MissileBuff> {
        public override bool OnSpawnCheck(
            Core.Spawner spawner, Transform[] locations, Transform location) {
            bool can = base.OnSpawnCheck(spawner, locations, location);
            if(can) {
                return !Session.qrLevel.done;
            }

            return can;
        }

        public override void OnTrigger(QRPlayer trigger) {
            base.OnTrigger(trigger);
            gameObject.SetActive(false);
        }
    }
}