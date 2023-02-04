using UnityEngine;
using UnityEngine.Animations;

public class Weapon : MonoBehaviour {
    [SerializeField]
    protected float _damage = 1, _speed = 5;

    protected Collider2D _collider;
    protected ParentConstraint _constraint;

    public void SetUp(Tomba tomba) {
        //transform.position = startPosition;
        /*_constraint = GetComponent<ParentConstraint>();
        ConstraintSource playerConstrait = new ConstraintSource();
        playerConstrait.weight = 1;
        playerConstrait.sourceTransform = tomba.transform;
        _constraint.AddSource(playerConstrait);
        _constraint.locked = true;
        _constraint.constraintActive = true;
        _constraint.translationAxis = Axis.X | Axis.Y | Axis.Z;*/

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //check who it collided with if damagebel entity -> deal damage/hit sound
        //if wall -> play wall hit effect/sound
    }
}
