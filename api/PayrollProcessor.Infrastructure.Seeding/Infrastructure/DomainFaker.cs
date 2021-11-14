using System;
using Bogus;

namespace PayrollProcessor.Infrastructure.Seeding.Infrastructure;

/// <summary>
/// Creates Fakers that use a specific factory function
/// See: https://github.com/bchavez/Bogus/issues/291#issuecomment-614371450
/// </summary>
/// <typeparam name="T"></typeparam>
public class DomainFaker<T> : Faker<T> where T : class
{
    public DomainFaker(Func<Faker, T> factory) =>
        CustomInstantiator(factory);
}
