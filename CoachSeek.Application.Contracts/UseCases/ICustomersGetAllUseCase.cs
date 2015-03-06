﻿using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomersGetAllUseCase : IBusinessRepositorySetter
    {
        IList<CustomerData> GetCustomers();
    }
}
