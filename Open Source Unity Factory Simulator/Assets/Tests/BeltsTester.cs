using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BeltTester
{
    
    public bool BeltSegmentEqualityChecker(List<Belt.BeltSegment> first, List<Belt.BeltSegment> second) {

        if (first.Count == second.Count) {
            for (int i = 0; i < first.Count; i++) {
                if (first[i].item != second[i].item) {
                    return false;
                }
                if (first[i].count != second[i].count) {
                    return false;
                }
            }
        } else {
            return false;
        }

        return true;
    }

    [Test]
    public void BeltSegmentEqualityCheckerTest() {
        var beltsegment1 = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 3, item = new Item() .SetEmpty()}
        };
        var beltsegment2 = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 3, item = new Item() .SetEmpty()}
        };
        var beltsegment3 = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 3, item = new Item() .SetEmpty()},
            new Belt.BeltSegment() {count = 2, item = new Item() {itemId = 1}},
        };
        var beltsegment4 = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 3, item = new Item() .SetEmpty()},
            new Belt.BeltSegment() {count = 2, item = new Item() {itemId = 1}}
        };
        var beltsegment5 = new List<Belt.BeltSegment>() {
        };
        var beltsegment6 = new List<Belt.BeltSegment>() {
        };
        
        Assert.IsTrue(BeltSegmentEqualityChecker(beltsegment1, beltsegment2));
        Assert.IsTrue(BeltSegmentEqualityChecker(beltsegment3, beltsegment4));
        Assert.IsTrue(BeltSegmentEqualityChecker(beltsegment5, beltsegment6));
        Assert.IsFalse(BeltSegmentEqualityChecker(beltsegment1, beltsegment3));
        Assert.IsFalse(BeltSegmentEqualityChecker(beltsegment2, beltsegment4));
        Assert.IsFalse(BeltSegmentEqualityChecker(beltsegment1, beltsegment5));
    }
    
    [Test]
    public void TestBeltCreation()
    {
        // Arrange
        var belt1 = new Belt(new Position(0, 0), new Position(2, 0));
        
        // Assert
        var correct = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 3, item = new Item() .SetEmpty()}
        };
        
        Assert.IsTrue(BeltSegmentEqualityChecker(belt1.items, correct));
    }
    
    
    [Test]
    public void TestBeltInsertItem()
    {
        // Arrange
        int numberOfCases = 8;
        var belts = new Belt[numberOfCases];
        
        belts[0] = new Belt(new Position(0, 0), new Position(2, 0));
        belts[1] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 2, item = new Item() {itemId = 1}},
            }
        };
        belts[2] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
                new Belt.BeltSegment() {count = 2, item = new Item() .SetEmpty()},
            }
        };
        belts[3] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 2, item = new Item() {itemId = 5}},
            }
        };
        belts[4] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
            }
        };
        belts[5] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 2, item = new Item() {itemId = 1}},
            }
        };
        belts[6] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
            }
        };
        belts[7] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item(){itemId = 1}},
            }
        };

        // Act
        var results = new bool[numberOfCases];
        
        results[0] = belts[0].TryInsertItemToBelt(new Item() {itemId = 1});
        results[1] = belts[1].TryInsertItemToBelt(new Item() {itemId = 1});
        results[2] = belts[2].TryInsertItemToBelt(new Item() {itemId = 1});
        results[3] = belts[3].TryInsertItemToBelt(new Item() {itemId = 2});
        results[4] = belts[4].TryInsertItemToBelt(new Item() {itemId = 1});
        results[5] = belts[5].TryInsertItemToBelt(new Item() .SetEmpty());
        results[6] = belts[6].TryInsertItemToBelt(new Item(){itemId = 1});
        results[7] = belts[7].TryInsertItemToBelt(new Item() {itemId = 1});
        


        // Assert
        var corrects = new List<Belt.BeltSegment>[numberOfCases];
        corrects[0] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            new Belt.BeltSegment() {count = 2, item = new Item() .SetEmpty()},
        };
        corrects[1]  = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 3, item = new Item() {itemId = 1}},
        };
        corrects[2]  = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            new Belt.BeltSegment() {count = 2, item = new Item() .SetEmpty()},
        };
        corrects[3]  = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 2}},
            new Belt.BeltSegment() {count = 2, item = new Item() {itemId = 5}},
        };
        corrects[4]  = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 2, item = new Item() {itemId = 1}},
            new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
        };
        corrects[5]  = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
            new Belt.BeltSegment() {count = 2, item = new Item() {itemId = 1}},
        };
        corrects[6]  = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
        };
        corrects[7]  = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
        };
        
        Assert.IsTrue(results[0]);
        Assert.IsTrue(BeltSegmentEqualityChecker(belts[0].items,corrects[0]));
        
        Assert.IsTrue(results[1]);
        Assert.IsTrue(BeltSegmentEqualityChecker(belts[1].items,corrects[1]));
        
        Assert.IsFalse(results[2]);
        Assert.IsTrue(BeltSegmentEqualityChecker(belts[2].items,corrects[2]));
        
        Assert.IsTrue(results[3]);
        Assert.IsTrue(BeltSegmentEqualityChecker(belts[3].items,corrects[3]));
        
        Assert.IsTrue(results[4]);
        Assert.IsTrue(BeltSegmentEqualityChecker(belts[4].items,corrects[4]));
        
        Assert.IsTrue(results[5]);
        Assert.IsTrue(BeltSegmentEqualityChecker(belts[5].items,corrects[5]));
        
        Assert.IsTrue(results[6]);
        Assert.IsTrue(BeltSegmentEqualityChecker(belts[6].items,corrects[7]));
        
        Assert.IsFalse(results[7]);
        Assert.IsTrue(BeltSegmentEqualityChecker(belts[6].items,corrects[7]));
    }
    
    [Test]
    public void TestBeltRemoveLastItem()
    {
        // Arrange
        int numberOfCases = 6;
        var belts = new Belt[numberOfCases];
        belts[0] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 2, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            }
        };
        belts[1] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 2, item = new Item() {itemId = 1}},
            }
        };
        belts[2] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 3, item = new Item() .SetEmpty()},
            }
        };
        belts[3] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            }
        };
        belts[4] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            }
        };
        belts[5] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
            }
        };

        // Act
        var items = new Item[numberOfCases];
        var results = new bool[numberOfCases];
        for (int i = 0; i < numberOfCases; i++) {
            results[i] = belts[i].TryRemoveLastItemFromBelt(out items[i]);
        }


        // Assert
        var corrects = new List<Belt.BeltSegment>[numberOfCases];
        var correctItems = new Item[numberOfCases];
        
        corrects[0] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 3, item = new Item() .SetEmpty()},
        };
        correctItems[0] = new Item() {itemId = 1};
        
        corrects[1] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
        };
        correctItems[1] = new Item() {itemId = 1};
        
        corrects[2] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 3, item = new Item() .SetEmpty()},
        };
        correctItems[2] = new Item() .SetEmpty();
        
        corrects[3] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            new Belt.BeltSegment() {count = 2, item = new Item() .SetEmpty()},
        };
        correctItems[3] = new Item() {itemId = 1};
        
        corrects[4] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
        };
        correctItems[4] = new Item() {itemId = 1};
        
        corrects[5] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
        };
        correctItems[5] = new Item().SetEmpty();

        Assert.AreEqual(items, correctItems);
        for (int i = 0; i < numberOfCases; i++) {
            Assert.AreEqual(results[i], !items[i].isEmpty());
            Assert.AreEqual(belts[i].items, corrects[i]);
        }
    }
    
    [Test]
    public void TestBeltUpdateLastSpaceRemoval()
    {
        // Arrange
        int numberOfCases = 4;
        var belts = new Belt[numberOfCases];
        
        belts[0] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 2, item = new Item() {itemId = 1}},
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
            }
        };
        belts[1] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
                new Belt.BeltSegment() {count = 2, item = new Item() .SetEmpty()},
            }
        };
        belts[2] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 3, item = new Item() .SetEmpty()},
            }
        };
        belts[3] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
            }
        };

        // Act
        for (int i = 0; i < numberOfCases; i++) {
            FactorySystem.UpdateBelt(belts[i]);
        }


        // Assert
        var corrects = new List<Belt.BeltSegment>[numberOfCases];
        
        corrects[0] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
            new Belt.BeltSegment() {count = 2, item = new Item() {itemId = 1}},
        };
        corrects[1] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
        };
        corrects[2] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 3, item = new Item() .SetEmpty()},
        };
        corrects[3] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 2, item = new Item() .SetEmpty()},
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
        };
        
        
        for (int i = 0; i < numberOfCases; i++) {
            Assert.IsTrue(BeltSegmentEqualityChecker(belts[i].items, corrects[i]));
        }
    }
    
    [Test]
    public void TestBeltUpdateMidSpaceRemoval()
    {
        // Arrange
        int numberOfCases = 4;
        var belts = new Belt[numberOfCases];
        
        belts[0] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 2}},
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            }
        };
        belts[1] = new Belt(new Position(0, 0), new Position(5, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 2, item = new Item() {itemId = 3}},
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 2}},
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            }
        };
        belts[2] = new Belt(new Position(0, 0), new Position(4, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 2}},
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            }
        };
        belts[3] = new Belt(new Position(0, 0), new Position(4, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
                new Belt.BeltSegment() {count = 2, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            }
        };

        // Act
        for (int i = 0; i < numberOfCases; i++) {
            FactorySystem.UpdateBelt(belts[i]);
        }


        // Assert
        var corrects = new List<Belt.BeltSegment>[numberOfCases];
        
        corrects[0] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 2}},
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
        };
        corrects[1] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 2, item = new Item() .SetEmpty()},
            new Belt.BeltSegment() {count = 2, item = new Item() {itemId = 3}},
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 2}},
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
        };
        corrects[2] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 2}},
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
        };
        corrects[3] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
        };
        
        
        for (int i = 0; i < numberOfCases; i++) {
            Assert.IsTrue(BeltSegmentEqualityChecker(belts[i].items, corrects[i]));
        }
    }
    
    [Test]
    public void TestBeltUpdateMidSpaceRemovalAndMerge()
    {
        // Arrange
        int numberOfCases = 3;
        var belts = new Belt[numberOfCases];
        
        belts[0] = new Belt(new Position(0, 0), new Position(2, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            }
        };
        belts[1] = new Belt(new Position(0, 0), new Position(5, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 2, item = new Item() {itemId = 2}},
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 2}},
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            }
        };
        belts[2] = new Belt(new Position(0, 0), new Position(4, 0)) {
            items = new List<Belt.BeltSegment>() {
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
                new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
                new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            }
        };

        // Act
        for (int i = 0; i < numberOfCases; i++) {
            FactorySystem.UpdateBelt(belts[i]);
        }


        // Assert
        var corrects = new List<Belt.BeltSegment>[numberOfCases];
        
        corrects[0] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
            new Belt.BeltSegment() {count = 2, item = new Item() {itemId = 1}},
        };
        corrects[1] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 2, item = new Item() .SetEmpty()},
            new Belt.BeltSegment() {count = 3, item = new Item() {itemId = 2}},
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
        };
        corrects[2] = new List<Belt.BeltSegment>() {
            new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
            new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
            new Belt.BeltSegment() {count = 1, item = new Item() .SetEmpty()},
            new Belt.BeltSegment() {count = 2, item = new Item() {itemId = 1}}, 
        };
        
        
        for (int i = 0; i < numberOfCases; i++) {
            Assert.IsTrue(BeltSegmentEqualityChecker(belts[i].items, corrects[i]));
        }
    }
}
