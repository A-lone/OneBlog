﻿using OneBlog.Core.Data.Contracts;
using OneBlog.Core.Data.ViewModels;
using System.Web.Http;

public class DashboardController : ApiController
{
    readonly IDashboardRepository repository;

    public DashboardController(IDashboardRepository repository)
    {
        this.repository = repository;
    }

    public DashboardVM Get()
    {
        return repository.Get();
    }
}