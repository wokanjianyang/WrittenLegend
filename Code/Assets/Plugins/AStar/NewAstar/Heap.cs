using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.NewAstar
{
    public class Heap<T> where T : Node
    {
        private readonly Comparison<T> cmp;

        private readonly Comparison<T> defaultCmp = delegate(T nodeA, T nodeB)
        {
            return nodeA.f - nodeB.f;
        };

        private readonly List<T> nodes;

        public Heap(Comparison<T> cmp)
        {
            this.cmp = cmp != null ? cmp : defaultCmp;
            nodes    = new List<T>();
        }


        public T Push(T x)
        {
            return Heappush(nodes, x, cmp);
        }


        public T Heappush(List<T> array, T item, Comparison<T> cmp)
        {
            if (cmp == null) cmp = defaultCmp;

            array.Add(item);
            return Siftdown(array, 0, array.Count - 1, cmp);
        }

        public T Siftdown(List<T> array, int startpos, int pos, Comparison<T> cmp)
        {
            T   newitem;
            T   parent;
            int parentpos;
            if (cmp == null) cmp = defaultCmp;

            newitem = array[pos];
            while (pos > startpos)
            {
                parentpos = (pos - 1) >> 1;
                parent    = array[parentpos];
                if (cmp(newitem, parent) < 0)
                {
                    array[pos] = parent;
                    pos        = parentpos;
                    continue;
                }

                break;
            }

            return array[pos] = newitem;
        }

        public T Pop()
        {
            return Heappop(nodes, cmp);
        }

        public T Heappop(List<T> array, Comparison<T> cmp)
        {
            T lastelt;
            T returnitem;
            if (cmp == null) cmp = defaultCmp;

            lastelt = array.Last();
            array.Remove(lastelt);
            if (array.Count > 0)
            {
                returnitem = array[0];
                array[0]   = lastelt;
                Siftup(array, 0, cmp);
            }
            else
            {
                returnitem = lastelt;
            }

            return returnitem;
        }

        public T Siftup(List<T> array, int pos, Comparison<T> cmp)
        {
            int childpos;
            int endpos;
            T   newitem;
            int rightpos;
            int startpos;
            if (cmp == null) cmp = defaultCmp;

            endpos   = array.Count;
            startpos = pos;
            newitem  = array[pos];
            childpos = 2 * pos + 1;
            while (childpos < endpos)
            {
                rightpos = childpos + 1;
                if (rightpos < endpos && !(cmp(array[childpos], array[rightpos]) < 0)) childpos = rightpos;

                array[pos] = array[childpos];
                pos        = childpos;
                childpos   = 2 * pos + 1;
            }

            array[pos] = newitem;
            return Siftdown(array, startpos, pos, cmp);
        }


        public bool Empty()
        {
            return nodes.Count == 0;
        }


        public T UpdateItem(List<T> array, T item, Comparison<T> cmp)
        {
            int pos;
            if (cmp == null) cmp = defaultCmp;

            pos = array.IndexOf(item);
            if (pos == -1) return null;

            Siftdown(array, 0, pos, cmp);
            return Siftup(array, pos, cmp);
        }


        public T UpdateItem(T x)
        {
            return UpdateItem(nodes, x, cmp);
        }
    }
}