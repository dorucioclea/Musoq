﻿using System.Collections.Generic;

namespace Musoq.Parser.Nodes
{
    public class NestedQueryFromNode : FromNode
    {
        public NestedQueryFromNode(QueryNode query, string schema, string method, IDictionary<string, int> columnToIndexMap)
            : base(string.Empty)
        {
            Query = query;
            Id = $"{nameof(NestedQueryFromNode)}{query.Id}";
            ColumnToIndexMap = columnToIndexMap;
        }

        public QueryNode Query { get; }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string Id { get; }

        public IDictionary<string, int> ColumnToIndexMap { get; }

        public override string ToString()
        {
            return $"from ({Query.ToString()})";
        }
    }
}