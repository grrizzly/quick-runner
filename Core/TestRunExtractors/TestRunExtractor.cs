﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace QuickRunner.Core.TestRunExtractors
{
    public abstract class TestRunExtractor
    {
        protected Assembly Assembly { get; private set; }

        protected RunnerOptions Options { get; private set; }

        protected List<ITestEnvironment> Environments { get; private set; } 

        protected TestRunExtractor(RunnerOptions options)
        {
            Options = options;
            Environments = options.Environments;
            Environments.ForEach(env => env.Initialize(options.AssemblyPath, options.ConfigFilepath));
        }

        public IEnumerable<TestRun> Execute()
        {
            LoadAssembly();
            var tests = FilterTests(GetTestMethods());
            return CreateRuns(tests);
        }

        protected static bool IsTestFixture(Type t)
        {
            return HasCustomAttribute(t, typeof(TestFixtureAttribute));
        }

        protected static bool IsTest(MethodInfo m)
        {
            return HasCustomAttribute(m, typeof (TestAttribute));
        }

        protected abstract IEnumerable<MethodInfo> FilterTests(IEnumerable<MethodInfo> tests);

        private void LoadAssembly()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ExternalAssemblyResolver; // Temporarily enhance dependency resolution
            Assembly = Assembly.LoadFrom(Path.Combine(Options.AssemblyPath, Options.AssemblyFileName));
            AppDomain.CurrentDomain.AssemblyResolve -= ExternalAssemblyResolver;
        }

        private Assembly ExternalAssemblyResolver(object sender, ResolveEventArgs args)
        {
            try
            {
                // Grab GAC or currently loaded assembly
                return Assembly.Load(args.Name);
            }
            catch
            {
                // Grab platform-specific assembly
                return Assembly.LoadFrom(Path.Combine(Options.AssemblyPath, args.Name + ".dll"));
            }
        }

        protected virtual IEnumerable<TestRun> CreateRuns(IEnumerable<MethodInfo> tests)
        {
            var i = 0;
            return tests
                .GroupBy(x => i++ % Environments.Count)
                .Zip(Environments, (testGroup, environment) => new TestRun(environment, testGroup.ToList()));
        }

        private IEnumerable<MethodInfo> GetTestMethods()
        {
            return Assembly.GetTypes()
                .Where(IsTestFixture)
                .SelectMany(t => t.GetMethods().Where(IsTest));
        }

        protected static bool HasCustomAttribute(ICustomAttributeProvider type, Type attributeType)
        {
            return type.GetCustomAttributes(attributeType, true).Length > 0;
        }
    }
}