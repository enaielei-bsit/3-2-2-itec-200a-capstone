/*
 * Date Created: Sunday, October 10, 2021 5:26 AM
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
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Utilities {
    public static class Extension {
        // string
        public static string TrimEnd(this string str, string substring) {
            if(str.EndsWith(substring)) return str.Substring(
                0, str.LastIndexOf(substring)
            );
            return str;
        }

        public static string CopyToClipboard(this string str) {
            GUIUtility.systemCopyBuffer = str;
            return str;
        }

        // IEnumerable
        public static T PickRandomly<T>(this IEnumerable<T> enumerable) {
            var array = enumerable.ToArray();
            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable) {
            var enumerable_ = enumerable.ToList();
            var newEnumerable = new List<T>();

            while(enumerable_.Count > 0) {
                var popped = enumerable_.PopRandomly();
                newEnumerable.Add(popped);
            }

            return newEnumerable;
        }

        public static string[] ToCodes(this IEnumerable<Locale> locales) {
            return locales.Select((l) => l.Identifier.Code).ToArray();
        }

        public static bool All(this IEnumerable<bool> enumerable) {
            return enumerable.All((i) => i);
        }

        public static bool Any(this IEnumerable<bool> enumerable) {
            return enumerable.Any((i) => i);
        }

        public static string Join<T>(
            this IEnumerable<T> enumerable, string separator
        ) {
            return string.Join(separator, enumerable);
        }

        // IList
        public static T Pop<T>(this IList<T> list, int index=0) {
            var item = list[index];
            list.RemoveAt(index);
            return item;
        }

        public static T PopRandomly<T>(this IList<T> list) {
            var index = Enumerable.Range(0, list.Count).PickRandomly();
            return list.Pop(index);
        }

        // GameObject
        public static bool IsEmpty(this GameObject gameObject) {
            return gameObject.transform.childCount == 0;
        }

        public static GameObject[] Children(this GameObject gameObject) {
            if(gameObject.IsEmpty()) return new GameObject[] {};
            return (from i in Enumerable.Range(0, gameObject.transform.childCount)
                select gameObject.transform.GetChild(i).gameObject).ToArray();
        }

        public static GameObject FirstChild(this GameObject gameObject) {
            if(gameObject.IsEmpty()) return null;
            return gameObject.transform.GetChild(0).gameObject;
        }

        public static GameObject LastChild(this GameObject gameObject) {
            if(gameObject.IsEmpty()) return null;
            return gameObject.transform.GetChild(
                gameObject.transform.childCount - 1).gameObject;
        }
    }
}