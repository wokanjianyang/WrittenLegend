using System;
using System.Collections.Generic;

namespace Game.PathFinc
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private readonly List<T> data;

        public PriorityQueue()
        {
            data = new List<T>();
        }

        public int Count => data.Count;

        public void Enqueue(T item)
        {
            data.Add(item);

            var childindex = data.Count - 1;

            while (childindex > 0)
            {
                var parentindex = (childindex - 1) / 2;

                if (data[childindex].CompareTo(data[parentindex]) >= 0) break;

                var tmp = data[childindex];
                data[childindex]  = data[parentindex];
                data[parentindex] = tmp;

                childindex = parentindex;
            }
        }

        public T Dequeue()
        {
            var lastindex = data.Count - 1;
            var frontItem = data[0];

            data[0] = data[lastindex];
            data.RemoveAt(lastindex);
            lastindex--;

            var parentindex = 0;

            while (true)
            {
                var childindex = parentindex * 2 + 1;

                if (childindex > lastindex) break;

                var rightchild = childindex + 1;

                if (rightchild <= lastindex && data[rightchild].CompareTo(data[childindex]) < 0) childindex = rightchild;

                if (data[parentindex].CompareTo(data[childindex]) <= 0) break;

                var tmp = data[childindex];
                data[childindex]  = data[parentindex];
                data[parentindex] = tmp;

                parentindex = childindex;
            }

            return frontItem;
        }

        public T Peek()
        {
            var frontItem = data[0];
            return frontItem;
        }

        public bool Contains(T item)
        {
            return data.Contains(item);
        }

        public List<T> ToList()
        {
            return data;
        }
    }
}