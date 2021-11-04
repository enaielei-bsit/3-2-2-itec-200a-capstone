/*
 * Date Created: Thursday, November 4, 2021 5:53 PM
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

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    using Spawns;

    public class QuestionSpawner : MatrixCellHandler {
        public readonly List<QuestionSpawn> spawns = new List<QuestionSpawn>();
        public QuestionSpawn spawn;

        public override void OnBuild(
            Matrix matrix, GameObject cell, Vector2Int index, float chance) {
            if(spawn && spawn.OnSpawn(matrix, cell, index, chance)) {
                spawns.Add(Instantiate(spawn, cell.transform));
            }
        }

        public override void OnPreBuild(Matrix matrix) {
            spawns.Clear();
        }
    }
}