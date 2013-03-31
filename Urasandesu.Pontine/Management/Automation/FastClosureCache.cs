/* 
 * File: FastClosureCache.cs
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
using Urasandesu.Pontine.Mixins.System.Management.Automation;
using Urasandesu.Pontine.Mixins.System.Management.Automation.Runspaces;

namespace Urasandesu.Pontine.Management.Automation
{
    public class FastClosureCache
    {
        readonly ScriptBlock m_source;
        readonly string m_fastClosureAutomaticParameterName;
        readonly string[] m_closedVariableNames;
        readonly object[] m_scopes;

        ScriptBlock m_closure;
        Dictionary<string, int> m_indexes = new Dictionary<string, int>();

        public FastClosureCache(ScriptBlock source, string closedVariableName)
            : this(source, new string[] { closedVariableName })
        {
        }

        public FastClosureCache(ScriptBlock source, string[] closedVariableNames)
            : this(source, ScriptBlockMixin.DefaultFastClosureAutomaticParameterName, closedVariableNames)
        {
        }

        public FastClosureCache(ScriptBlock source, string fastClosureAutomaticParameterName, string closedVariableName)
            : this(source, fastClosureAutomaticParameterName, new string[] { closedVariableName })
        {
        }

        public FastClosureCache(ScriptBlock source, string fastClosureAutomaticParameterName, string[] closedVariableNames)
            : this(source, fastClosureAutomaticParameterName, new object[] { 0 }, closedVariableNames)
        {
        }

        public FastClosureCache(ScriptBlock source, string fastClosureAutomaticParameterName, object[] scopes, string closedVariableName)
            : this(source, fastClosureAutomaticParameterName, scopes, new string[] { closedVariableName })
        {
        }

        public FastClosureCache(ScriptBlock source, string fastClosureAutomaticParameterName, object[] scopes, string[] closedVariableNames)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (string.IsNullOrEmpty(fastClosureAutomaticParameterName))
                throw new ArgumentNullException("fastClosureAutomaticParameterName");
            if (scopes == null)
                throw new ArgumentNullException("scopes");
            if (closedVariableNames == null)
                throw new ArgumentNullException("closedVariableNames");

            m_source = source;
            m_fastClosureAutomaticParameterName = fastClosureAutomaticParameterName;
            m_scopes = scopes;
            m_closedVariableNames = closedVariableNames;
        }

        public ScriptBlock GetNewOrUpdatedFastClosure()
        {
            if (m_closure == null)
            {
                m_closure = m_source.GetNewFastClosureWithSpecifiedParameterName(m_fastClosureAutomaticParameterName, m_scopes);
            }
            else
            {
                var aggregatedVariables = RunspaceMixin.DefaultRunspace.GetAggregatedVariables(m_scopes);
                foreach (var closedVariableName in m_closedVariableNames)
                {
                    var value = aggregatedVariables[closedVariableName].Value;

                    var @params = m_closure.Get_RuntimeDefinedParameters();
                    @params[closedVariableName].Value = value;

                    var paramList = m_closure.Get_RuntimeDefinedParameterList();
                    if (!m_indexes.ContainsKey(closedVariableName))
                    {
                        for (int i = 0; i < paramList.Count; i++)
                        {
                            if (paramList[i].Name == closedVariableName)
                            {
                                paramList[i].Value = value;
                                m_indexes.Add(closedVariableName, i);
                                break;
                            }
                        }
                    }
                    else
                    {
                        paramList[m_indexes[closedVariableName]].Value = value;
                    }
                }
            }
            return m_closure;
        }
    }
}
