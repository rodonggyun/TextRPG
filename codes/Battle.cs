using System;
using System.Drawing;
using System.Reflection.Emit;

namespace Text_RPG;

public static class Battle
{
    public static void StartBattle(bool isBoss)
    {
        GameState.currentEnemyHp = isBoss ? GameState.rand.Next(60, 100) : GameState.rand.Next(25, 40);
        string enemyName = isBoss ? " 보스" : " 몬스터";

        while (GameState.currentEnemyHp > 0 && GameState.hp > 0)
        {
            Console.Clear();
            Console.WriteLine($"===== {enemyName} 전투 =====");
            Console.WriteLine($" 층수: {GameState.floor}    HP: {GameState.hp}/{GameState.maxHp}   레벨: {GameState.level}   EXP: {GameState.exp}/{GameState.ExpToLevelUp()}");
            Console.WriteLine($" 적 HP: {GameState.currentEnemyHp}");

            Console.WriteLine("\n 스킬 목록:");
            for (int i = 0; i < GameState.skills.Count; i++)
            {
                var s = GameState.skills[i];
                Console.WriteLine($" {i + 1}. {s.Name} ({s.Description}) - 쿨다운: {(s.IsAvailable ? "Ready" : s.CurrentCooldown + "턴")}");
            }

            Console.Write("\n 행동 선택 (A: 기본공격, 1~3: 스킬): ");
            string input = Console.ReadLine();

            Console.Clear();

            if (input == "A" || input == "a")
            {
                int dmg = GameState.rand.Next(10, 20);
                GameState.currentEnemyHp -= dmg;
                Console.WriteLine($" 기본 공격으로 {dmg} 피해!");
            }
            else if (int.TryParse(input, out int index) && index >= 1 && index <= GameState.skills.Count)
            {
                var s = GameState.skills[index - 1];
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
            if (GameState.currentEnemyHp > 0)
            {
                int dmg = isBoss ? GameState.rand.Next(10, 18) : GameState.rand.Next(6, 12);
                GameState.hp -= dmg;
                Console.WriteLine($" 적의 반격! {dmg} 피해를 입었습니다. (HP: {GameState.hp})");
            }

            foreach (var s in GameState.skills)
                s.TickCooldown();

            ContinuePrompt();
        }

        if (GameState.hp > 0)
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
}
