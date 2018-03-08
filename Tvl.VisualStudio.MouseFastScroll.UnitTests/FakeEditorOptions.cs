// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Utilities;

    internal class FakeEditorOptions : IEditorOptions
    {
        private readonly Dictionary<string, EditorOptionDefinition> _supportedOptions = new Dictionary<string, EditorOptionDefinition>();
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        public FakeEditorOptions(ExportProvider exportProvider, IPropertyOwner scope)
        {
            foreach (var optionDefinition in exportProvider.GetExportedValues<EditorOptionDefinition>())
            {
                if (!optionDefinition.IsApplicableToScope(scope))
                {
                    continue;
                }

                _supportedOptions[optionDefinition.Name] = optionDefinition;
            }
        }

        public event EventHandler<EditorOptionChangedEventArgs> OptionChanged
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        public IEnumerable<EditorOptionDefinition> SupportedOptions => _supportedOptions.Values;

        public IEditorOptions GlobalOptions => throw new NotImplementedException();

        public IEditorOptions Parent
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public bool ClearOptionValue(string optionId)
        {
            return _values.Remove(optionId);
        }

        public bool ClearOptionValue<T>(EditorOptionKey<T> key)
        {
            return ClearOptionValue(key.Name);
        }

        public T GetOptionValue<T>(string optionId)
        {
            return (T)(GetOptionValue(optionId) ?? default(T));
        }

        public T GetOptionValue<T>(EditorOptionKey<T> key)
        {
            return GetOptionValue<T>(key.Name);
        }

        public object GetOptionValue(string optionId)
        {
            if (!_values.TryGetValue(optionId, out var value))
            {
                if (_supportedOptions.TryGetValue(optionId, out var definition))
                {
                    value = definition.DefaultValue;
                }
            }

            return value;
        }

        public bool IsOptionDefined(string optionId, bool localScopeOnly)
        {
            return _values.ContainsKey(optionId)
                || (!localScopeOnly && _supportedOptions.ContainsKey(optionId));
        }

        public bool IsOptionDefined<T>(EditorOptionKey<T> key, bool localScopeOnly)
        {
            return IsOptionDefined(key.Name, localScopeOnly);
        }

        public void SetOptionValue(string optionId, object value)
        {
            _values[optionId] = value;
        }

        public void SetOptionValue<T>(EditorOptionKey<T> key, T value)
        {
            SetOptionValue(key.Name, value);
        }
    }
}
