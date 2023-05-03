using System.Collections.Generic;

namespace Utilities
{
	public static class Extensions
	{
		public static (int x, int y) GetCoordinates<T>(this List<List<T>> myList, T myObject)
		{
			int w = myList.Count;

			for (int x = 0; x < w; ++x)
			{
				int h = myList[x].Count;
				for (int y = 0; y < h; ++y)
				{
					if (myList[x][y].Equals(myObject))
						return (x, y);
				}
			}

			return (-1, -1);
		}
	}
}