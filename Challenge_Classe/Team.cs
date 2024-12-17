using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Challenge_Classe.Relation;

namespace Challenge_Classe
{
    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<TeamChallenge> TeamChallenges { get; set; }

        // Liste des utilisateurs dans l'équipe
        [JsonIgnore]
        public ICollection<UserTeam> UserTeams { get; set; }
    }
}
