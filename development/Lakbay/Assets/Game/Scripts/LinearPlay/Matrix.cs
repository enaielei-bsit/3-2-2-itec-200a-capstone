/*
 * Date Created: Monday, October 11, 2021 9:57 AM
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
    public class Matrix : Core.Entity {
        public GameObject root;
        public Axis orientation = Axis.Y;
        public AxesDirection2 direction = AxesDirection2.positive;
        public Vector2 anchor = new Vector2(0.5f, 0.5f);
        public Vector2 count = new Vector2(3, 3);
        public Vector2 cellSize = new Vector2(1, 1);
        public List<MatrixCellHandler> cellHandlers = new List<MatrixCellHandler>();

        protected virtual Vector2 size => new Vector2(
            cellSize.x * count.x, cellSize.y * count.y
        );
        protected virtual Tuple<Axis, Axis> axes => GetAxes(orientation);

        [ContextMenu("Populate")]
        public virtual void Populate() {
            if(root) {
                if(Application.isPlaying) root.DestroyChildren();
                else root.DestroyChildrenImmediately();

                for(int rowIndex = 0; rowIndex < count.y; rowIndex++) {
                    var row = new GameObject($"Row_{rowIndex}");
                    row.transform.SetParent(root.transform);
                    row.transform.position = GetPosition(
                        new Vector2(-1, rowIndex)
                    );

                    for(int cellIndex = 0; cellIndex < count.x; cellIndex++) {
                        var cell = new GameObject($"Cell_{cellIndex}");
                        cell.transform.SetParent(row.transform);
                        cell.transform.localPosition = GetPosition(
                            new Vector2(cellIndex, -1)
                        );
                        foreach(var cellHandler in cellHandlers) {
                            cellHandler.OnPopulate(
                                cell, new Vector2(cellIndex, rowIndex));
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
            if(!root) root = gameObject;
        }
    }
}