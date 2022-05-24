/*
 * Date Created: Friday, January 7, 2022 1:56 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.Core
{
    using TMPro;
    using Utilities;

    public class KnowledgeExplorerUI : Controller
    {
        protected Path _current;
        public virtual Path current => _current;

        public LocalizedPathEvent rootEvent;
        public Path root;
        public Button rootSeparator;

        [Space]
        public Content content;
        public WatchableViewer watchableViewer;

        [Space]
        public LayoutGroup contents;
        public Button folderEntry;
        public Button readableEntry;
        public Button watchableEntry;

        [Space]
        public Color selectedPath = Color.green;
        public LayoutGroup paths;
        public Button path;
        public RectTransform pathSeparator;

        public virtual void Build(Path path)
        {
            if (!path) return;
            root = path;
            _current = path;
            path.ResolveParent();
            if (paths)
            {
                this.paths.transform.DestroyChildren();
                var paths = GetFullPath(path);
                foreach (var p in paths)
                {
                    if (p == paths.First())
                    {
                        var sep = Instantiate(rootSeparator, this.paths.transform);
                        sep.onClick.AddListener(OnPathClick(p));
                        if (p == paths.Last())
                            sep.image.color = selectedPath;
                        continue;
                    }

                    var bpath = Instantiate(this.path, this.paths.transform);
                    bpath.onClick.AddListener(OnPathClick(p));
                    var text = bpath.GetComponentInChildren<TextMeshProUGUI>();
                    text?.SetText(p.parsedName);

                    if (p != paths.Last())
                    {
                        Instantiate(pathSeparator, this.paths.transform);
                    }
                    else
                    {
                        bpath.image.color = selectedPath;
                    }
                }
            }

            if (contents)
            {
                contents.transform.DestroyChildren();
                var npaths = path.paths.OrderBy((p) => p.parsedName);
                foreach (var p in npaths)
                {
                    var folder = Instantiate(folderEntry, contents.transform);
                    var text = folder.GetComponentInChildren<TextMeshProUGUI>();
                    text?.SetText(p.parsedName);
                    folder.onClick.AddListener(OnPathClick(p));
                }

                var files = path.files;
                if(path.order == Path.Order.Alphabetical)
                    files = files.OrderBy((f) => f.name).ToList();
                var btn = readableEntry;
                if (path.type == Path.Type.Readable)
                    btn = readableEntry;
                else if (path.type == Path.Type.Watchable)
                    btn = watchableEntry;
                foreach (var file in files)
                {
                    var nfile = Instantiate(btn, contents.transform);
                    var text = nfile.GetComponentInChildren<TextMeshProUGUI>();
                    // file.name.GetLocale(null, out string name);
                    string name = file.name.TrimEnd("EN").TrimEnd("FIL");
                    text?.SetText(name);
                    nfile.onClick.AddListener(OnFileClick(path.type, file));
                }
            }
        }

        public virtual UnityAction OnFileClick(Path.Type type, TextAsset file)
        {
            return () =>
            {
                // content.gameObject.SetActive(false);
                // watchableViewer.gameObject.SetActive(false);

                if (type == Path.Type.Readable)
                {
                    if (!content) return;
                    watchableViewer?.gameObject.SetActive(false);
                    content?.gameObject.SetActive(true);
                    content?.Build(file.text.DeserializeAsYaml<List<Entry>>());
                }
                else if (type == Path.Type.Watchable)
                {
                    if (!watchableViewer) return;
                    content?.gameObject.SetActive(false);
                    watchableViewer?.gameObject.SetActive(true);
                    watchableViewer?.Build(file.text.DeserializeAsYaml<Watchable>());
                }
                (transform as RectTransform).ForceUpdateRectTransforms();
            };
        }

        public virtual UnityAction OnPathClick(Path path)
        {
            return () => Build(path);
        }

        public static Path[] GetFullPath(Path path)
        {
            var paths = new List<Path>();
            while (path != null)
            {
                paths.Add(path);
                path = path.parent;
            }

            paths.Reverse();
            return paths.ToArray();
        }

        public virtual void GoToParent()
        {
            if (current && current.parent) Build(current.parent);
        }

        public virtual void Build() => Build(root);
    }
}