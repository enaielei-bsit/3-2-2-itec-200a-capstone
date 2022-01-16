/*
 * Date Created: Sunday, January 9, 2022 1:46 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using TMPro;
using UnityEngine.Video;

namespace Ph.CoDe_A.Lakbay.Core
{
    public class VideoViewer : Viewer<VideoPlayer, VideoClip>
    {
        public TextMeshProUGUI source;
        public TextMeshProUGUI description;

        public override void Show(VideoClip value)
        {
            base.Show(value);
            source?.gameObject.SetActive(false);
            description?.gameObject.SetActive(false);
            if (component) component.clip = value;
        }

        public virtual void Show(VideoClip value, string description, string source)
        {
            Show(value);
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