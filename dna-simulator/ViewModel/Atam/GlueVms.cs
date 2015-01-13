using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace dna_simulator.ViewModel.Atam
{
    /* This class is just here to give DataTemplates in XAML a unique type to bind to.
     * Maybe there's a better way? */
    public class GlueVms : ViewModelBase, ICollection<GlueVm>, INotifyCollectionChanged
    {
        private readonly ObservableCollection<GlueVm> _glues = new ObservableCollection<GlueVm>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public int Count { get { return _glues.Count; } }

        public bool IsReadOnly { get { return false; } }

        public IEnumerator<GlueVm> GetEnumerator()
        {
            return _glues.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(GlueVm item)
        {
            _glues.Add(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, _glues.IndexOf(item)));
        }

        public void Clear()
        {
            _glues.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(GlueVm item)
        {
            return _glues.Contains(item);
        }

        public void CopyTo(GlueVm[] array, int arrayIndex)
        {
            _glues.CopyTo(array, arrayIndex);
        }

        public bool Remove(GlueVm item)
        {
            var index = _glues.IndexOf(item);
            var result = _glues.Remove(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
            return result;
        }

        public void OnCollectionChanged(NotifyCollectionChangedEventArgs eventArgs)
        {
            var handler = CollectionChanged;
            if (handler == null) return;
            handler(this, eventArgs);
        }
    }
}