using GooseShared;
using SamEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetGoose
{
    class RunToBedTask : GooseTaskInfo
    {
        public RunToBedTask()
        {
            // Should this Task be picked at random by the goose?
            canBePickedRandomly = false;

            // Describes the task, for readability by other interfaces.
            shortName = "Run to the Bed";
            description = "This task makes the goose run to his bed, target position set in Main";

            // The key used to access this task in the GooseTaskDatabase:
            // "Task.GetTaskByID" in the API takes this as an argument.
            taskID = "RunToBed";
            // Hot tip: can be nice to set this from a public constant string. Easier access by other parts of your mod.
        }

        public class RunToBedTaskData : GooseTaskData
        {
            public float timeStarted;
        }

        public override GooseTaskData GetNewTaskData(GooseEntity goose)
        {
            RunToBedTaskData taskData = new RunToBedTaskData();
            taskData.timeStarted = Time.time;
            return taskData;
        }

        public override void RunTask(GooseEntity goose)
        {
            RunToBedTaskData data = (RunToBedTaskData)goose.currentTaskData;

            API.Goose.setSpeed(goose, GooseEntity.SpeedTiers.Run);
        }
    }
}
