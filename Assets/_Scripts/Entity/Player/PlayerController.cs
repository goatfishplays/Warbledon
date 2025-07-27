using FishNet.Managing;
using FishNet.Managing.Server;
using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Transporting;
using GameKit.Dependencies.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{

    public struct ReplicateData : IReplicateData
    {
        public Vector2 PlanarInputs;
        public bool Sprinting;

        public ReplicateData(Vector2 planarInputs, bool sprinting) : this()
        {
            PlanarInputs = planarInputs;
            Sprinting = sprinting;
        }

        private uint _tick;
        public void Dispose() { }
        public uint GetTick() => _tick;
        public void SetTick(uint value) => _tick = value;
    }

    public struct ReconcileData : IReconcileData
    {
        // Prediction RB is used to sync states and forces automatically
        public PredictionRigidbody2D predRB;

        public ReconcileData(PredictionRigidbody2D pr) : this()
        {
            predRB = pr;
        }

        private uint _tick;
        public void Dispose() { }
        public uint GetTick() => _tick;
        public void SetTick(uint val) => _tick = val;
    }


    public Entity entity;
    public PredictionRigidbody2D predRB;
    public bool sprintApplied = false; // TODO rn only have this because currently sprint adding and removing uses hashtable and checking that every tick sounds bad, might also wanna change this to a buffer of like 2 ticks

    /**
    Cache these because will run every tick
    **/

    private InputAction movementAction;
    private InputAction sprintAction;

    private ReplicateData _lastCreatedInputs = default;

    [Header("Sprint")]
    [SerializeField] private float sprintMult = 1.75f;
    private const string SPRINT_SPEED_MULT_ID = "Sprint";
    private PlayerManager playerManager => PlayerManager.instance;


    public override void OnStartClient()
    {
        predRB = ObjectCaches<PredictionRigidbody2D>.Retrieve();
        predRB.Initialize(entity.entityMovement.RB_FOR_READING_ONLY);

        if (IsOwner)
        {
            playerManager.Initalize(this);
            // TurnController tc = GetComponent<TurnController>();
            // tc.displayName = GameControl.username;
            // GameControlWithRequests.instance.SendServerTeamJoinReq(tc.GetComponent<NetworkObject>(), GameControl.username); // TODO fix this also broken

            movementAction = playerManager.movementAction;
            sprintAction = playerManager.shiftAction;
        }
        base.TimeManager.OnTick += TimeManager_OnTick;
        base.TimeManager.OnPostTick += TimeManager_OnPostTick;


        entity.rb = predRB;
        entity.entityMovement.rb = predRB;
    }

    public override void OnStopClient()
    {
        if (IsOwner)
        {
            PlayerManager.instance.UnInitalize();


        }
        base.TimeManager.OnTick -= TimeManager_OnTick;
        base.TimeManager.OnPostTick -= TimeManager_OnPostTick;

        ObjectCaches<PredictionRigidbody2D>.StoreAndDefault(ref predRB);
    }

    private void TimeManager_OnTick()
    {
        RunInputs(CreateReplicateData());
    }

    private void TimeManager_OnPostTick()
    {
        CreateReconcile();
    }

    private ReplicateData CreateReplicateData()
    {
        if (!base.IsOwner)
        {
            return default;
        }


        ReplicateData data = new ReplicateData(movementAction.ReadValue<Vector2>(), sprintAction.ReadValue<float>() > 0);

        return data;
    }

    [Replicate]
    private void RunInputs(ReplicateData data, ReplicateState state = ReplicateState.Invalid, Channel channel = Channel.Unreliable)
    {
        // if (!IsOwner && !IsServerInitialized)
        // {
        if (state.IsFuture()) // Will only be predicting up to 2 ticks in the future
        {
            uint lastCreatedTick = _lastCreatedInputs.GetTick();

            uint thisTick = data.GetTick();

            if (thisTick - lastCreatedTick <= 2)
            {
                data.Sprinting = _lastCreatedInputs.Sprinting;
                data.PlanarInputs = _lastCreatedInputs.PlanarInputs;
            }
        }
        else if (state.IsReplayedCreated())
        {
            _lastCreatedInputs.Dispose();
            _lastCreatedInputs = data;
        }
        // }


        if (!sprintApplied && data.Sprinting)
        {
            StartSprint();
        }
        else if (sprintApplied && !data.Sprinting)
        {
            StopSprint();
        }


        entity.entityMovement.Move(data.PlanarInputs);
        // entity.entityMovement.rb.Velocity(data.PlanarInputs * 12);

        predRB.Simulate();
    }

    public override void CreateReconcile()
    {
        ReconcileData rd = new ReconcileData(predRB);

        ReconcileState(rd);
    }

    [Reconcile]
    private void ReconcileState(ReconcileData data, Channel channel = Channel.Unreliable)
    {
        predRB.Reconcile(data.predRB);
    }


    private void StartSprint()
    {
        entity.entityMovement.AddTargetSpeedMult(SPRINT_SPEED_MULT_ID, sprintMult);
        sprintApplied = true;
    }
    private void StopSprint()
    {
        entity.entityMovement.RemoveTargetSpeedMult(SPRINT_SPEED_MULT_ID);
        sprintApplied = false;
    }





}
