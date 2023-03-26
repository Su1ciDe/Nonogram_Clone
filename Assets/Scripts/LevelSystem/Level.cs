namespace LevelSystem
{
	public class Level
	{
		public bool[][] Cells;

		public Level(bool[][] cells)
		{
			Cells = (bool[][])cells.Clone();
		}
	}
}