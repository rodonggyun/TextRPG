using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

static class SaveSystem
{
    public static void Save(Player player, int slot)
    {
        string filename = $"save_slot{slot}.json";
        player.SaveTime = DateTime.Now;
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(player, options);
        File.WriteAllText(filename, json);
        Console.WriteLine($"[슬롯 {slot}] 저장 완료!");
    }

    public static Player Load(int slot)
    {
        string filename = $"save_slot{slot}.json";
        if (!File.Exists(filename))
        {
            Console.WriteLine($"[슬롯 {slot}] 저장된 파일이 없습니다.");
            return null;
        }

        try
        {
            string json = File.ReadAllText(filename);
            Player player = JsonSerializer.Deserialize<Player>(json);
            Console.WriteLine($"[슬롯 {slot}] 불러오기 성공!");
            return player;
        }
        catch
        {
            Console.WriteLine($"[슬롯 {slot}] 불러오기 실패!");
            return null;
        }
    }

    public static string GetSaveInfo(int slot)
    {
        string filename = $"save_slot{slot}.json";
        if (!File.Exists(filename)) return "[빈 슬롯]";

        try
        {
            string json = File.ReadAllText(filename);
            Player player = JsonSerializer.Deserialize<Player>(json);
            return $"[{player.Name} - {player.Job}] {player.SaveTime:yyyy-MM-dd HH:mm:ss}";
        }
        catch
        {
            return "[저장 데이터 손상]";
        }
    }
}
