/* 
 * File: SessionStateInternalProxy.cs
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
    public class SessionStateInternalProxy
    {
        public static readonly Type Type = typeof(Runspace).Assembly.GetType("System.Management.Automation.SessionStateInternal");

        object m_target;

        internal SessionStateInternalProxy(object target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (target.GetType() != Type)
                throw new ArgumentException(string.Format("Value is not a type '{0}'.", Type.FullName), "target");

            m_target = target;
        }

        const string PropertyName_CurrentScope = "CurrentScope";

        class PropertyGetterDelegate_CurrentScope
        {
            public static readonly Exec Get = Type.GetPropertyGetterDelegate(PropertyName_CurrentScope);
        }

        public SessionStateScopeProxy CurrentScope
        {
            get
            {
                return new SessionStateScopeProxy(PropertyGetterDelegate_CurrentScope.Get(m_target));
            }
        }

        const string PropertyName_GlobalScope = "GlobalScope";

        class PropertyGetterDelegate_GlobalScope
        {
            public static readonly Exec Get = Type.GetPropertyGetterDelegate(PropertyName_GlobalScope);
        }

        public SessionStateScopeProxy GlobalScope
        {
            get
            {
                return new SessionStateScopeProxy(PropertyGetterDelegate_GlobalScope.Get(m_target));
            }
        }

        const string PropertyName_ModuleScope = "ModuleScope";

        class PropertyGetterDelegate_ModuleScope
        {
            public static readonly Exec Get = Type.GetPropertyGetterDelegate(PropertyName_ModuleScope);
        }

        public SessionStateScopeProxy ModuleScope
        {
            get
            {
                return new SessionStateScopeProxy(PropertyGetterDelegate_ModuleScope.Get(m_target));
            }
        }

        const string PropertyName_ScriptScope = "ScriptScope";

        class PropertyGetterDelegate_ScriptScope
        {
            public static readonly Exec Get = Type.GetPropertyGetterDelegate(PropertyName_ScriptScope);
        }

        public SessionStateScopeProxy ScriptScope
        {
            get
            {
                return new SessionStateScopeProxy(PropertyGetterDelegate_ScriptScope.Get(m_target));
            }
        }
    }
}
