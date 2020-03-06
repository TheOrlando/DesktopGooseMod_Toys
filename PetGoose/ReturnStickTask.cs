using GooseShared;
using SamEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetGoose
{
    class ReturnStickTask : GooseTaskInfo
    {
        public ReturnStickTask()
        {
            // Should this Task be picked at random by the goose?
            canBePickedRandomly = false;

            // Describes the task, for readability by other interfaces.
            shortName = "10 Second Charge to Mouse with Stick";
            description = "This task makes the goose charge for 10 seconds to the mouse, used with the stick but the target location is set in Main";

            // The key used to access this task in the GooseTaskDatabase:
            // "Task.GetTaskByID" in the API takes this as an argument.
            taskID = "ReturnStick";
            // Hot tip: can be nice to set this from a public constant string. Easier access by other parts of your mod.
        }

        public class ReturnStickTaskData : GooseTaskData
        {
            public float timeStarted;
        }

        public override GooseTaskData GetNewTaskData(GooseEntity goose)
        {
            ReturnStickTaskData taskData = new ReturnStickTaskData();
            taskData.timeStarted = Time.time;
            return taskData;
        }

        public override void RunTask(GooseEntity goose)
        {
            ReturnStickTaskData data = (ReturnStickTaskData)goose.currentTaskData;

            API.Goose.setSpeed(goose, GooseEntity.SpeedTiers.Charge);

            if (Time.time - data.timeStarted > 10)
            {
                API.Goose.setSpeed(goose, GooseEntity.SpeedTiers.Walk);
                API.Goose.setTaskRoaming(goose);
            }
        }
    }
}
