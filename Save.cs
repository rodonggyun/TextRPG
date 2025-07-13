using System;
using System.IO;

namespace Text_RPG;

public static class Save
{
    static string saveFile = "save.txt";

    public static void SaveGame()
    {
        using (StreamWriter writer = new StreamWriter(saveFile))
        {
            writer.WriteLine(GameState.floor);
            writer.WriteLine(GameState.level);
            writer.WriteLine(GameState.exp);
            writer.WriteLine(GameState.hp);
            writer.WriteLine(GameState.maxHp);
            foreach (var skill in GameState.skills)
            {
                writer.WriteLine(skill.CurrentCooldown);
            }
        }
        Console.WriteLine("게임이 저장되었습니다.");
    }

    public static bool LoadGame()
    {
        if (!File.Exists(saveFile)) return false;

        try
        {
            string[] lines = File.ReadAllLines(saveFile);
            int index = 0;
            GameState.floor = int.Parse(lines[index++]);
            GameState.level = int.Parse(lines[index++]);
            GameState.exp = int.Parse(lines[index++]);
            GameState.hp = int.Parse(lines[index++]);
            GameState.maxHp = int.Parse(lines[index++]);

            GameState.InitSkills(); // 스킬 초기화 후 쿨다운 적용
            for (int i = 0; i < GameState.skills.Count; i++)
            {
                GameState.skills[i].CurrentCooldown = int.Parse(lines[index++]);
            }

            return true;
        }
        catch
        {
            Console.WriteLine("불러오기에 실패했습니다.");
            return false;
        }
    }
}
