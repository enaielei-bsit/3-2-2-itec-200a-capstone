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

using Utilities;

namespace Ph.CoDe_A.Lakbay.LinearPlay {
    public class Spawn : Core.Entity {
        [Serializable]
        public struct MatrixAxis {
            public int indexInterval;
            public int maxCount;
            public MaxCountType maxCountType;

            public MatrixAxis(
                int indexInterval, int maxCount, MaxCountType maxCountType) {
                this.indexInterval = indexInterval;
                this.maxCount = maxCount;
                this.maxCountType = maxCountType;
            }

            public MatrixAxis(
                int indexInterval, int maxCount
            ) : this(indexInterval, maxCount, MaxCountType.Offset) {}

            public MatrixAxis(
                int indexInterval, MaxCountType maxCountType
            ) : this(indexInterval, 0, maxCountType) {}

            public MatrixAxis(
                int indexInterval
            ) : this(indexInterval, -1) {}

            public int GetMaxCount(int count) {
                return maxCountType == MaxCountType.None
                    ? count : (maxCountType == MaxCountType.Definite
                    ? maxCount : count + maxCount);
            }
        }

        // None means no limit, Definite sets the limit exactly to the number,
        // and Offset adds the number to count.x.
        public enum MaxCountType {None, Definite, Offset}

        public float chance = 0.5f;
        public MatrixAxis row = new MatrixAxis(1);
        public MatrixAxis column = new MatrixAxis(1, MaxCountType.None);

        public virtual bool OnSpawn(
            Matrix matrix, GameObject cell, Vector2Int index, float chance) {
            var rowSpawns = cell.transform.parent.gameObject.Children().Select(
                (g) => g.GetComponentsInChildren<Spawn>());
            var columnSpawns = matrix.root
                .Children().Select((e, i) => e.Children()[index.x]
                .GetComponentsInChildren<Spawn>());
            
            int rsc = rowSpawns.Select((c) => c.Length).Sum();
            int csc = columnSpawns.Select((c) => c.Length).Sum();
            if(chance <= this.chance
                && rsc < row.GetMaxCount(matrix.count.x)
                && (index.y + 1) % row.indexInterval == 0
                && csc < column.GetMaxCount(matrix.count.y)
                && (index.x + 1) % column.indexInterval == 0
            ) {
                return true;
            }

            return false;
        }
    }
}