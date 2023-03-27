namespace LevelSystem
{
	public class Level
	{
		public int LevelNo;
		public bool[][] Cells;

		public Level(bool[][] cells, int levelNo)
		{
			Cells = (bool[][])cells.Clone();
			LevelNo = levelNo;
		}
	}
}