using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;

namespace Whitebox.Containers.Autofac.Tests
{
    class ModuleTestHarness
    {
        readonly Queue<object> _messages;
        readonly IContainer _container;
        readonly WhiteboxProfilingModule _module;
        readonly ModelMapper _modelMapper;
        readonly ILifetimeScope _rootScope;
        static readonly FieldInfo RootScopeField = typeof(Container).GetField("_rootLifetimeScope", BindingFlags.NonPublic | BindingFlags.Instance);

        public ModuleTestHarness() : this (cb => {}) { }

        public ModuleTestHarness(Action<ContainerBuilder> configurationAction)
        {
            var queue = new TestQueue();
            _module = new WhiteboxProfilingModule(queue);
            _module.ModelMapper.IdTracker.RememberIds();
            _modelMapper = _module.ModelMapper;
            var builder = new ContainerBuilder();
            builder.RegisterModule(_module);
            configurationAction(builder);
            _container = builder.Build();
            _rootScope = (ILifetimeScope) RootScopeField.GetValue(_container);
            _messages = queue.Messages;
        }

        public ModelMapper ModelMapper
        {
            get { return _modelMapper; }
        }

        public IContainer Container
        {
            get { return _container; }
        }

        public void ClearMessages()
        {
            _messages.Clear();
        }

        public TMessage GetSingleMessageOrDefault<TMessage>()
        {
            return _messages.OfType<TMessage>().SingleOrDefault();
        }

        public void GetSingleMessageOrDefault<TMessage>(out TMessage message)
        {
            message = _messages.OfType<TMessage>().SingleOrDefault();
        }

        public string GetLifetimeScopeId(ILifetimeScope lifetimeScope)
        {
            var scope = lifetimeScope;
            if (lifetimeScope is IContainer)
                scope = _rootScope;
            return _module.ModelMapper.IdTracker.GetIdOrFail(scope);
        }

        public string RootScopeId
        {
            get { return GetLifetimeScopeId(_rootScope); }
        }
    }
}