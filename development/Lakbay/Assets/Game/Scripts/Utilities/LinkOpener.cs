/*
 * Date Created: Friday, May 20, 2022 10:36 PM
 * Author: 
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utilities {

    // Source: https://forum.unity.com/threads/clickable-link-within-a-text.910226/#post-7320628
    [RequireComponent(typeof(TMP_Text))]
    public class LinkOpener : MonoBehaviour, IPointerClickHandler {
    
        public void OnPointerClick(PointerEventData eventData) {
            TMP_Text pTextMeshPro = GetComponent<TMP_Text>();
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, eventData.position, null);  // If you are not in a Canvas using Screen Overlay, put your camera instead of null
            if (linkIndex != -1) { // was a link clicked?
                TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];
                Debug.Log(linkInfo.GetLinkID());
                Application.OpenURL(linkInfo.GetLinkID());
            }
        }
    
    }
}