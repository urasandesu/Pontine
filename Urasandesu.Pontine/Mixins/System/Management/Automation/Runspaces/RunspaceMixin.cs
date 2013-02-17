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
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Mixins.System;
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
    }
}
