using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public enum TombaStateType {
    Idle,
    Walk,
    Run,
    Dash,
    Turn,
    Jump,
    Fall,
    OnWallDown,
    OnWallUp,
    OnWallIdle,
    OnLedge,
    EdgeClimb,
    HitAir,
    HitDie,
    HitGround,
    HitRecovery,
    StartFall,
    None
}

public class TombaStateFactory
{
    private readonly IEnumerable<Type> _stateTypes;
    private List<TombaState> _states;
    private Tomba _context;

    public TombaStateFactory(Tomba context) {
        _context = context;
        _states = new List<TombaState>();
        Assembly currentAssembly = Assembly.GetExecutingAssembly();

        _stateTypes = currentAssembly.GetTypes()
            .Where(t => typeof(TombaState).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var item in Enum.GetValues(typeof(TombaStateType)).Cast<TombaStateType>()) {
            TombaState state = Create(item);
            if(state != null) {
                _states.Add(state);
            }
            
        }
    }

    private TombaState Create(TombaStateType stateType) {
        return _stateTypes
            .Select(type => CreateSpecific(type, stateType))
            .FirstOrDefault((stateType => stateType != null));
    }

    private TombaState CreateSpecific(Type type, TombaStateType stateType) {
        TombaState stateInstance = (TombaState)Activator.CreateInstance(type, args:_context); ;

        return stateInstance.Is(stateType) ? stateInstance : null;
    }

    public TombaState GetState(TombaStateType stateType) {
        foreach (var item in _states) {
            if (item.Is(stateType)) return item;
        }
        throw new Exception("[TombaStateFactory]: State '"+ stateType.ToString() +"' Not Found");
    }
}
