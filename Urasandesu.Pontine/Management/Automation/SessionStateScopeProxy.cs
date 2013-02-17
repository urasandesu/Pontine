/* 
 * File: SessionStateScopeProxy.cs
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

namespace Urasandesu.Pontine.Management.Automation
{
    public class SessionStateScopeProxy
    {
        public static readonly Type Type = typeof(Runspace).Assembly.GetType("System.Management.Automation.SessionStateScope");

        object m_target;

        internal SessionStateScopeProxy(object target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (target.GetType() != Type)
                throw new ArgumentException(string.Format("Value is not a type '{0}'.", Type.FullName), "target");

            m_target = target;
        }

        const string PropertyName_Variables = "Variables";

        class PropertyGetterDelegate_Variables
        {
            public static readonly Exec Get = Type.GetPropertyGetterDelegate(PropertyName_Variables);
        }

        public IDictionary<string, PSVariable> Variables
        {
            get
            {
                return (IDictionary<string, PSVariable>)PropertyGetterDelegate_Variables.Get(m_target);
            }
        }

        const string PropertyName_Parent = "Parent";

        class PropertyGetterDelegate_Parent
        {
            public static readonly Exec Get = Type.GetPropertyGetterDelegate(PropertyName_Parent);
        }

        class PropertySetterDelegate_Parent
        {
            public static readonly Effect Set = Type.GetPropertySetterDelegate(PropertyName_Parent);
        }

        public SessionStateScopeProxy Parent
        {
            get
            {
                return new SessionStateScopeProxy(PropertyGetterDelegate_Parent.Get(m_target));
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                PropertySetterDelegate_Parent.Set(m_target, value.m_target);
            }
        }
    }
}
