// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Speedygeek.ZendeskAPI.Contract;
using Speedygeek.ZendeskAPI.Testing;

namespace Speedygeek.ZendeskAPI.Configuration
{
    /// <summary>
    /// A set of properties that affect Zendesk.Rest behavior.
    /// </summary>
    public class SettingsBase
    {
        // current values are dictionary-backed so we can check for key existence. Can't do null-coalescing
        // because if a setting is set to null at the request level, that should stick.
        private readonly IDictionary<string, object> _currentValues = new Dictionary<string, object>();

        private SettingsBase _defaults;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsBase"/> class.
        /// </summary>
        public SettingsBase()
        {
            ResetDefaults();
        }

        /// <summary>
        /// Gets or sets the default values to fall back on when values are not explicitly set on this instance.
        /// </summary>
        public virtual SettingsBase Defaults
        {
            get => _defaults ?? ZendeskConfig.GlobalSettings;
            set => _defaults = value;
        }

        /// <summary>
        /// Gets or sets the HTTP request timeout.
        /// </summary>
        public TimeSpan? Timeout
        {
            get => Get(() => Timeout);
            set => Set(() => Timeout, value);
        }

        /// <summary>
        /// Gets or sets object used to serialize and deserialize JSON.
        /// </summary>
        public ISerializer JsonSerializer
        {
            get => Get(() => JsonSerializer);
            set => Set(() => JsonSerializer, value);
        }

        /// <summary>
        /// Resets all overridden settings to their default values. For example, on a ZenRequest,
        /// all settings are reset to ZenClient-level settings.
        /// </summary>
        public virtual void ResetDefaults()
        {
            _currentValues.Clear();
        }

        /// <summary>
        /// Gets a settings value from this instance if explicitly set, otherwise from the default settings that back this instance.
        /// </summary>
        /// <typeparam name="T">The  type for property to be retrieved.</typeparam>
        /// <param name="property"> expression for which property to retrieve is value.</param>
        /// <returns>the value for the requesting property or default for the <typeparamref name="T"/>.</returns>
        protected T Get<T>(Expression<Func<T>> property)
        {
            var p = (property.Body as MemberExpression).Member as PropertyInfo;
            var testVals = HttpTest.Current?.Settings._currentValues;
            return testVals?.ContainsKey(p.Name) == true ? (T)testVals[p.Name] :
                _currentValues.ContainsKey(p.Name) ? (T)_currentValues[p.Name] :
                Defaults != null ? (T)p.GetValue(Defaults) : default;
        }

        /// <summary>
        /// Sets a settings value for this instance.
        /// </summary>
        /// <typeparam name="T">The  type of property to be set.</typeparam>
        /// <param name="property"> expression for which property is to be updated.</param>
        /// <param name="value"> value the property should be updated to.</param>
        protected void Set<T>(Expression<Func<T>> property, T value)
        {
            var p = (property.Body as MemberExpression).Member as PropertyInfo;
            _currentValues[p.Name] = value;
        }
    }
}
