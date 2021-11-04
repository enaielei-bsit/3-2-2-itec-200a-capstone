/*
 * Date Created: Monday, October 11, 2021 3:00 PM
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

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    [Serializable]
    public struct MatrixAxis {
        public int indexInterval;
        public int maxCount;
        public MaxCountType maxCountType;
        // In the inspector, the values allowed are...
        // E.g. "0 1 2 3-5 6 7 10-15 16"
        [SerializeField]
        private string _onlyIndices;
        public int[] onlyIndices {
            get => GetIndices(_onlyIndices);
            set => _onlyIndices = value.Join(" ");
        }
        [SerializeField]
        private string _notIndices;
        public int[] notIndices {
            get => GetIndices(_notIndices);
            set => _notIndices = value.Join(" ");
        }

        public MatrixAxis(
            int indexInterval, int maxCount,
            MaxCountType maxCountType,
            int[] onlyIndices=null,
            int[] notIndices=null) {
            this.indexInterval = indexInterval;
            this.maxCount = maxCount;
            _onlyIndices = onlyIndices != null ? onlyIndices.Join(" ") : "";
            _notIndices = notIndices != null ? notIndices.Join(" ") : "";
            this.maxCountType = maxCountType;
        }

        public MatrixAxis(
            int indexInterval, int maxCount,
            int[] onlyIndices=null,
            int[] notIndices=null
        ) : this(indexInterval, maxCount,
                MaxCountType.Offset, onlyIndices, notIndices) {}

        public MatrixAxis(
            int indexInterval, MaxCountType maxCountType,
            int[] onlyIndices=null,
            int[] notIndices=null
        ) : this(indexInterval, 0, maxCountType, onlyIndices, notIndices) {}

        public MatrixAxis(
            int indexInterval,
            int[] onlyIndices=null,
            int[] notIndices=null
        ) : this(indexInterval, -1, onlyIndices, notIndices) {}

        public int GetMaxCount(int count) {
            return maxCountType == MaxCountType.None
                ? count : (maxCountType == MaxCountType.Definite
                ? maxCount : count + maxCount);
        }

        public int[] GetIndices(string indices) {
            indices = indices.Trim();
            var indices_ = new List<int>();
            if(indices != null && indices.Length > 0) {
                var strnums = indices.Split(' ').ToList();
                for(int i = 0; i < strnums.Count; i++) {
                    if(int.TryParse(strnums[i], out int num)) {
                        indices_.Add(num);
                    } else {
                        var range = strnums[i].Split('-');
                        if(int.TryParse(range[0], out int start)
                            && int.TryParse(range[1], out int end)) {
                            indices_.AddRange(Enumerable.Range(
                                start, end - start
                            ));
                        }
                    }
                }
            }

            return indices_.ToArray();
        }
    }

    // None means no limit, Definite sets the limit exactly to the number,
    // and Offset adds the number to count.x.
    public enum MaxCountType {None, Definite, Offset}

    public class Spawn : Core.Entity {
        public float chance = 0.5f;
        public MatrixAxis row = new MatrixAxis(1);
        public MatrixAxis column = new MatrixAxis(1, MaxCountType.None);

        public virtual bool OnSpawn(
            Matrix matrix, GameObject cell, Vector2Int index, float chance) {
            var rowSpawns = cell.transform.parent.gameObject.Children().Select(
                (g) => g.GetComponentsInChildren(GetType()));
            var columnSpawns = matrix.root
                .Children().Select((e, i) => e.Children()[index.x]
                .GetComponentsInChildren(GetType()));
            
            int rsc = rowSpawns.Select((c) => c.Length).Sum();
            int csc = columnSpawns.Select((c) => c.Length).Sum();
            if(chance <= this.chance
                && rsc < row.GetMaxCount(matrix.count.x)
                && (index.y + 1) % row.indexInterval == 0
                && csc < column.GetMaxCount(matrix.count.y)
                && (index.x + 1) % column.indexInterval == 0
                && (row.onlyIndices.Length == 0
                    || row.onlyIndices.Contains(index.y))
                && (column.onlyIndices.Length == 0
                    || column.onlyIndices.Contains(index.x))
                && (row.notIndices.Length == 0
                    || !row.notIndices.Contains(index.y))
                && (column.notIndices.Length == 0
                    || !column.notIndices.Contains(index.x))
            ) {
                return true;
            }

            return false;
        }
    }
}