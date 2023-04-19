﻿using System;
using System.Collections.Generic;
using Musoq.Schema.DataSources;
using Musoq.Schema.Managers;

namespace Musoq.Evaluator.Tests.Schema.Basic
{
    public class TestSchema<T, TTable> : SchemaBase
    {
        public TestSchema(IEnumerable<T> sources, IDictionary<string, int> testNameToIndexMap, IDictionary<int, Func<T, object>> testIndexToObjectAccessMap)
            : base("test", CreateLibrary())
        {
            AddSource<EntitySource<T>>("entities", sources, testNameToIndexMap, testIndexToObjectAccessMap);
            AddTable<TTable>("entities");
        }

        private static MethodsAggregator CreateLibrary()
        {
            var methodManager = new MethodsManager();
            var propertiesManager = new PropertiesManager();

            var lib = new TestLibrary();

            propertiesManager.RegisterProperties(lib);
            methodManager.RegisterLibraries(lib);

            return new MethodsAggregator(methodManager, propertiesManager);
        }
    }
}