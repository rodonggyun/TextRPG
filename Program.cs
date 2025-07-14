using System;
using System.Collections.Generic;
using System.IO;

namespace Text_RPG;

class Program
{
    static void Main()
    {
        GameState.InitSkills();

        Console.CursorVisible = false;
        Console.WriteLine(" 스킬 탑 오르기 RPG 시작!");
        Console.WriteLine("불러오시겠습니까? (Y/N): ");
        string choice = Console.ReadLine().ToUpper();

        if (choice == "Y" && Save.LoadGame()) Console.WriteLine("게임 데이터를 불러왔습니다!");

        else GameState.InitSkills();

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"\n 현재 층: {GameState.floor}");
            Console.WriteLine($" HP: {GameState.hp}/{GameState.maxHp}   레벨: {GameState.level}  ⭐ EXP: {GameState.exp}/{GameState.ExpToLevelUp()}");

            GameState.isBoss = GameState.floor % 10 == 0;
            Battle.StartBattle(GameState.isBoss);

            if (GameState.hp <= 0)
            {
                Console.WriteLine("\n 사망! 탑에서 떨어졌습니다.");
                break;
            }

            int earnedExp = GameState.rand.Next(10, 20);
            Console.WriteLine($"\n EXP +{earnedExp}");
            GameState.exp += earnedExp;
            GameState.LevelUpIfNeeded();

            Console.WriteLine("\n저장하시겠습니까? (Y/N):");
            if (Console.ReadLine().ToUpper() == "Y") Save.SaveGame();

            Console.WriteLine(" 계속하려면 Enter...");
            Console.ReadLine();
            GameState.floor++;
        }
    }
}
