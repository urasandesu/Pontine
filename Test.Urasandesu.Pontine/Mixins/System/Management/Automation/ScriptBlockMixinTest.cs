/* 
 * File: ScriptBlockMixinTest.cs
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


using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using NUnit.Framework;
using Urasandesu.Pontine.Mixins.System.Management.Automation;
using Urasandesu.Pontine.Mixins.System.Management.Automation.Runspaces;

namespace Test.Urasandesu.Pontine.Mixins.System.Management.Automation
{
    [TestFixture]
    public class ScriptBlockMixinTest
    {
        [Test]
        public void GetSessionStateTest_ShouldReturnSetValue()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var sessionState = new SessionState();
            var scriptBlock = ScriptBlock.Create(@"$PSVersionTable");
            scriptBlock.Set_SessionState(sessionState);

            // Act
            var actual = scriptBlock.Get_SessionState();

            // Assert
            Assert.AreSame(sessionState, actual);
        }


        [Test]
        public void CloneTest_ShouldReturnClonedObject()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var scriptBlock = ScriptBlock.Create(@"$PSVersionTable");

            // Act
            var actual = scriptBlock.Clone(true);

            // Assert
            Assert.AreEqual(@"$PSVersionTable", actual.ToString());
            Assert.AreNotSame(scriptBlock, actual);
        }


        [Test]
        public void Get__parameterDeclarationTest_ShouldReturnNull_IfParameterDoesNotExist()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var scriptBlock = ScriptBlock.Create(@"$null");

            // Act
            var decl = scriptBlock.Get__parameterDeclaration();

            // Assert
            Assert.IsNull(decl);
        }


        [Test]
        public void Get__parameterDeclarationTest_ShouldReturnParameter_IfParameterExists()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var scriptBlock = ScriptBlock.Create(@"param ($value) $null");

            // Act
            var decl = scriptBlock.Get__parameterDeclaration();

            // Assert
            Assert.IsNotNull(decl);
        }


        [Test]
        public void Get_RuntimeDefinedParameterListTest_ShouldReturnEmpty_IfParameterDoesNotExist()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var scriptBlock = ScriptBlock.Create(@"$null");

            // Act
            var paramList = scriptBlock.Get_RuntimeDefinedParameterList();

            // Assert
            CollectionAssert.IsEmpty(paramList);
        }


        [Test]
        public void Get_RuntimeDefinedParameterListTest_ShouldReturnParameterList_IfParameterExists()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var scriptBlock = ScriptBlock.Create(@"param ($value) $null");

            // Act
            var paramList = scriptBlock.Get_RuntimeDefinedParameterList();

            // Assert
            CollectionAssert.IsNotEmpty(paramList);
            Assert.AreEqual(1, paramList.Count);
            Assert.AreEqual("value", paramList[0].Name);
            Assert.IsFalse(paramList[0].IsSet);
        }


        [Test]
        public void Get_RuntimeDefinedParametersTest_ShouldReturnEmpty_IfParameterDoesNotExist()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var scriptBlock = ScriptBlock.Create(@"$null");

            // Act
            var @params = scriptBlock.Get_RuntimeDefinedParameters();

            // Assert
            CollectionAssert.IsEmpty(@params);
        }


        [Test]
        public void Get_RuntimeDefinedParametersTest_ShouldReturnParameters_IfParameterExists()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var scriptBlock = ScriptBlock.Create(@"param ($value) $null");

            // Act
            var @params = scriptBlock.Get_RuntimeDefinedParameters();

            // Assert
            CollectionAssert.IsNotEmpty(@params);
            Assert.AreEqual(1, @params.Count);
            Assert.IsTrue(@params.ContainsKey("value"));
            Assert.AreEqual("value", @params["value"].Name);
            Assert.IsFalse(@params["value"].IsSet);
        }


        [Test]
        public void Get__runtimeDefinedParameterListTest_ShouldReturnEmpty_IfParameterDoesNotExist()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var scriptBlock = ScriptBlock.Create(@"$null");

            // Act
            var paramList1 = scriptBlock.Get__runtimeDefinedParameterList();
            scriptBlock.Get_RuntimeDefinedParameterList();
            var paramList2 = scriptBlock.Get__runtimeDefinedParameterList();

            // Assert
            Assert.IsNull(paramList1);
            CollectionAssert.IsEmpty(paramList2);
        }


        [Test]
        public void Get__runtimeDefinedParameterListTest_ShouldReturnParameterList_IfParameterExists()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var scriptBlock = ScriptBlock.Create(@"param ($value) $null");

            // Act
            var paramList1 = scriptBlock.Get__runtimeDefinedParameterList();
            scriptBlock.Get_RuntimeDefinedParameterList();
            var paramList2 = scriptBlock.Get__runtimeDefinedParameterList();

            // Assert
            Assert.IsNull(paramList1);
            CollectionAssert.IsNotEmpty(paramList2);
            Assert.AreEqual(1, paramList2.Count);
            Assert.AreEqual("value", paramList2[0].Name);
            Assert.IsFalse(paramList2[0].IsSet);
        }


        [Test]
        public void Set__runtimeDefinedParameterListTest_ShouldOverwrite_IfNotInitialized()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var scriptBlock = ScriptBlock.Create(@"param ($value) $null");
            var paramList = new List<RuntimeDefinedParameter>();
            var param = new RuntimeDefinedParameter();
            param.Name = "arg";
            paramList.Add(param);

            // Act
            scriptBlock.Set__runtimeDefinedParameterList(paramList);
            var actual = scriptBlock.Get_RuntimeDefinedParameterList();

            // Assert
            CollectionAssert.IsNotEmpty(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("value", actual[0].Name);
            Assert.IsFalse(actual[0].IsSet);
        }


        [Test]
        public void Set__runtimeDefinedParameterListTest_ShouldNotOverwrite_IfInitialized()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var scriptBlock = ScriptBlock.Create(@"param ($value) $null");
            var paramList = new List<RuntimeDefinedParameter>();
            var param = new RuntimeDefinedParameter();
            param.Name = "arg";
            paramList.Add(param);

            // Act
            var actual = scriptBlock.Get_RuntimeDefinedParameterList();
            scriptBlock.Set__runtimeDefinedParameterList(paramList);
            actual = scriptBlock.Get_RuntimeDefinedParameterList();

            // Assert
            CollectionAssert.IsNotEmpty(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("arg", actual[0].Name);
            Assert.IsFalse(actual[0].IsSet);
        }


        [Test]
        public void Get__runtimeDefinedParametersTest_ShouldReturnEmpty_IfParameterDoesNotExist()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var scriptBlock = ScriptBlock.Create(@"$null");

            // Act
            var paramList1 = scriptBlock.Get__runtimeDefinedParameters();
            scriptBlock.Get_RuntimeDefinedParameters();
            var paramList2 = scriptBlock.Get__runtimeDefinedParameters();

            // Assert
            Assert.IsNull(paramList1);
            CollectionAssert.IsEmpty(paramList2);
        }


        [Test]
        public void Get__runtimeDefinedParametersTest_ShouldReturnParameterList_IfParameterExists()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var scriptBlock = ScriptBlock.Create(@"param ($value) $null");

            // Act
            var paramList1 = scriptBlock.Get__runtimeDefinedParameters();
            scriptBlock.Get_RuntimeDefinedParameters();
            var paramList2 = scriptBlock.Get__runtimeDefinedParameters();

            // Assert
            Assert.IsNull(paramList1);
            CollectionAssert.IsNotEmpty(paramList2);
            Assert.AreEqual(1, paramList2.Count);
            Assert.AreEqual("value", paramList2["value"].Name);
            Assert.IsFalse(paramList2["value"].IsSet);
        }


        [Test]
        public void Set__runtimeDefinedParametersTest_ShouldOverwrite_IfNotInitialized()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var scriptBlock = ScriptBlock.Create(@"param ($value) $null");
            var @params = new RuntimeDefinedParameterDictionary();
            var param = new RuntimeDefinedParameter();
            param.Name = "arg";
            @params.Add("arg", param);

            // Act
            scriptBlock.Set__runtimeDefinedParameters(@params);
            var actual = scriptBlock.Get_RuntimeDefinedParameters();

            // Assert
            CollectionAssert.IsNotEmpty(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("value", actual["value"].Name);
            Assert.IsFalse(actual["value"].IsSet);
        }


        [Test]
        public void Set__runtimeDefinedParametersTest_ShouldNotOverwrite_IfInitialized()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var scriptBlock = ScriptBlock.Create(@"param ($value) $null");
            var @params = new RuntimeDefinedParameterDictionary();
            var param = new RuntimeDefinedParameter();
            param.Name = "arg";
            @params.Add("arg", param);

            // Act
            var actual = scriptBlock.Get_RuntimeDefinedParameters();
            scriptBlock.Set__runtimeDefinedParameters(@params);
            actual = scriptBlock.Get_RuntimeDefinedParameters();

            // Assert
            CollectionAssert.IsNotEmpty(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("arg", actual["arg"].Name);
            Assert.IsFalse(actual["arg"].IsSet);
        }


        [Test]
        public void GetNewFastClosureTest_ShouldReturnClosure()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var results = default(Collection<PSObject>);
            var command =
@"
function Hoge {
    param ($value)
    [Urasandesu.Pontine.Mixins.System.Management.Automation.ScriptBlockMixin]::GetNewFastClosure({ $value })
}

$command = Hoge (10)
& $command
";

            // Act
            using (var pipeline = runspace.CreatePipeline(command, false))
                results = pipeline.Invoke();

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(10, results[0].BaseObject);
        }
    }
}
