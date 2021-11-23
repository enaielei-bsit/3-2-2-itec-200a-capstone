/*
 * Date Created: Monday, November 22, 2021 7:27 AM
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

using TMPro;

using Utilities;

namespace Ph.CoDe_A.Lakbay.Core {
    public class Content : Controller {
        [Serializable]
        public struct Group<T0, T1>
            where T0 : Component {
            public LayoutGroup layout;
            public T0 component;
            public Viewer<T0, T1> viewer;
        }

        protected readonly Dictionary<Entry.Type, LayoutGroup> _layouts
            = new Dictionary<Entry.Type, LayoutGroup>();

        public LayoutGroup root;
        public Group<TextMeshProUGUI, string> textGroup;
        public Group<Image, Sprite> imageGroup;
        public List<Entry> content = new List<Entry>();

        public override void Awake() {
            base.Awake();
        }

        public virtual void Build() => Build(content.ToArray());

        public virtual void Build(IEnumerable<Entry> content) =>
            Build(content.ToArray());

        public virtual void Build(params Entry[] content) {
            if(root) {
                this.root.transform.DestroyChildren();
                var root = this.root;

                foreach(var entry in content) {
                    switch(entry.type) {
                        case Entry.Type.Text:
                            Build(entry, textGroup,
                                (c) => c.SetText(entry.value),
                                (v) => v.Show(entry.value));
                            break;
                        case Entry.Type.Image:
                            // TODO: Set Image...
                            Build(entry, imageGroup,
                                (c) => c.sprite = default,
                                (v) => v.Show(default));
                            break;
                        default: break;
                    }
                }
            }
        }

        public virtual T0 Build<T0, T1>(
            Entry entry, Group<T0, T1> group,
            Action<T0> onComponentBuild=default,
            Action<Viewer<T0, T1>> onViewerBuild=default)
            where T0 : Component {
            T0 component = default;
            var root = this.root;
            LayoutGroup layout = default;

            if(_layouts.ContainsKey(entry.type)) layout = _layouts[entry.type];
            else if(group.layout) layout = group.layout;
            _layouts[entry.type] = layout;

            if(layout) {
                root = Instantiate(layout, root.transform);
            }

            if(group.component) {
                component =
                    Instantiate(group.component, root.transform);
                onComponentBuild?.Invoke(component);
            }

            if(group.viewer && component) {
                var button = component.gameObject.EnsureComponent<Button>();
                button.onClick.AddListener(
                    () => onViewerBuild?.Invoke(group.viewer));
            }

            return component;
        }
    }
}