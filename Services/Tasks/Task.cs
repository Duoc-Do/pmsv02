using System;
using System.Diagnostics;
using WebApp.Models;
using System.Linq;
using System.Linq.Dynamic;

using WebApp.Services.Logging;
//using Nop.Core.Domain.Tasks;
//using Nop.Core.Infrastructure;
//using Nop.Services.Logging;


namespace WebApp.Services.Tasks
{
    /// <summary>
    /// Task
    /// </summary>
    public partial class Task
    {
        private bool _enabled;
        private readonly string _type;
        private readonly string _name;
        private readonly bool _stopOnError;
        private DateTime? _lastStartUtc;
        private DateTime? _lastSuccessUtc;
        private DateTime? _lastEndUtc;
        private bool _isRunning;

        /// <summary>
        /// Ctor for Task
        /// </summary>
        private Task()
        {
            this._enabled = true;
        }

        /// <summary>
        /// Ctor for Task
        /// </summary>
        /// <param name="task">Task </param>
        public Task(SenScheduleTask task)
        {
            this._type = task.Type;
            this._enabled = task.Enabled;
            this._stopOnError = task.StopOnError;
            this._name = task.Name;
        }
    

        private ITask CreateTask()
        {
            //ITask task = null;
            //if (this.Enabled)
            //{
            //    var type2 = System.Type.GetType(this.Type);
            //    if (type2 != null)
            //    {
            //        object instance;
            //        if (!TryResolve(type2, out instance))
            //        {
            //            //not resolved
            //            instance =ResolveUnregistered(type2);
            //        }
            //        task = instance as ITask;
            //    }
            //}
            //return task;

            ITask task = null;
            if (this.Enabled)
            {
                var type2 = System.Type.GetType(this._type);
                if (type2 != null)
                {
                    object instance = Activator.CreateInstance(type2);
                    if (instance != null)
                    {
                        task = instance as ITask;
                    }
                }
            }

            return task;
        }

        /// <summary>
        /// Executes the task
        /// </summary>
        public void Execute()
        {
            this._isRunning = true;
            try
            {
                var task = this.CreateTask();
                if (task != null)
                {
                    this._lastStartUtc = DateTime.UtcNow;
                    task.Execute();
                    this._lastEndUtc = this._lastSuccessUtc = DateTime.UtcNow;
                }
            }
            catch (Exception exc)
            {
                this._enabled = !this.StopOnError;
                this._lastEndUtc = DateTime.UtcNow;

                ////log error
                ILogger logger = new WebApp.Services.Logging.DefaultLogger();
                logger.Error(string.Format("Error while running the '{0}' schedule task. {1}", this._name, exc.Message), exc);
            }

            try
            {
                //find current schedule task
                //var scheduleTaskService = EngineContext.Current.Resolve<IScheduleTaskService>();
                var db = new Models.SenContext();
                var scheduleTask = db.SenScheduleTasks.Where(m => m.Type == this._type).SingleOrDefault();
                //scheduleTaskService.GetTaskByType(this._type);
                if (scheduleTask != null)
                {
                    scheduleTask.LastStartUtc = this.LastStartUtc;
                    scheduleTask.LastEndUtc = this.LastEndUtc;
                    scheduleTask.LastSuccessUtc = this.LastSuccessUtc;
                    
                    //SenViet.GlobeVariant.sysentities.Attach(
                    //scheduleTaskService.UpdateTask(scheduleTask);
                    //SenViet.GlobeVariant.sysentities.ObjectStateManager.ChangeObjectState(scheduleTask, System.Data.EntityState.Modified);

                    db.Entry(scheduleTask).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(string.Format("Error saving schedule task datetimes. Exception: {0}", exc));
            }
            this._isRunning = false;
        }

        /// <summary>
        /// A value indicating whether a task is running
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return this._isRunning;
            }
        }

        /// <summary>
        /// Datetime of the last start
        /// </summary>
        public DateTime? LastStartUtc
        {
            get
            {
                return this._lastStartUtc;
            }
        }

        /// <summary>
        /// Datetime of the last end
        /// </summary>
        public DateTime? LastEndUtc
        {
            get
            {
                return this._lastEndUtc;
            }
        }

        /// <summary>
        /// Datetime of the last success
        /// </summary>
        public DateTime? LastSuccessUtc
        {
            get
            {
                return this._lastSuccessUtc;
            }
        }

        /// <summary>
        /// A value indicating type of the task
        /// </summary>
        public string Type
        {
            get
            {
                return this._type;
            }
        }

        /// <summary>
        /// A value indicating whether to stop task on error
        /// </summary>
        public bool StopOnError
        {
            get
            {
                return this._stopOnError;
            }
        }

        /// <summary>
        /// Get the task name
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
        }

        /// <summary>
        /// A value indicating whether the task is enabled
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this._enabled;
            }
        }
    }
}
