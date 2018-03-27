﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PantherDI.Registry.Registration
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct Dependency
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
            Ignored = false;
        }


        private string DebuggerDisplay => (Ignored ? "Ignored" : "") + $"Dependency<{ExpectedType}>({String.Join(",", RequiredContracts.Select(x =>x.ToString()))})";

        public Type ExpectedType { get; }

        public ISet<object> RequiredContracts { get; }

        public bool Ignored { get; set; }


        #region Equality members

        public bool Equals(Dependency other)
        {
            // ReSharper disable once CheckForReferenceEqualityInstead.2
            return Equals(ExpectedType, other.ExpectedType) && RequiredContracts.SetEquals(other.RequiredContracts);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Dependency && Equals((Dependency) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ExpectedType != null ? ExpectedType.GetHashCode() : 0;
                foreach (var contract in RequiredContracts)
                {
                    hashCode = (hashCode * 397) ^ contract.GetHashCode();
                }
                hashCode = (hashCode * 397) ^ Ignored.GetHashCode();
                return hashCode;
            }
        }

        #endregion

        public static bool operator==(Dependency o1, Dependency o2)
        {
            return o1.Equals(o2);
        }

        public static bool operator !=(Dependency o1, Dependency o2)
        {
            return !o1.Equals(o2);
        }
    }
}