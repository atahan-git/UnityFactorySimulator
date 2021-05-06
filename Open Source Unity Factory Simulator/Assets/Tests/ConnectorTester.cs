using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ConnectorTester
{

	
	Belt GetFullBelt() {
		return new Belt(new Position(0, 0), new Position(0, 0)) {
			items = new List<Belt.BeltSegment>() {
				new Belt.BeltSegment() {count = 1, item = new Item() {itemId = 1}},
			}
		};
	}
	
	Belt GetEmptyBelt() {
		return new Belt(new Position(0, 0), new Position(0, 0)) {
			items = new List<Belt.BeltSegment>() {
				new Belt.BeltSegment() {count = 1, item = new Item().SetEmpty()},
			}
		};
	}

	bool CheckBeltEmptyness(Belt belt) {
		return belt.items[0].item.isEmpty();
	}

	[Test]
	public void TestBeltGenerator() {
		var beltfull = GetFullBelt();
		var beltempty = GetEmptyBelt();
		
		Assert.IsFalse(CheckBeltEmptyness(beltfull));
		Assert.IsTrue(CheckBeltEmptyness(beltempty));
	}
	
	Building GetFullBuilding() {
		return new Building() {
			inputItems = new List<RecipeItemSlot>(){new RecipeItemSlot(){amount = 5, craftAmount =  1, maxAmount = 5, item = new Item(){itemId = 1}}},
			outputItems = new List<RecipeItemSlot>(){new RecipeItemSlot(){amount = 5, craftAmount =  1, maxAmount = 5, item = new Item(){itemId = 1}}}
		};
	}
	
	Building GetEmptyBuilding() {
		return new Building() {
			inputItems = new List<RecipeItemSlot>(){new RecipeItemSlot(){amount = 0, craftAmount =  1, maxAmount = 5, item = new Item(){itemId = 1}}},
			outputItems = new List<RecipeItemSlot>(){new RecipeItemSlot(){amount = 0, craftAmount =  1, maxAmount = 5, item = new Item(){itemId = 1}}}
		};
	}

	int GetBuildingInputCount(Building building) {
		return building.inputItems[0].amount;
	}
	
	int GetBuildingOutputCount(Building building) {
		return building.outputItems[0].amount;
	}

	[Test]
	public void TestBuildingGenerator() {
		var buildingFull = GetFullBuilding();
		var buildingEmpty = GetEmptyBuilding();
		
		Assert.AreEqual(GetBuildingInputCount(buildingFull), 5);
		Assert.AreEqual(GetBuildingOutputCount(buildingFull),5);
		Assert.AreEqual(GetBuildingInputCount(buildingEmpty),0);
		Assert.AreEqual(GetBuildingOutputCount(buildingEmpty),0);
	}
	
	[Test]
	public void TestConnectorOneInOneOutBeltToBelt() {
		// Arrange
		int numberOfCases = 4;
		var connectors = new Connector[numberOfCases];
		connectors[0] = new Connector() {
			inputBelts = new List<Belt>() {GetEmptyBelt()},
			outputBelts = new List<Belt>() {GetEmptyBelt()}
		};
		connectors[1] = new Connector() {
			inputBelts = new List<Belt>() {GetFullBelt()},
			outputBelts = new List<Belt>() {GetFullBelt()}
		};
		connectors[2] = new Connector() {
			inputBelts = new List<Belt>() {GetFullBelt()},
			outputBelts = new List<Belt>() {GetEmptyBelt()}
		};
		connectors[3] = new Connector() {
			inputBelts = new List<Belt>() {GetEmptyBelt()},
			outputBelts = new List<Belt>() {GetFullBelt()}
		};
		
		// Act
		for (int i = 0; i < numberOfCases; i++) {
			FactorySystem.UpdateConnector(connectors[i]);
		}


		// Assert
		Assert.IsTrue(CheckBeltEmptyness( connectors[0].inputBelts[0]));
		Assert.IsTrue(CheckBeltEmptyness( connectors[0].outputBelts[0]));
		
		Assert.IsFalse(CheckBeltEmptyness( connectors[1].inputBelts[0]));
		Assert.IsFalse(CheckBeltEmptyness( connectors[1].outputBelts[0]));
		
		Assert.IsTrue(CheckBeltEmptyness( connectors[2].inputBelts[0]));
		Assert.IsFalse(CheckBeltEmptyness( connectors[2].outputBelts[0]));
		
		Assert.IsTrue(CheckBeltEmptyness( connectors[3].inputBelts[0]));
		Assert.IsFalse(CheckBeltEmptyness( connectors[3].outputBelts[0]));
	}
	
	
	
	
	[Test]
	public void TestConnectorOneInOneOutBuildingToBuilding() {
		// Arrange
		int numberOfCases = 4;
		var connectors = new Connector[numberOfCases];
		connectors[0] = new Connector() {
			inputBuildings = new List<Building>() {GetEmptyBuilding()},
			outputBuildings = new List<Building>() {GetEmptyBuilding()}
		};
		connectors[1] = new Connector() {
			inputBuildings = new List<Building>() {GetFullBuilding()},
			outputBuildings = new List<Building>() {GetFullBuilding()}
		};
		connectors[2] = new Connector() {
			inputBuildings = new List<Building>() {GetFullBuilding()},
			outputBuildings = new List<Building>() {GetEmptyBuilding()}
		};
		connectors[3] = new Connector() {
			inputBuildings = new List<Building>() {GetEmptyBuilding()},
			outputBuildings = new List<Building>() {GetFullBuilding()}
		};
		
		// Act
		for (int i = 0; i < numberOfCases; i++) {
			FactorySystem.UpdateConnector(connectors[i]);
		}


		// Assert
		Assert.AreEqual(GetBuildingInputCount( connectors[0].inputBuildings[0]), 0);
		Assert.AreEqual(GetBuildingOutputCount( connectors[0].inputBuildings[0]), 0);
		Assert.AreEqual(GetBuildingInputCount( connectors[0].outputBuildings[0]), 0);
		Assert.AreEqual(GetBuildingOutputCount( connectors[0].outputBuildings[0]), 0);
		
		Assert.AreEqual(GetBuildingInputCount( connectors[1].inputBuildings[0]), 5);
		Assert.AreEqual(GetBuildingOutputCount( connectors[1].inputBuildings[0]), 5);
		Assert.AreEqual(GetBuildingInputCount( connectors[1].outputBuildings[0]), 5);
		Assert.AreEqual(GetBuildingOutputCount( connectors[1].outputBuildings[0]), 5);
		
		Assert.AreEqual(GetBuildingInputCount( connectors[2].inputBuildings[0]), 5);
		Assert.AreEqual(GetBuildingOutputCount( connectors[2].inputBuildings[0]), 4);
		Assert.AreEqual(GetBuildingInputCount( connectors[2].outputBuildings[0]), 1);
		Assert.AreEqual(GetBuildingOutputCount( connectors[2].outputBuildings[0]), 0);
		
		Assert.AreEqual(GetBuildingInputCount( connectors[3].inputBuildings[0]), 0);
		Assert.AreEqual(GetBuildingOutputCount( connectors[3].inputBuildings[0]), 0);
		Assert.AreEqual(GetBuildingInputCount( connectors[3].outputBuildings[0]), 5);
		Assert.AreEqual(GetBuildingOutputCount( connectors[3].outputBuildings[0]), 5);
		
		
		// Act 2
		FactorySystem.UpdateConnector(connectors[2]);
		
		
		// Assert 2
		Assert.AreEqual(GetBuildingInputCount( connectors[2].inputBuildings[0]), 5);
		Assert.AreEqual(GetBuildingOutputCount( connectors[2].inputBuildings[0]), 3);
		Assert.AreEqual(GetBuildingInputCount( connectors[2].outputBuildings[0]), 2);
		Assert.AreEqual(GetBuildingOutputCount( connectors[2].outputBuildings[0]), 0);
	}
	
	[Test]
	public void TestConnectorOneInOneOutBelttoBuilding() {
		// Arrange
		int numberOfCases = 2;
		var connectors = new Connector[numberOfCases];
		connectors[0] = new Connector() {
			inputBelts = new List<Belt>() {GetFullBelt()},
			outputBuildings = new List<Building>() {GetEmptyBuilding()}
		};
		connectors[1] = new Connector() {
			inputBuildings = new List<Building>() {GetFullBuilding()},
			outputBelts = new List<Belt>() {GetEmptyBelt()}
		};
		
		// Act
		for (int i = 0; i < numberOfCases; i++) {
			FactorySystem.UpdateConnector(connectors[i]);
		}


		// Assert
		Assert.AreEqual(CheckBeltEmptyness( connectors[0].inputBelts[0]), true);
		Assert.AreEqual(GetBuildingInputCount( connectors[0].outputBuildings[0]), 1);
		Assert.AreEqual(GetBuildingOutputCount( connectors[0].outputBuildings[0]), 0);
		
		
		Assert.AreEqual(GetBuildingInputCount( connectors[1].inputBuildings[0]), 5);
		Assert.AreEqual(GetBuildingOutputCount( connectors[1].inputBuildings[0]), 4);
		Assert.AreEqual(CheckBeltEmptyness( connectors[1].outputBelts[0]), false);
	}
	
	//Ideally there will be other tests for cases like
	// two in one out
	// one in two out
	// cases when item types dont match
	// etc.
    
}
