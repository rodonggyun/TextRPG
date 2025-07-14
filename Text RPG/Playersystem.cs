using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Player
{
    public string Name { get; set; }
    public string Job { get; set; }
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public int STR { get; set; }
    public int DEX { get; set; }
    public int INT { get; set; }
    public int DEF { get; set; }
    public DateTime SaveTime { get; set; } = DateTime.Now;

    public void DisplayStats()
    {
        Console.WriteLine($"\n[{Name} - {Job}]");
        Console.WriteLine($"HP: {HP}/{MaxHP}");
        Console.WriteLine($"STR: {STR} | DEX: {DEX} | INT: {INT} | DEF: {DEF}");
        Console.WriteLine($"마지막 저장: {SaveTime:yyyy-MM-dd HH:mm:ss}");
    }
}