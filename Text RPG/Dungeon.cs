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

public class BattleInfo
{
    public bool myTurn = true; //플레이어 차례, 기본값 true
    public bool eTurn = false; //몬스터 차례
}

// 전투 및 선택지 관련 클래스
public class Dungeon
{
    /* main 함수에서
    Dungeon dungeon = new Dungeon();
    dungeon.Battle();
    으로 함수 호출 */

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

        return new MonsterInfo() //객체 생성+초기화
        {
            type = type,
            name = name,
            hp = baseHp + level * 10, //1레벨이면 hp+10, 2레벨이면 hp+20, ...
            level = level,
            exp = level * 10,
            gold = level * 5,
            atk = level * 2,
            dead = false
        };
    }

    void Battle() //전투 화면
    {
        while (true)
        {
            Console.Clear();
            MonsterAppear();

            Console.WriteLine("\n[내 정보]");
            /*Console.WriteLine($"\n[{Name} - {Job}] (Lv. {Level})");
            Console.WriteLine($"HP: {HP}/{MaxHP}");
            Console.WriteLine($"EXP: {Exp}/{ExpToLevel} | 남은 스탯 포인트: {StatPoints}");
            Console.WriteLine($"▶ 공격력: {AttackPower} | 회피율: {Evasion} | 방어력: {Defense}"); */

            string[] options = { "공격한다.", "도망간다." };
            int choice = ShowMenu(options);

            if (choice == 0)
            {
                PlayerPhase();
            }
            else if (choice == 1)
            {
                Run();
                break;
            }
        }
    }

    void MonsterAppear()
    { //몬스터 랜덤 등장
        appearMonsters.Clear();  // 이전 몬스터들 삭제

        Random rand = new Random();
        int monsterCount = rand.Next(1, 5); //한 번에 나오는 몬스터는 1~4마리(랜덤)

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
            Console.WriteLine($"Lv.{m.level} {m.name} (HP: {m.hp})");
        }
    }


    BattleInfo battleInfo = new BattleInfo();
    Player player = new Player();



    void PlayerPhase()
    { //플레이어 턴
        Console.WriteLine("Battle!!\n");
        for (int i = 0; i < appearMonsters.Count; i++)
        { //몬스터 출력
            MonsterInfo m = appearMonsters[i];
            Console.WriteLine($"[{i + 1}] Lv.{m.level} {m.name} (HP: {m.hp})");
        }

        Console.WriteLine("\n[내 정보]");
        /*Console.WriteLine($"\n[{Name} - {Job}] (Lv. {Level})");
        Console.WriteLine($"HP: {HP}/{MaxHP}");
        Console.WriteLine($"EXP: {Exp}/{ExpToLevel} | 남은 스탯 포인트: {StatPoints}");
        Console.WriteLine($"▶ 공격력: {AttackPower} | 회피율: {Evasion} | 방어력: {Defense}"); */

        //때릴 수 있는 몬스터 선택
        //후 현재 보유중인 스킬목록 출력
        //이후 스킬 선택
        //공격
        //전투 종료 메서드 출력
    }

    void EnemyPhase()
    {
        if (!battleInfo.myTurn && battleInfo.eTurn) //몬스터 차례일 때
        {
            for (int i = 0; i < appearMonsters.Count; i++)
            {
                if (!appearMonsters[i].dead) //등장한 몬스터가 죽어있지 않을 때
                {
                    Console.WriteLine($"Battle!!\n");
                    Console.WriteLine($"Lv.{appearMonsters[i].level} {appearMonsters[i].name}의 공격!");
                    Console.WriteLine($"{appearMonsters[i].atk}만큼의 데미지가 달았습니다.\n");
                    Console.WriteLine($"\n[{player.Name} - {player.Job}] (Lv. {player.Level})");
                    Console.Write($"HP: {player.HP}");
                    player.HP -= appearMonsters[i].atk;
                    Console.WriteLine($" -> {player.HP}\n");
                    Console.WriteLine("계속하려면 아무 키나 누르세요...");
                    Console.ReadKey(true); //키 입력 대기, true는 입력문자 안 보이게
                }
                battleInfo.eTurn = false;
                battleInfo.myTurn = true;
            }
        }
    }

    void Run()
    {
        Console.Clear();
        Console.WriteLine("당신은 전투에서 도망쳤습니다!");
        Console.WriteLine("의지가 꺾이는 기분이 듭니다...\n");
        Console.WriteLine("당신의 자존감 -10\n");
        Console.WriteLine("계속하려면 아무 키나 누르세요...");
        Console.ReadKey(true);
        //Enter(player); 이거 player 어떤값을 받는 건가요? 마을로 이동하려는데
    }

    void BattleEnd()
    { //전투 종료
        //마을로 돌아가기, 전투 계속하기 선택 가능
        //전투 계속하기 누르면 Battle() 메서드 호출
        //추후 탑 층수 올라가는 변수, 방 고르는 함수 필요함
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