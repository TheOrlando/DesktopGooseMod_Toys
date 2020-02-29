using GooseShared;
using SamEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetGoose
{
    class ChargeToStickTask : GooseTaskInfo
    {
        public ChargeToStickTask()
        {
            // Should this Task be picked at random by the goose?
            canBePickedRandomly = true;

            // Describes the task, for readability by other interfaces.
            shortName = "10 Second Charge to Stick";
            description = "This task makes the goose charge for 10 seconds, used with the stick but the target location is set in Main";

            // The key used to access this task in the GooseTaskDatabase:
            // "Task.GetTaskByID" in the API takes this as an argument.
            taskID = "ChargeToStick";
            // Hot tip: can be nice to set this from a public constant string. Easier access by other parts of your mod.
        }

        public class ChargeToStickTaskData : GooseTaskData
        {
            public float timeStarted;
        }

        public override GooseTaskData GetNewTaskData(GooseEntity goose)
        {
            ChargeToStickTaskData taskData = new ChargeToStickTaskData();
            taskData.timeStarted = Time.time;
            return taskData;
        }

        public override void RunTask(GooseEntity goose)
        {
            ChargeToStickTaskData data = (ChargeToStickTaskData)goose.currentTaskData;

            API.Goose.setSpeed(goose, GooseEntity.SpeedTiers.Charge);

            if (Time.time - data.timeStarted > 10)
            {
                API.Goose.setSpeed(goose, GooseEntity.SpeedTiers.Walk);
                API.Goose.setTaskRoaming(goose);
            }
        }
    }
}
