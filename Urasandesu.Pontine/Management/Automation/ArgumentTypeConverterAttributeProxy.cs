/* 
 * File: ArgumentTypeConverterAttributeProxy.cs
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

namespace Urasandesu.Pontine.Management.Automation
{
    public class ArgumentTypeConverterAttributeProxy : ArgumentTransformationAttribute
    {
        public static readonly Type Type = typeof(ArgumentTransformationAttribute).Assembly.GetType("System.Management.Automation.ArgumentTypeConverterAttribute");

        class ConstructorDelegate_Types
        {
            public static readonly Work New = Type.GetConstructorDelegate(new Type[] { typeof(Type[]) });
        }

        ArgumentTransformationAttribute m_target;

        public ArgumentTypeConverterAttributeProxy(ArgumentTransformationAttribute target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (target.GetType() != Type)
                throw new ArgumentException(string.Format("Value is not a type '{0}'.", Type.FullName), "target");

            m_target = target;
        }

        public ArgumentTypeConverterAttributeProxy(params Type[] types)
        {
            m_target = (ArgumentTransformationAttribute)ConstructorDelegate_Types.New(new object[] { types });
        }

        public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
        {
            return m_target.Transform(engineIntrinsics, inputData);
        }

        const string FieldName__convertTypes = "_convertTypes";

        class FieldGetterDelegate__convertTypes
        {
            public static readonly Exec Get = Type.GetFieldGetterDelegate(FieldName__convertTypes);
        }

        public Type[] ConvertTypes
        {
            get
            {
                return (Type[])FieldGetterDelegate__convertTypes.Get(m_target);
            }
        }

    }
}
