public class MapGeneratorCell
{
    public int X { get; set; }
    public int Y { get; set; }
}

public class MapGenerator
{
    private int _width = 15;
    private int _height = 11;
    public int WidthOffset { get; set; }
    public int HeightOffset { get; set; }

    public MapGeneratorCell[,] GenerateMap()
    {
        MapGeneratorCell[,] map = new MapGeneratorCell[_width, _height];

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                map[x, y] = new MapGeneratorCell { X = x - WidthOffset, Y = y - HeightOffset };
            }
        }

        return map;
    }
}
