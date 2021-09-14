﻿using Data.Models;
using NUnit.Framework;
using Services.Mapping;
using System.Collections.Generic;
using System.Linq;
using Web.Models;
using Web.Models.ViewModels;

namespace Tests.Service.Tests
{
    public class MappingTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            MappingConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);
        }

        [Test]
        public void ProjectTo_ShouldNotThrowException()
        {
            List<EmployeeData> employeeNullList = new()
            {
                new EmployeeData
                {
                }
            };

            Assert.DoesNotThrow(() => employeeNullList.AsQueryable().ProjectTo<EmployeeDataViewModel>());
        }

        [Test]
        public void ProjectTo_ShouldNotThrowExceptionWithMembers()
        {
            List<EmployeeData> employeeNullList = new()
            {
                new EmployeeData
                {
                }
            };

            Assert.DoesNotThrow(() => employeeNullList.AsQueryable().ProjectTo<EmployeeDataViewModel>(""));
        }

    }
}
