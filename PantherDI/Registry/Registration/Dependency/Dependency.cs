﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using PantherDI.Comparers;

namespace PantherDI.Registry.Registration.Dependency
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Dependency : IDependency
    {
        [DebuggerStepThrough]
        public Dependency(Type expectedType, params object[] requiredContracts)
        {
            if (expectedType == null) throw new ArgumentNullException(nameof(expectedType));

            if (!requiredContracts.Any())
            {
                requiredContracts = new object[] {expectedType};
            }

            ExpectedType = expectedType;
            RequiredContracts = new HashSet<object>(requiredContracts);
        }

        private string DebuggerDisplay => (Ignored ? "Ignored" : "") + $"Dependency<{ExpectedType}>({String.Join(",", RequiredContracts.Select(x =>x.ToString()))})";

        public Type ExpectedType { get; }

        public ISet<object> RequiredContracts { get; }

        public bool Ignored { get; set; }

        #region Equality members

        protected bool Equals(Dependency other)
        {
            return new EqualityComparer().Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            return ((IEqualityComparer)new EqualityComparer()).Equals((object)this, obj);
        }

        public override int GetHashCode()
        {
            return new EqualityComparer().GetHashCode(this);
        }

        #region Overrides of Object

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        #endregion

        #endregion

        public class EqualityComparer : IEqualityComparer, IEqualityComparer<IDependency>
        {
            bool IEqualityComparer.Equals(object x, object y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(null, x) != ReferenceEquals(null, y)) return false;

                if (!typeof(IDependency).GetTypeInfo().IsAssignableFrom(x.GetType().GetTypeInfo()))
                    throw new ArgumentException("Compared items must be of type IDependency", nameof(x));

                if (!typeof(IDependency).GetTypeInfo().IsAssignableFrom(y.GetType().GetTypeInfo()))
                    throw new ArgumentException("Compared items must be of type IDependency", nameof(y));

                return Equals((IDependency)x, (IDependency)y);
            }

            int IEqualityComparer.GetHashCode(object x)
            {
                if (!typeof(IDependency).GetTypeInfo().IsAssignableFrom(x.GetType().GetTypeInfo()))
                    throw new ArgumentException("Compared items must be of type IDependency", nameof(x));

                return GetHashCode((IDependency) x);
            }

            public bool Equals(IDependency x, IDependency y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(null, x) || ReferenceEquals(null, y)) return false;
                if (x.ExpectedType != y.ExpectedType) return false;
                if (!SetComparer<object>.Instance.Equals(x.RequiredContracts, y.RequiredContracts)) return false;
                if (x.Ignored != y.Ignored) return false;

                return true;
            }

            public int GetHashCode(IDependency obj)
            {
                var hashCode = obj.ExpectedType != null ? obj.ExpectedType.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ SetComparer<object>.Instance.GetHashCode(obj.RequiredContracts);
                hashCode = (hashCode * 397) ^ obj.Ignored.GetHashCode();
                return hashCode;
            }
        }
    }
}