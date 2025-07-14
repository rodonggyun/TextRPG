using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace c_test
{
    // 몬스터 정보 클래스
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
    static class Test
    {
        static List<MonsterInfo> monsterList = new List<MonsterInfo>() {
            new MonsterInfo() {type=1, name="슬라임", hp=100, level=1, exp=10, dead=false, gold=10, atk=1},
            new MonsterInfo() {type=2, name="미니언", hp=100, level=3, exp=30, dead=false, gold=30, atk=3},
            new MonsterInfo() {type=3, name="대포미니언", hp=100, level=5, exp=50, dead=false, gold=20, atk=10}
        };

        public static void Battle()
        {
            Console.WriteLine("Battle!!\n");

            for (int i = 0; i < monsterList.Count; i++)
            {
                Console.WriteLine($"{monsterList[i].level} {monsterList[i].name}\tHP: {monsterList[i].hp}");
            }

            Console.WriteLine("\n[내 정보]");
            // 상태창 출력 추가 예정

            string[] options = { "공격한다.", "도망간다.", "춤을 춘다." };
            int choice = ShowMenu(options);

            Console.WriteLine($"\n당신은 '{options[choice]}'를 선택했습니다!");
        }

        static int ShowMenu(string[] options)
        {
            int selected = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("무엇을 할까요?\n");

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selected)
                        Console.WriteLine("▶ " + options[i]);
                    else
                        Console.WriteLine("  " + options[i]);
                }

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

            } while (key != ConsoleKey.Enter && key != ConsoleKey.Spacebar);

            return selected;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Test.Battle(); // 전투 시작
        }
    }
}
