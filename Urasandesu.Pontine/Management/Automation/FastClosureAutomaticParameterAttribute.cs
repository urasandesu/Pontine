using System;

namespace Urasandesu.Pontine.Management.Automation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class FastClosureAutomaticParameterAttribute : Attribute
    {
    }
}
