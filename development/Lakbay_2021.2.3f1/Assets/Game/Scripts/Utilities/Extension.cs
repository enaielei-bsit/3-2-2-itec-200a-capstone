/*
 * Date Created: Sunday, October 10, 2021 5:26 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Humanizer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Utilities.Helper;

namespace Utilities {
    public static class Extension {
        // float
        public static bool Within(this float value, float min, float max) {
            return value >= min && value <= max;
        }

        public static bool Between(this float value, float min, float max) {
            return value > min && value < max;
        }

        // source: https://stackoverflow.com/a/2691042/14733693
        public static float Mod(this float n1, float n2) {
            float result = n1 % n2;
            if((result < 0 && n2 > 0) || (result > 0 && n2 <0)) {
                result += n2;
            }
            return result;
        }

        // int
        public static bool Within(this int value, int min, int max) {
            return ((float) value).Within(min, max);
        }

        public static bool Between(this int value, int min, int max) {
            return ((float) value).Between(min, max);
        }

        public static int Mod(this int n1, int n2) {
            int result = n1 % n2;
            if((result < 0 && n2 > 0) || (result > 0 && n2 <0)) {
                result += n2;
            }
            return result;
        }

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

        public static T DeserializeAsYaml<T>(this string str) {
            return Helper.YamlDeserializer.Deserialize<T>(str);
        }

        // source: https://stackoverflow.com/a/298990/14733693
        private static string TrimMatchingQuotes(this string input, char quote) {
            if ((input.Length >= 2) && 
                (input[0] == quote) && (input[input.Length - 1] == quote))
                return input.Substring(1, input.Length - 2);

            return input;
        }

        private static IEnumerable<string> Split(this string str, 
                                            Func<char, bool> controller) {
            int nextPiece = 0;

            for (int c = 0; c < str.Length; c++)
            {
                if (controller(str[c]))
                {
                    yield return str.Substring(nextPiece, c - nextPiece);
                    nextPiece = c + 1;
                }
            }

            yield return str.Substring(nextPiece);
        }

        public static IEnumerable<string> ToCommandLine(this string str) {
            bool inQuotes = false;

            return str.Split((c) => {
                    if (c == '\"') inQuotes = !inQuotes;

                    return !inQuotes && c == ' ';
                })
                .Select(arg => arg.Trim().TrimMatchingQuotes('\"'))
                .Where(arg => !string.IsNullOrEmpty(arg));
        }

        public static Locale GetLocale(this string str, Locale[] locales, out string name) {
            var names = str.Humanize().Split(' ').ToList();
            var code = names.Pop(names.Count - 1);
            name = names.Join(" ").Dehumanize();
            return Array.Find(
                locales, (l) => l.Identifier.Code.ToLower() == code.ToLower());
        }

        public static Locale GetLocale(this string str, out string name) {
            return str.GetLocale(Helper.locales, out name);
        }

        public static Locale GetLocale(this string str) => str.GetLocale(out string name);

        // T
        public static bool Either<T>(
            this T obj, out T value, params T[] values) {
            value = default;
            foreach(var value_ in values) {
                if(obj.Equals(value_)) {
                    value = value_;
                    return true;
                }
            }

            return false;
        }

        public static bool Either<T>(
            this T obj, params T[] values) {
            return obj.Either(out var value, values);
        }

        public static Tuple<bool, T1, T1> Setter<T0, T1>(
            this T0 obj, ref T1 old, T1 @new=default, Action<T0, T1, T1> @event=null) {
            Tuple<bool, T1, T1> rv = new Tuple<bool, T1, T1>(
                !old.Equals(@new), old, @new);
            if(rv.Item1) @event?.Invoke(obj, @old, @new);
            return rv;
        }

        public static Tuple<bool, T1, T1> Setter<T0, T1>(
            this T0 obj, ref T1 old, T1 @new=default, Action<T1, T1> @event=null) {
            return obj.Setter(ref old, @new, (t, o, n) => @event?.Invoke(o, n));
        }

        public static Tuple<bool, T1, T1> Setter<T0, T1>(
            this T0 obj, ref T1 old, T1 @new=default, UnityEvent<T1> @event=null) {
            return obj.Setter(ref old, @new, (t, o, n) => @event?.Invoke(n));
        }

        public static string SerializeAsYaml<T>(this T obj) {
            return Helper.YamlSerializer.Serialize(obj);
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

        public static IEnumerable<KeyValuePair<int, T>> Enumerate<T>(
            this IEnumerable<T> enumerable) {
            return enumerable.Select((c, i) => new KeyValuePair<int, T>(i, c));
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
        public static Component EnsureComponent(
            this GameObject gameObject, Type type) {
            var component = gameObject.GetComponent(type);
            if(!component) component = gameObject.AddComponent(type);
            return component;
        }

        public static T EnsureComponent<T>(this GameObject gameObject)
            where T : Component {
            return (T) gameObject.EnsureComponent(typeof(T));
        }

        public static bool IsPersistent(this GameObject gameObject) {
            return gameObject.scene.IsDontDestroyOnLoad();
        }

        public static void MakePersistent(this GameObject gameObject) {
            if(!gameObject.IsPersistent()) UnityEngine.Object.DontDestroyOnLoad(gameObject);
        }

        public static bool IsExisting(this GameObject gameObject) {
            return gameObject.scene.IsValid();
        }

        public static GameObject CreateFirstInstance(
            this GameObject gameObject, string name=null) {
            var go = gameObject.IsExisting()
                ? gameObject : GameObject.Instantiate(gameObject);
            if(name == null) go.name = gameObject.name;
            else go.name = name;
            return go;
        }

        // Component
        public static T CreateFirstInstance<T>(this T component, string name=null)
            where T : Component {
            var go = component.gameObject.CreateFirstInstance(name);
            return go.GetComponent<T>();
        }

        // Transform
        public static bool IsEmpty(this Transform transform) {
            return transform.childCount == 0;
        }

        public static Transform[] GetChildren(this Transform transform) {
            if(transform.IsEmpty()) return new Transform[] {};
            return (from i in Enumerable.Range(
                0, transform.childCount)
                select transform.GetChild(i)).ToArray();
        }

        public static Transform FirstChild(this Transform transform) {
            if(transform.IsEmpty()) return null;
            return transform.GetChild(0);
        }

        public static Transform LastChild(this Transform transofrm) {
            if(transofrm.IsEmpty()) return null;
            return transofrm.GetChild(
                transofrm.childCount - 1);
        }

        public static void DestroyChildren(
            this Transform transform, float time=0.0f) {
            var children = transform.GetChildren();
            foreach(var child in children) {
                if(time > 0.0f) GameObject.Destroy(child.gameObject, time);
                else GameObject.Destroy(child.gameObject);
            }
        }

        public static void DestroyChildrenImmediately(
            this Transform transform, bool allowDestroyingAssets=false) {
            var children = transform.GetChildren();
            foreach(var child in children) {
                GameObject.DestroyImmediate(child.gameObject, allowDestroyingAssets);
            }
        }

        // MonoBehaviour
        public static Coroutine Run(
            this MonoBehaviour mono,
            ConditionalRunCondition condition=default,
            ConditionalRunOnStart onStart=default,
            ConditionalRunOnProgress onProgress=default,
            ConditionalRunOnFinish onFinish=default,
            bool fixedUpdate=false
        ) {
            return mono.StartCoroutine(Helper.Run(
                condition, onStart, onProgress, onFinish, fixedUpdate
            ));
        }

        public static Coroutine Run(
            this MonoBehaviour mono,
            float duration,
            TimedRunOnStart onStart=default,
            TimedRunOnProgress onProgress=default,
            TimedRunOnFinish onFinish=default,
            bool fixedUpdate=false
        ) {
            return mono.StartCoroutine(Helper.Run(
                duration, onStart, onProgress, onFinish, fixedUpdate
            ));
        }

        public static Coroutine Run(
            this MonoBehaviour mono,
            ConditionalRunOnStart onStart=default,
            ConditionalRunOnProgress onProgress=default,
            bool fixedUpdate=false
        ) {
            return mono.StartCoroutine(Helper.Run(
                onStart,
                onProgress,
                fixedUpdate
            ));
        }

        public static bool IsDontDestroyOnLoad(this Scene scene) {
            return scene.name == "DontDestroyOnLoad";
        }

        // LocalizedString
        public static void Set(this LocalizedString str, IEnumerable<object> args) {
            str.Arguments = new List<object>() {};
            str.Add(args);
        }

        public static void Set(this LocalizedString str, params object[] args) {
            str.Set(args.AsEnumerable());
        }

        public static void Add(this LocalizedString str, IEnumerable<object> args) {
            foreach(var arg in args.Enumerate()) {
                str.Arguments.Add(new Dictionary<string, object>() {{
                    arg.Key.ToString(), arg.Value}});
            }
        }

        public static void Add(this LocalizedString str, params object[] args) {
            str.Add(args.AsEnumerable());
        }
    }
}