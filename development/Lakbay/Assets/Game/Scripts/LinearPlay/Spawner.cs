/*
 * Date Created: Monday, October 11, 2021 3:02 PM
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
    public class Spawner : MatrixCellHandler {
        public List<Spawn> spawns = new List<Spawn>();

        public override void OnPopulate(GameObject cell, Vector2 index) {
            foreach(var spawn in spawns) {
                if(spawn.OnSpawn(cell, index)) {
                    var newSpawn = Instantiate(spawn, cell.transform);
                }
            }
        }
    }
}