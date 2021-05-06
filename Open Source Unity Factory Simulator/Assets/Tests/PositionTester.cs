using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PositionTester
{
    [Test]
    public void TestDistance()
    {
        // Arrange
        var a = new Position(0, 0);
        var b = new Position(5, 0);
        var c = new Position(3, 1);

        // Act
        var dist1 = Position.Distance(a, b);
        var dist2 = Position.Distance(a, c);


        // Assert
        Assert.AreEqual(5, dist1);
        Assert.AreEqual(4, dist2);
    }
    
    [Test]
    public void TestMoveTowards()
    {
        // Arrange
        var a = new Position(0, 0);
        var b = new Position(5, 0);
        var c = new Position(3, 1);


        // Assert
        Assert.AreEqual(new Position(1,0), Position.MoveTowards(a, b,1));
        Assert.AreEqual(new Position(2,0), Position.MoveTowards(a, b,2));
        Assert.AreEqual(new Position(5,0), Position.MoveTowards(a, b,Position.Distance(a,b)));
        Assert.AreEqual(new Position(1,1), Position.MoveTowards(a, c,1));
        Assert.AreEqual(new Position(2,1), Position.MoveTowards(a, c, 2));
        Assert.AreEqual(new Position(3,1), Position.MoveTowards(a, c, Position.Distance(a,c)));
    }
    
    [Test]
    public void TestCardinalDirection()
    {
        // Arrange
        var a = new Position(0, 0);
        var b = new Position(0, 5);
        var c = new Position(1, 0);
        var d = new Position(0, -3);
        var e = new Position(-2, 0);
        var f = new Position(0, 0);

        // Act
        var card1 = Position.CardinalDirection(a, f);
        var card2 = Position.CardinalDirection(a, b);
        var card3 = Position.CardinalDirection(a, c);
        var card4 = Position.CardinalDirection(a, d);
        var card5 = Position.CardinalDirection(a, e);
        var card6 = Position.CardinalDirection(b, d);
        var card7 = Position.CardinalDirection(c, e);


        // Assert
        Assert.AreEqual(0, card1);
        Assert.AreEqual(1, card2);
        Assert.AreEqual(2, card3);
        Assert.AreEqual(3, card4);
        Assert.AreEqual(4, card5);
        Assert.AreEqual(3, card6);
        Assert.AreEqual(4, card7);
    }
    
}
