/* 
 * File: RunspaceMixin.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2012 Akira Sugiura
 *  
 *  This software is MIT License.
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.Mixins.System.Linq;
using Urasandesu.Pontine.Management.Automation;

namespace Urasandesu.Pontine.Mixins.System.Management.Automation.Runspaces
{
    public static class RunspaceMixin
    {
        public static readonly string[] AutomaticVariableNames = new string[] { "$", "?", "^", "_", "args", "ConfirmPreference", "ConsoleFileName", "DebugPreference", "Error", "ErrorActionPreference", "ErrorView", "ExecutionContext", "false", "FormatEnumerationLimit", "HOME", "Host", "input", "LASTEXITCODE", "MaximumAliasCount", "MaximumDriveCount", "MaximumErrorCount", "MaximumFunctionCount", "MaximumHistoryCount", "MaximumVariableCount", "MyInvocation", "NestedPromptLevel", "null", "OutputEncoding", "PID", "PROFILE", "ProgressPreference", "PSBoundParameters", "PSCulture", "PSEmailServer", "PSHOME", "PSSessionApplicationName", "PSSessionConfigurationName", "PSSessionOption", "PSUICulture", "PSUnitPath", "PSVersionTable", "PWD", "ReportErrorShowExceptionClass", "ReportErrorShowInnerException", "ReportErrorShowSource", "ReportErrorShowStackTrace", "ShellId", "StackTrace", "this", "true", "VerbosePreference", "WarningPreference", "WhatIfPreference" };
        public static readonly HashSet<string> AutomaticVariableNameMap;

        static RunspaceMixin()
        {
            AutomaticVariableNameMap = new HashSet<string>();
            foreach (var variableName in AutomaticVariableNames)
                AutomaticVariableNameMap.Add(variableName);
        }

        public static Runspace DefaultRunspace
        {
            get
            {
                if (Runspace.DefaultRunspace == null)
                    new RunspaceInvoke();
                return Runspace.DefaultRunspace;
            }
        }
        
        const string PropertyName_ExecutionContext = "ExecutionContext";

        class PropertyGetterDelegate_ExecutionContext
        {
            public static readonly Exec Get = typeof(Runspace).GetPropertyGetterDelegate(PropertyName_ExecutionContext);
        }

        public static ExecutionContextProxy Get_ExecutionContext(this Runspace source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new ExecutionContextProxy(PropertyGetterDelegate_ExecutionContext.Get(source));
        }

        internal readonly static Func<object, int> ObjectToScopeValue =
            scope =>
            {
                var s = default(string);
                var i = default(int?);
                if ((s = scope as string) != null)
                    return StringToScopeValue(s);
                else if ((i = scope as int?) != null)
                    return (int)i;
                else
                    throw new ArgumentException("Cannot process argument because the value of argument \"scopes\" is invalid.", "scopes");
            };

        internal readonly static Func<string, int> StringToScopeValue =
            scope =>
            {
                var i = default(int);
                if (string.IsNullOrEmpty(scope) || string.Compare(scope, "Local", true) == 0)
                    return 0;
                else if (int.TryParse(scope, out i))
                    return i;
                else
                    throw new ArgumentException("Cannot process argument because the value of argument \"scopes\" is invalid.", "scopes");
            };

        public static IDictionary<string, PSVariable> GetAggregatedVariables(this Runspace source, params object[] scopes)
        {
            if (scopes == null)
                throw new ArgumentNullException("scopes");

            return source.GetAggregatedVariables(scopes.Select(ObjectToScopeValue).ToArray());
        }

        public static IDictionary<string, PSVariable> GetAggregatedVariables(this Runspace source, int[] scopeValues)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var aggregatedVariables = new Dictionary<string, PSVariable>();
            foreach (var scopeValue in scopeValues)
            {
                var lastScope = default(SessionStateScopeProxy);
                var variables = default(IDictionary<string, PSVariable>);
                switch (scopeValue)
                {
                    case int.MaxValue:
                        if (lastScope == null)
                            lastScope = source.Get_ExecutionContext().EngineSessionState.CurrentScope;
                        for (lastScope = lastScope.Parent; lastScope != null; lastScope = lastScope.Parent)
                            if (variables != null)
                                variables.AddOrUpdateRange(lastScope.Variables);
                            else
                                variables = new Dictionary<string, PSVariable>(lastScope.Variables);
                        break;
                    default:
                        var currentScope = source.Get_ExecutionContext().EngineSessionState.CurrentScope;
                        for (int i = 0; i < scopeValue && currentScope.Parent != null; i++)
                            currentScope = currentScope.Parent;
                        variables = new Dictionary<string, PSVariable>(currentScope.Variables);
                        lastScope = currentScope;
                        break;
                }
                aggregatedVariables.AddOrUpdateRange(variables);
            }
            return aggregatedVariables;
        }

        public static IDictionary<string, PSVariable> GetAggregatedUserDefinedMutableVariables(this Runspace source, params object[] scopes)
        {
            if (scopes == null)
                throw new ArgumentNullException("scopes");

            return source.GetAggregatedUserDefinedMutableVariables(scopes.Select(ObjectToScopeValue).ToArray());
        }

        public static IDictionary<string, PSVariable> GetAggregatedUserDefinedMutableVariables(this Runspace source, int[] scopeValues)
        {
            var variableEntries = source.GetAggregatedVariables(scopeValues);
            variableEntries.MutableForEach((@this, entry) =>
            {
                var variable = entry.Value;
                if ((variable.Options & ScopedItemOptions.Constant) == ScopedItemOptions.Constant ||
                    (variable.Options & ScopedItemOptions.ReadOnly) == ScopedItemOptions.ReadOnly ||
                    (variable.Options & ScopedItemOptions.AllScope) == ScopedItemOptions.AllScope ||
                    AutomaticVariableNameMap.Contains(variable.Name))
                    @this.Remove(variable.Name);
            });
            return variableEntries;
        }

        public static void CopyVariablesTo(this Runspace source, Runspace destination, params object[] scopes)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");

            var variableEntries = source.GetAggregatedUserDefinedMutableVariables(scopes);
            foreach (var variableEntry in variableEntries)
            {
                var variable = variableEntry.Value;
                destination.SessionStateProxy.SetVariable(variable.Name, variable.Value);
            }
        }
    }
}
