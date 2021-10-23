/*
 * Date Created: Monday, October 11, 2021 9:57 AM
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
    public class Matrix : Core.Entity {
        protected virtual Vector2 size => new Vector2(
            cellSize.x * count.x, cellSize.y * count.y
        );

        public bool randomPopulation = true;
        [SerializeField]
        protected GameObject _root;
        public GameObject root {
            get => _root ?? gameObject;
            set => _root = value;
        }
        public Axis orientation = Axis.Y;
        public AxesDirection2 direction = AxesDirection2.positive;
        public Vector2 anchor = new Vector2(0.5f, 0.5f);
        public Vector2Int count = new Vector2Int(3, 3);
        public Vector2 cellSize = new Vector2(1, 1);
        public List<MatrixCellHandler> cellHandlers =
            new List<MatrixCellHandler>();

        protected virtual Tuple<Axis, Axis> axes => GetAxes(orientation);

        [ContextMenu("Populate")]
        public virtual void Populate() {
            if(root) {
                if(Application.isPlaying) root.DestroyChildren();
                else root.DestroyChildrenImmediately();

                var _rowIndices = Enumerable.Range(0, count.y);
                var _cellIndices = Enumerable.Range(0, count.x);
                GameObject row, cell;

                // Pre-create the rows and columns.
                foreach(int rowIndex in _rowIndices) {
                    row = new GameObject($"Row_{rowIndex}");
                    row.transform.SetParent(root.transform);
                    row.transform.position = GetPosition(
                        new Vector2(-1, rowIndex)
                    );

                    foreach(int cellIndex in _cellIndices) {
                        cell = new GameObject($"Cell_{cellIndex}");
                        cell.transform.SetParent(row.transform);
                        cell.transform.localPosition = GetPosition(
                            new Vector2(cellIndex, -1)
                        );
                    }
                }

                // Call cellHandlers.
                var rows = root.Children().ToList();
                var rowIndices = _rowIndices.ToList();
                while(rowIndices.Count > 0) {
                    int rowIndex = 0;
                    if(randomPopulation) rowIndex = rowIndices.PopRandomly();
                    else rowIndex = rowIndices.Pop();

                    var cellIndices = _cellIndices.ToList();
                    while(cellIndices.Count > 0) {
                        int cellIndex = 0;
                        if(randomPopulation)
                            cellIndex = cellIndices.PopRandomly();
                        else cellIndex = cellIndices.Pop();

                        float chance = UnityEngine.Random.value;
                        foreach(var cellHandler in cellHandlers) {
                            var cell_ = root.transform
                                .GetChild(rowIndex)
                                .GetChild(cellIndex).gameObject;
                                
                            cellHandler.OnPopulate(
                                this, cell_,
                                new Vector2Int(cellIndex, rowIndex),
                                chance
                            );
                        }
                    }
                }
            }
        }

        public virtual Vector3 GetPosition(
            Vector2 index
        ) {
            var axes = GetAxes(orientation);
            int xdir = (int) direction.x;
            int ydir = (int) direction.y;
            int xi = (int) axes.Item1;
            int yi = (int) axes.Item2;
            var pos = Vector3.zero;
            float half = 0.0f;
            // Cell
            if(index.x >= 0) {
                half = ((size.x * anchor.x) - (cellSize.x * anchor.x));
                pos[xi] = ((cellSize.x * index.x) - half) * xdir;
            }
            // Row
            if(index.y >= 0) {
                half = ((size.y * anchor.y) - (cellSize.y * anchor.y));
                pos[yi] = ((cellSize.y * index.y) - half) * ydir;
            }
            return pos;
        }

        public static Tuple<Axis, Axis> GetAxes(Axis orientation) {
            var x = orientation == Axis.X ? Axis.Z : Axis.X;
            var y = orientation == Axis.X ? Axis.Y
                : (orientation == Axis.Y ? Axis.Z : Axis.Y);

            return new Tuple<Axis, Axis>(x, y);
        }

        public override void Awake() {
            base.Awake();
        }
    }
}