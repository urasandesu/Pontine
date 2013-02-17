/* 
 * File: ScriptBlockMixin.cs
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
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.NAnonym.Mixins.System.Linq;
using Urasandesu.Pontine.Management.Automation;
using Urasandesu.Pontine.Mixins.System.Management.Automation.Runspaces;

namespace Urasandesu.Pontine.Mixins.System.Management.Automation
{
    public static class ScriptBlockMixin
    {
        public const string DefaultFastClosureAutomaticParameterName = "Params";

        const string PropertyName_SessionState = "SessionState";

        class PropertyGetterDelegate_SessionState
        {
            public static readonly Exec Get = typeof(ScriptBlock).GetPropertyGetterDelegate(PropertyName_SessionState);
        }

        public static SessionState Get_SessionState(this ScriptBlock source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (SessionState)PropertyGetterDelegate_SessionState.Get(source);
        }

        class PropertySetterDelegate_SessionState
        {
            public static readonly Effect Set = typeof(ScriptBlock).GetPropertySetterDelegate(PropertyName_SessionState);
        }

        public static void Set_SessionState(this ScriptBlock source, SessionState value)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            PropertySetterDelegate_SessionState.Set(source, value);
        }

        const string MethodName_Clone = "Clone";

        class MethodDelegate_Clone_bool
        {
            public static readonly Exec Invoke = typeof(ScriptBlock).GetMethodDelegate(MethodName_Clone, new Type[] { typeof(bool) });
        }

        public static ScriptBlock Clone(this ScriptBlock source, bool cloneHelpInfo)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (ScriptBlock)MethodDelegate_Clone_bool.Invoke(source, cloneHelpInfo);
        }

        public static ScriptBlock CloneWithIsolatedParameterDefinition(this ScriptBlock source)
        {
            var destination = source.Clone(false);

            var srcParamList = source.Get_RuntimeDefinedParameterList();
            var dstParamList = destination.Get_RuntimeDefinedParameterList();
            dstParamList = new List<RuntimeDefinedParameter>(srcParamList);
            destination.Set__runtimeDefinedParameterList(dstParamList);

            var srcParamDic = source.Get_RuntimeDefinedParameters();
            var dstParamDic = destination.Get_RuntimeDefinedParameters();
            dstParamDic = new RuntimeDefinedParameterDictionary();
            dstParamDic.AddRange(srcParamDic);
            destination.Set__runtimeDefinedParameters(dstParamDic);

            return destination;
        }

        public static ScriptBlock GetNewFastClosure(this ScriptBlock source)
        {
            return source.GetNewFastClosure(null);
        }

        public static ScriptBlock GetNewFastClosure(this ScriptBlock source, string scope)
        {
            return source.GetNewFastClosure(scope, DefaultFastClosureAutomaticParameterName);
        }

        public static ScriptBlock GetNewFastClosure(this ScriptBlock source, string scope, string fastClosureAutomaticParameterName)
        {
            var closure = source.CloneWithIsolatedParameterDefinition();
            var scopeValue = 0;
            if (string.IsNullOrEmpty(scope) || string.Compare(scope, "Local", true) == 0)
                scopeValue = 0;
            else if (string.Compare(scope, "Global", true) == 0)
                scopeValue = -1;
            else if (string.Compare(scope, "Script", true) == 0)
                scopeValue = -2;
            else if (!int.TryParse(scope, out scopeValue))
                throw new ArgumentException("Cannot process argument because the value of argument \"scope\" is invalid.", "scope");

            var paramList = closure.Get_RuntimeDefinedParameterList();
            var @params = closure.Get_RuntimeDefinedParameters();

            var paramDecl = closure.Get__parameterDeclaration();
            if (paramDecl == null)
            {
                var dummy = ScriptBlock.Create(@"param ([parameter(ValueFromRemainingArguments = $true)][Object[]]$" + fastClosureAutomaticParameterName + @" = @()) $null");
                closure.Set__parameterDeclaration(dummy.Get__parameterDeclaration());
                paramList.AddRange(dummy.Get_RuntimeDefinedParameterList());
                @params.AddRange(dummy.Get_RuntimeDefinedParameters());
            }

            var targetScope = default(SessionStateScopeProxy);
            switch (scopeValue)
            {
                case -2:
                    targetScope = RunspaceMixin.DefaultRunspace.Get_ExecutionContext().EngineSessionState.ScriptScope;
                    break;
                case -1:
                    targetScope = RunspaceMixin.DefaultRunspace.Get_ExecutionContext().EngineSessionState.GlobalScope;
                    break;
                default:
                    targetScope = RunspaceMixin.DefaultRunspace.Get_ExecutionContext().EngineSessionState.CurrentScope;
                    for (int i = 0; i < scopeValue && targetScope.Parent != null; i++)
                        targetScope = targetScope.Parent;
                    break;
            }
            var allVariables = targetScope.Variables;
            var userDefinedVariables = allVariables.Where(_ => !RunspaceMixin.AutomaticVariableNameMap.Contains(_.Key) && fastClosureAutomaticParameterName != _.Key);
            foreach (var userDefinedVariable in userDefinedVariables)
            {
                var variable = userDefinedVariable.Value;
                var name = variable.Name;
                var parameterType = variable.Value == null ? typeof(object) : variable.Value.GetType();
                var attributes = variable.Attributes;
                var param = new RuntimeDefinedParameter(name, parameterType, attributes);
                param.Value = variable.Value;
                paramList.Add(param);
                @params.Add(param.Name, param);
            }

            return closure;
        }

        const string FieldName__initialized = "_initialized";

        class FieldGetterDelegate__initialized
        {
            public static readonly Exec Get = typeof(ScriptBlock).GetFieldGetterDelegate(FieldName__initialized);
        }

        public static bool Get__initialized(this ScriptBlock source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (bool)FieldGetterDelegate__initialized.Get(source);
        }

        class FieldSetterDelegate__initialized
        {
            public static readonly Effect Set = typeof(ScriptBlock).GetFieldSetterDelegate(FieldName__initialized);
        }

        public static void Set__initialized(this ScriptBlock source, bool value)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            FieldSetterDelegate__initialized.Set(source, value);
        }
        
        const string FieldName__parameterDeclaration = "_parameterDeclaration";

        class FieldGetterDelegate__parameterDeclaration
        {
            public static readonly Exec Get = typeof(ScriptBlock).GetFieldGetterDelegate(FieldName__parameterDeclaration);
        }

        public static ParameterDeclarationNodeProxy Get__parameterDeclaration(this ScriptBlock source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (ParameterDeclarationNodeProxy)ParseTreeNodeProxy.New(FieldGetterDelegate__parameterDeclaration.Get(source));
        }

        class FieldSetterDelegate__parameterDeclaration
        {
            public static readonly Effect Set = typeof(ScriptBlock).GetFieldSetterDelegate(FieldName__parameterDeclaration);
        }

        public static void Set__parameterDeclaration(this ScriptBlock source, ParameterDeclarationNodeProxy value)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            FieldSetterDelegate__parameterDeclaration.Set(source, value.Target);
        }

        const string PropertyName_RuntimeDefinedParameterList = "RuntimeDefinedParameterList";

        class PropertyGetterDelegate_RuntimeDefinedParameterList
        {
            public static readonly Exec Get = typeof(ScriptBlock).GetPropertyGetterDelegate(PropertyName_RuntimeDefinedParameterList);
        }

        public static List<RuntimeDefinedParameter> Get_RuntimeDefinedParameterList(this ScriptBlock source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (List<RuntimeDefinedParameter>)PropertyGetterDelegate_RuntimeDefinedParameterList.Get(source);
        }

        const string PropertyName_RuntimeDefinedParameters = "RuntimeDefinedParameters";

        class PropertyGetterDelegate_RuntimeDefinedParameters
        {
            public static readonly Exec Get = typeof(ScriptBlock).GetPropertyGetterDelegate(PropertyName_RuntimeDefinedParameters);
        }

        public static RuntimeDefinedParameterDictionary Get_RuntimeDefinedParameters(this ScriptBlock source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (RuntimeDefinedParameterDictionary)PropertyGetterDelegate_RuntimeDefinedParameters.Get(source);
        }

        const string FieldName__runtimeDefinedParameterList = "_runtimeDefinedParameterList";

        class FieldGetterDelegate__runtimeDefinedParameterList
        {
            public static readonly Exec Get = typeof(ScriptBlock).GetFieldGetterDelegate(FieldName__runtimeDefinedParameterList);
        }

        public static List<RuntimeDefinedParameter> Get__runtimeDefinedParameterList(this ScriptBlock source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (List<RuntimeDefinedParameter>)FieldGetterDelegate__runtimeDefinedParameterList.Get(source);
        }

        class FieldSetterDelegate__runtimeDefinedParameterList
        {
            public static readonly Effect Set = typeof(ScriptBlock).GetFieldSetterDelegate(FieldName__runtimeDefinedParameterList);
        }

        public static void Set__runtimeDefinedParameterList(this ScriptBlock source, List<RuntimeDefinedParameter> value)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            FieldSetterDelegate__runtimeDefinedParameterList.Set(source, value);
        }

        const string FieldName__runtimeDefinedParameters = "_runtimeDefinedParameters";

        class FieldGetterDelegate__runtimeDefinedParameters
        {
            public static readonly Exec Get = typeof(ScriptBlock).GetFieldGetterDelegate(FieldName__runtimeDefinedParameters);
        }

        public static RuntimeDefinedParameterDictionary Get__runtimeDefinedParameters(this ScriptBlock source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (RuntimeDefinedParameterDictionary)FieldGetterDelegate__runtimeDefinedParameters.Get(source);
        }

        class FieldSetterDelegate__runtimeDefinedParameters
        {
            public static readonly Effect Set = typeof(ScriptBlock).GetFieldSetterDelegate(FieldName__runtimeDefinedParameters);
        }

        public static void Set__runtimeDefinedParameters(this ScriptBlock source, RuntimeDefinedParameterDictionary value)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            FieldSetterDelegate__runtimeDefinedParameters.Set(source, value);
        }
    }
}
