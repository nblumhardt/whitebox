using System;
using System.Collections.Generic;
using System.Linq;
using Whitebox.Model;

namespace Whitebox.Core.Application
{
    public class Component : IApplicationItem
    {
        readonly string _id;
        readonly TypeData _limitType;
        readonly IEnumerable<Service> _services;
        readonly OwnershipModel _ownership;

        public Component(string id, TypeData limitType, IEnumerable<Service> services, OwnershipModel ownership)
        {
            if (limitType == null) throw new ArgumentNullException("limitType");
            _id = id;
            _limitType = limitType;
            _services = services;
            _ownership = ownership;
        }

        public OwnershipModel Ownership
        {
            get { return _ownership; }
        }

        public IEnumerable<Service> Services
        {
            get { return _services; }
        }

        public TypeData LimitType
        {
            get { return _limitType; }
        }

        public string Id { get { return _id; } }

        public string Description
        {
            get
            {
                if (LimitType.Identity.DisplayName != typeof(object).Name)
                    return LimitType.Identity.DisplayFullName;

                return "Unknown (" + string.Join(", ", Services.Select(s => s.ServiceType.Identity.DisplayName)) + ")";
            }
        }

        public bool IsTracked
        {
            get { return LimitType.IsDisposable && Ownership == OwnershipModel.OwnedByLifetimeScope; }
        }
    }
}
