using Xunit;
using SpaceBattle.Lib;
using SpaceBattle.Lib.Command;
using SpaceBattle.Lib.Interfaces;
using System.Linq;

namespace SpaceBattle.Tests;

public class SpatialGridTests
{
    [Fact]
    public void Add_And_GetNearby_ReturnsCorrectNeighbors()
    {
        var grid = new SpatialGrid(10.0);
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();

        grid.Add(id1, new Vector(5, 5));
        grid.Add(id2, new Vector(15, 5));
        grid.Add(id3, new Vector(100, 100));

        var nearby1 = grid.GetNearby(id1, new Vector(5, 5), 10.0).ToList();
        Assert.Contains(id2, nearby1);
        Assert.DoesNotContain(id3, nearby1);
        Assert.DoesNotContain(id1, nearby1); // сам объект не возвращается
    }

    [Fact]
    public void Update_MovesObjectToNewCell()
    {
        var grid = new SpatialGrid(10.0);
        var id = Guid.NewGuid();
        grid.Add(id, new Vector(5, 5));

        // Обновляем позицию
        grid.Update(id, new Vector(5, 5), new Vector(25, 5));

        // Добавляем другой объект в новую позицию
        var id2 = Guid.NewGuid();
        grid.Add(id2, new Vector(25, 5));

        // Объекты id и id2 должны оказаться в одной ячейке
        var nearbyId2 = grid.GetNearby(id2, new Vector(25, 5), 5.0).ToList();
        Assert.Contains(id, nearbyId2); // id теперь сосед id2

        // В старой ячейке (5,5) объект id отсутствует
        var id3 = Guid.NewGuid();
        grid.Add(id3, new Vector(5, 5));
        var nearbyId3 = grid.GetNearby(id3, new Vector(5, 5), 5.0).ToList();
        Assert.DoesNotContain(id, nearbyId3);
    }

    [Fact]
    public void Remove_DeletesObjectFromGrid()
    {
        var grid = new SpatialGrid(10.0);
        var id = Guid.NewGuid();
        grid.Add(id, new Vector(5, 5));

        // Удаляем id
        grid.Remove(id, new Vector(5, 5));

        // Добавляем другой объект в ту же позицию
        var id2 = Guid.NewGuid();
        grid.Add(id2, new Vector(5, 5));

        // Проверяем, что id больше не сосед для id2
        var nearby = grid.GetNearby(id2, new Vector(5, 5), 5.0).ToList();
        Assert.DoesNotContain(id, nearby);
        Assert.DoesNotContain(id2, nearby); // себя не содержит
        Assert.Empty(nearby); // других объектов нет

        // Дополнительная проверка: id2 присутствует в сетке
        var id3 = Guid.NewGuid();
        grid.Add(id3, new Vector(5, 5));
        var nearbyId3 = grid.GetNearby(id3, new Vector(5, 5), 5.0).ToList();
        Assert.Contains(id2, nearbyId3);
    }
}
