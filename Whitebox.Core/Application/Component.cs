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
        readonly SharingModel _sharing;
        readonly IDictionary<string, string> _metadata;

        public Component(string id, TypeData limitType, IEnumerable<Service> services, OwnershipModel ownership, SharingModel sharing, IDictionary<string, string> metadata)
        {
            if (limitType == null) throw new ArgumentNullException("limitType");
            if (services == null) throw new ArgumentNullException("services");
            if (metadata == null) throw new ArgumentNullException("metadata");
            _id = id;
            _limitType = limitType;
            _services = services.ToArray();
            _ownership = ownership;
            _sharing = sharing;
            _metadata = metadata;
        }

        public SharingModel Sharing
        {
            get { return _sharing; }
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

        public IDictionary<string, string> Metadata
        {
            get { return _metadata; }
        }
    }
}
