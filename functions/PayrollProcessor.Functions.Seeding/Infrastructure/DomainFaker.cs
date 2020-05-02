using System;
using System.Collections.Generic;
using System.Reflection;
using Bogus;

namespace PayrollProcessor.Functions.Seeding.Infrastructure
{
    /// <summary>
    /// Creates Fakers that use a specific constructor
    /// See: https://github.com/bchavez/Bogus/issues/291#issuecomment-614371450
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DomainFaker<T> : Faker<T> where T : class
    {
        public DomainFaker(ConstructorInfo constructor) =>
            CustomInstantiator(f => ResolveConstructor(f, constructor));

        protected virtual T ResolveConstructor(Faker _, ConstructorInfo constructor)
        {
            var pi = constructor.GetParameters();

            var liveParameters = new List<object>();

            foreach (var param in pi)
            {
                if (param.ParameterType == typeof(Guid))
                {
                    liveParameters.Add(Guid.NewGuid());
                }
            }

            return Activator.CreateInstance(typeof(T), liveParameters.ToArray()) as T;
        }
    }
}
