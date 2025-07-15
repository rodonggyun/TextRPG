using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MonsterInfo
{
    public int type;
    public string name;
    public int hp;
    public int level;
    public int exp;
    public bool dead;
    public int gold;
    public int atk;
}

// 전투 및 선택지 관련 클래스
public class Dungeon
{
    //main 함수에서
    ///Dungeon dungeon = new Dungeon();
    //dungeon.Battle();
    //으로 함수 호출

    List<MonsterInfo> monsterList = new List<MonsterInfo>();
    List<MonsterInfo> appearMonsters = new List<MonsterInfo>();

    public Dungeon()
    { //몬스터 리스트 생성자, 생성자에서 초기화
        monsterList.Add(CreateMonster(1, "슬라임", 10, 1, 4));  //슬라임 레벨 1일때 체력 20부터 등장
        monsterList.Add(CreateMonster(2, "고블린", 15, 1, 6)); //고블린 1레벨은 체력 15부터 등장
        monsterList.Add(CreateMonster(3, "오크", 20, 2, 7));
    }

    MonsterInfo CreateMonster(int type, string name, int baseHp, int minLevel, int maxLevel)
    { //몬스터 생성
        Random rand = new Random();
        int level = rand.Next(minLevel, maxLevel + 1); //레벨은 최소 레벨 ~ 최대 레벨 랜덤 범위

        return new MonsterInfo()
        {
            type = type,
            name = name,
            hp = baseHp + level * 10, //1레벨이면 hp10, 2레벨이면 hp20, ...
            level = level,
            exp = level * 10,
            gold = level * 5,
            atk = level * 2,
            dead = false
        };
    }

    void Battle()
    {
        Console.Clear(); // 전체 초기화 (1회만)

        MonsterAppear();

        foreach (var m in appearMonsters)
        {
            Console.WriteLine($"Lv.{m.level} {m.name} (HP: {m.hp})");
        }

        Console.WriteLine("\n[내 정보]");
        // 상태 정보 출력 예정

        string[] options = { "공격한다.", "도망간다.", "춤을 춘다." };
        int choice = ShowMenu(options);

        Console.WriteLine($"\n당신은 '{options[choice]}'를 선택했습니다!");
    }


    void MonsterAppear()
    { //몬스터 랜덤 등장
        Random rand = new Random();
        int monsterCount = rand.Next(1, 5); //한 번에 나오는 몬스터는 1~4마리(랜덤)

        List<MonsterInfo> appearMonsters = new List<MonsterInfo>(); //출현 몬스터들을 새 리스트로 생성

        for (int i = 0; i < monsterCount; i++)
        { //뽑은 숫자만큼
            int index = rand.Next(monsterList.Count); //몬스터 랜덤 등장
            MonsterInfo original = monsterList[index]; //몬스터는 리스트에서 무작위로 가져옴.(얘는 복사한 객체가 아니라 원본 몬스터임)

            MonsterInfo clone = new MonsterInfo() //원본 몬스터에게 피해 안 가게끔 깊은 복사
            {
                type = original.type,
                name = original.name,
                hp = original.hp,
                level = original.level,
                exp = original.exp,
                dead = false,
                gold = original.gold,
                atk = original.atk
            };

            appearMonsters.Add(clone); //출현 몬스터 리스트에 복제한 값 넣기
        }

        Console.WriteLine("Battle!!\n");

        for (int i = 0; i < appearMonsters.Count; i++)
        { //몬스터 출력
            MonsterInfo m = appearMonsters[i];  //appearMonsters의 자료형이 MonsterInfo라 m 앞에 붙임
            Console.WriteLine($"[{i + 1}] Lv.{m.level} {m.name} (HP: {m.hp})");
        }
    }

    int ShowMenu(string[] options) //방향키 움직이는 함수
    {
        int selected = 0;
        ConsoleKey key;

        int cursorTop = Console.CursorTop; // 현재 커서 위치 저장 (몬스터 출력 후 위치)

        do
        {
            // 메뉴 출력 부분만 갱신
            Console.SetCursorPosition(0, cursorTop);

            for (int i = 0; i < options.Length; i++)
            {
                // 줄 지우기: 출력 전에 현재 줄 clear
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, cursorTop + i);

                if (i == selected)
                    Console.WriteLine("▶ " + options[i]);
                else
                    Console.WriteLine("  " + options[i]);
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            key = keyInfo.Key;

            if (key == ConsoleKey.UpArrow)
                selected = (selected - 1 + options.Length) % options.Length;
            else if (key == ConsoleKey.DownArrow)
                selected = (selected + 1) % options.Length;

        } while (key != ConsoleKey.Enter && key != ConsoleKey.Spacebar);

        return selected;
    }
}