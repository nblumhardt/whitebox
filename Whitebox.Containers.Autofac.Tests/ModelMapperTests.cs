using System.Linq;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using NUnit.Framework;
using Whitebox.Model;

namespace Whitebox.Containers.Autofac.Tests
{
    public class ModelMapperTests
    {
        [TestFixture]
        public class WhenMappingAComponentModel
        {
            ModelMapper _modelMapper;
            IComponentRegistration _componentRegistration, _componentRegistrationTarget;
            ComponentModel _componentModel;

            const string MetadataKey = "MK";
            const int MetadataValue = 42;

            [SetUp]
            public void SetUp()
            {
                _modelMapper = new ModelMapper();

                _componentRegistrationTarget = RegistrationBuilder
                    .ForType(typeof(object))
                    .CreateRegistration();

                _componentRegistration = RegistrationBuilder
                    .ForType(typeof(string))
                    .WithMetadata(MetadataKey, MetadataValue)
                    .Targeting(_componentRegistrationTarget)
                    .CreateRegistration();

                TypeModel unused;
                _modelMapper.GetOrAddTypeModel(typeof(object), out unused);
                _modelMapper.GetOrAddTypeModel(typeof(string), out unused);
                _componentModel = _modelMapper.GetComponentModel(_componentRegistration);
            }
            
            [Test]
            public void TheIdOfTheComponentIsMapped()
            {
                Assert.AreEqual(_componentModel.Id, _modelMapper.GetComponentId(_componentRegistration));
            }
            
            [Test]
            public void TheIdOfTheComponentTargetIsMapped()
            {
                Assert.AreEqual(_componentModel.TargetComponentId, _modelMapper.GetComponentId(_componentRegistrationTarget));
            }

            [Test]
            public void TheServicesOfTheComponentAreMapped()
            {
                CollectionAssert.AreEquivalent(_componentRegistration.Services.Select(_modelMapper.GetServiceModel), _componentModel.Services);
            }

            [Test]
            public void TheMetadataOfTheComponentIsMapped()
            {
                // This includes the internal `__RegistrationOrder` metadata; we might wish to hide this from the profiler.
                Assert.AreEqual(2, _componentModel.Metadata.Count);
                Assert.AreEqual(MetadataValue.ToString(), _componentModel.Metadata[MetadataKey]);
            }

            [Test]
            public void TheLimitTypeIsMapped()
            {
                Assert.AreEqual(_modelMapper.GetTypeId(_componentRegistration.Activator.LimitType), _componentModel.LimitTypeId);
            }

            [Test]
            public void InstanceOwnershipIsMapped()
            {
                Assert.AreEqual(_componentRegistration.Ownership.ToString(), _componentModel.Ownership.ToString());
            }

            [Test]
            public void InstanceSharingIsMapped()
            {
                Assert.AreEqual(_componentRegistration.Sharing.ToString(), _componentModel.Sharing.ToString());
            }

            [Test]
            public void LifetimeIsMapped()
            {
                Assert.AreEqual(LifetimeModel.CurrentScope, _componentModel.Lifetime);
            }

            [Test]
            public void TheActivatorTypeIsMapped()
            {
                Assert.AreEqual(ActivatorModel.Reflection, _componentModel.Activator);
            }
        }

        [TestFixture]
        public class WhenMappingAKeyedServiceModel
        {
            ModelMapper _modelMapper;
            KeyedService _service;
            ServiceModel _mapped;

            [SetUp]
            public void SetUp()
            {
                _modelMapper = new ModelMapper();
                TypeModel unused;
                _modelMapper.GetOrAddTypeModel(typeof(string), out unused);
                _service = new KeyedService(new object(), typeof(string));
                _mapped = _modelMapper.GetServiceModel(_service);                
            }

            [Test]
            public void TheKeyIsMapped()
            {
                Assert.AreEqual(_service.ServiceKey.ToString(), _mapped.Key);
            }

            [Test]
            public void TheTypeIsMapped()
            {
                Assert.AreEqual(_modelMapper.GetTypeId(_service.ServiceType), _mapped.ServiceTypeId);
            }

            [Test]
            public void TheDescriptionIsMapped()
            {
                Assert.AreEqual(_service.Description, _mapped.Description);
            }
        }

        [TestFixture]
        public class WhenMappingATypedServiceModel
        {
            ModelMapper _modelMapper;
            TypedService _service;
            ServiceModel _mapped;

            [SetUp]
            public void SetUp()
            {
                _modelMapper = new ModelMapper();
                TypeModel unused;
                _modelMapper.GetOrAddTypeModel(typeof(string), out unused);
                _service = new TypedService(typeof(string));
                _mapped = _modelMapper.GetServiceModel(_service);
            }

            [Test]
            public void TheKeyIsNull()
            {
                Assert.IsNull(_mapped.Key);
            }

            [Test]
            public void TheTypeIsMapped()
            {
                Assert.AreEqual(_modelMapper.GetTypeId(_service.ServiceType), _mapped.ServiceTypeId);
            }

            [Test]
            public void TheDescriptionIsMapped()
            {
                Assert.AreEqual(_service.Description, _mapped.Description);
            }
        }
    }
}
