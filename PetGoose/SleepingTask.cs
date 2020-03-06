using GooseShared;
using SamEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetGoose
{
    class SleepingTask : GooseTaskInfo
    {
        public SleepingTask()
        {
            // Should this Task be picked at random by the goose?
            canBePickedRandomly = false;

            // Describes the task, for readability by other interfaces.
            shortName = "Sleep";
            description = "This task makes the goose sleep";

            // The key used to access this task in the GooseTaskDatabase:
            // "Task.GetTaskByID" in the API takes this as an argument.
            taskID = "Sleeping";
            // Hot tip: can be nice to set this from a public constant string. Easier access by other parts of your mod.
        }

        public class SleepingTaskData : GooseTaskData
        {
            public float timeStarted;
        }

        public override GooseTaskData GetNewTaskData(GooseEntity goose)
        {
            SleepingTaskData taskData = new SleepingTaskData();
            taskData.timeStarted = Time.time;
            return taskData;
        }

        public override void RunTask(GooseEntity goose)
        {

        }
    }
}
