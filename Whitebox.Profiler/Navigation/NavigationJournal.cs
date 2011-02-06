using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Autofac.Util;

namespace Whitebox.Profiler.Navigation
{
    class NavigationJournal : Disposable, INotifyPropertyChanged
    {
        readonly Stack<NavigationEntry> _backStack = new Stack<NavigationEntry>();
        readonly Stack<NavigationEntry> _forwardStack = new Stack<NavigationEntry>();
        NavigationEntry _current;

        public NavigationEntry Current
        {
            get { return _current; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (ReferenceEquals(value, _current)) return;

                if (Current != null && ReferenceEquals(Current.Content, value.Content))
                {
                    value.Dispose();
                    return;
                }

                ClearForwardStack();

                if (_current != null)
                    _backStack.Push(_current);

                _current = value;

                NotifyNavigationPropertiesChanged();
            }
        }

        void ClearForwardStack()
        {
            foreach (var forwardEntry in _forwardStack)
                forwardEntry.Dispose();
            _forwardStack.Clear();
        }

        void ClearBackStack()
        {
            foreach (var navigationEntry in BackHistory)
                navigationEntry.Dispose();
            _backStack.Clear();            
        }

        public bool CanGoBack
        {
            get { return _backStack.Any(); }
        }

        public bool CanGoForward
        {
            get { return _forwardStack.Any(); }
        }

        public void GoBack()
        {
            if (!CanGoBack)
                throw new InvalidOperationException("Can't go back.");

            _forwardStack.Push(_current);
            _current = _backStack.Pop();

            NotifyNavigationPropertiesChanged();
        }

        public void GoForward()
        {
            if (!CanGoForward)
                throw new InvalidOperationException("Can't go forward.");

            _backStack.Push(_current);
            _current = _forwardStack.Pop();

            NotifyNavigationPropertiesChanged();
        }

        void NotifyNavigationPropertiesChanged()
        {
            NotifyPropertyChanged("Current");
            NotifyPropertyChanged("CanGoBack");
            NotifyPropertyChanged("CanGoForward");
            NotifyPropertyChanged("BackHistory");
            NotifyPropertyChanged("ForwardHistory");
        }

        void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<NavigationEntry> BackHistory { get { return _backStack; } }

        public IEnumerable<NavigationEntry> ForwardHistory { get { return _forwardStack; } }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearHistory();
                if (Current != null)
                    Current.Dispose();
            }

            base.Dispose(disposing);
        }

        public void ClearHistory()
        {
            ClearForwardStack();
            ClearBackStack();

            NotifyPropertyChanged("CanGoBack");
            NotifyPropertyChanged("CanGoForward");
            NotifyPropertyChanged("BackHistory");
            NotifyPropertyChanged("ForwardHistory");
        }
    }
}
