/* 
 * File: TypeAcceleratorsProxy.cs
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
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Mixins.System;

namespace Urasandesu.Pontine.Management.Automation
{
    public static class TypeAcceleratorsProxy
    {
        public static readonly Type Type = typeof(PSObject).Assembly.GetType("System.Management.Automation.TypeAccelerators");

        const string PropertyName_Get = "Get";

        class PropertyGetterDelegate_Get
        {
            public static readonly Exec Get = Type.GetPropertyGetterDelegate(PropertyName_Get);
        }

        public static Dictionary<string, Type> Get_Get()
        {
            return (Dictionary<string, Type>)PropertyGetterDelegate_Get.Get(null);
        }

        const string MethodName_Add = "Add";

        class MethodDelegate_Add_string_Type
        {
            public static readonly Exec Invoke = Type.GetMethodDelegate(MethodName_Add, new Type[] { typeof(string), typeof(Type) });
        }

        public static void Add(string typeName, Type type)
        {
            MethodDelegate_Add_string_Type.Invoke(null, typeName, type);
        }

        const string MethodName_Remove = "Remove";

        class MethodDelegate_Remove_string
        {
            public static readonly Exec Invoke = Type.GetMethodDelegate(MethodName_Remove, new Type[] { typeof(string) });
        }

        public static bool Remove(string typeName)
        {
            return (bool)MethodDelegate_Remove_string.Invoke(null, typeName);
        }
    }
}
