/* 
 * File: TypeAcceleratorsProxyTest.cs
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
using Urasandesu.Pontine.Management.Automation;
using Urasandesu.Pontine.Mixins.System.Management.Automation.Runspaces;

namespace Test.Urasandesu.Pontine.Mixins.System.Management.Automation
{
    [TestFixture]
    public class TypeAcceleratorsProxyTest
    {
        [Test]
        public void Get_GetTest_ShouldReturnTypeAccelerators()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;

            // Act
            var accelerators = TypeAcceleratorsProxy.Get_Get();

            // Assert
            CollectionAssert.IsNotEmpty(accelerators);
            Assert.AreEqual(typeof(int), accelerators["int"]);
        }

        
        [Test]
        public void AddTest_ShouldAddNewAccelerator()
        {
            AppDomain.CurrentDomain.RunAtIsolatedDomain(() =>
            {
                // Arrange
                var runspace = RunspaceMixin.DefaultRunspace;

                // Act
                TypeAcceleratorsProxy.Add("PSMemberTypes", typeof(PSMemberTypes));

                // Assert
                var results = default(Collection<PSObject>);
                var command = default(string);
                command =
@"
([PSMemberTypes])
";
                using (var pipeline = runspace.CreatePipeline(command, false))
                    results = pipeline.Invoke();
                Assert.AreEqual(typeof(PSMemberTypes), results[0].BaseObject);
            });
        }


        [Test]
        public void RemoveTest_ShouldRemoveAccelerator()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            TypeAcceleratorsProxy.Add("MshMemberMatchOptions", typeof(MshMemberMatchOptions));

            // Act
            var result = TypeAcceleratorsProxy.Remove("MshMemberMatchOptions");

            // Assert
            Assert.IsTrue(result);
        }
    }
}
