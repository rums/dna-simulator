using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace dna_simulator.Model
{
    public class ObservableSet<T> : ObservableCollection<T>
    {
        public ObservableSet()
        {
        }

        public ObservableSet(IEnumerable<T> items)
            : base(items)
        {
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var i in items)
                Add(i);
        }

        protected override void InsertItem(int index, T item)
        {
            if (!Contains(item))
                base.InsertItem(index, item);
        }

        protected override void SetItem(int index, T item)
        {
            int i = IndexOf(item);
            if (i >= 0 && i != index) throw new ArgumentException("Duplicate item");

            base.SetItem(index, item);
        }
    }
}