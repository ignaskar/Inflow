﻿using Inflow.Modules.Customers.Core.Domain.ValueObjects;
using Inflow.Modules.Customers.Core.Exceptions;
using Inflow.Shared.Abstractions.Kernel.ValueObjects;

namespace Inflow.Modules.Customers.Core.Domain.Entities;

internal class Customer
{
    public Guid Id { get; }
    public Email Email { get; }
    public Name Name { get; private set; }
    public FullName FullName { get; private set; }
    public Address Address { get; private set; }
    public Nationality Nationality { get; private set; }
    public Identity Identity { get; private set; }
    public string Notes { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? VerifiedAt { get; private set; }

    // For EF
    private Customer()
    {

    }

    public Customer(Email email, DateTime createdAt)
    {
        Id = Guid.NewGuid();
        Email = email;
        CreatedAt = createdAt;
    }

    public void Complete(
        Name name, 
        FullName fullName, 
        Address address, 
        Nationality nationality, 
        Identity identity)
    {
        if (!IsActive)
        {
            throw new CustomerNotActiveException(Id);
        }

        if (CompletedAt.HasValue)
        {
            throw new CustomerAlreadyCompletedException(Id);
        }

        Name = name;
        FullName = fullName;
        Address = address;
        Nationality = nationality;
        Identity = identity;
        CompletedAt = DateTime.Now;
    }

    public void Verify()
    {
        if (!IsActive)
        {
            throw new CustomerNotActiveException(Id);
        }

        if (!CompletedAt.HasValue || VerifiedAt.HasValue)
        {
            throw new CannotVerifyCustomerException(Id);
        }

        VerifiedAt = DateTime.Now;
    }

    public void Lock(string notes = null)
    {
        IsActive = false;
        Notes = notes?.Trim();
    }

    public void Unlock(string notes = null)
    {
        IsActive = true;
        Notes = notes?.Trim();
    }
}