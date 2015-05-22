# Perspectives Story Graph Editor

A graph editor designed for narratives.

## Perspectives
* Places (Map)
* People (Relationships)
* Events (Timeline)
* Custom
	* User 1
	* User 2

Perspectives are different arrangements

## Elements
* Node
	* Person
	* Group
	* Place
	* Thing
	* Event
* Edge
	* Action
	* Membership
	* Relationship
	* Path
	* Dependency

### Element Attributes
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

### Node Attributes
* <NodeType>
* Positions<Perspective>[]

### Edge Attributes
* <EdgeType> 
* Contributors<Element>[]
* Effects<Element>[]