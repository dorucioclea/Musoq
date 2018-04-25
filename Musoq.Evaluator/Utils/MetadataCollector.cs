﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Musoq.Schema;

namespace Musoq.Evaluator.Utils
{
    public class MetadataCollector
    {
        private readonly Dictionary<string, ISchemaTable> _tables;
        private readonly Dictionary<string, List<ISchemaColumn>> _realColumns;
        private readonly List<string> _aliases;

        public MetadataCollector()
        {
            _tables = new Dictionary<string, ISchemaTable>();
            _realColumns = new Dictionary<string, List<ISchemaColumn>>();
            _aliases = new List<string>();
        }

        public void AddTable(string name, ISchemaTable table)
        {
            _tables.Add(name, table);
            _realColumns.Add(name, new List<ISchemaColumn>());
            _aliases.Add(name);
        }

        public void AddUsedColumn(string name, ISchemaColumn column)
        {
            var cols = _tables[name].Columns;

            if (!cols.Any(f => f.ColumnName == column.ColumnName && f.ColumnType == column.ColumnType))
                throw new Exception();

            _realColumns[name].Add(column);
        }

        public ISchemaColumn[] GetUsedColumns(string name)
        {
            return _realColumns[name].ToArray();
        }

        public ISchemaColumn[] GetAllColumns(string name)
        {
            return _tables[name].Columns;
        }

        public ISchemaTable GetTable(string name)
        {
            return _tables[name];
        }
    }
}