using System;

namespace Whitebox.Profiler.Features.Session
{
    public partial class SessionView
    {
        public SessionView()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                SlideTransition.SlideDirection = ViewModel.SlideDirection;
                ViewModel.PropertyChanged += (s1, e1) =>
                {
                    if (e1.PropertyName == "SlideDirection")
                        SlideTransition.SlideDirection = ViewModel.SlideDirection;
                };
            };
        }

        public void Close()
        {
            ViewModel.Close();
        }

        SessionViewModel ViewModel
        {
            get { return (SessionViewModel) DataContext; }
        }
    }
}
