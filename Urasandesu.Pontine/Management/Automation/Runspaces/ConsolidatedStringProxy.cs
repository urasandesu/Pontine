/* 
 * File: ConsolidatedStringProxy.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;

namespace Urasandesu.Pontine.Management.Automation.Runspaces
{
    public class ConsolidatedStringProxy : IList<string>, ICollection<string>, IEnumerable<string>, IList, ICollection, IEnumerable
    {
        public static readonly Type Type = typeof(TypeTable).Assembly.GetType("System.Management.Automation.Runspaces.ConsolidatedString");

        public ConsolidatedStringProxy(Collection<string> target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (target.GetType() != Type)
                throw new ArgumentException(string.Format("Value is not a type '{0}'.", Type.FullName), "target");

            Target = target;
        }

        public Collection<string> Target { get; private set; }

        public int IndexOf(string item)
        {
            return Target.IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            Target.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Target.RemoveAt(index);
        }

        public string this[int index]
        {
            get
            {
                return Target[index];
            }
            set
            {
                Target[index] = value;
            }
        }

        public void Add(string item)
        {
            Target.Add(item);
        }

        public void Clear()
        {
            Target.Clear();
        }

        public bool Contains(string item)
        {
            return Target.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            Target.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return Target.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((IList)Target).IsReadOnly; }
        }

        public bool Remove(string item)
        {
            return Target.Remove(item);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return Target.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Add(object value)
        {
            return ((IList)Target).Add(value);
        }

        public bool Contains(object value)
        {
            return ((IList)Target).Contains(value);
        }

        public int IndexOf(object value)
        {
            return ((IList)Target).IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            ((IList)Target).Insert(index, value);
        }

        public bool IsFixedSize
        {
            get { return ((IList)Target).IsFixedSize; }
        }

        public void Remove(object value)
        {
            ((IList)Target).Remove(value);
        }

        object IList.this[int index]
        {
            get
            {
                return ((IList)Target)[index];
            }
            set
            {
                ((IList)Target)[index] = value;
            }
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)Target).CopyTo(array, index);
        }

        public bool IsSynchronized
        {
            get { return ((ICollection)Target).IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return ((ICollection)Target).SyncRoot; }
        }
    }
}
