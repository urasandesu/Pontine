/* 
 * File: MshMemberMatchOptionsConverter.cs
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
using System.ComponentModel;
using System.Globalization;
using Urasandesu.Pontine.Mixins.System.Management.Automation;

namespace Urasandesu.Pontine.Management.Automation
{
    public class MshMemberMatchOptionsConverter : EnumConverter
    {
        public MshMemberMatchOptionsConverter(Type type)
            : base(type)
        {
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == MshMemberMatchOptionsMixin.Type)
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == MshMemberMatchOptionsMixin.Type)
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var intValue = value as int?;
            if (intValue.HasValue && 0 <= intValue.Value && intValue.Value <= 3)
            {
                var result = default(int);
                if ((intValue.Value & (int)MshMemberMatchOptions.IncludeHidden) != 0) result |= (int)MshMemberMatchOptions.IncludeHidden;
                if ((intValue.Value & (int)MshMemberMatchOptions.OnlySerializable) != 0) result |= (int)MshMemberMatchOptions.OnlySerializable;
                return (MshMemberMatchOptions)result;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == MshMemberMatchOptionsMixin.Type)
            {
                var enumValue = value as MshMemberMatchOptions?;
                if (enumValue.HasValue)
                {
                    var intValue = (int)enumValue.Value;
                    return TypeDescriptor.GetConverter(destinationType).ConvertFrom(intValue.ToString());
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
