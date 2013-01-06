using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Urasandesu.Pontine.Mixins.System.Management.Automation;

namespace Test.Urasandesu.Pontine.Mixins.System.Management.Automation
{
    [TestFixture]
    public class ArgumentTypeConverterAttributeMixinTest
    {
        [Test]
        public void NewTest_ShouldReturnInstance()
        {
            // Arrange
            // nop

            // Act
            var actual = ArgumentTypeConverterAttributeMixin.New(typeof(int));

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual("System.Management.Automation.ArgumentTypeConverterAttribute", actual.GetType().FullName);
        }
    }
}
