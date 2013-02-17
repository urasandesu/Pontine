﻿/* 
 * File: PSObjectProxy.cs
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

namespace Urasandesu.Pontine.Management.Automation
{
    public class PSObjectProxy
    {
        public const string PSObjectProxyMemberSetName = "psobjectex";
        PSObject m_target;
        PSMemberInfoCollectionProxy<PSMemberInfo> m_members;

        public PSObjectProxy(PSObject target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            m_target = target;
        }

        public PSMemberInfoCollectionProxy<PSMemberInfo> Members
        {
            get
            {
                if (m_members == null)
                    m_members = new PSMemberInfoIntegratingCollectionProxy<PSMemberInfo>(m_target.Members);
                return m_members;
            }
        }
    }
}
