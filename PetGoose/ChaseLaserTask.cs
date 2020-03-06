using GooseShared;
using SamEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetGoose
{
    class ChaseLaserTask : GooseTaskInfo
    {
        public ChaseLaserTask()
        {
            // Should this Task be picked at random by the goose?
            canBePickedRandomly = false;

            // Describes the task, for readability by other interfaces.
            shortName = "Charge to the laser";
            description = "This task makes the goose charge to the laser, the target location is set in Main";

            // The key used to access this task in the GooseTaskDatabase:
            // "Task.GetTaskByID" in the API takes this as an argument.
            taskID = "ChaseLaser";
            // Hot tip: can be nice to set this from a public constant string. Easier access by other parts of your mod.
        }

        public class ChaseLaserTaskData : GooseTaskData
        {
            public float timeStarted;
        }

        public override GooseTaskData GetNewTaskData(GooseEntity goose)
        {
            ChaseLaserTaskData taskData = new ChaseLaserTaskData();
            taskData.timeStarted = Time.time;
            return taskData;
        }

        public override void RunTask(GooseEntity goose)
        {
            ChaseLaserTaskData data = (ChaseLaserTaskData)goose.currentTaskData;

            API.Goose.setSpeed(goose, GooseEntity.SpeedTiers.Charge);
        }
    }
}
