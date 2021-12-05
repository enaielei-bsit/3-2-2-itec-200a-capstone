/*
 * Date Created: Wednesday, October 13, 2021 10:39 AM
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
    public class ShieldSpawn : SkillSpawn<Buffs.ShieldBuff> {
        public override void OnTrigger(QRPlayer player) {
            base.OnTrigger(player);
            gameObject.SetActive(false);
        }
    }
}