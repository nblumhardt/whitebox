using System;
using System.Diagnostics;
using Autofac;
using Autofac.Core;
using Autofac.Core.Diagnostics;
using Autofac.Core.Resolving;
using Whitebox.Connector;
using Whitebox.Messages;
using Whitebox.Model;

namespace Whitebox.Containers.Autofac
{
    public class WhiteboxProfilingModule : Module, IContainerAwareComponent
    {
        readonly IWriteQueue _client;
        readonly ModelMapper _modelMapper = new ModelMapper();

        public WhiteboxProfilingModule(IWriteQueue client)
        {
            if (client == null) throw new ArgumentNullException("client");
            _client = client;
        }

        public WhiteboxProfilingModule()
            : this (new NamedPipesWriteQueue())
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterInstance(this).As<IContainerAwareComponent>();

            var processInfo = Process.GetCurrentProcess();
            Send(new ProfilerConnectedMessage(processInfo.MainModule.FileName, processInfo.Id));
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            base.AttachToComponentRegistration(componentRegistry, registration);

            var includedTypes = _modelMapper.GetReferencedTypes(registration);
            foreach (var includedType in includedTypes)
                SendTypeModelIfNeeded(includedType);
           var message = new ComponentAddedMessage(_modelMapper.GetComponentModel(registration));            
            Send(message);
        }

        void SendTypeModelIfNeeded(Type type)
        {
            TypeModel typeModel;
            if (_modelMapper.GetOrAddTypeModel(type, out typeModel))
            {
                var message = new TypeDiscoveredMessage(typeModel);
                Send(message);
            }
        }

        protected override void AttachToRegistrationSource(IComponentRegistry componentRegistry, IRegistrationSource registrationSource)
        {
            base.AttachToRegistrationSource(componentRegistry, registrationSource);

            var message = new RegistrationSourceAddedMessage(_modelMapper.GetRegistrationSourceModel(registrationSource));
            Send(message);
        }

        public void SetContainer(IContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            var rootScope = container.Resolve<ILifetimeScope>();
            AttachToLifetimeScope(rootScope);
        }

        void AttachToLifetimeScope(ILifetimeScope lifetimeScope, ILifetimeScope parent = null)
        {
            var lifetimeScopeModel = _modelMapper.GetLifetimeScopeModel(lifetimeScope, parent);
            var message = new LifetimeScopeBeginningMessage(lifetimeScopeModel);
            Send(message);

            lifetimeScope.CurrentScopeEnding += (s, e) =>
            {
                Send(new LifetimeScopeEndingMessage(lifetimeScopeModel.Id));
                _modelMapper.IdTracker.ForgetId(lifetimeScope);
            };

            lifetimeScope.ChildLifetimeScopeBeginning += (s, e) => AttachToLifetimeScope(e.LifetimeScope, lifetimeScope);
            lifetimeScope.ResolveOperationBeginning += (s, e) => AttachToResolveOperation(e.ResolveOperation, lifetimeScopeModel);
        }

        void AttachToResolveOperation(IResolveOperation resolveOperation, LifetimeScopeModel lifetimeScope)
        {
            
            var resolveOperationModel = _modelMapper.GetResolveOperationModel(resolveOperation, lifetimeScope, new StackTrace());
            Send(new ResolveOperationBeginningMessage(resolveOperationModel));
            resolveOperation.CurrentOperationEnding += (s, e) =>
            {
                var message = e.Exception != null ?
                    new ResolveOperationEndingMessage(resolveOperationModel.Id, e.Exception.GetType().AssemblyQualifiedName, e.Exception.Message) :
                    new ResolveOperationEndingMessage(resolveOperationModel.Id);
                Send(message);
            };
            resolveOperation.InstanceLookupBeginning += (s, e) => AttachToInstanceLookup(e.InstanceLookup, resolveOperationModel);
        }

        void AttachToInstanceLookup(IInstanceLookup instanceLookup, ResolveOperationModel resolveOperation)
        {
            var instanceLookupModel = _modelMapper.GetInstanceLookupModel(instanceLookup, resolveOperation);
            Send(new InstanceLookupBeginningMessage(instanceLookupModel));
            instanceLookup.InstanceLookupEnding += (s, e) => Send(new InstanceLookupEndingMessage(instanceLookupModel.Id, e.NewInstanceActivated));
            instanceLookup.CompletionBeginning += (s, e) => Send(new InstanceLookupCompletionBeginningMessage(instanceLookupModel.Id));
            instanceLookup.CompletionEnding += (s, e) => Send(new InstanceLookupCompletionEndingMessage(instanceLookupModel.Id));
        }

        void Send(object message)
        {
            _client.Enqueue(message);
        }

        internal ModelMapper ModelMapper { get { return _modelMapper; } }
    }
}
