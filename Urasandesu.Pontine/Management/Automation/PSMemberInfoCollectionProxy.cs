/* 
 * File: PSMemberInfoCollectionProxy.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using Urasandesu.Pontine.Mixins.System.Management.Automation;

namespace Urasandesu.Pontine.Management.Automation
{
    public abstract class PSMemberInfoCollectionProxy<T> : IEnumerable<T>, IEnumerable where T : PSMemberInfo
    {
        public PSMemberInfoCollection<T> Target { get; private set; }

        public PSMemberInfoCollectionProxy(PSMemberInfoCollection<T> target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (!typeof(PSMemberInfoCollection<T>).IsAssignableFrom(target.GetType()))
                throw new ArgumentException(string.Format("Value is not a type '{0}'.", typeof(PSMemberInfoCollection<T>).FullName), "target");

            Target = target;
        }

        public virtual T this[string name]
        {
            get { return Target[name]; }
        }

        public virtual void Add(T member)
        {
            Target.Add(member);
        }

        public virtual void Add(T member, bool preValidated)
        {
            Target.Add(member, preValidated);
        }

        public virtual void Remove(string name)
        {
            Target.Remove(name);
        }

        public virtual ReadOnlyPSMemberInfoCollection<T> Match(string name)
        {
            return Target.Match(name);
        }

        public virtual ReadOnlyPSMemberInfoCollection<T> Match(string name, PSMemberTypes memberTypes)
        {
            return Target.Match(name, memberTypes);
        }

        public virtual ReadOnlyPSMemberInfoCollection<T> Match(string name, PSMemberTypes memberTypes, MshMemberMatchOptions matchOptions)
        {
            return PSMemberInfoCollectionMixin.Match(Target, name, memberTypes, matchOptions);
        }

        public static bool IsReservedName(string name)
        {
            return PSMemberInfoCollectionMixin.IsReservedName(name);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return Target.GetEnumerator();
        }
    }
}
