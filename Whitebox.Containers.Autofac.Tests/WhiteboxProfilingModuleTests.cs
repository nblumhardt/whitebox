using System.Linq;
using System.Threading;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using NUnit.Framework;
using Whitebox.Messages;

namespace Whitebox.Containers.Autofac.Tests
{
    public class WhiteboxProfilingModuleTests
    {
        [TestFixture]
        public class WhenAComponentIsRegistered
        {
            ModuleTestHarness _fixture;
            IComponentRegistration _componentRegistration;
            ComponentAddedMessage _message;

            [SetUp]
            public void SetUp()
            {
                _componentRegistration = RegistrationBuilder
                    .ForType(typeof(string))
                    .CreateRegistration();

               _fixture = new ModuleTestHarness();

                _fixture.ClearMessages();
                _fixture.Container.ComponentRegistry.Register(_componentRegistration);

                _message = _fixture.GetSingleMessageOrDefault<ComponentAddedMessage>();
            }

            [Test]
            public void AComponentAddedMessageIsSent()
            {
                Assert.IsNotNull(_message);
            }

            [Test]
            public void TheMessageDescribesTheAddedComponent()
            {
                Assert.AreEqual(_message.Component.Id, _fixture.ModelMapper.GetComponentId(_componentRegistration));
            }
        }

        [TestFixture]
        public class WhenTheContainerIsInitialised
        {
            ModuleTestHarness _fixture;
            ContainerInitialisedMessage _containerInitialisedMessage;
            LifetimeScopeBeginningMessage _rootScopeBeginningMessage;
            
            [SetUp]
            public void SetUp()
            {
                _fixture = new ModuleTestHarness();
                _containerInitialisedMessage = _fixture.GetSingleMessageOrDefault<ContainerInitialisedMessage>();
                _rootScopeBeginningMessage = _fixture.GetSingleMessageOrDefault<LifetimeScopeBeginningMessage>();
            }

            [Test]
            public void AnInitialisationMessageIsSent()
            {
                Assert.IsNotNull(_containerInitialisedMessage);
            }

            [Test]
            public void TheContainerIdIsPassed()
            {
                Assert.AreEqual(_fixture.RootScopeId, _containerInitialisedMessage.RootLifetimeScope.Id);
            }

            [Test]
            public void TheContainerTagIsPassed()
            {
                Assert.AreEqual(_fixture.RootScopeId, _containerInitialisedMessage.RootLifetimeScope.Id);
            }

            [Test]
            public void TheRootScopeHasNoParent()
            {
                Assert.IsNull(_containerInitialisedMessage.RootLifetimeScope.ParentLifetimeScopeId);
            }

            [Test]
            public void TheRootScopeBegins()
            {
                Assert.IsNotNull(_rootScopeBeginningMessage);
            }

            [Test]
            public void TheRootScopeCarriesTheContainerId()
            {
                Assert.AreEqual(_fixture.RootScopeId, _rootScopeBeginningMessage.LifetimeScope.Id);
            }

            [Test]
            public void TheRootScopeIdIsValid()
            {
                Assert.AreNotEqual(IdTracker.UnknownId, _fixture.RootScopeId);
            }
        }

        [TestFixture]
        public class WhenTheContainerIsDisposed
        {
            ModuleTestHarness _fixture;
            LifetimeScopeEndingMessage _lifetimeScopeEndingMessage;

            [SetUp]
            public void SetUp()
            {
                _fixture = new ModuleTestHarness();
                _fixture.ClearMessages();
                _fixture.Container.Dispose();
                _lifetimeScopeEndingMessage = _fixture.GetSingleMessageOrDefault<LifetimeScopeEndingMessage>();
            }   

            [Test]
            public void TheRootScopeEndingMessageIsSent()
            {
                Assert.IsNotNull(_lifetimeScopeEndingMessage);
            }

            [Test]
            public void TheMessageCarriesTheContainersId()
            {
                Assert.AreEqual(_fixture.RootScopeId, _lifetimeScopeEndingMessage.LifetimeScopeId);
            }
        }

        [TestFixture]
        public class WhenANestedLifetimeScopeExecutes
        {
            ModuleTestHarness _fixture;
            ILifetimeScope _lifetimeScope;
            LifetimeScopeBeginningMessage _lifetimeScopeBeginningMessage;
            LifetimeScopeEndingMessage _lifetimeScopeEndingMessage;

            [SetUp]
            public void SetUp()
            {
                _fixture = new ModuleTestHarness();
                _fixture.ClearMessages();
                _lifetimeScope = _fixture.Container.BeginLifetimeScope();
                _lifetimeScope.Dispose();
                _lifetimeScopeBeginningMessage = _fixture.GetSingleMessageOrDefault<LifetimeScopeBeginningMessage>();
                _lifetimeScopeEndingMessage = _fixture.GetSingleMessageOrDefault<LifetimeScopeEndingMessage>();
            }

            [Test]
            public void TheLifetimeScopeBeginningMessageIsSent()
            {
                Assert.IsNotNull(_lifetimeScopeBeginningMessage);
            }

            [Test]
            public void TheBeginningMessageCarriesTheIdOfTheScope()
            {
                Assert.AreEqual(_fixture.GetLifetimeScopeId(_lifetimeScope), _lifetimeScopeBeginningMessage.LifetimeScope.Id);
            }

            [Test]
            public void TheParentOfTheScopeIsTheContainer()
            {
                Assert.AreEqual(_fixture.RootScopeId, _lifetimeScopeBeginningMessage.LifetimeScope.ParentLifetimeScopeId);
            }

            [Test]
            public void TheLifetimeScopeEndingMessageIsSent()
            {
                Assert.IsNotNull(_lifetimeScopeEndingMessage);
            }

            [Test]
            public void TheEndingMessageCarriesTheIdOfTheScope()
            {
                Assert.AreEqual(_fixture.GetLifetimeScopeId(_lifetimeScope), _lifetimeScopeEndingMessage.LifetimeScopeId);
            }

            [Test]
            public void TheLifetimeScopeGetsADistinctId()
            {
                Assert.AreNotEqual(_fixture.RootScopeId, _fixture.GetLifetimeScopeId(_lifetimeScope));
            }

            [Test]
            public void TheLifetimeScopeIdIsValid()
            {
                Assert.AreNotEqual(IdTracker.UnknownId, _fixture.GetLifetimeScopeId(_lifetimeScope));
            }
        }

        [TestFixture]
        public class WhenAResolveOperationExecutes
        {
            ModuleTestHarness _fixture;
            ResolveOperationBeginningMessage _resolveOperationBeginning;
            ResolveOperationEndingMessage _resolveOperationEnding;

            [SetUp]
            public void SetUp()
            {
                _fixture = new ModuleTestHarness(builder => builder.RegisterType<object>());
                _fixture.ClearMessages();
                _fixture.Container.Resolve<object>();
                _resolveOperationBeginning = _fixture.GetSingleMessageOrDefault<ResolveOperationBeginningMessage>();
                _resolveOperationEnding = _fixture.GetSingleMessageOrDefault<ResolveOperationEndingMessage>();
            }

            [Test]
            public void TheResolveOperationBeginningMessageIsSent()
            {
                Assert.IsNotNull(_resolveOperationBeginning);
            }

            [Test]
            public void TheResolveOperationEndingMessageIsSent()
            {
                Assert.IsNotNull(_resolveOperationEnding);
            }

            [Test]
            public void TheResolveOperationBeginningMessageIncludesAnId()
            {
                Assert.IsNotNullOrEmpty(_resolveOperationBeginning.ResolveOperation.Id);
            }

            [Test]
            public void TheBeginAndEndMessagesShareAnId()
            {
                Assert.AreEqual(_resolveOperationBeginning.ResolveOperation.Id, _resolveOperationEnding.ResolveOperationId);
            }

            [Test]
            public void TheBeginningMessageCarriesTheIdOfTheScope()
            {
                Assert.AreEqual(_fixture.RootScopeId, _resolveOperationBeginning.ResolveOperation.LifetimeScopeId);
            }

            [Test]
            public void TheIdOfTheCallingThreadIsSent()
            {
                Assert.AreEqual(_fixture.ModelMapper.GetThreadId(Thread.CurrentThread), _resolveOperationBeginning.ResolveOperation.ThreadId);
            }

            [Test]
            public void TheCallingMethodIsSent()
            {
                Assert.AreEqual("SetUp", _resolveOperationBeginning.ResolveOperation.CallingMethodName);
            }

            [Test]
            public void TheCallingTypeIsSent()
            {
                Assert.AreEqual(GetType().AssemblyQualifiedName, _resolveOperationBeginning.ResolveOperation.CallingTypeAssemblyQualifiedName);
            }
        }

        [TestFixture]
        public class WhenAComponentIsActivated
        {
            ModuleTestHarness _fixture;
            IComponentRegistration _registration;
            ResolveOperationBeginningMessage _resolveOperationBeginningMessage;
            InstanceLookupBeginningMessage _instanceLookupBeginningMessage;
            InstanceLookupEndingMessage _instanceLookupEndingMessage;
            InstanceLookupCompletionBeginningMessage _instanceLookupCompletionBeginningMessage;
            InstanceLookupCompletionEndingMessage _instanceLookupCompletionEndingMessage;

            [SetUp]
            public void SetUp()
            {
                _registration = RegistrationBuilder.ForType<object>().CreateRegistration();
                _fixture = new ModuleTestHarness(b => b.RegisterComponent(_registration));

                _fixture.ClearMessages();
                _fixture.Container.Resolve<object>();

                _fixture.GetSingleMessageOrDefault(out _resolveOperationBeginningMessage);
                _fixture.GetSingleMessageOrDefault(out _instanceLookupBeginningMessage);
                _fixture.GetSingleMessageOrDefault(out _instanceLookupEndingMessage);
                _fixture.GetSingleMessageOrDefault(out _instanceLookupCompletionBeginningMessage);
                _fixture.GetSingleMessageOrDefault(out _instanceLookupCompletionEndingMessage);
            }

            [Test]
            public void TheGetOrCreateInstanceBeginningMessageIsSent()
            {
                Assert.IsNotNull(_instanceLookupBeginningMessage);
            }

            [Test]
            public void TheGetOrCreateInstanceEndingMessageIsSent()
            {
                Assert.IsNotNull(_instanceLookupEndingMessage);
            }

            [Test]
            public void TheResolveOperationIdIsTransferredInTheBeginningMessage()
            {
                Assert.AreEqual(_resolveOperationBeginningMessage.ResolveOperation.Id, _instanceLookupBeginningMessage.InstanceLookup.ResolveOperationId);
            }

            [Test]
            public void TheInstanceLookupIdIsTransferredInTheEndingMessage()
            {
                Assert.AreEqual(_instanceLookupBeginningMessage.InstanceLookup.Id, _instanceLookupEndingMessage.InstanceLookupId);
            }

            [Test]
            public void TheBeginningMessageContainsTheComponentId()
            {
                Assert.AreEqual(_fixture.ModelMapper.GetComponentId(_registration), _instanceLookupBeginningMessage.InstanceLookup.ComponentId);
            }

            [Test]
            public void ANewInstanceWasActivated()
            {
                Assert.That(_instanceLookupEndingMessage.NewInstanceActivated);
            }

            [Test]
            public void ThereWereNoParameters()
            {
                Assert.That(!_instanceLookupBeginningMessage.InstanceLookup.Parameters.Any());
            }

            [Test]
            public void TheComponentIsActivatedInTheRootScope()
            {
                Assert.AreEqual(_fixture.RootScopeId, _instanceLookupBeginningMessage.InstanceLookup.ActivationScopeId);
            }

            [Test]
            public void TheInstanceLookupCompletionBeginningMessageIsSent()
            {
                Assert.IsNotNull(_instanceLookupCompletionBeginningMessage);
            }

            [Test]
            public void TheInstanceLookupCompletionEndingMessageIsSent()
            {
                Assert.IsNotNull(_instanceLookupCompletionEndingMessage);
            }
        }

        [TestFixture]
        public class WhenATransientComponentIsActivatedFromANestedScope
        {
            ModuleTestHarness _fixture;
            IComponentRegistration _registration;
            ILifetimeScope _nestedScope;
            InstanceLookupBeginningMessage _instanceLookupBeginningMessage;

            [SetUp]
            public void SetUp()
            {
                _registration = RegistrationBuilder.ForType<object>().CreateRegistration();
                _fixture = new ModuleTestHarness(b => b.RegisterComponent(_registration));
                _nestedScope = _fixture.Container.BeginLifetimeScope();

                _fixture.ClearMessages();
                _nestedScope.Resolve<object>();

                _fixture.GetSingleMessageOrDefault(out _instanceLookupBeginningMessage);
            }

            [Test]
            public void TheComponentWasActivatedInTheNestedScope()
            {
                Assert.AreEqual(_fixture.GetLifetimeScopeId(_nestedScope), _instanceLookupBeginningMessage.InstanceLookup.ActivationScopeId);
            }
        }

        [TestFixture]
        public class WhenARootScopeComponentIsActivatedFromANestedScope
        {
            ModuleTestHarness _fixture;
            IComponentRegistration _registration;
            InstanceLookupBeginningMessage _instanceLookupBeginningMessage;

            [SetUp]
            public void SetUp()
            {
                _registration = RegistrationBuilder.ForType<object>().SingleInstance().CreateRegistration();
                _fixture = new ModuleTestHarness(b => b.RegisterComponent(_registration));
                var nestedScope = _fixture.Container.BeginLifetimeScope();

                _fixture.ClearMessages();
                nestedScope.Resolve<object>();

                _fixture.GetSingleMessageOrDefault(out _instanceLookupBeginningMessage);
            }

            [Test]
            public void TheComponentWasActivatedInTheRootScope()
            {
                Assert.AreEqual(_fixture.RootScopeId, _instanceLookupBeginningMessage.InstanceLookup.ActivationScopeId);
            }
        }

        [TestFixture]
        public class WhenAnExistingSharedInstanceIsLookedUp
        {
            ModuleTestHarness _fixture;
            IComponentRegistration _registration;
            InstanceLookupBeginningMessage _instanceLookupBeginningMessage;
            InstanceLookupEndingMessage _instanceLookupEndingMessage;
            InstanceLookupCompletionBeginningMessage _instanceLookupCompletionBeginningMessage;
            InstanceLookupCompletionEndingMessage _instanceLookupCompletionEndingMessage;

            [SetUp]
            public void SetUp()
            {
                _registration = RegistrationBuilder.ForType<object>().SingleInstance().CreateRegistration();
                _fixture = new ModuleTestHarness(b => b.RegisterComponent(_registration));
                _fixture.Container.Resolve<object>();

                _fixture.ClearMessages();
                _fixture.Container.Resolve<object>();

                _fixture.GetSingleMessageOrDefault(out _instanceLookupBeginningMessage);
                _fixture.GetSingleMessageOrDefault(out _instanceLookupEndingMessage);
                _fixture.GetSingleMessageOrDefault(out _instanceLookupCompletionBeginningMessage);
                _fixture.GetSingleMessageOrDefault(out _instanceLookupCompletionEndingMessage);
            }

            [Test]
            public void TheGetOrCreateInstanceBeginningMessageIsSent()
            {
                Assert.IsNotNull(_instanceLookupBeginningMessage);
            }

            [Test]
            public void TheGetOrCreateInstanceEndingMessageIsSent()
            {
                Assert.IsNotNull(_instanceLookupEndingMessage);
            }

            [Test]
            public void ANewInstanceWasNotActivated()
            {
                Assert.IsFalse(_instanceLookupEndingMessage.NewInstanceActivated); 
            }

            [Test]
            public void TheInstanceLookupCompletionBeginningMessageIsNotSent()
            {
                Assert.IsNull(_instanceLookupCompletionBeginningMessage);
            }

            [Test]
            public void TheInstanceLookupCompletionEndingMessageIsNotSent()
            {
                Assert.IsNull(_instanceLookupCompletionEndingMessage);
            }
        }
    }
}
