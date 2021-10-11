/*
 * Date Created: Monday, October 11, 2021 3:00 PM
 * Author: nommel-isanar <nommel.isanar.lavapie.amolat@gmail.com>
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

namespace Ph.CoDe_A.Lakbay.LinearPlay {
    public class Spawn : Core.Entity {
        public float chance = 0.5f;

        public virtual bool OnSpawn(GameObject cell, Vector2 index) {
            float chance = UnityEngine.Random.value;
            if(chance < this.chance) {
                return true;
            }

            return false;
        }
    }
}