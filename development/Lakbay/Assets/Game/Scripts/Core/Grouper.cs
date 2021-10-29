/*
 * Date Created: Sunday, October 24, 2021 5:11 PM
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

namespace Ph.CoDe_A.Lakbay.Core {
    [CreateAssetMenu(
        fileName="Grouper",
        menuName="Game/Core/Grouper"
    )]
    public class Grouper : ContentEntryHandler {
        public TextGroup textGroup;

        public override void OnBuild(ContentBuilder contentBuilder, Entry entry) {
            var type = entry.type;
            var root = contentBuilder.root;
            string value = entry.value;
            switch(type) {
                case(Entry.Type.Text):
                    if(textGroup && textGroup.widget) {
                        var group = GetRecentGroup(root, textGroup);
                        var widget = Instantiate(group.widget, root.transform);
                        widget.component?.SetText(value);
                    }
                    break;
                case(Entry.Type.Document):
                    // TODO: Load Document...
                    // value = ...
                    goto case Entry.Type.Text;
                default: break;
            }
        }

        public virtual T GetRecentGroup<T>(GameObject root, T group)
            where T : Group {
            var children = root.GetComponentsInChildren<T>();
            T grp = default;
            if(children != null && children.Length > 0) {
                grp = children.Last();
            } else grp = Instantiate(
                group, root.transform);

            return grp;
        }
    }
}