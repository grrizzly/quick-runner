﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QuickRunner.Core;
using QuickRunner.Core.TestRunExtractors;

namespace Core_Specs.TestRunExtractors
{
    [TestFixture]
    public class EnvironmentNamespacesExtractorSpec : TestRunExtractorSpecBase
    {
        private IEnumerable<TestRun> _runs; 

        [SetUp]
        public void Before()
        {   
            Options.Environments = new List<ITestEnvironment>
            {
                RunnerDefaults.CreateMockEnvironment("foo", new List<string> { "Slowlenium.A" }),
                RunnerDefaults.CreateMockEnvironment("bar"),
            };

            _runs = new EnvironmentNamespacesExtractor(Options).Execute();
        }

        [Test]
        public void ShouldHaveTestsInTheRun()
        {
            Assert.GreaterOrEqual(_runs.First().Tests.Count, 1);
        }

        [Test]
        public void ShouldAssignSpecifiedNamespacesToEachEnvironment()
        {
            Assert.IsTrue(_runs.First().Tests.All(m => m.DeclaringType.Namespace == "Slowlenium.A"));
        }

        [Test]
        public void ShouldNotCreateRunsForEnvironmentsWithoutANamespaceSpecifier()
        {
            Assert.AreEqual(1, _runs.Count());
        }
    }
}