using System;
using System.Windows.Controls;
using Autofac.Util;

namespace Whitebox.Profiler.Navigation
{
    class NavigationEntry : Disposable
    {
        readonly Control _content;
        readonly IDisposable _releaseHandle;
        readonly string _title;

        public NavigationEntry(Control content, IDisposable releaseHandle, string title)
        {
            if (content == null) throw new ArgumentNullException("content");
            if (releaseHandle == null) throw new ArgumentNullException("releaseHandle");
            if (title == null) throw new ArgumentNullException("title");
            _content = content;
            _releaseHandle = releaseHandle;
            _title = title;
        }

        public string Title
        {
            get { return _title; }
        }

        public Control Content
        {
            get { return _content; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _releaseHandle.Dispose();

            base.Dispose(disposing);
        }
    }
}
