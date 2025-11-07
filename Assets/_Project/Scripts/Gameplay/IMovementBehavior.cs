namespace DefaultNamespace
{
    public interface IMovementBehavior
    {
        void Move();
        void ReverseDirection();
        float GetVelocityMagnitude();
        void RandomlyRotateDirection();
        
        //Maybe there should be more ways to control movement outside, for example, to control what happens on collision
        void OnInterrupted();
    }
}