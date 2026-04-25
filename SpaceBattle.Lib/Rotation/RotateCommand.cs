namespace SpaceBattle.Lib;

public class RotateCommand : ICommand
{
    private readonly IRotatingObject RotatingObject;

    public RotateCommand(IRotatingObject rotatingObject)
    {
        RotatingObject = rotatingObject;
    }

    public void Execute()
    {
        if (RotatingObject.Angle == null)
        {
            throw new InvalidOperationException("Cannot get angle");
        }

        if (RotatingObject.AngularVelocity == null)
        {
            throw new InvalidOperationException("Cannot get angular velocity");
        }

        try
        {
            RotatingObject.Angle = RotatingObject.Angle + RotatingObject.AngularVelocity;
        }
        catch
        {
            throw new InvalidOperationException("Cannot change angle");
        }
    }
}
