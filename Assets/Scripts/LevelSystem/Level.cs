using Utility;

namespace LevelSystem
{
	public class Level
	{
		public bool IsProcedural;
		public int LevelNo;
		public Difficulty Difficulty;
		public bool[][] Cells;

		public Level(bool[][] cells, int levelNo, bool isProcedural, Difficulty difficulty = Difficulty.Easy)
		{
			Cells = (bool[][])cells.Clone();
			LevelNo = levelNo;
			IsProcedural = isProcedural;
			Difficulty = difficulty;
		}
	}
}