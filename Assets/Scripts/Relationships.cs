using System.Collections.Generic;

public static class Relationships
{
    public static Dictionary<string, RelationshipStatus> relationships = new Dictionary<string, RelationshipStatus>(){
        {"bff", RelationshipStatus.STRANGERS},
        {"Constants", RelationshipStatus.STRANGERS},
        {"Candy", RelationshipStatus.STRANGERS},
        {"Jibb", RelationshipStatus.STRANGERS},
        {"ruler", RelationshipStatus.STRANGERS}
    };
}
