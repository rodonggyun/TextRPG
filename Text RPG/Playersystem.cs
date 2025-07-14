using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Player
{
    public string Name { get; set; }
    public string Job { get; set; }
    public string SubJob;
    public string ThirdJob;
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public int STR { get; set; }
    public int DEX { get; set; }
    public int INT { get; set; }
    public int DEF { get; set; }
    public int Level { get; set; } = 1;
    public int Exp { get; set; } = 0;
    public int StatPoints { get; set; } = 0;
    public DateTime SaveTime { get; set; } = DateTime.Now;
    public int ExpToLevel => Level * 100;

    public void DisplayStats()
    {
        Console.WriteLine($"\n[{Name} - {Job}] (Lv. {Level})");
        Console.WriteLine($"HP: {HP}/{MaxHP}");
        Console.WriteLine($"STR: {STR} | DEX: {DEX} | INT: {INT} | DEF: {DEF}");
        Console.WriteLine($"EXP: {Exp}/{ExpToLevel} | 남은 스탯 포인트: {StatPoints}");
        Console.WriteLine($"▶ 공격력: {AttackPower} | 회피율: {Evasion} | 방어력: {Defense}");
        Console.WriteLine($"▶ 경험치 보정률: {ExpBonus * 100:F0}%");
        Console.WriteLine($"마지막 저장: {SaveTime:yyyy-MM-dd HH:mm:ss}");
    }
    public void GainExp(int baseAmount)
    {
        int actualAmount = (int)(baseAmount * ExpBonus); // 보정 적용
        Exp += actualAmount;

        Console.WriteLine($"{baseAmount}의 경험치를 획득했습니다! (보정 적용: {actualAmount})");

        while (Exp >= ExpToLevel)
        {
            Exp -= ExpToLevel;
            Level++;
            StatPoints += 5;
            Console.WriteLine($"레벨이 {Level}로 올랐습니다! 스탯 포인트 +5");
        }
    }

    public void AllocateStatPoints()
    {
        while (StatPoints > 0)
        {
            Console.Clear();
            Console.WriteLine("스탯 포인트를 분배하세요.");
            DisplayStats();
            Console.WriteLine("\n[1] STR (힘)\n[2] DEX (민첩)\n[3] INT (지능)\n[4] DEF (방어)\n[0] 완료");
            Console.Write("선택: ");

            string input = Console.ReadLine();
            if (input == "0") break;

            switch (input)
            {
                case "1": STR++; StatPoints--; break;
                case "2": DEX++; StatPoints--; break;
                case "3": INT++; StatPoints--; break;
                case "4": DEF++; StatPoints--; break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadKey();
                    break;
            }
        }

        Console.WriteLine("스탯 분배를 완료했습니다.");
        Console.ReadKey();
    }

    public int AttackPower
    {
        get
        {
            return Job switch
            {
                "전사" => STR,
                "도적" => DEX,
                "마법사" => INT,
                "성기사" => STR,
                _ => 0
            };
        }
    }

    public int Evasion
    {
        get
        {
            return Job switch
            {
                "전사" => DEX,
                "도적" => INT,
                "마법사" => DEX,
                "성기사" => DEX,
                _ => 0
            };
        }
    }

    public double ExpBonus
    {
        get
        {
            int stat = Job switch
            {
                "전사" => INT,
                "도적" => STR,
                "마법사" => STR,
                "성기사" => INT,
                _ => 0
            };
            return 1.0 + (stat * 0.1);
        }
    }

    public int Defense => DEF;

    public void CheckForSecondJobChange()
    {
        if (Level >= 10 && SubJob == null)
        {
            var options = GetSecondJobOptions(Job);
            int selection = MenuSelector.Select($"{Job}의 2차 전직을 선택하세요!", options);
            if (selection >= 0)
            {
                SubJob = options[selection];
                Console.WriteLine($"\n▶ 당신은 {SubJob}으로 전직했습니다!\n");
                ApplySecondJobBonus(SubJob);
            }
        }
    }

    public void CheckForThirdJobChange()
    {
        if (Level >= 20 && ThirdJob == null && SubJob != null)
        {
            var options = GetThirdJobOptions(SubJob);
            int selection = MenuSelector.Select($"{SubJob}의 3차 전직을 선택하세요!", options);
            if (selection >= 0)
            {
                ThirdJob = options[selection];
                Console.WriteLine($"\n▶ 당신은 {ThirdJob}으로 3차 전직했습니다!!\n");
                ApplyThirdJobBonus(ThirdJob);
            }
        }
    }

    private List<string> GetSecondJobOptions(string job)
    {
        return job switch
        {
            "전사" => new List<string> { "버서커", "가디언" },
            "도적" => new List<string> { "어쌔신", "섀도우댄서" },
            "마법사" => new List<string> { "아크메이지", "소서러" },
            "성기사" => new List<string> { "크루세이더", "템플러" },
            _ => new List<string>()
        };
    }

    private List<string> GetThirdJobOptions(string subJob)
    {
        return subJob switch
        {
            "버서커" => new List<string> { "블러드나이트" },
            "가디언" => new List<string> { "아이언월" },
            "어쌔신" => new List<string> { "나이트크로우" },
            "섀도우댄서" => new List<string> { "팬텀댄서" },
            "아크메이지" => new List<string> { "엘더위치" },
            "소서러" => new List<string> { "디멘터" },
            "크루세이더" => new List<string> { "팔라딘" },
            "템플러" => new List<string> { "세인트" },
            _ => new List<string>()
        };
    }

    private void ApplySecondJobBonus(string subJob)
    {
        switch (subJob)
        {
            case "버서커": STR += 10; break;
            case "가디언": DEF += 10; break;
            case "어쌔신": DEX += 10; break;
            case "섀도우댄서": DEX += 5; INT += 5; break;
            case "아크메이지": INT += 10; break;
            case "소서러": INT += 7; DEF += 3; break;
            case "크루세이더": STR += 5; INT += 5; break;
            case "템플러": DEF += 5; INT += 5; break;
        }
    }

    private void ApplyThirdJobBonus(string thirdJob)
    {
        switch (thirdJob)
        {
            case "블러드나이트": STR += 15; DEF -= 5; break;
            case "아이언월": DEF += 15; break;
            case "나이트크로우": DEX += 15; break;
            case "팬텀댄서": INT += 10; DEX += 5; break;
            case "엘더위치": INT += 15; break;
            case "디멘터": INT += 10; DEF += 5; break;
            case "팔라딘": STR += 7; DEF += 7; break;
            case "세인트": INT += 10; HP += 20; break;
        }
    }
}