# FamilyRoots

WORK IN PROGRESS!

Set of APIs to store genealogy tree in Neo4j database.

## Database 

### Database setup

FamilyRoots web API is build on standard Neo4j graph database (current version: `4.1.5`); 
It requires [APOC plugin](https://neo4j.com/labs/apoc/4.1/) to be installed and enabled. Main use for that plugin is [UUID generation](https://neo4j.com/labs/apoc/4.1/graph-updates/uuid/) on nodes.

### Database constraints:

```
CREATE CONSTRAINT ON (person:Person)
ASSERT person.uuid IS UNIQUE
```
