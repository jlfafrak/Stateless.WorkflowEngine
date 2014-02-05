﻿using Stateless.WorkflowEngine.Exceptions;
using Stateless.WorkflowEngine.Models;
using Stateless.WorkflowEngine.Services;
using Stateless.WorkflowEngine.Stores;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stateless.WorkflowEngine
{
    public interface IWorkflowClient
    {
        /// <summary>
        /// Checks to see if a single-instance workflow has already been registered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool IsSingleInstanceWorkflowRegistered<T>() where T : Workflow;

        /// <summary>
        /// Registers a new workflow with the engine.  Single instance workflows that already exist will result in 
        /// an exception being raised.
        /// </summary>
        /// <param name="workflow">The workflow.</param>
        /// <returns>True if a new workflow was started, otherwise false.</returns>
        void RegisterWorkflow(Workflow workflow);
    }
    
    public class WorkflowClient : IWorkflowClient
    {
        private readonly IWorkflowStore _workflowStore;

        public WorkflowClient(IWorkflowStore workflowStore)
        {
            _workflowStore = workflowStore;
        }

        /// <summary>
        /// Checks to see if a single-instance workflow has already been registered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool IsSingleInstanceWorkflowRegistered<T>() where T : Workflow
        {
            IEnumerable<T> workflows = _workflowStore.GetAllByType<T>();
            T wf = workflows.SingleOrDefault();
            if (wf == null) return false;
            if (!wf.IsSingleInstance) throw new WorkflowException(String.Format("A workflow of type {0} is registered, but not as a single instance workflow.", typeof(T).FullName));
            return true;
        }

        /// <summary>
        /// Registers a new workflow with the engine.  Single instance workflows that already exist will result in 
        /// an exception being raised.
        /// </summary>
        /// <param name="workflow">The workflow.</param>
        /// <returns>True if a new workflow was started, otherwise false.</returns>
        public void RegisterWorkflow(Workflow workflow)
        {
            IWorkflowRegistrationService regService = ObjectFactory.GetInstance<IWorkflowRegistrationService>();
            regService.RegisterWorkflow(_workflowStore, workflow);
        }
    }
}
