/* 
 * File: PSVariableMixinTest.cs
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
using System.Management.Automation;
using NUnit.Framework;
using Urasandesu.NAnonym.Mixins.System;
using Urasandesu.Pontine.Mixins.System.Management.Automation;
using Urasandesu.Pontine.Mixins.System.Management.Automation.Runspaces;

namespace Test.Urasandesu.Pontine.Mixins.System.Management.Automation
{
    [TestFixture]
    public class PSVariableMixinTest
    {
        [Test]
        public void NewTest_ShouldReturnInstance()
        {
            // Arrange
            // nop

            // Act
            var actual = PSVariableMixin.New("m_value", 42, ScopedItemOptions.Private, typeof(int));

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual("System.Management.Automation.PSVariable", actual.GetType().FullName);
        }


        [Test]
        public void NewTest_ShouldReturnDefaultVariable()
        {
            // Arrange
            var name = string.Format("__variable_{0}", Guid.NewGuid().ToString().Split('-')[1]);
            var value = typeof(string).Default();
            var options = ScopedItemOptions.None;

            // Act
            var actual = PSVariableMixin.New(name, value, options, typeof(string));

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(string.Empty, actual.Value);
        }


        [Test]
        public void GetVariableType_ShouldReturnConvertibleType()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var results = default(Collection<PSObject>);
            var command = 
@"
Remove-Variable m_value -ErrorAction SilentlyContinue
[int]$m_value = 42
Get-Variable m_value -ErrorAction SilentlyContinue
";
            using (var pipeline = runspace.CreatePipeline(command, false))
                results = pipeline.Invoke();
            var target = (PSVariable)results[0].BaseObject;

            // Act
            var actual = target.GetVariableType();

            // Assert
            Assert.AreEqual(typeof(int), actual);
        }


        [Test]
        public void GetVariableType_ShouldReturnTypeOfObject_IfTypeIsNotDisignated()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var results = default(Collection<PSObject>);
            var command = 
@"
Remove-Variable m_value -ErrorAction SilentlyContinue
$m_value = 42
Get-Variable m_value -ErrorAction SilentlyContinue
";
            using (var pipeline = runspace.CreatePipeline(command, false))
                results = pipeline.Invoke();
            var target = (PSVariable)results[0].BaseObject;

            // Act
            var actual = target.GetVariableType();

            // Assert
            Assert.AreEqual(typeof(object), actual);
        }
    }
}
