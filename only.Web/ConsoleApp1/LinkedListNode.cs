using System;
using System.Collections.Generic;
using System.Text;

namespace only.Identity.Client
{
    public class LinkedListNode<T>
    {
        public LinkedListNode(T value)
        {
            Value = value;
        }
        public T Value { get;private set; }
        public LinkedListNode<T> Next { get;internal set; }
        public LinkedListNode<T> Prev { get; internal set; }
    }
}
