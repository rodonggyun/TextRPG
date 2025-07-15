using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_RPG;

namespace Text_RPG
{
    
    
        public class Skill
        {
            public string Name;
            public string Description;
            public int Cooldown;
            public Action<Player, Player> Player;
            public bool IsUltimate;
            public Action<Player, Player> Effect;

            public Skill(string name, string desc, int cooldown, Action<Player, Player> effect, bool isUltimate = false)
            {
                Name = name;
                Description = desc;
                Cooldown = cooldown;
                Effect = effect;
                IsUltimate = isUltimate;
            }
        internal class UnlockSkill
        {

        }
    }
}
