﻿using NUnit.Framework;
using Rainbow.ObjectFlow.Engine;
using Rainbow.ObjectFlow.Framework;
using Rhino.Mocks;

namespace Objectflow.core.tests.Concurrency
{
    [TestFixture]
    public class WhenParallisingOperations
    {
        private Workflow<string> _workflow;
        private MockRepository _mocker;
        private WorkflowEngine<string> _engine;
        [SetUp]
        public void Given()
        {
            _mocker = new MockRepository();
            _engine = _mocker.DynamicMock<WorkflowEngine<string>>();
            _workflow = new Workflow<string>(_engine);
        }

        [Test]
        public void ShouldUseExecutonEngine()
        {
            _workflow.Do(a => a += ", yellow").And.Do(b => b += ", orange").Then();
            var func1 = ((ParallelInvoker<string>)_workflow.RegisteredOperations[0].Command).RegisteredOperations[0];
            var func2 = ((ParallelInvoker<string>)_workflow.RegisteredOperations[0].Command).RegisteredOperations[1];

            var ps = new ParallelInvoker<string>(_engine);

            ps.RegisteredOperations.Add(func1);
            ps.RegisteredOperations.Add(func2);

            Expect.Call(_engine.Execute(func1, "Red")).Return("Red, yellow").IgnoreArguments().Repeat.Twice();
            _mocker.ReplayAll();

            var result = ps.Execute("Red");
            _mocker.VerifyAll();
        }
    }
}
