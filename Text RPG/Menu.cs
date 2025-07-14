using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class MenuSelector
{
    public static int Select(string title, List<string> options, bool canCancel = false)
    {
        int selected = 0;
        ConsoleKey key;

        do
        {
            Console.Clear();
            Console.WriteLine($"▶ {title}\n");
            

            for (int i = 0; i < options.Count; i++)
            {
                if (i == selected)
                    Console.WriteLine($"> {options[i]} <");
                else
                    Console.WriteLine($"  {options[i]}");
            }

            Console.WriteLine("\n↑↓ 방향키로 선택, Z: 확인  X: 취소");

            key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow)
                selected = (selected - 1 + options.Count) % options.Count;
            else if (key == ConsoleKey.DownArrow)
                selected = (selected + 1) % options.Count;
            else if (key == ConsoleKey.X && canCancel)
                return -1;

        } while (key != ConsoleKey.Z);

        return selected;
    }
}