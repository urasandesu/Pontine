/* 
 * File: LanguagePrimitivesMixin.cs
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
    public static class LanguagePrimitivesMixin
    {
        const string MethodName_ConvertStringToType = "ConvertStringToType";

        class MethodDelegate_ConvertStringToType_string_out_Exception
        {
            public static readonly Exec Invoke = typeof(LanguagePrimitives).GetMethodDelegate(MethodName_ConvertStringToType, new Type[] { typeof(string), typeof(Exception).MakeByRefType() });
        }

        public static Type ConvertStringToType(string typeName, out Exception exception)
        {
            var args = new object[] { typeName, null };
            var result = (Type)MethodDelegate_ConvertStringToType_string_out_Exception.Invoke(null, args);
            exception = (Exception)args[1];
            return result;
        }
    }
}
