using System.Windows.Controls;
using Autofac;
using Whitebox.Profiler.Features.Session;

namespace Whitebox.Profiler
{
    class ProfilerSession : ViewModel
    {
        string _title;
        readonly ILifetimeScope _scope;
        readonly SessionView _content;

        public ProfilerSession(string title, ILifetimeScope scope, SessionView content)
        {
            _title = title;
            _scope = scope;
            _content = content;
        }

        public SessionView Content
        {
            get { return _content; }
        }

        public ILifetimeScope Scope
        {
            get { return _scope; }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title) return;
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }
    }
}