/* 
 * File: PSObjectMixin.cs
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.Pontine.Management.Automation;
using Urasandesu.Pontine.Management.Automation.Runspaces;

namespace Urasandesu.Pontine.Mixins.System.Management.Automation
{
    public static class PSObjectMixin
    {
        const string MethodName_Refresh = "Refresh";

        class MethodDelegate_Refresh_object
        {
            public static readonly Exec Invoke = typeof(PSObject).GetMethodDelegate(MethodName_Refresh, new Type[] { typeof(object) });
        }

        public static void Refresh(this PSObject source, object obj)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            MethodDelegate_Refresh_object.Invoke(source, obj);
        }

        const string PropertyName_InstanceMembers = "InstanceMembers";

        class PropertyGetterDelegate_InstanceMembers
        {
            public static readonly Exec Get = typeof(PSObject).GetPropertyGetterDelegate(PropertyName_InstanceMembers);
        }

        public static PSMemberInfoInternalCollectionProxy<PSMemberInfo> Get_InstanceMembers(this PSObject source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new PSMemberInfoInternalCollectionProxy<PSMemberInfo>((PSMemberInfoCollection<PSMemberInfo>)PropertyGetterDelegate_InstanceMembers.Get(source));
        }

        class PropertySetterDelegate_InstanceMembers
        {
            public static readonly Effect Set = typeof(PSObject).GetPropertySetterDelegate(PropertyName_InstanceMembers);
        }

        public static void Set_InstanceMembers(this PSObject source, PSMemberInfoInternalCollectionProxy<PSMemberInfo> value)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            PropertySetterDelegate_InstanceMembers.Set(source, value.Target);
        }

        const string FieldName_hasGeneratedReservedMembers = "hasGeneratedReservedMembers";

        class FieldGetterDelegate_hasGeneratedReservedMembers
        {
            public static readonly Exec Get = typeof(PSObject).GetFieldGetterDelegate(FieldName_hasGeneratedReservedMembers);
        }

        public static bool Get_hasGeneratedReservedMembers(this PSObject source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (bool)FieldGetterDelegate_hasGeneratedReservedMembers.Get(source);
        }

        class FieldSetterDelegate_hasGeneratedReservedMembers
        {
            public static readonly Effect Set = typeof(PSObject).GetFieldSetterDelegate(FieldName_hasGeneratedReservedMembers);
        }

        public static void Set_hasGeneratedReservedMembers(this PSObject source, bool value)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            FieldSetterDelegate_hasGeneratedReservedMembers.Set(source, value);
        }

        const string PropertyName_InternalTypeNames = "InternalTypeNames";

        class PropertyGetterDelegate_InternalTypeNames
        {
            public static readonly Exec Get = typeof(PSObject).GetPropertyGetterDelegate(PropertyName_InternalTypeNames);
        }

        public static ConsolidatedStringProxy Get_InternalTypeNames(this PSObject source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new ConsolidatedStringProxy((Collection<string>)PropertyGetterDelegate_InternalTypeNames.Get(source));
        }

        class PropertySetterDelegate_InternalTypeNames
        {
            public static readonly Effect Set = typeof(PSObject).GetPropertySetterDelegate(PropertyName_InternalTypeNames);
        }

        public static void Set_InternalTypeNames(this PSObject source, ConsolidatedStringProxy value)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            PropertySetterDelegate_InternalTypeNames.Set(source, value.Target);
        }

        const string FieldName_immediateBaseObject = "immediateBaseObject";

        class FieldGetterDelegate_immediateBaseObject
        {
            public static readonly Exec Get = typeof(PSObject).GetFieldGetterDelegate(FieldName_immediateBaseObject);
        }

        public static object Get_immediateBaseObject(this PSObject source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return FieldGetterDelegate_immediateBaseObject.Get(source);
        }

        class FieldSetterDelegate_immediateBaseObject
        {
            public static readonly Effect Set = typeof(PSObject).GetFieldSetterDelegate(FieldName_immediateBaseObject);
        }

        public static void Set_immediateBaseObject(this PSObject source, object value)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            FieldSetterDelegate_immediateBaseObject.Set(source, value);
        }

        public static PSObject Clone(this PSObject source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var clone = (PSObject)source.MemberwiseClone();
            
            clone.Refresh(source.ImmediateBaseObject);

            foreach (var member in source.Get_InstanceMembers().Where(_ => clone.Members[_.Name] == null))
                clone.Members.Add(member.Clone());
            
            clone.Set_hasGeneratedReservedMembers(false);
            
            clone.TypeNames.Clear();
            foreach (var typeName in source.Get_InternalTypeNames())
                clone.TypeNames.Add(typeName);
            
            clone.Set_immediateBaseObject(source.ImmediateBaseObject.SmartlyClone());
            
            return clone;
        }
    }
}
