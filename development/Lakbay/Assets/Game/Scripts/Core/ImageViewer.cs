/*
 * Date Created: Monday, November 22, 2021 8:25 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.Core
{
    public class ImageViewer : Viewer<Image, Sprite>
    {
        public TextMeshProUGUI source;
        public TextMeshProUGUI description;

        public override void Show(
            Sprite sprite)
        {
            base.Show(sprite);
            source?.gameObject.SetActive(false);
            description?.gameObject.SetActive(false);
            if (component)
            {
                component.sprite = sprite;
            }
        }

        public virtual void Show(
            Sprite sprite, string description, string source
        )
        {
            Show(sprite);
            this.description?.gameObject.SetActive(false);
            this.source?.gameObject.SetActive(false);
            if (!string.IsNullOrEmpty(description))
            {
                this.description?.gameObject.SetActive(true);
                this.description?.SetText(description);
            }
            if (!string.IsNullOrEmpty(source))
            {
                this.source?.gameObject.SetActive(true);
                this.source?.SetText(source);
            }
        }
    }
}