/* 
 * File: PSMemberInfoCollectionMixinTest.cs
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


using System.Linq;
using System.Management.Automation;
using NUnit.Framework;
using Urasandesu.Pontine.Management.Automation;
using Urasandesu.Pontine.Mixins.System.Management.Automation;
using Urasandesu.Pontine.Mixins.System.Management.Automation.Runspaces;

namespace Test.Urasandesu.Pontine.Mixins.System.Management.Automation
{
    [TestFixture]
    public class PSMemberInfoCollectionMixinTest
    {
        [Test]
        public void MatchTest_ShouldReturnMatchedValues()
        {
            // Arrange
            var runspace = RunspaceMixin.DefaultRunspace;
            var a = new PSObject();
            var m_value = new PSNoteProperty("m_value", 42);
            m_value.Set_isHidden(true);
            a.Members.Add(m_value);

            // Act
            var result = a.Members.Match("*", PSMemberTypes.All, MshMemberMatchOptions.IncludeHidden);

            // Assert
            Assert.AreEqual(10, result.Count);
            Assert.IsTrue(result.Select(_ => _.Name).Contains("m_value"));
        }
    }
}
