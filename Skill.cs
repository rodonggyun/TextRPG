using System;


namespace Text_RPG
{
    public class Skill
    {
        public string Name;
        public string Description;
        public int MaxCooldown;
        public int CurrentCooldown;
        public Action Effect;

        public Skill(string name, string desc, int cooldown, Action effect)
        {
            Name = name;
            Description = desc;
            MaxCooldown = cooldown;
            CurrentCooldown = 0;
            Effect = effect;
        }

        public bool IsAvailable => CurrentCooldown == 0;
        public void Trigger() => CurrentCooldown = MaxCooldown;
        public void TickCooldown() 
        { 
            if (CurrentCooldown > 0) CurrentCooldown--; 
        }
    }
}
