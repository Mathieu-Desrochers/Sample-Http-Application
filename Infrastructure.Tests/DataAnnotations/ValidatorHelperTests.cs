
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleHttpApplication.Infrastructure.Code.DataAnnotations;

namespace SampleHttpApplication.Infrastructure.Tests.DataAnnotations
{
    [TestClass]
    public class ValidatorHelperTests
    {
        public class Person
        {
            public int Age;
        }

        [TestMethod]
        public void Test()
        {
            ValidatorHelper.RegisterValidation<Person, int>();
            ValidationError<int>[] validationErrors = ValidatorHelper.ValidateObject<Person, int>(new Person());
        }
    }
}
