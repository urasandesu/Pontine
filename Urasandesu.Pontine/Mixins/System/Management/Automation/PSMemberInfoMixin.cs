/* 
 * File: PSMemberInfoMixin.cs
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

namespace Urasandesu.Pontine.Mixins.System.Management.Automation
{
    public static class PSMemberInfoMixin
    {
        const string FieldName_isHidden = "isHidden";
        
        class FieldGetterDelegate_isHidden
        {
            public static readonly Exec Get = typeof(PSMemberInfo).GetFieldGetterDelegate(FieldName_isHidden);
        }
        
        public static bool Get_isHidden(this PSMemberInfo source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (bool)FieldGetterDelegate_isHidden.Get(source);
        }

        class FieldSetterDelegate_isHidden
        {
            public static readonly Effect Set = typeof(PSMemberInfo).GetFieldSetterDelegate(FieldName_isHidden);
        }

        public static void Set_isHidden(this PSMemberInfo source, bool value)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            FieldSetterDelegate_isHidden.Set(source, value);
        }

        const string FieldName_instance = "instance";

        class FieldGetterDelegate_instance
        {
            public static readonly Exec Get = typeof(PSMemberInfo).GetFieldGetterDelegate(FieldName_instance);
        }

        public static PSObject Get_instance(this PSMemberInfo source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (PSObject)FieldGetterDelegate_instance.Get(source);
        }

        class FieldSetterDelegate_instance
        {
            public static readonly Effect Set = typeof(PSMemberInfo).GetFieldSetterDelegate(FieldName_instance);
        }

        public static void Set_instance(this PSMemberInfo source, PSObject value)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            FieldSetterDelegate_instance.Set(source, value);
        }

        const string FieldName_isInstance = "isInstance";

        class FieldGetterDelegate_isInstance
        {
            public static readonly Exec Get = typeof(PSMemberInfo).GetFieldGetterDelegate(FieldName_isInstance);
        }

        public static bool Get_isInstance(this PSMemberInfo source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (bool)FieldGetterDelegate_isInstance.Get(source);
        }

        class FieldSetterDelegate_isInstance
        {
            public static readonly Effect Set = typeof(PSMemberInfo).GetFieldSetterDelegate(FieldName_isInstance);
        }

        public static void Set_isInstance(this PSMemberInfo source, bool value)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            FieldSetterDelegate_isInstance.Set(source, value);
        }

        public static PSMemberInfo Clone(this PSMemberInfo source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var variableProperty = default(PSVariableProperty);
            var noteProperty = default(PSNoteProperty);
            if ((variableProperty = source as PSVariableProperty) != null)
            {
                return variableProperty.Clone();
            }
            else if ((noteProperty = source as PSNoteProperty) != null)
            {
                return noteProperty.Clone();
            }
            else
            {
                return source.Copy();
            }
        }
    }
}
