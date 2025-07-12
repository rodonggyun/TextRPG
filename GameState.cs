using System;
using System.Collections.Generic;


namespace Text_RPG
{
    public static class GameState
    {
        public static int floor = 1;
        public static int level = 1;
        public static int exp = 0;
        public static int hp = 100;
        public static int maxHp = 100;
        public static int currentEnemyHp;
        public static bool isBoss = false;

        public static Random rand = new Random();
        public static List<Skill> skills;


        public static void InitSkills()
        {
            skills = new List<Skill>()
            {
                new Skill("파워 슬래시", "30~50 데미지", 3, () => {
                    int dmg = rand.Next(30, 51);
                    currentEnemyHp -= dmg;
                    Console.WriteLine($" 파워 슬래시로 {dmg} 피해!");
                }),
                new Skill("힐", "HP 30 회복", 4, () => {
                    hp += 30;
                    if (hp > maxHp) hp = maxHp;
                    Console.WriteLine($" 체력을 30 회복!");
                }),
                new Skill("광역참격", "15~25 x2 피해", 5, () => {
                    int d1 = rand.Next(15, 26);
                    int d2 = rand.Next(15, 26);
                    currentEnemyHp -= (d1 + d2);
                    Console.WriteLine($" 광역참격으로 {d1} + {d2} 피해!");
                })
            };
        }


        public static void LevelUpIfNeeded()
        {
            while (exp >= ExpToLevelUp())
            {
                exp -= ExpToLevelUp();
                level++;
                maxHp += 10;
                hp = maxHp;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($" 레벨업! Lv.{level} (최대 HP {maxHp})");
                Console.ResetColor();
            }
        }

        public static int ExpToLevelUp()
        {
            return (int)(10 * Math.Pow(level, 1.2));
        }
    }
}