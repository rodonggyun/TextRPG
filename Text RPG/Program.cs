using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Numerics;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Player player = NewGameOrLoad();

        while (true)
        {
            int selected = MenuSelector.Select("무엇을 하시겠습니까?", new List<string>
            {
                "스탯 확인", "저장/불러오기", "마을 방문", "게임 종료" 
            });

            Console.Clear();
            if (selected == 0) player.DisplayStats();
            else if (selected == 1) SaveLoadMenu(ref player);
            else if (selected == 2)
            {
                Town town = new Town();
                town.Enter(player);
            }
            else if (selected == 3) break;
            Console.WriteLine("\n아무 키나 눌러 계속...");
            Console.ReadKey();
        }
    }

    static Player NewGameOrLoad()
    {
        int selected = MenuSelector.Select("게임 시작 방식 선택", new List<string>
        {
            "새 게임 시작", "저장된 게임 불러오기"
        });

        if (selected == 1)
        {
            int slot = SelectSaveSlot("불러오기");
            if (slot != -1)
            {
                var loaded = SaveSystem.Load(slot);
                if (loaded != null) return loaded;
                Console.WriteLine("불러오기에 실패하여 새 게임을 시작합니다.");
                Console.ReadKey();
            }
        }

        return CreateNewPlayer();
    }

    static void SaveLoadMenu(ref Player player)
    {
        var options = new List<string> { "저장한다", "불러온다", "나간다" };

        while (true)
        {
            int selected = MenuSelector.Select("저장/불러오기 메뉴", options, true);
            Console.Clear();

            if (selected == -1 || options[selected] == "나간다") break;

            if (options[selected] == "저장한다")
            {
                int slot = SelectSaveSlot("저장");
                if (slot != -1) SaveSystem.Save(player, slot);
            }
            else if (options[selected] == "불러온다")
            {
                int slot = SelectSaveSlot("불러오기");
                if (slot != -1)
                {
                    var loaded = SaveSystem.Load(slot);
                    if (loaded != null) player = loaded;
                }
            }

            Console.WriteLine("\n아무 키나 누르세요...");
            Console.ReadKey();
        }
    }

    static int SelectSaveSlot(string action)
    {
        List<string> options = new List<string>();
        for (int i = 1; i <= 3; i++)
        {
            string info = SaveSystem.GetSaveInfo(i);
            options.Add($"슬롯 {i} - {info}");
        }

        int selected = MenuSelector.Select($"{action}할 슬롯을 선택하세요", options, true);
        return selected == -1 ? -1 : selected + 1;
    }

    static Player CreateNewPlayer()
    {
        Console.Clear();
        Console.Write("플레이어 이름을 입력하세요: ");
        string name = Console.ReadLine();

        int selected = MenuSelector.Select("직업을 선택하세요", new List<string>
        {
            "전사", "도적", "마법사", "성직자"
        });
        Player player = new Player { Name = name };

        switch (selected)
        {
            case 0:
                player.Job = "전사";
                player.HP = player.MaxHP = 150;
                player.STR = 10; player.DEX = 5; player.INT = 2; player.DEF = 8;
                break;
            case 1:
                player.Job = "도적";
                player.HP = player.MaxHP = 100;
                player.STR = 6; player.DEX = 10; player.INT = 4; player.DEF = 4;
                break;
            case 2:
                player.Job = "마법사";
                player.HP = player.MaxHP = 80;
                player.STR = 2; player.DEX = 4; player.INT = 12; player.DEF = 3;
                break;
            case 3:
                player.Job = "성직자";
                player.HP = player.MaxHP = 110;
                player.STR = 4; player.DEX = 4; player.INT = 8; player.DEF = 6;
                break;
        }

        return player;
    }
}