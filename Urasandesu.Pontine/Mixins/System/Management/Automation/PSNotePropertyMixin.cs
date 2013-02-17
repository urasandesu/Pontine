﻿/* 
 * File: PSNotePropertyMixin.cs
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
using System.Management.Automation;
using Urasandesu.NAnonym.Mixins.System;

namespace Urasandesu.Pontine.Mixins.System.Management.Automation
{
    public static class PSNotePropertyMixin
    {
        public static PSNoteProperty Clone(this PSNoteProperty source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            
            var clone = (PSNoteProperty)source.Copy();
            clone.Set_instance(null);
            var value = default(object);
            var pso = default(PSObject);
            if (source.Value == null)
            {
                value = null;
            }
            else if ((pso = source.Value as PSObject) != null)
            {
                value = pso.Clone();
            }
            else
            {
                value = source.Value.SmartlyClone();
            }
            clone.Value = value;
            return clone;
        }
    }
}
