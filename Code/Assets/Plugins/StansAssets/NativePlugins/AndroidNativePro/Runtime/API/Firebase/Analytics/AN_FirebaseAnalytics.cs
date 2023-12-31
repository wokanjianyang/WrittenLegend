#if AN_FIREBASE_ANALYTICS && (UNITY_IOS || UNITY_ANDROID)
#define API_ENABLED
#endif

using System;
using System.Collections.Generic;

#if API_ENABLED
using Firebase.Analytics;
#endif

namespace SA.Android.Firebase.Analytics
{
    /// <summary>
    /// Firebase Analytics API proxy.
    /// </summary>
    public static class AN_FirebaseAnalytics
    {

        /// <summary>
        /// Returns `true` if firebase analytics is enabled in the project and `false` otherwise.
        /// </summary>
        public static bool IsAvailable {
            get {
#if API_ENABLED
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Logs an app event.
        /// </summary>
        /// <param name="eventName">event name.</param>
        public static void LogEvent(string eventName)
        {
#if API_ENABLED
            FirebaseAnalytics.LogEvent(eventName);
#endif
        }

        /// <summary>
        /// Logs an app event.
        /// </summary>
        /// <param name="eventName">event name.</param>
        /// <param name="data">event data.</param>
        public static void LogEvent(string eventName, IDictionary<string, object> data)
        {
#if API_ENABLED
            var parameters = new List<Parameter>();
            foreach (var pair in data) {
                var key = pair.Key;
                var value = pair.Value;
                Parameter p = null;

                switch (value)
                {
                    case double _:
                    case float _:
                        p = new Parameter(key, Convert.ToDouble(value));
                        break;
                    case short _:
                    case int _:
                    case long _:
                        p = new Parameter(key, Convert.ToInt64(value));
                        break;
                }

                if(p == null) {
                    p = new Parameter(key, Convert.ToString(value));
                }

                parameters.Add(p);
            }


            FirebaseAnalytics.LogEvent(eventName, parameters.ToArray());
#endif
        }

        /// <summary>
        /// Tracking Monetization (optional).
        /// </summary>
        /// <param name="productId">The id of the purchased item.</param>
        /// <param name="amount">The price of the item.</param>
        /// <param name="currency">
        ///  Abbreviation of the currency used for the transaction. For example “USD” (United
        ///  States Dollars). See http:en.wikipedia.orgwikiISO_4217 for a standardized list
        ///  of currency abbreviations.
        /// </param>
        public static void Transaction(string productId, float amount, string currency)
        {
#if API_ENABLED
            var parameters = new List<Parameter>();
            parameters.Add(new Parameter(FirebaseAnalytics.ParameterCurrency, currency));
            parameters.Add(new Parameter(FirebaseAnalytics.ParameterItemId, productId));
            parameters.Add(new Parameter(FirebaseAnalytics.ParameterPrice, amount));
            parameters.Add(new Parameter(FirebaseAnalytics.ParameterQuantity, 1));

            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventEcommercePurchase, parameters.ToArray());
#endif
        }

        /// <summary>
        /// Sets the duration of inactivity that terminates the current session.
        /// The default value is (30 minutes).
        /// </summary>
        /// <param name="timeSpan">time span</param>
        public static void SetSessionTimeoutDuration(TimeSpan timeSpan)
        {
#if AN_FIREBASE_ANALYTICS && (UNITY_IOS || UNITY_ANDROID)
            FirebaseAnalytics.SetSessionTimeoutDuration(timeSpan);
#endif
        }

        /// <summary>
        /// Sets the user ID property.
        /// </summary>
        /// <param name="userId">user id.</param>
        public static void SetUserId(string userId)
        {
#if API_ENABLED
            FirebaseAnalytics.SetUserId(userId);
#endif
        }

        /// <summary>
        /// Sets a user property to a given value.
        /// </summary>
        /// <param name="name">property name.</param>
        /// <param name="property">property value.</param>
        public static void SetUserProperty(string name, string property)
        {
#if API_ENABLED
            FirebaseAnalytics.SetUserProperty(name, property);
#endif
        }
    }
}
