﻿/* 
 * File: RunspaceMixinTest.cs
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
using System.Management.Automation;
using NUnit.Framework;
using Urasandesu.Pontine.Mixins.System.Management.Automation.Runspaces;

namespace Test.Urasandesu.Pontine.Mixins.System.Management.Automation.Runspaces
{
    [TestFixture]
    public class RunspaceMixinTest
    {
        [Test]
        public void CopyVariablesToTest_CanAccessToVariablesInAnotherRunspace()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var results = default(Collection<PSObject>);
            var command =
@"
$value = 42
$runspace = [runspaceFactory]::CreateRunspace()
$runspace.Open()
[Urasandesu.Pontine.Mixins.System.Management.Automation.Runspaces.RunspaceMixin]::CopyVariablesTo(([runspace]::DefaultRunspace), $runspace, 0)
$pipeline = $runspace.CreatePipeline({ $value })
$pipeline.Invoke()
$runspace.Dispose()
";

            // Act
            using (var pipeline = runspace.CreatePipeline(command, false))
                results = pipeline.Invoke();

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(42, results[0].BaseObject);
        }
    }
}
