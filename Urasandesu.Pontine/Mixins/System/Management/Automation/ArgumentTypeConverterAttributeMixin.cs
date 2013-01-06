using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using Urasandesu.NAnonym.Mixins.System;

namespace Urasandesu.Pontine.Mixins.System.Management.Automation
{
    public static class ArgumentTypeConverterAttributeMixin
    {
        static Type ms_thisType = typeof(ArgumentTransformationAttribute).Assembly.GetType("System.Management.Automation.ArgumentTypeConverterAttribute");
        class Ctor_Types_Holder
        {
            public static Delegate ms_ctor = ms_thisType.GetConstructorDelegate(new Type[] { typeof(Type[]) });
        }
        public static ArgumentTransformationAttribute New(params Type[] types)
        {
            return (ArgumentTransformationAttribute)Ctor_Types_Holder.ms_ctor.DynamicInvoke(new object[] { types });
        }
    }
}
