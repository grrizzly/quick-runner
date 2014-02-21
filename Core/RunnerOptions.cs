﻿using System.Collections.Generic;

namespace QuickRunner.Core
{
    public class RunnerOptions
    {
        public string AssemblyPath { get; set; }

        public string AssemblyFileName { get; set; }

        /// <summary>
        /// Gets or sets the filepath to the config file containing the appSettings section for the assembly.
        /// The filepath should be relative to the assembly path
        /// </summary>
        public string ConfigFilepath { get; set; }

        public List<TestEnvironment> Environments { get; set; }
    }
}