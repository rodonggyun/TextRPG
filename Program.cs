using System;
using System.Collections.Generic;

class TowerGameWithSkills
{
    static int floor = 1;
    static int level = 1;
    static int exp = 0;
    static int hp = 100;
    static int maxHp = 100;
    static int currentEnemyHp;
    static bool isBoss = false;

    static Random rand = new Random();

    class Skill
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
        public void TickCooldown() { if (CurrentCooldown > 0) CurrentCooldown--; }
    }

    static List<Skill> skills;

    static void Main()
    {
        InitSkills();

        Console.CursorVisible = false;
        Console.WriteLine(" 스킬 탑 오르기 RPG 시작!");

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"\n 현재 층: {floor}");
            Console.WriteLine($" HP: {hp}/{maxHp}   레벨: {level}  ⭐ EXP: {exp}/{ExpToLevelUp()}");

            isBoss = floor % 10 == 0;
            StartBattle(isBoss);

            if (hp <= 0)
            {
                Console.WriteLine("\n 사망! 탑에서 떨어졌습니다.");
                break;
            }

            int earnedExp = rand.Next(10, 20);
            Console.WriteLine($"\n EXP +{earnedExp}");
            exp += earnedExp;
            LevelUpIfNeeded();

            Console.WriteLine(" 계속하려면 Enter...");
            Console.ReadLine();
            floor++;
        }
    }

    static void InitSkills()
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

    static void StartBattle(bool isBoss)
    {
        currentEnemyHp = isBoss ? rand.Next(60, 100) : rand.Next(25, 40);
        string enemyName = isBoss ? " 보스" : " 몬스터";

        while (currentEnemyHp > 0 && hp > 0)
        {
            Console.Clear();
            Console.WriteLine($"===== {enemyName} 전투 =====");
            Console.WriteLine($" HP: {hp}/{maxHp}   레벨: {level}   EXP: {exp}/{ExpToLevelUp()}");
            Console.WriteLine($" 적 HP: {currentEnemyHp}");

            Console.WriteLine("\n 스킬 목록:");
            for (int i = 0; i < skills.Count; i++)
            {
                var s = skills[i];
                Console.WriteLine($" {i + 1}. {s.Name} ({s.Description}) - 쿨다운: {(s.IsAvailable ? "Ready" : s.CurrentCooldown + "턴")}");
            }

            Console.Write("\n 행동 선택 (A: 기본공격, 1~3: 스킬): ");
            string input = Console.ReadLine();

            Console.Clear();

            if (input == "A" || input == "a")
            {
                int dmg = rand.Next(10, 20);
                currentEnemyHp -= dmg;
                Console.WriteLine($" 기본 공격으로 {dmg} 피해!");
            }
            else if (int.TryParse(input, out int index) && index >= 1 && index <= skills.Count)
            {
                var s = skills[index - 1];
                if (s.IsAvailable)
                {
                    s.Effect();
                    s.Trigger();
                }
                else
                {
                    Console.WriteLine($" {s.Name}은 {s.CurrentCooldown}턴 남았습니다!");
                    ContinuePrompt();
                    continue;
                }
            }
            else
            {
                Console.WriteLine(" 잘못된 입력입니다.");
                ContinuePrompt();
                continue;
            }

            // 적 반격
            if (currentEnemyHp > 0)
            {
                int dmg = isBoss ? rand.Next(10, 18) : rand.Next(6, 12);
                hp -= dmg;
                Console.WriteLine($" 적의 반격! {dmg} 피해를 입었습니다. (HP: {hp})");
            }

            foreach (var s in skills)
                s.TickCooldown();

            ContinuePrompt();
        }

        if (hp > 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n {(isBoss ? "보스" : "몬스터")} 처치 성공!");
            Console.ResetColor();
        }
    }

    static void ContinuePrompt()
    {
        Console.WriteLine("\n Enter 키를 눌러 계속...");
        Console.ReadLine();
    }

    static void LevelUpIfNeeded()
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

    static int ExpToLevelUp()
    {
        return (int)(10 * Math.Pow(level, 1.2));
    }
}