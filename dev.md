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
* `Contributors<Element>[]`
* `Effects<Element>[]`

# GUI
* Node Editor Pane
	* Element List
		* Local/Global
	* Element Attributes
* Tool Pane
	* New Node
	* Delete Node
	* Create Edge
	* 
* Menu
	* New
	* Save
	* Save As
	* Load

# Interaction Design
* Move Node
* Link Node
* Select Node ()
* Enter Node (double-click node)
* New Node (double-click background)

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