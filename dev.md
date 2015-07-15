# Perspectives
Perspectives are different arrangements of nodes. Think of them as alternate planes of existence. Nodes can exist on many perspectives, and may generally are differently-arranged on each.

* Places (Map)
* People (Relationships)
* Events (Timeline)
* Custom (multiple User created and named)

# Elements
* Node
	* Person
	* Group
	* Place
	* Thing
	* Event
	* Note
* Edge
	* Action
	* Membership
	* Relationship
	* Path
	* Dependency

## Element Attributes
* Name
* GUID
* Start Date
* End Date
* Description
* Custom Fields
	* Type
	* Value
* Icon
* Children

## Node Attributes
* `<NodeType>`
* `Positions<Perspective>[]`
* Age

## Edge Attributes
* `<EdgeType>`

# GUI
* Node Editor Pane
	* Element List
		* Local/Global *not implemented yet: #43*
	* Element Attributes
* Menu
	* New
	* Save
	* Save As
	* Load
	* Help
* Tool Pane *removed as unnecessary: #44*
	* New Node
	* Delete Node
	* Create Edge

# Interaction Design

## NodeInteraction
* Move Node (drag node)
* Link Nodes (right-click-drag node to node)
* Add Node to parent (drag and drop node on other node) *not implemented yet: #45*
* Select Just This Node (click node)
* Add Node to Selection (Ctrl+Click node)
* Edit Node (double-click node)

## EdgeInteraction
* Move Edge Head (drag head end of Edge)
* Move Edge Tail (drag tail end of Edge)

## BackgroundInteraction
* New Node (double-click background)
* Pan View (drag background)

# Control Flow

## Editing
* Manipulate User Interface
* Data Bind to Object
* Object Generates JSON

## Saving
* Save Button
* File JSON
* Element JSON
* IO save file

## Loading
* Load Button
* Load Screen
* Set Load path
* load file at path
* parse as JSON
* Construct Elements