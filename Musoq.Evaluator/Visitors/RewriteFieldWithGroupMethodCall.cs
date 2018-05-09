﻿using System.Collections.Generic;
using System.Linq;
using Musoq.Parser;
using Musoq.Parser.Nodes;

namespace Musoq.Evaluator.Visitors
{
    public class RewriteFieldWithGroupMethodCall : RewriteQueryVisitor
    {
        public RewriteFieldWithGroupMethodCall(TransitionSchemaProvider schemaProvider, int fieldOrder, FieldNode[] fields)
            : base(schemaProvider, new List<AccessMethodNode>())
        {
            _fieldOrder = fieldOrder;
            _fields = fields;
        }

        public FieldNode Expression { get; private set; }

        private int _fieldOrder;
        private readonly FieldNode[] _fields;

        public override void Visit(FieldNode node)
        {
            base.Visit(node);
            Expression = Nodes.Pop() as FieldNode;
        }

        public override void Visit(AccessColumnNode node)
        {
            Nodes.Push(new AccessColumnNode($"{node.Alias}.{node.Name}", string.Empty, node.ReturnType, TextSpan.Empty));
        }

        public override void Visit(AccessMethodNode node)
        {
            if (node.IsAggregateMethod)
            {
                Nodes.Pop();

                var wordNode = node.Arguments.Args[0] as WordNode;
                Nodes.Push(new AccessColumnNode(wordNode.Value, string.Empty, node.ReturnType, TextSpan.Empty));
            }
            else if (_fields.Select(f => f.Expression.ToString()).Contains(node.ToString()))
            {
                Nodes.Push(new AccessColumnNode(node.ToString(), string.Empty, node.ReturnType, TextSpan.Empty));
            }
            else
            {
                base.Visit(node);
            }
        }

        public override void Visit(AccessCallChainNode node)
        {
            Nodes.Push(new AccessColumnNode(node.ToString(), string.Empty, node.ReturnType, TextSpan.Empty));
        }
    }
}