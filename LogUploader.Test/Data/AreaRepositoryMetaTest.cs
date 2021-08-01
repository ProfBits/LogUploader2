using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Data.New;
using LogUploader.Data.Repositories;

using NUnit.Framework;

namespace LogUploader.Test.Data
{
    public class AreaRepositoryMetaTest
    {
        [Test]
        public void AllAreaRepositoriesAreTested()
        {
            int numOfTestClasses = TestHelper.GetAllSubClassesOfType(typeof(AreaRepositoryTest)).Count();
            int numOfClassesToTest = GetAllSubClassesOfType(typeof(AreaRepository<>)) +
                                     GetAllSubClassesOfType(typeof(MultiAreaRepository<>));


            if (numOfTestClasses != numOfClassesToTest)
                Assert.Warn($"Nuber of test subclasses of {nameof(AreaRepositoryTest)} is not equal to" +
                    $"the number of concrete implementaions of {nameof(AreaRepository<GameArea>)} and" +
                    $"{nameof(MultiAreaRepository<MultiGameArea>)}.\n" +
                    $"Expected: {numOfClassesToTest}\n" +
                    $"Got: {numOfTestClasses}");
        }

        private static int GetAllSubClassesOfType(Type baseType)
        {
            var AllTypesOfBaseType = from x in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assambly => assambly.GetTypes())
                                     let y = x.BaseType
                                     where !x.IsAbstract && !x.IsInterface &&
                                     y != null && y.IsGenericType &&
                                     y.GetGenericTypeDefinition() == baseType
                                     select x;
            return AllTypesOfBaseType.Count();
        }
    }
}
