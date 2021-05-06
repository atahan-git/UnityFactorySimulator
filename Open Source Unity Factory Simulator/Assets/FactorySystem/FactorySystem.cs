using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI;

public class FactorySystem : MonoBehaviour
{
    public List<Belt> belts = new List<Belt>();
    public List<Connector> connectors = new List<Connector>();
    public List<Building> buildings = new List<Building>();
    
    
    /// Create a sample factory
    void Start()
    {
        // Miner
        buildings.Add(new Building() {
            buildingId = 0,
            inputItems = new List<RecipeItemSlot>(), // No input = infinite creation
            outputItems = new List<RecipeItemSlot>(){new RecipeItemSlot(){craftAmount = 1, maxAmount = 2,item = new Item(){itemId = 1}}},
            craftProgressReq = 2,
            buildingPositions = new List<Position>(){new Position(0,0)}
        });
        
        // Smelter
        buildings.Add(new Building() {
            buildingId = 1,
            inputItems = new List<RecipeItemSlot>(){new RecipeItemSlot(){craftAmount = 1, maxAmount = 2, item = new Item(){itemId = 1}}},
            outputItems = new List<RecipeItemSlot>(){new RecipeItemSlot(){craftAmount = 1, maxAmount = 2,item = new Item(){itemId = 2}}},
            craftProgressReq = 4,
            buildingPositions = new List<Position>(){new Position(6,5), new Position(6,6)}
        });

        // belts
        belts.Add(new Belt(new Position(2, 0), new Position(5, 0)));
        belts.Add(new Belt(new Position(6, 1), new Position(6, 3)));
        belts.Add(new Belt(new Position(8, 1), new Position(8, 3)));
        belts.Add(new Belt(new Position(8, 6), new Position(14, 6)));
        
        
        // connectors
        connectors.Add(new Connector(){
            startPos = new Position(1,0), endPos = new Position(1,0), 
            inputBuildings = new List<Building>(){buildings[0]},
            outputBelts = new List<Belt>(){belts[0]}
        });
        
        connectors.Add(new Connector() {
            startPos = new Position(6,0), endPos = new Position(8,0), 
            inputBelts = new List<Belt>(){belts[0]},
            outputBelts = new List<Belt>(){belts[1],belts[2]}
        });
        
        connectors.Add(new Connector() {
            startPos = new Position(6,4), endPos = new Position(8,4), 
            inputBelts = new List<Belt>(){belts[1],belts[2]},
            outputBuildings = new List<Building>(){buildings[1]}
        });
        
        connectors.Add(new Connector() {
            startPos = new Position(7,6), endPos = new Position(7,6), 
            inputBuildings = new List<Building>(){buildings[1]},
            outputBelts = new List<Belt>(){belts[3]}
        });
        
        // This is the regular game update
        InvokeRepeating("RegularUpdate",0f,0.25f);
    }

    void RegularUpdate()
    {
        SimUpdate();
        DrawAscii();
    }

    /// <summary>
    /// Update Belt Slots Appropriately
    /// </summary>
    /// <example>
    /// eg:
    /// belt = >[###OO#O]>
    /// internal representation = #x3, Ox2, #x1, Ox1
    /// 
    /// so we find the first empty spot, the second #, then remove it:
    /// internal representation = #x3, Ox2, Ox1
    /// 
    /// Now for the internal representation to make sense we merge the two O, and add an empty space to the beginning:
    /// internal representation = #x4, Ox3
    ///
    /// Final Belt:
    /// belt = >[####OOO]>
    /// 
    /// </example>
    /// <param name="belt"></param>
    public static void UpdateBelt(Belt belt) {
        bool thereIsEmptySlot = false;

        var beltsCount = belt.items.Count - 1;
        for (int i = beltsCount; i >= 0; i--) { 
            if (belt.items[i].item.isEmpty()) {
                var count = belt.items[i].count;
                count -= 1;
                bool slotDestroyed;
                if (count > 0) {
                    belt.items[i] = new Belt.BeltSegment() {count = count, item = belt.items[i].item};
                    slotDestroyed = false;
                } else {
                    belt.items.RemoveAt(i);
                    slotDestroyed = true;
                }
                    
                //If we destroyed a slot, me might need to merge the new touching slots
                if (slotDestroyed) {
                    // We cannot merge if the destroyed slot was at the very end or at the very start
                    if (i != beltsCount) {
                        if (i != 0) {
                            // Check if the current slot (right after the empty slot) and the previous slot have the same item
                            if (belt.items[i].item == belt.items[i - 1].item) {
                                belt.items[i - 1] = new Belt.BeltSegment() {
                                    count = belt.items[i].count + belt.items[i - 1].count,
                                    item = belt.items[i].item
                                };
                                
                                belt.items.RemoveAt(i);
                            }
                        }
                    }
                }

                thereIsEmptySlot = true;
                break;
            }
        }

        if (thereIsEmptySlot) {
            if (belt.items[0].item.isEmpty()) {
                var count = belt.items[0].count;
                count += 1;
                belt.items[0] = new Belt.BeltSegment() {count = count, item = belt.items[0].item};
            } else {
                belt.items.Insert(0, new Belt.BeltSegment(){count = 1, item = new Item().SetEmpty()});
            }
        }
    }


    /// <summary>
    /// There are 3 states:
    /// craftProgress == -1
    ///     This is when the crafting is complete.
    ///     In this state we check if there are enough input items, and if so start the crafting process
    /// craftProgress > 0
    ///     This is the crafting state
    ///     In this state we reduce the crafting process by 1 each time
    /// craftProgress == 0
    ///     This is the frame when crafting is done
    ///     In this state we add the output items
    /// </summary>
    /// <param name="building"></param>
    public static void UpdateBuilding(Building building) {
        
        // Move the crafting process
        if(building.craftProgress >= 0)
            building.craftProgress -= 1;
            
        // Crafting complete, check if we can start crafting again
        if (building.craftProgress == -1) {
            bool canCraft = true;

            // Check enough input items
            foreach (var inputItem in building.inputItems) {
                if (inputItem.amount < inputItem.craftAmount) {
                    canCraft = false;
                    break;
                }
            }

            if (canCraft) {
                // Check enough output items slot
                foreach (var outputItem in building.outputItems) {
                    if (outputItem.amount + outputItem.craftAmount > outputItem.maxAmount) {
                        canCraft = false;
                        break;
                    }
                }
            }

            // Use craft items 
            if (canCraft) {
                foreach (var inputItem in building.inputItems) {
                    inputItem.amount -= inputItem.craftAmount;
                }

                building.craftProgress = building.craftProgressReq;
            }
        }

        // Generate output items when crafting is done
        if (building.craftProgress == 0) {
            foreach (var outputItem in building.outputItems) {
                outputItem.amount += outputItem.craftAmount;
            }
        }
    }

    /// <summary>
    /// In each sim step each connect will find the next input slot item, and put it into the next free output slot.
    /// </summary>
    /// <param name="connector"></param>
    public static void UpdateConnector(Connector connector) {
        Item curItem = new Item().SetEmpty();

        // Move over every possible input source once
        for (int i = 0; i < connector.InputLength; i++) {
            var n = (i + connector.inputCounter) % connector.InputLength;
            if (n < connector.inputBelts.Count) {
                // Try to take the last item from a belt
                if (connector.inputBelts[n].GetLastItem(out curItem)) {
                    connector.inputCounter = n + 1;

                    if (ConnectorTryToPlaceItem(connector, curItem)) {
                        connector.inputBelts[n].TryRemoveLastItemFromBelt(out curItem);
                        break;
                    }
                }
            } else {
                // Try to take an item from a building
                if (connector.inputBuildings[n - connector.inputBelts.Count].GetItem(out curItem)) {
                    connector.inputCounter = n + 1;
                    
                    if (ConnectorTryToPlaceItem(connector, curItem)) {
                        connector.inputBuildings[n - connector.inputBelts.Count].TryTakeItemFromBuilding(out curItem);
                        break;
                    }
                }
            }
        }

        
    }

    static bool ConnectorTryToPlaceItem(Connector connector, Item curItem) {
        // Move over every possible output source once to try to find an empty slot
        for (int i = 0; i < connector.OutputLength; i++) {
            var n = (i + connector.outputCounter) % connector.OutputLength;
            if (n < connector.outputBelts.Count) {
                // Try to insert to the belt
                if (connector.outputBelts[n].TryInsertItemToBelt(curItem)) {
                    curItem.SetEmpty();
                    connector.outputCounter = n + 1;
                    return true;
                }
            } else {
                // Try to insert the item to building
                if (connector.outputBuildings[n - connector.outputBelts.Count].TryInsertItemToBuilding(curItem)) {
                    connector.outputCounter = n + 1;
                    curItem.SetEmpty();
                    return true;
                }
            }
        }

        return false;
    }

    void SimUpdate() {
        
        foreach (var belt in belts) {
            UpdateBelt(belt);
        }
        
        foreach (var building in buildings) {
            UpdateBuilding(building);
        }
        
        
        foreach (var connector in connectors) {
            UpdateConnector(connector);
        }
        
    }


    public Text screen;
    public int xWidth = 15;
    public int yHeight = 15;
    private char[] itemIdtoName = new[] {'$','O', 'I'};
    private char[] beltDirectionToName = new[] {'O','▲','►', '▼', '◄'};
    private char[] buildingIdtoName = new[] {'M', 'S'};
    void DrawAscii() {

        Dictionary<Position, char> grid = new Dictionary<Position, char>();


        foreach (var building in buildings) {
            foreach (var position in building.buildingPositions) {
                grid[position] = buildingIdtoName[building.buildingId];
            }
        }

        foreach (var belt in belts) {
            int curIndex = -1;
            int curCounter = 0;
            int beltCardinalDirection = Position.CardinalDirection(belt.startPos, belt.endPos);
            
            for (int i = 0; i < belt.length; i++) {
                
                curCounter -= 1;

                if (curCounter <= 0) {
                    curIndex += 1;
                    curCounter = belt.items[curIndex].count;
                }

                char toDraw;
                if (belt.items[curIndex].item.isEmpty()) {
                    toDraw = beltDirectionToName[beltCardinalDirection];
                } else {
                    toDraw = itemIdtoName[belt.items[curIndex].item.itemId];
                }

                grid[Position.MoveTowards(belt.startPos, belt.endPos, i)] = toDraw;
            }
        }
        
        foreach (var connector in connectors) {
            for (int i = 0; i < connector.Length; i++) {
                grid[Position.MoveTowards(connector.startPos, connector.endPos, i)] = '#';
            }
        }


        string map = "";
        for (int y = xWidth-1; y >= 0; y--) {
            for (int x = 0; x < yHeight; x++) {
                if (grid.TryGetValue(new Position(x, y), out char value)) {
                    map += value;
                } else {
                    map += "~";
                }
            }

            map += "\n";
        }

        screen.text = map;
    }
}

/// <summary>
/// One of the core building blocks of the belt system.
/// A belt has a start point and a end point, and it is a straight line.
/// It holds item in the way (# = empty slot, O = ore slot)
/// (2, null), (3, ore), (1, null), (1, ore) >> ##OOO#O
/// </summary>
public class Belt {
    public Position startPos;
    public Position endPos;

    public int length {
        get { return Position.Distance(startPos, endPos) + 1; }
    }
        
    public List<BeltSegment> items = new List<BeltSegment>();

    /// <summary>
    /// Removes and returns the last item in the belt, whether actual item or empty
    /// automatically replaces it with empty and does reshaping as needed
    /// </summary>
    /// <returns>Returns an item (can be empty item)</returns>
    public bool TryRemoveLastItemFromBelt(out Item item) {
        var lastIndex = items.Count - 1;
        item = items[lastIndex].item;
        
        // If the last item is not empty
        if (!item.isEmpty()) {
            int count = items[lastIndex].count;
            count -= 1; // reduce its count by 1

            // If the entire section gets to zero
            if (count == 0) {
                // If the belt has only one section, it is an edge case
                // In this case we just replace the current slot with an empty slot
                if (lastIndex == 0) {

                    items[lastIndex] = new BeltSegment() {count = 1, item = new Item().SetEmpty()};
                } else {
                    // Remove the last section
                    items.RemoveAt(lastIndex);
                    lastIndex -= 1;

                    // If the previous section was also empty, add to it
                    if (items[lastIndex].item.isEmpty()) {
                        items[lastIndex] = new BeltSegment() {count = items[lastIndex].count + 1, item = items[lastIndex].item};
                    } else {
                        // If the previous section was not empty, then add an empty section at the end
                        items.Add(new BeltSegment() {count = 1, item = new Item().SetEmpty()});
                    }
                }
            } else {
                //if the entire section does not get to zero, reduce its count and add an empty section at the end
                items[lastIndex] = new BeltSegment() {count = count, item = item};
                items.Add(new BeltSegment() {count = 1, item = new Item().SetEmpty()});

            }

            return true;
        } else {
            return false;
        }
    }
    
    /// <summary>
    /// Returns the last item in the belt without removing it.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool GetLastItem(out Item item) {
        var lastIndex = items.Count - 1;
        item = items[lastIndex].item;
        return !item.isEmpty();
    }

    /// <summary>
    /// Tries to insert an item to the beginning of the belt
    /// Automatically merges slots if the next slot matches with the item
    /// </summary>
    /// <returns>Returns true only if there is empty slot at the start of the belt</returns>
    public bool TryInsertItemToBelt(Item itemToInsert) {
        if (itemToInsert.isEmpty()) {
            return true;
        }
        
        if (items[0].item.isEmpty()) {
            if (items[0].count == 1) {
                if (items.Count > 1 && items[1].item == itemToInsert) {
                    items[1] = new BeltSegment() {item = itemToInsert, count = items[1].count + 1};
                    items.RemoveAt(0);
                } else {
                    items[0] = new BeltSegment() {item = itemToInsert, count = 1};
                }
            } else {
                items[0] = new BeltSegment() {item = items[0].item, count = items[0].count - 1};
                items.Insert(0, new BeltSegment(){item = itemToInsert, count = 1});
            }

            return true;
        } else {
            return false;
        }
    }
    

    public struct BeltSegment {
        public int count;
        public Item item;
    }

    public Belt(Position startPos, Position endPos) {
        this.startPos = startPos;
        this.endPos = endPos;
        
        items.Add(new BeltSegment(){count = length, item = new Item().SetEmpty()});
    }
}

/// <summary>
/// The second core building block of the belt system.
/// A connector will connect belts with other belts or belts with buildings.
/// A connector will have inputs and outputs, and it will try to balance input/output equally
/// A connector will also always be a straight line
/// </summary>
public class Connector {
    public Position startPos;
    public Position endPos;

    public int Length {
        get { return Position.Distance(startPos, endPos) + 1; }
    }


    public int InputLength {
        get { return inputBelts.Count + inputBuildings.Count; }
    }
    
    public int OutputLength {
        get { return outputBelts.Count + outputBuildings.Count; }
    }

    public int inputCounter;
    public List<Belt> inputBelts = new List<Belt>();
    public List<Building> inputBuildings = new List<Building>();
        
    public int outputCounter;
    public List<Belt> outputBelts = new List<Belt>();
    public List<Building> outputBuildings = new List<Building>();
}

public class Building {
    public int buildingId;

    public List<Position> buildingPositions = new List<Position>();
        
    public List<RecipeItemSlot> inputItems = new List<RecipeItemSlot>();
    public List<RecipeItemSlot> outputItems = new List<RecipeItemSlot>();
    public int craftProgress;
    public int craftProgressReq;

    /// <summary>
    /// Try to insert an item to the buildings input slots
    /// Only works if the item type matches and there is enough space
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool TryInsertItemToBuilding(Item item) {
        for (int m = 0; m < inputItems.Count; m++) {
            if (inputItems[m].item == item) {
                if (inputItems[m].amount < inputItems[m].maxAmount) {
                    inputItems[m].amount += 1;
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Tries to take an item from a building output slot.
    /// Always gives priority to the smaller indexed output slot.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool TryTakeItemFromBuilding(out Item item) {
        for (int m = 0; m < outputItems.Count; m++) {
            if (outputItems[m].amount > 0) {
                outputItems[m].amount -= 1;
                item = outputItems[m].item;
                return true;
            }
        }

        item = new Item().SetEmpty();
        return false;
    }

    /// <summary>
    /// Gets the next item that would come out of the building without removing it
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool GetItem(out Item item) {
        for (int m = 0; m < outputItems.Count; m++) {
            if (outputItems[m].amount > 0) {
                item = outputItems[m].item;
                return true;
            }
        }

        item = new Item().SetEmpty();
        return false;
    }
}

public class RecipeItemSlot {
    public Item item;
    public int amount;
    public int craftAmount;
    public int maxAmount;
}
    
public struct Item {
    
    public int itemId;
    
    public static bool operator ==(Item a, Item b) {
        return a.itemId == b.itemId;
    }
	
    public static bool operator !=(Item a, Item b) {
        return a.itemId != b.itemId;
    }

    public Item SetEmpty() {
        itemId = 0;
        return this;
    }
    public bool isEmpty() {
        return itemId == 0;
    }
}