/* 
 * File: PSObjectMixinTest.cs
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


using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using NUnit.Framework;
using Urasandesu.Pontine.Mixins.System.Management.Automation;
using Urasandesu.Pontine.Mixins.System.Management.Automation.Runspaces;

namespace Test.Urasandesu.Pontine.Mixins.System.Management.Automation
{
    [TestFixture]
    public class PSObjectMixinTest
    {
        [Test]
        public void CloneTest_ShouldReturnClone()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var results = default(Collection<PSObject>);
            var command = default(string);
            command =
@"
$a = New-Object psobject
$a | Add-Member NoteProperty Value 10 -PassThru
";
            using (var pipeline = runspace.CreatePipeline(command, false))
                results = pipeline.Invoke();
            var a = results[0];

            // Act
            var b = a.Clone();

            // Assert
            Assert.AreNotSame(a, b);
            Assert.AreSame(a.ImmediateBaseObject, b.ImmediateBaseObject);
            Assert.AreEqual(5, b.Members.Count());
            Assert.AreEqual(a.Members.Count(), b.Members.Count());
            var aValue = a.Members["Value"] as PSNoteProperty;
            var bValue = b.Members["Value"] as PSNoteProperty;
            Assert.AreNotSame(aValue, bValue);
            Assert.AreEqual(aValue.Value, bValue.Value);
        }

        [Test]
        public void CloneTest_ShouldReturnClone_EvenIfHiddenMemberIsContained()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var results = default(Collection<PSObject>);
            var command =
@"
$a = New-Object psobject
$a | Add-Member NoteProperty Value 10 -PassThru
";
            using (var pipeline = runspace.CreatePipeline(command, false))
                results = pipeline.Invoke();
            var a = results[0];
            ((PSNoteProperty)a.Members["Value"]).Set_isHidden(true);

            // Act
            var b = a.Clone();

            // Assert
            Assert.AreNotSame(a, b);
            Assert.AreSame(a.ImmediateBaseObject, b.ImmediateBaseObject);
            Assert.AreEqual(4, b.Members.Count());
            Assert.AreEqual(a.Members.Count(), b.Members.Count());
            var aValue = a.Members["Value"] as PSNoteProperty;
            var bValue = b.Members["Value"] as PSNoteProperty;
            Assert.AreNotSame(aValue, bValue);
            Assert.AreEqual(aValue.Value, bValue.Value);
        }
    }
}
