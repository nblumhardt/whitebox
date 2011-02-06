using System;
using System.Windows.Input;
using Whitebox.Core.Analytics;
using Whitebox.Profiler.Util;

namespace Whitebox.Profiler.Features.Events
{
    class MessageEventViewModel : EventViewModel
    {
        readonly MessageRelevance _relevance;
        readonly string _title;
        readonly string _message;
        readonly ICommand _showEvent;

        public MessageEventViewModel(MessageRelevance relevance, string title, string message)
        {
            if (title == null) throw new ArgumentNullException("title");
            if (message == null) throw new ArgumentNullException("message");
            _relevance = relevance;
            _title = title;
            _message = message;
            _showEvent = new RelayCommand(() => { });
        }

        public MessageRelevance Relevance
        {
            get { return _relevance; }
        }

        public string Title
        {
            get { return _title; }
        }

        public string Message
        {
            get { return _message; }
        }

        public override ICommand ShowEvent
        {
            get { return _showEvent; }
        }

        public override string Icon
        {
            get
            {
                switch (Relevance)
                {
                    case MessageRelevance.Error:
                        return @"..\..\Resources\Error-24.png";
                    case MessageRelevance.Warning:
                        return @"..\..\Resources\Warning-24.png";
                    default:
                        return @"..\..\Resources\Info-24.png";
                }
            }
        }
    }
}
