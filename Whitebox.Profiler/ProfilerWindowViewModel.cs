using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Autofac;
using Whitebox.Core;
using Whitebox.Profiler.Features.Session;
using Whitebox.Profiler.Util;

namespace Whitebox.Profiler
{
    class ProfilerWindowViewModel : ViewModel
    {
        readonly ObservableCollection<ProfilerSession> _activeSessions = new ObservableCollection<ProfilerSession>();
        readonly ILifetimeScope _componentContext;
        int _selectedSessionIndex;
        const string WaitingTitle = "Waiting...";

        public ProfilerWindowViewModel(ILifetimeScope componentContext)
        {
            if (componentContext == null) throw new ArgumentNullException("componentContext");
            _componentContext = componentContext;
            CloseSession = new RelayCommand<ProfilerSession>(OnCloseSession, CanCloseSession);
            StartSession();
            SelectedSessionIndex = 0;
        }

        static bool CanCloseSession(ProfilerSession profilerSession)
        {
            return profilerSession.Title != WaitingTitle;
        }

        void OnCloseSession(ProfilerSession profilerSession)
        {
            _activeSessions.Remove(profilerSession);
            if (_selectedSessionIndex >= _activeSessions.Count)
                SelectedSessionIndex = _activeSessions.Count - 1;
            profilerSession.Content.Close();
            profilerSession.Scope.Dispose();
        }

        public ObservableCollection<ProfilerSession> ActiveSessions { get { return _activeSessions; } }

        public int SelectedSessionIndex
        {
            get { return _selectedSessionIndex; }
            set
            {
                if (value == _selectedSessionIndex) return;
                _selectedSessionIndex = value;
                NotifyPropertyChanged("SelectedSessionIndex");
            }
        }

        void StartSession()
        {
            var sessionScope = _componentContext.BeginLifetimeScope(Constants.ProfilerSessionScopeTag);
            var sessionView = sessionScope.Resolve<SessionView>();
            var sessionViewModel = (SessionViewModel) sessionView.DataContext;

            var session = new ProfilerSession(WaitingTitle, sessionScope, sessionView);
            _activeSessions.Add(session);
            var tabIndex = _activeSessions.Count - 1;

            sessionViewModel.Connected += (s, args) =>
            {
                session.Title = Path.GetFileName(args.ProcessName);
                SelectedSessionIndex = tabIndex;
                StartSession();
            };

            sessionViewModel.Start();
        }

        public ICommand CloseSession { get; private set; }
    }
}
