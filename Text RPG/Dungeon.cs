using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class MonsterInfo
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
    public static class Dungeon
    {
        static List<MonsterInfo> monsterList = new List<MonsterInfo>() {
            new MonsterInfo() {type=1, name="슬라임", hp=100, level=1, exp=10, dead=false, gold=10, atk=1},
            new MonsterInfo() {type=2, name="미니언", hp=100, level=3, exp=30, dead=false, gold=30, atk=3},
            new MonsterInfo() {type=3, name="대포미니언", hp=100, level=5, exp=50, dead=false, gold=20, atk=10}
        };


        static void Battle()
        {
            MonsterAppear();

            Console.WriteLine("\n[내 정보]");
            //상태창 가져오는 작업 필요함

            string[] options = { "공격한다.", "도망간다.", "춤을 춘다." };
            int choice = ShowMenu(options);

            Console.WriteLine($"\n당신은 '{options[choice]}'를 선택했습니다!");

        }


        static void MonsterAppear()
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

        static int ShowMenu(string[] options) //방향키 움직이는 함수
        {
            int selected = 0;

            ConsoleKey key;
            do
            {
                Console.WriteLine("\n무엇을 할까요?\n");

                // 선택지 출력
                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selected)
                        Console.WriteLine("▶ " + options[i]);
                    else
                        Console.WriteLine("  " + options[i]);
                }

                // 키 입력 감지
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                key = keyInfo.Key;

                if (key == ConsoleKey.UpArrow)
                {
                    selected--;
                    if (selected < 0) selected = options.Length - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selected++;
                    if (selected >= options.Length) selected = 0;
                }

            } while (key != ConsoleKey.Enter && key != ConsoleKey.Spacebar); //z키로 확인할거면 수정 필요

            return selected;
        }
    }
}