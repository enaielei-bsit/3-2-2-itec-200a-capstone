/*
 * Date Created: Monday, October 11, 2021 3:02 PM
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

using Utilities;

namespace Ph.CoDe_A.Lakbay.LinearPlay {
    [CreateAssetMenu(
        fileName="Spawner",
        menuName="Game/LinearPlay/Spawner"
    )]
    public class Spawner : MatrixCellHandler {
        public List<Spawn> spawns = new List<Spawn>();

        public override void OnPopulate(
            Matrix matrix,
            GameObject cell, Vector2Int index, float chance) {
            var spawns = this.spawns.Shuffle();
            // printLog(index, chance);
            foreach(var spawn in spawns) {
                if(spawn.OnSpawn(matrix, cell, index, chance)) {
                    var newSpawn = Instantiate(spawn, cell.transform);
                }
            }
        }
    }
}