using System.Collections.Generic;
using SamEngine;
using System.Drawing;

namespace GooseShared
{
    // I know, the raising of events from outside the class isn't ideal. Consequence of writing in non-OO style in an OO language. Event subscription is nice tho.
    /// <summary>
    /// A directory of events you can subscribe to for executing code while the goose runs.
    /// </summary>
    public class InjectionPoints
    {
        // Mods
        public delegate void PostModLoadEventHandler();
        /// <summary> </summary>
        public static event PostModLoadEventHandler PostModsLoaded;
        public static void RaisePostModLoad() { PostModsLoaded?.Invoke(); }

        // PreTick
        public delegate void PreTickEventHandler(GooseEntity goose);
        /// <summary> Fires before the goose 'tick' function. </summary>
        public static event PreTickEventHandler PreTickEvent;
        public static void RaisePreTick(GooseEntity goose) { PreTickEvent?.Invoke(goose); }
        // PostTick
        public delegate void PostTickEventHandler(GooseEntity goose);
        /// <summary> Fires after the goose 'tick' function. </summary>
        public static event PostTickEventHandler PostTickEvent;
        public static void RaisePostTick(GooseEntity goose) { PostTickEvent?.Invoke(goose); }

        // PreUpdateRig
        public delegate void PreUpdateRigEventHandler(GooseEntity goose);
        /// <summary> Fires before the goose 'updateRig' function </summary>
        public static event PreUpdateRigEventHandler PreUpdateRigEvent;
        public static void RaisePreUpdateRig(GooseEntity goose) { PreUpdateRigEvent?.Invoke(goose); }
        // PostUpdateRig
        public delegate void PostUpdateRigEventHandler(GooseEntity goose);
        /// <summary> Fires after the goose's 'updateRig' function. </summary>
        public static event PostUpdateRigEventHandler PostUpdateRigEvent;
        public static void RaisePostUpdateRig(GooseEntity goose) { PostUpdateRigEvent?.Invoke(goose); }

        // PreRender
        public delegate void PreRenderEventHandler(GooseEntity goose, Graphics g);
        /// <summary> Fires during rendering, before the goose draws itself. </summary>
        public static event PreRenderEventHandler PreRenderEvent;
        public static void RaisePreRender(GooseEntity goose, Graphics g) { PreRenderEvent?.Invoke(goose, g); }
        // PostRender
        public delegate void PostRenderEventHandler(GooseEntity goose, Graphics g);
        /// <summary> Fires during rendering, after the goose draws itself. </summary>
        public static event PostRenderEventHandler PostRenderEvent;
        public static void RaisePostRender(GooseEntity goose, Graphics g) { PostRenderEvent?.Invoke(goose, g); }
    }
    
    // Mod entry points should implement this interface.
    /// <summary>
    /// Implemented by a mod's entry-point class.
    /// </summary>
    public interface IMod
    {
        // TODO: Make a "ModAPIFunctions" global that doesn't need to be passed in 'Init' - like Input.
        /// <summary>
        /// The function called as the mod is loaded, for the mod to initialize itself.
        /// </summary>
        void Init();
    }

    // This object contains references to a crapton of useful functions. Just don't modify its contents. Working on locking it down.
    /// <summary>
    /// This object contains references to a crapton of useful functions. Just don't modify its contents.
    /// </summary>
    public static class API
    {
        /// <summary> Goose-related helper functions. </summary>
        public static GooseFunctionPointers Goose;
        public class GooseFunctionPointers
        {
            /* Helpers */
            // Set the speed tier of the goose
            public delegate void SetSpeedFunction(GooseEntity g, GooseEntity.SpeedTiers tier);
            public SetSpeedFunction setSpeed;

            // Set the goose's target position to the closest offscreen position, weighted to the center of whatever screen edge it's on - return the chosen direction.
            public delegate ScreenDirection SetTargetOffscreenFunction(GooseEntity g, bool canExitTop = false);
            public SetTargetOffscreenFunction setTargetOffscreen;

            // Check if goose is at target
            public delegate bool IsGooseAtTargetFunction(GooseEntity g, float distanceToTrigger);
            public IsGooseAtTargetFunction isGooseAtTarget;

            // Check goose distance to target
            public delegate float GetDistanceToTargetFunction(GooseEntity g);
            public GetDistanceToTargetFunction getDistanceToTarget;

            /* Tasks */
            // Set the goose's current task by its ID
            public delegate void SetCurrentTaskByIDFunction(GooseEntity g, string id, bool honk = true);
            public SetCurrentTaskByIDFunction setCurrentTaskByID;

            // Set the goose's task to a random task
            public delegate void ChooseRandomTaskFunction(GooseEntity g);
            public ChooseRandomTaskFunction chooseRandomTask;

            // Set the current task to the default task
            public delegate void SetTaskToRoaming(GooseEntity g);
            public SetTaskToRoaming setTaskRoaming;

            /* Sound */
            // Play a honk sound
            public delegate void PlayHonckSoundFunction();
            public PlayHonckSoundFunction playHonckSound;


        }
        
        /// <summary> Helper functions for mods. </summary>
        public static ModHelperFunctions Helper;
        public class ModHelperFunctions
        {
            public delegate string GetModDirectoryFunction(IMod mod);
            /// <summary>
            /// Returns the specified mod's directory.
            /// </summary>
            public GetModDirectoryFunction getModDirectory;
        }

        /// <summary> For querying the TaskDatabase. </summary>
        public static TaskDatabaseQueryFunctions TaskDatabase;
        public class TaskDatabaseQueryFunctions
        {
            // Database Queries
            public delegate int GetTaskIndexByIDFunction(string id);
            /// <summary> Returns a Task's database index from its string ID. </summary>
            public GetTaskIndexByIDFunction getTaskIndexByID;

            public delegate string[] GetAllLoadedTaskIDsFunction(); // Get a list of task IDs
            /// <summary> Returns a list of all loaded Task's IDs. </summary>
            public GetAllLoadedTaskIDsFunction getAllLoadedTaskIDs;

            public delegate string GetNextRandomTaskFunction(); // Get a random task ID.
            /// <summary> Returns a random, randomly-pickable Task's ID. </summary>
            public GetNextRandomTaskFunction getRandomTaskID;
        }

    }

    #region Tasks/AI
    /*public interface GooseTaskDatabaseInterface
    {
        // Registering your task with the database
        void RegisterTask(GooseTaskInfo task, string id);

        // Database Queries
        GooseTaskInfo GetTask(int index); // Get a task info by its index.
        int GetTaskIndexByID(string id); // Get a task index by its string ID.
        int GetNextRandomTask(); // Get a random task index.
    }*/

    // This is a representation of all data required to create a task
    public interface GooseTaskData { }
    public abstract class GooseTaskInfo
    {
        public bool canBePickedRandomly = true;
        public string shortName = "Human-readable name here";
        public string description = "The person who programmed this forgot to give this task a description!";
        public string taskID = "";

        public abstract GooseTaskData GetNewTaskData(GooseEntity s);
        public abstract void RunTask(GooseEntity s);
    }
    #endregion

    #region Goose Entity Itself
    /// <summary>
    /// Describes the default set of colors and brushes the program uses to draw the goose.
    /// </summary>
    public class GooseRenderData
    {
        // Rendering stuff
        public Pen DrawingPen;
        public Bitmap shadowBitmap;
        public TextureBrush shadowBrush;
        public Pen shadowPen;
        public SolidBrush brushGooseWhite, brushGooseOrange, brushGooseOutline;
    }

    /// <summary>
    /// The main set of data for the goose!
    /// </summary>
    public class GooseEntity
    {
        public delegate void TickFunction(GooseEntity g);
        public TickFunction tick;
        public delegate void UpdateRigFunction(Rig rig, Vector2 centerPosition, float direction);
        public UpdateRigFunction updateRig;
        public delegate void RenderFunction(GooseEntity g, Graphics gfx);
        public RenderFunction render;

        /// <summary>
        /// The enum describing the speeds you can set in GooseFunctions.SetSpeed()
        /// </summary>
        public enum SpeedTiers { Walk, Run, Charge }

        /// <summary>
        /// The set of parameters the goose uses when animating, accelerating, and moving.
        /// </summary>
        public ParametersTable parameters;
        public class ParametersTable
        {
            // The maximum speeds in the goose's 3 default speed tiers.
            public float WalkSpeed = 80;
            public float RunSpeed = 200;
            public float ChargeSpeed = 400;
            
            public float AccelerationNormal = 1300; // The maximum acceleration in the goose's Walk and Run speed tiers.
            public float AccelerationCharged = 2300; // The maximum acceleration in the goose's Charged speed tier.

            public float StopRadius = -10f;
            
            public float StepTimeNormal = 0.2f; // The step interval in the goose's Walk and Run speed tiers.
            public float StepTimeCharged = 0.1f; // The step interval in the goose's Charged speed tier.

            public float DurationToTrackMud = 15f;
        }

        public GooseRenderData renderData;

        /// <summary> The current position. </summary>
        public Vector2 position = new Vector2(300, 300);
        /// <summary> The current velocity. </summary>
        public Vector2 velocity = new Vector2(0, 0);
        /// <summary> The current direction the goose is facing, in degrees </summary>
        public float direction = 90;
        /// <summary> The target direction of the goose, described as a unit vector. </summary>
        public Vector2 targetDirection;
        /// <summary> Override whether the goose is extending its neck - resets every frame. </summary>
        public bool extendingNeck;
        /// <summary> The target position. Set this point, and the goose will automatically locomote to it. </summary>
        public Vector2 targetPos = new Vector2(300, 300);
        /// <summary> Determines the current maximum speed. </summary>
        public float currentSpeed;
        /// <summary> Determines the current rate of acceleration. </summary>
        public float currentAcceleration;
        /// <summary> Determines the interval in seconds at which the goose's feets will step. </summary>
        public float stepInterval; // MOVE TO: Procedural feet?
        /// <summary> Determines whether the goose can decelerate immediately upon reaching its target location, or whether it will float around it. </summary>
        public bool canDecelerateImmediately = true;

        /// <summary> The time at which the goose should stop tracking mud </summary>
        // TODO: Move this into a location where it makes more sense?
        public float trackMudEndTime = -1;
        public FootMark[] footMarks = new FootMark[64];
        public int footMarkIndex = 0;

        /// <summary> The current location of all the goose's body parts, for rendering. </summary>
        public Rig rig = new Rig();

        /// <summary> The integer index of the currently running task in the TaskDatabase. </summary>
        public int currentTask = -1;
        /// <summary> The currently running task's dataset. </summary>
        public GooseTaskData currentTaskData;
        /// <summary> A queue of tasks to be selected from the Wandering state. If empty, we will pick a random task. </summary>
        public List<int> taskIndexQueue;

        public GooseEntity(TickFunction t, UpdateRigFunction ur, RenderFunction r)
        {
            tick = t;
            updateRig = ur;
            render = r;
        }
    }
    #endregion

    public class Rig
    {
        /* Feets */
        public ProceduralFeets feets;

        /* Under Body*/
        public const int UnderBodyRadius = 15;
        public const int UnderBodyLength = 7;
        public const int UnderBodyElevation = 9;
        public Vector2 underbodyCenter;

        /* Body */
        public const int BodyRadius = 22;
        public const int BodyLength = 11;
        public const int BodyElevation = 14;
        public Vector2 bodyCenter;

        /* Necc */
        // Properties
        public const int NeccRadius = 13;
        public const int NeccHeight1 = 20; // First Position
        public const int NeccExtendForward1 = 3;
        public const int NeccHeight2 = 10; // Second Position
        public const int NeccExtendForward2 = 16;
        public float neckLerpPercent;
        public Vector2 neckCenter;
        public Vector2 neckBase;
        public Vector2 neckHeadPoint;

        /* Head */
        public const int HeadRadius1 = 15;
        public const int HeadLength1 = 3;
        public const int HeadRadius2 = 10;
        public const int HeadLength2 = 5; // 7, or 5
        public Vector2 head1EndPoint;
        public Vector2 head2EndPoint;

        /* Eyes */
        public const int EyeRadius = 2;
        public const int EyeElevation = 3;
        public const float IPD = 5;
        public const float EyesForward = 5;
    }

    public partial class ProceduralFeets
    {
        public Vector2 lFootPos, rFootPos;
        public float lFootMoveTimeStart = -1, rFootMoveTimeStart = -1;
        public Vector2 lFootMoveOrigin, rFootMoveOrigin;
        public Vector2 lFootMoveDir, rFootMoveDir;

        public int feetDistanceApart = 6;
        public const float wantStepAtDistance = 5;
        public const float overshootFraction = 0.4f;
    }
    

    #region Random assorted things that don't fit anywhere

    public enum ScreenDirection { Left, Top, Right }

    // Representation of a foot mark particle
    public struct FootMark
    {
        public const float ShrinkTime = 1f;
        public const float Lifetime = 8.5f;
        public Vector2 position;
        public float time;
    }
    #endregion
}