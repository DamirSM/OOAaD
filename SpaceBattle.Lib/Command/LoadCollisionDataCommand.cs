using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using SpaceBattle.Lib;

namespace SpaceBattle.Lib.Command;

public class LoadCollisionDataCommand : ICommand
{
    private class CircleDto
    {
        public int[] Center { get; set; }
        public double Radius { get; set; }
    }

    private class CollisionDataDto
    {
        public string Type { get; set; }
        public List<CircleDto> LocalCircles { get; set; }
    }

    public void Execute()
    {
        string json = File.ReadAllText("collision_data.json");
        var data = JsonSerializer.Deserialize<List<CollisionDataDto>>(json);
        if (data == null) throw new InvalidOperationException("Failed to load collision data");

        var collisionDataList = new List<CollisionData>();
        foreach (var dto in data)
        {
            var circles = new List<Circle>();
            foreach (var circleDto in dto.LocalCircles)
            {
                var center = new Vector(circleDto.Center);
                circles.Add(new Circle(center, circleDto.Radius));
            }
            collisionDataList.Add(new CollisionData { Type = dto.Type, LocalCircles = circles });
        }

        Ioc.Resolve<ICommand>("IoC.Register", "Collision.Shape",
            (Func<object[], object>)(args =>
            {
                string type = (string)args[0];
                var circles = collisionDataList.First(d => d.Type == type).LocalCircles;
                return circles.AsReadOnly();
            })
        ).Execute();
    }
}