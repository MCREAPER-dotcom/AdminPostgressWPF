using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfTESTOVOE.Model;

namespace WpfTESTOVOE.Extension
{
    public class ThreadSafeObservableCollection<T> : ObservableCollection<T>
    {
        private SynchronizationContext SynchronizationContext;

        public ThreadSafeObservableCollection()
        {
            SynchronizationContext = SynchronizationContext.Current;

            // current synchronization context will be null if we're not in UI Thread
            if (SynchronizationContext == null)
                throw new InvalidOperationException("This collection must be instantiated from UI Thread, if not, you have to pass SynchronizationContext to constructor.");
        }

        public ThreadSafeObservableCollection(SynchronizationContext synchronizationContext)
        {
            if (synchronizationContext == null)
                throw new ArgumentNullException("synchronizationContext");

            SynchronizationContext = synchronizationContext;
        }
        protected override void SetItem(int index, T item)
        {
            SynchronizationContext.Send(new SendOrPostCallback((param) => base.SetItem(index, item)), null);
        }
        public void ChangeItem(int index, T item)
        {
            SetItem(index, item);
        }
    }
}
