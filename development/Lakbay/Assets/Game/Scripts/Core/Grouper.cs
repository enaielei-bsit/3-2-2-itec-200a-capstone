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
        public GameObject textEntryGroup;
        public TextWidget textWidget;
        public GameObject imageEntryGroup;
        public ImageWidget imageWidget;

        public override void OnBuild(ContentBuilder contentBuilder, Entry entry) {
            var cb = contentBuilder;
            var type = entry.type;
            var root = contentBuilder.root;
            string value = entry.value;
            switch(type) {
                case(Entry.Type.Text):
                    if(!textWidget) break;
                    if(textEntryGroup) {
                        var widgets = cb.GetComponentsInChildren<TextWidget>();
                        if(widgets.Length > 0)
                            root = widgets.Last().transform.parent.gameObject;
                        if(root == contentBuilder.root)
                            root = Instantiate(textEntryGroup, root.transform);
                    }

                    var widget = Instantiate(textWidget, root.transform);
                    widget.component?.SetText(value);
                    break;
                case(Entry.Type.Document):
                    // TODO: Load Document...
                    // value = ...
                    goto case Entry.Type.Text;
                default: break;
            }
        }
    }
}