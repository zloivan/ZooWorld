namespace DefaultNamespace
{
    public interface IMovementBehavior
    {
        void Move();
        void ReverseDirection();
        float GetVelocityMagnitude();
        void RandomlyRotateDirection();
    }
}