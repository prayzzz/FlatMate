using System;
using JetBrains.Annotations;

namespace FlatMate.Common.Attributes
{
    public enum DependencyLifetime
    {
        Transient,
        Scoped,
        Singleton
    }

    /// <summary>
    /// Indicates that the marked class should be injected into the DependencyInjection-Container.
    /// Uses <see cref="DependencyLifetime.Scoped"/> as default lifetime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [MeansImplicitUse]
    public class InjectAttribute : Attribute
    {
        /// <summary>
        /// Uses <see cref="DependencyLifetime.Scoped"/> as lifetime.
        /// </summary>
        /// <param name="serviceTypes"></param>
        public InjectAttribute(params Type[] serviceTypes)
        {
            Lifetime = DependencyLifetime.Scoped;
            ServiceTypes = serviceTypes;
        }

        public InjectAttribute(DependencyLifetime lifetime, params Type[] serviceTypes)
        {
            Lifetime = lifetime;
            ServiceTypes = serviceTypes;
        }

        public DependencyLifetime Lifetime { get; }

        public Type[] ServiceTypes { get; }
    }
}