namespace DefaultNamespace
{
    public interface IMovementBehavior
    {
        void Move();
        void ReverseDirection();
        bool CheckIfCanCollide();
        float GetVelocityMagnitude();
    }
}