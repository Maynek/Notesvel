//********************************
// (c) 2021 Ada Maynek
// This software is released under the MIT License.
//********************************
using System;
using System.Collections;
using System.Collections.Generic;

namespace Maynek.Notesvel.Library
{
    public class Table<TKey, TValue> : IEnumerable<TValue>
    {
        //================================
        // Properties
        //================================
        private List<TValue> List { get; } = new List<TValue>();
        private Dictionary<TKey, TValue> Dictionary { get; } = new Dictionary<TKey, TValue>();

        public TValue this[TKey key]
        {
            get { return this.Dictionary[key]; }
            set { this.Dictionary[key] = value; }
        }

        public TValue this[int index]
        {
            get { return this.List[index]; }
            set { this.List[index] = value; }
        }

        public int Count
        {
            get { return this.List.Count;  }      
        }


        //================================
        // Methods
        //================================
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return this.List.GetEnumerator();
        }

        public void Add(TKey key, TValue value)
        {
            this.List.Add(value);
            this.Dictionary.Add(key, value);
        }

        public bool Contains(TKey key)
        {
            return this.Dictionary.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            if (this.Dictionary.ContainsKey(key))
            {
                var item = this.Dictionary[key];
                if (this.List.Remove(item))
                {
                    this.Dictionary.Remove(key);
                    return true;
                }
            }

            return false;
        }

        public void Sort(IComparer<TValue> comparer)
        {
            this.List.Sort(comparer);
        }

        public void Sort(Comparison<TValue> comparison)
        {
            this.List.Sort(comparison);
        }

    }
}
