using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace EducationCSharp5
{
    internal class Program
    {

        public class Player
        {
            public string Name { get; set; }
            public int Health { get; set; }
            public int Strength { get; set; }
            public int Defense { get; set; }
            public int Experience { get; set; }

            public static void CreatePlayer(out Player player,string namePlayer)
            {
                player = new Player();
                player.Name = namePlayer;
                player.Health = 10;
                player.Strength = 1;
                player.Defense = 0;
                player.Experience = 0;
            }
        }

        public class Enemy
        {
            public string Name { get; set; }
            public int Health { get; set; }
            public int Strength { get; set; }
            public int Defense { get; set; }
        }

        class Vzaimodeistia
        {
            class Player
            {
                //static int PlayerAttack(ref Enemy enemy, Player player)
                //{
                //}

                //static int PlayerDefense(string Name)
                //{
                //    return
                //}

                //static int PlayerTakingDamage(string Name)
                //{
                //    return
                //}
            }
            class Enemy
            {
                //static int EnemyAttack(string Name)
                //{
                //    return
                //}

                //static int EnemyGainingExperience(string Name)
                //{
                //    return
                //}

                //static int EnemyDefense(string Name)
                //{
                //    return
                //}

                //static int EnemyTakingDamage(string Name)
                //{
                //    return;
                //}
            }
        }

        class Menu
        {
            string[] bottom1 { get; set; } = { "9", "Exit" };
            string[] bottom2 { get; set; } = { "0", "Save" };

            public static void WindowMenu(Menu menu)
            {
                foreach (var item in menu)
                {
                    
                }

            }

        static void Main(string[] args)
        {

            List<Player> list = new List<Player>();
            
            string namePlayer;
            Console.WriteLine("Введите имя игрока: ");
            namePlayer = Console.ReadLine()?.Trim();
            Player findPlayer;

            if (string.IsNullOrWhiteSpace(namePlayer))
            {
                findPlayer = list.Find(p => p.Name.Equals(namePlayer,StringComparison.OrdinalIgnoreCase));
            }

            Player.CreatePlayer(out Player player,namePlayer);

            Console.WriteLine(player.Name);
            Console.WriteLine(player.Health);

                

        }
    }
}
