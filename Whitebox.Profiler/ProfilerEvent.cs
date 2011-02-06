using System;

namespace Whitebox.Profiler
{
    public class ProfilerEvent
    {
        readonly string _description;
        readonly object _model;

        public ProfilerEvent(string description, object model)
        {
            if (description == null) throw new ArgumentNullException("description");
            if (model == null) throw new ArgumentNullException("model");
            _description = description;
            _model = model;
        }

        public object Model
        {
            get { return _model; }
        }

        public string Description
        {
            get { return _description; }
        }
    }
}
