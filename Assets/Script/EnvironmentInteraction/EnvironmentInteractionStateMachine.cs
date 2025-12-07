using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;

public class EnvironmentInteractionStateMachine : StateManager<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
    public enum EEnvironmentInteractionState
    {
        Search,
        Approach,
        Rise,
        Touch,
        Reset
    }

    private EnvironmentInteractionContext _context;

    [SerializeField] private TwoBoneIKConstraint _leftIkConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIkConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiRotationConstraint;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _rootCollider;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (_context != null && _context.ClosestPointOnColliderFromShoulder != null)
        {
            Gizmos.DrawSphere(_context.ClosestPointOnColliderFromShoulder, .03f);
        }
    }
    void Awake()
    {
        ValidateConstraints();

        _context = new EnvironmentInteractionContext(_leftIkConstraint, _rightIkConstraint, 
            _leftMultiRotationConstraint, _rightMultiRotationConstraint, _rigidbody, _rootCollider, transform.root);
        InitializeState();
        ConstructEnvironmentDetectionCollider();
    }
    private void ValidateConstraints()
    {
        Assert.IsNotNull(_leftIkConstraint, "Left IK constraint is not assigned.");
        Assert.IsNotNull(_rightIkConstraint, "Right IK constraint is not assigned.");
        Assert.IsNotNull(_leftMultiRotationConstraint, "Left multi-rotation constraint is not assigned.");
        Assert.IsNotNull(_rightMultiRotationConstraint, "Right multi-rotation constraint is not assigned.");
        Assert.IsNotNull(_rigidbody, "Rigidbody used to control character is not assigned.");
        Assert.IsNotNull(_rootCollider, "RootCollider attached to character is not assigned.");
    }
    private void InitializeState()
    {
        // Add States to inherited StateManager "States" dictionary and set Initial State
        States.Add(EEnvironmentInteractionState.Reset, new ResetState(_context, EEnvironmentInteractionState.Reset));
        States.Add(EEnvironmentInteractionState.Touch, new TouchState(_context, EEnvironmentInteractionState.Touch));
        States.Add(EEnvironmentInteractionState.Rise, new RiseState(_context, EEnvironmentInteractionState.Rise));
        States.Add(EEnvironmentInteractionState.Approach, new ApproachState(_context, EEnvironmentInteractionState.Approach));
        States.Add(EEnvironmentInteractionState.Search, new SearchState(_context, EEnvironmentInteractionState.Search));
        CurrentState = States[EEnvironmentInteractionState.Reset];
    }
    private void ConstructEnvironmentDetectionCollider()
    {
        float wingspan = _rootCollider.height;

        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(wingspan, wingspan, wingspan);
        boxCollider.center = new Vector3(_rootCollider.center.x, _rootCollider.center.y + (.25f * wingspan), _rootCollider.center.z + (.5f * wingspan));
        boxCollider.isTrigger = true;

        _context.ColliderCenterY = _rootCollider.center.y;
    }
}