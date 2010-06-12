﻿using System.Collections.Generic;
using Rainbow.ObjectFlow.Engine;
using Rainbow.ObjectFlow.Interfaces;

namespace Rainbow.ObjectFlow.Constraints.Operators
{
    /// <summary>
    /// Defines constraints allowed with ConstraintOperators.
    /// </summary>
    public abstract class ConstraintOperator : CheckConstraint
    {
        private static Stack<CheckConstraint> CallStack
        {
            get
            {
                return WorkflowEngine<CheckConstraint>.CallStack;
            }
        }

        /// <summary>
        /// Creates a SuccessCheckConstraint that can work with BasicOperations of T
        /// </summary>
        /// <typeparam name="T">Type the Constraint works with on</typeparam>
        /// <param name="operaton">the operation the constraint evaluates as successding or failing</param>
        /// <returns>A SuccessCheckConstraint</returns>
        public SuccessCheckConstraint<T> Successfull<T>(IOperation<T> operaton)
        {
            IList<CheckConstraint> exp = UnwindStack();

            return new SuccessCheckConstraint<T>(operaton, exp);
        }

        /// <summary>
        /// Creates a BooleanCheckConstraint that works with Booleans.
        /// </summary>
        /// <param name="condition">true/False</param>
        /// <returns>A BooleanCheckConstraint that can evaluate a boolean value</returns>
        public BooleanCheckConstraint IsTrue(bool condition)
        {
            IList<CheckConstraint> exp = UnwindStack();

            return new BooleanCheckConstraint(exp, condition);
        }

        private static IList<CheckConstraint> UnwindStack()
        {
            IList<CheckConstraint> exp = new List<CheckConstraint>();
            while (CallStack.Count > 0)
            {
                exp.Add(CallStack.Pop());
            }

            return exp;
        }
    }
}