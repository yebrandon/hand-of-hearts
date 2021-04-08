using System.Collections.Generic;

public static class Relationships
{
    public static Dictionary<string, RelationshipStatus> relationships = new Dictionary<string, RelationshipStatus>(){
        {"Blossom", RelationshipStatus.HEARTWON},
        {"Constants", RelationshipStatus.NOTMET},
        {"Candy", RelationshipStatus.NOTMET},
        {"Jibb", RelationshipStatus.NOTMET},
        {"Rosa", RelationshipStatus.NOTMET}
    };

    public static void ResetRelationships()
    {
        relationships = new Dictionary<string, RelationshipStatus>(){
        {"Blossom", RelationshipStatus.HEARTWON},
        {"Constants", RelationshipStatus.NOTMET},
        {"Candy", RelationshipStatus.NOTMET},
        {"Jibb", RelationshipStatus.NOTMET},
        {"Rosa", RelationshipStatus.NOTMET}
    };
    }
}
