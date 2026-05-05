using Fractural.Tasks;

public class CustomGetTargetsTrait() : FigureTrait
{
	public override async GDTask Activate(Figure figure)
	{
		await base.Activate(figure);
	}

	public override async GDTask Deactivate(Figure figure)
	{
		await base.Deactivate(figure);
	}
}