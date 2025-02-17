﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="APIHelper.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   APIHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable InconsistentNaming
namespace Sharlayan.Utilities {
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Sharlayan.Models;
    using Sharlayan.Models.Structures;
    using Sharlayan.Models.XIVDatabase;

    using StatusItem = Sharlayan.Models.XIVDatabase.StatusItem;

    internal static class APIHelper {
        private static WebClient _webClient = new WebClient {
            Encoding = Encoding.UTF8,
            CachePolicy =new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore)
        };

        public static async Task GetActions(ConcurrentDictionary<uint, ActionItem> actions, string patchVersion = "latest") {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "actions.json");
            if (File.Exists(file) && MemoryHandler.Instance.UseLocalCache) {
                EnsureDictionaryValues(actions, file);
            }
            else {
#if DEBUG
                await APIResponseToDictionary(actions, file, $"https://raw.githubusercontent.com/qitana/sharlayan-resources/develop/xivdatabase/{patchVersion}/actions.json");
#else
                await APIResponseToDictionary(actions, file, $"https://qitana.github.io/sharlayan-resources/xivdatabase/{patchVersion}/actions.json");
#endif
            }
        }

        public static async Task<IEnumerable<Signature>> GetSignatures(ProcessModel processModel, string patchVersion = "latest") {
            var architecture = processModel.IsWin64
                                   ? "x64"
                                   : "x86";
            var file = Path.Combine(Directory.GetCurrentDirectory(), $"signatures-{architecture}.json");
            if (File.Exists(file) && MemoryHandler.Instance.UseLocalCache) {
                var json = FileResponseToJSON(file);
                return JsonConvert.DeserializeObject<IEnumerable<Signature>>(json, Constants.SerializerSettings);
            }
            else {
                var json = await APIResponseToJSON($"https://raw.githubusercontent.com/FFXIVAPP/sharlayan-resources/master/signatures/{patchVersion}/{architecture}.json");
                IEnumerable<Signature> resolved = JsonConvert.DeserializeObject<IEnumerable<Signature>>(json, Constants.SerializerSettings);

                File.WriteAllText(file, JsonConvert.SerializeObject(resolved, Formatting.Indented, Constants.SerializerSettings), Encoding.GetEncoding(932));

                return resolved;
            }
        }

        public static async Task GetStatusEffects(ConcurrentDictionary<uint, StatusItem> statusEffects, string patchVersion = "latest") {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "statuses.json");
            if (File.Exists(file) && MemoryHandler.Instance.UseLocalCache) {
                EnsureDictionaryValues(statusEffects, file);
            }
            else {
                await APIResponseToDictionary(statusEffects, file, $"https://raw.githubusercontent.com/FFXIVAPP/sharlayan-resources/master/xivdatabase/{patchVersion}/statuses.json");
            }
        }

        public static async Task<StructuresContainer> GetStructures(ProcessModel processModel, string patchVersion = "latest") {
            var architecture = processModel.IsWin64
                                   ? "x64"
                                   : "x86";
            var file = Path.Combine(Directory.GetCurrentDirectory(), $"structures-{architecture}.json");
            if (File.Exists(file) && MemoryHandler.Instance.UseLocalCache) {
                return EnsureClassValues<StructuresContainer>(file);
            }

            return await APIResponseToClass<StructuresContainer>(file, $"https://raw.githubusercontent.com/FFXIVAPP/sharlayan-resources/master/structures/{patchVersion}/{architecture}.json");
        }

        public static async Task GetZones(ConcurrentDictionary<uint, MapItem> mapInfos, string patchVersion = "latest") {
            // These ID's link to offset 7 in the old JSON values.
            // eg: "map id = 4" would be 148 in offset 7.
            // This is known as the TerritoryType value
            // - It maps directly to SaintCoins map.csv against TerritoryType ID
            var file = Path.Combine(Directory.GetCurrentDirectory(), "zones.json");
            if (File.Exists(file) && MemoryHandler.Instance.UseLocalCache) {
                EnsureDictionaryValues(mapInfos, file);
            }
            else {
                await APIResponseToDictionary(mapInfos, file, $"https://raw.githubusercontent.com/FFXIVAPP/sharlayan-resources/master/xivdatabase/{patchVersion}/zones.json");
            }
        }

        private static async Task<T> APIResponseToClass<T>(string file, string uri) {
            var json = await APIResponseToJSON(uri);
            var resolved = JsonConvert.DeserializeObject<T>(json, Constants.SerializerSettings);

            File.WriteAllText(file, JsonConvert.SerializeObject(resolved, Formatting.Indented, Constants.SerializerSettings), Encoding.UTF8);

            return resolved;
        }

        private static async Task APIResponseToDictionary<T>(ConcurrentDictionary<uint, T> dictionary, string file, string uri) {
            var json = await APIResponseToJSON(uri);
            ConcurrentDictionary<uint, T> resolved = JsonConvert.DeserializeObject<ConcurrentDictionary<uint, T>>(json, Constants.SerializerSettings);

            foreach (KeyValuePair<uint, T> kvp in resolved) {
                dictionary.AddOrUpdate(kvp.Key, kvp.Value, (k, v) => kvp.Value);
            }

            File.WriteAllText(file, JsonConvert.SerializeObject(dictionary, Formatting.Indented, Constants.SerializerSettings), Encoding.UTF8);
        }

        private static async Task<string> APIResponseToJSON(string uri) {
            return await _webClient.DownloadStringTaskAsync(uri);
        }

        private static T EnsureClassValues<T>(string file) {
            var json = FileResponseToJSON(file);
            return JsonConvert.DeserializeObject<T>(json, Constants.SerializerSettings);
        }

        private static void EnsureDictionaryValues<T>(ConcurrentDictionary<uint, T> dictionary, string file) {
            var json = FileResponseToJSON(file);
            ConcurrentDictionary<uint, T> resolved = JsonConvert.DeserializeObject<ConcurrentDictionary<uint, T>>(json, Constants.SerializerSettings);

            foreach (KeyValuePair<uint, T> kvp in resolved) {
                dictionary.AddOrUpdate(kvp.Key, kvp.Value, (k, v) => kvp.Value);
            }
        }

        private static string FileResponseToJSON(string file) {
            using (var streamReader = new StreamReader(file)) {
                return streamReader.ReadToEnd();
            }
        }
    }
}