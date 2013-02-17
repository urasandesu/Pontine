/* 
 * File: ExecutionContextProxy.cs
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
using System.Management.Automation.Runspaces;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Mixins.System;

namespace Urasandesu.Pontine.Management.Automation
{
    public class ExecutionContextProxy
    {
        public static readonly Type Type = typeof(Runspace).Assembly.GetType("System.Management.Automation.ExecutionContext");

        object m_target;

        internal ExecutionContextProxy(object target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (target.GetType() != Type)
                throw new ArgumentException(string.Format("Value is not a type '{0}'.", Type.FullName), "target");

            m_target = target;
        }

        const string PropertyName_EngineSessionState = "EngineSessionState";

        class PropertyGetterDelegate_EngineSessionState
        {
            public static readonly Exec Get = Type.GetPropertyGetterDelegate(PropertyName_EngineSessionState);
        }

        public SessionStateInternalProxy EngineSessionState
        {
            get
            {
                return new SessionStateInternalProxy(PropertyGetterDelegate_EngineSessionState.Get(m_target));
            }
        }
    }
}
