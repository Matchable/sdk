﻿namespace MatchableSDK
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Utils;

    /// <summary>
    /// Actions sent to the Matchable API
    /// </summary>
    class MatchableAction
    {
        /// <summary>
        /// Add a new action with the given type and parameters for the default player 
        /// This action is saved locally and can be sent via the SendPlayerActions() method
        /// </summary>
        /// <param name="type">Action type (ex: game_start)</param>
        /// <param name="parameters">Action parameters (JSON string)</param>
        public static Hashtable Create(string type, object parameters)
        {
            Hashtable action = new Hashtable();
            action.Add("player_id", MatchableSettings.GetPlayerId());
            action.Add("type", type);
            action.Add("parameters", parameters);
            action.Add("date", TimeStamp.UnixTimeStampUTC());
            return action;
        }

        /// <summary>
        /// Sends the "start_session" action with the game version provided in MatchableSettings or PlayerSettings
        /// and system information about the device
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <code>
        /// StartCoroutine(MatchableAction.StartSession((response) =>
        /// {
        ///     Debug.Log(response.ToJsonString());
        /// }));
        /// </code>
        public static IEnumerator StartSession(Action<MatchableResponse> callback)
        {
            Hashtable systemInfo = new Hashtable();
            systemInfo.Add("device_model", SystemInfo.deviceModel);
            systemInfo.Add("device_type", SystemInfo.deviceType);
            systemInfo.Add("operating_system", SystemInfo.operatingSystem);

            Hashtable parameters = new Hashtable();
            parameters.Add("version", MatchableSettings.GetGameVersion());
            parameters.Add("system_info", systemInfo);

            yield return Matchable.SendAction("start_session", parameters, callback);
        }

        /// <summary>
        /// Sends the "start_game" action with the provided parameters
        /// </summary>
        /// <param name="parameters">The game specific parameters.</param>
        /// <param name="callback">The callback.</param>
        /// <code>
        /// Hashtable parameters = new Hashtable();
        /// parameters.Add("game_type", "tactical");
        /// parameters.Add("xp", "0");
        /// parameters.Add("player_lvl", "1");
        /// StartCoroutine(MatchableAction.StartGame((parameters, response) =>
        /// {
        ///     Debug.Log(response.ToJsonString());
        /// }));
        /// </code>
        public static IEnumerator StartGame(Hashtable parameters, Action<MatchableResponse> callback)
        {
            yield return Matchable.SendAction("start_game", parameters, callback);
        }

        /// <summary>
        /// Sends the "game_result" action with the provided parameters
        /// </summary>
        /// <param name="parameters">The game result parameters.</param>
        /// <param name="callback">The callback.</param>
        /// <code>
        /// Hashtable parameters = new Hashtable();
        /// parameters.Add("game_type", "tactical");
        /// parameters.Add("xp", "0");
        /// parameters.Add("player_lvl", "1");
        /// StartCoroutine(MatchableAction.GameResult((parameters, response) =>
        /// {
        ///     Debug.Log(response.ToJsonString());
        /// }));
        /// </code>
        public static IEnumerator GameResult(Hashtable parameters, Action<MatchableResponse> callback)
        {
            yield return Matchable.SendAction("game_result", parameters, callback);
        }

        /// <summary>
        /// Sends the retention action with the given type
        /// Sent each time you give a bonus or booster to the player
        /// </summary>
        /// <param name="type">The type of retention action (ex: invite_friend, free_crystals, daily_reward).</param>
        /// <param name="callback">The callback.</param>
        /// <code>
        /// StartCoroutine(MatchableAction.Retention("invite_friend", response) =>
        /// {
        ///     Debug.Log(response.ToJsonString());
        /// }));
        /// </code>
        public static IEnumerator Retention(string type, Action<MatchableResponse> callback)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("retention_type", type);
            yield return Matchable.SendAction("retention_action", parameters, callback);
        }

        /// <summary>
        /// Sends the conversion action with the given type
        /// Sent each time a player does any conversion action and gain rewards.
        /// </summary>
        /// <param name="type">The type of conversion action (ex: purchase).</param>
        /// <param name="callback">The callback.</param>
        /// <code>
        /// StartCoroutine(MatchableAction.Conversion("purchase", response) =>
        /// {
        ///     Debug.Log(response.ToJsonString());
        /// }));
        /// </code>
        public static IEnumerator Conversion(string type, Action<MatchableResponse> callback)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("conversion_type", type);
            yield return Matchable.SendAction("conversion_action", parameters, callback);
        }
    }
}
