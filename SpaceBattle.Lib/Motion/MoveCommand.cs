namespace SpaceBattle.Lib;

public class MoveCommand : ICommand
{
    private readonly IMovingObject _movingObject;

    public MoveCommand(IMovingObject movingObject)
    {
        _movingObject = movingObject;
    }

    public void Execute()
    {
        try
        {
            if (_movingObject.Position == null)
            {
                throw new InvalidOperationException("Cannot get position");
            }

            if (_movingObject.Velocity == null)
            {
                throw new InvalidOperationException("Cannot get velocity");
            }
            var newPosition = _movingObject.Position + _movingObject.Velocity;
            _movingObject.Position = newPosition;
        }

        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
}
