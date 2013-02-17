/* 
 * File: PSMemberInfoCollectionMixin.cs
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
using System.Management.Automation;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.Pontine.Management.Automation;

namespace Urasandesu.Pontine.Mixins.System.Management.Automation
{
    public static class PSMemberInfoCollectionMixin
    {
        class TypeHolder<T> where T : PSMemberInfo
        {
            const string MethodName_Match = "Match";

            public class MethodDelegate_Match_string_PSMemberTypes_MshMemberMatchOptions
            {
                public static readonly Exec Invoke = typeof(PSMemberInfoCollection<T>).GetMethodDelegate(MethodName_Match, new Type[] { typeof(string), typeof(PSMemberTypes), MshMemberMatchOptionsMixin.Type });
            }

            const string MethodName_IsReservedName = "IsReservedName";

            public class MethodDelegate_IsReservedName_string
            {
                public static readonly Exec Invoke = typeof(PSMemberInfoCollection<T>).GetMethodDelegate(MethodName_IsReservedName, new Type[] { typeof(string) });
            }
        }

        public static bool IsReservedName(string name)
        {
            return (bool)TypeHolder<PSMemberInfo>.MethodDelegate_IsReservedName_string.Invoke(null, name) || string.Equals(name, PSObjectProxy.PSObjectProxyMemberSetName, StringComparison.OrdinalIgnoreCase);
        }

        public static ReadOnlyPSMemberInfoCollection<T> Match<T>(this PSMemberInfoCollection<T> source, string name, PSMemberTypes memberTypes, MshMemberMatchOptions matchOptions) where T : PSMemberInfo
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (ReadOnlyPSMemberInfoCollection<T>)TypeHolder<T>.MethodDelegate_Match_string_PSMemberTypes_MshMemberMatchOptions.Invoke(source, name, memberTypes, matchOptions.Target());
        }
    }
}
