﻿namespace MatchableSDK
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;
#if UNITY_4_6 || UNITY_5
    using UnityEngine.EventSystems;
#endif
    using Utils;

    /// <summary>
    /// Provide methods to call Matchable API.
    /// For more information on integrating and using the Matchable SDK
    /// please visit our help site documentation at https://wiki.matchable.io/doku.php?id=info:api:v0.9
    /// </summary>
    public sealed class Matchable
    {

        /// <summary>
        /// Retrieve all the statistics available for the default player
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <code>
        /// StartCoroutine(Matchable.GetStats((response) =>
        /// {
        ///     Debug.Log(response.ToJsonString());
        /// }));
        /// </code>
        public static IEnumerator GetStats(Action<MatchableResponse> callback)
        {
            if (MatchableSettings.IsPluginEnabled())
            {
                WWW request = new WWW(MatchableSettings.GetPlayerEndpoint());
                yield return request;
                MatchableResponse response = new MatchableResponse(request);
                yield return response;
                callback(response);
            }
        }

        /// <summary>
        /// Send a player action with the given type and parameters.
        /// Then executes the given callback when the response is received.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public static IEnumerator SendAction(string type, object parameters, Action<MatchableResponse> callback)
        {
			Debug.LogError("SendAction(): parameter 'type' is required");

            if (MatchableSettings.IsPluginEnabled())
            {
                if (type == null)
                {
                    Debug.LogError("SendAction(): parameter 'type' is required");
                    yield return null;
                }

                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Content-Type", "application/json");

                Hashtable action = MatchableAction.Create(type, parameters);

                // Simple hack to wrap the action inside a JSON array
                string data = "[" + MJSON.Serialize(action) + "]";
                if (MatchableSettings.IsLoggingEnabled())
                {
                    Debug.Log("Sent action:" + data);
                }
                byte[] postData = AsciiEncoding.StringToAscii(data);

                WWW request = new WWW(MatchableSettings.GetActionsEndpoint(), postData, headers);
                yield return request;
                MatchableResponse response = new MatchableResponse(request);
                yield return response;
                callback(response);
            }
        }

        /// <summary>
        /// Retrieve a retention action for a given player. 
        /// This action is obtained using a strategy based on the different scores computed by Matchable. 
        /// The strategies can be specifically developped for each customer in collaboration with Matchable's data scientists.
        /// </summary>
        /// /// <code>
        /// StartCoroutine(Matchable.GetAdvisor((response) =>
        /// {
        ///     Debug.Log(response.ToJsonString());
        /// }));
        /// </code>
        public static IEnumerator GetAdvisor(Action<MatchableResponse> callback)
        {
			if (MatchableSettings.IsPluginEnabled())
            {
                WWW request = new WWW(MatchableSettings.GetAdvisorEndpoint());
                yield return request;
                MatchableResponse response = new MatchableResponse(request);
                yield return response;
                callback(response);
            }
        }

        /// <summary>
        /// Initializes the SDK plugin. (optional)
        /// Enabled by default in Matchable > EditSettings
        /// </summary>
        public static void Init()
        {
            MatchableSettings.SetPluginEnabled(true);
        }

        /// <summary>
        /// Disable the SDK plugin.
        /// Prevents all the method calls (useful for debuging other plugins)
        /// </summary>
        public static void Disable()
        {
            MatchableSettings.SetPluginEnabled(false);
        }
    }
}