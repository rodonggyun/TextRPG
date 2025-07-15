using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class ShowMenu
{
    public static int ShowMenus(List<string> options) //방향키 움직이는 함수
    {
        int selected = 0;
        ConsoleKey key;

        int cursorTop = Console.CursorTop; // 현재 커서 위치 저장 (몬스터 출력 후 위치)

        do
        {
            // 메뉴 출력 부분만 갱신
            Console.SetCursorPosition(0, cursorTop);

            for (int i = 0; i < options.Count; i++)
            {
                // 줄 지우기: 출력 전에 현재 줄 clear
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, cursorTop + i);

                if (i == selected)
                    Console.WriteLine("> " + options[i]);
                else
                    Console.WriteLine("  " + options[i]);
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            key = keyInfo.Key;

            if (key == ConsoleKey.UpArrow)
                selected = (selected - 1 + options.Count) % options.Count;
            else if (key == ConsoleKey.DownArrow)
                selected = (selected + 1) % options.Count;

        } while (key != ConsoleKey.Z);

        return selected;
    }
}