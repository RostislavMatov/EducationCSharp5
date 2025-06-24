using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.SqlServer.Server;
using System.Runtime.InteropServices;

namespace EducationCSharp5
{
    internal class Program
    {
        class Player
        {
            public string Name { get; set; }
            public int Health { get; set; }
            public int Strength { get; set; }
            public int Defense { get; set; }
            public int Experience { get; set; }

            private Random random = new Random();

            public static void CreatePlayer(out Player player, string namePlayer)
            {
                player = new Player
                {
                    Name = namePlayer,
                    Health = 10,
                    Strength = 2,
                    Defense = 1,
                    Experience = 0
                };
            }

            public void Attack(Enemy enemy)
            {
                int damage = random.Next(1, Strength + 3) - enemy.Defense;
                if (damage > 0)
                {
                    enemy.Health -= damage;
                    Console.WriteLine($"{Name} атакует {enemy.Name} и наносит {damage} урона!");
                }
                else
                {
                    Console.WriteLine($"{Name} не может нанести урон {enemy.Name} из-за его защиты!");
                }
            }

            public void GainExperience(int amount)
            {
                Experience += amount;
                Console.WriteLine($"{Name} получает {amount} опыта! Текущий опыт: {Experience}");

                // Проверка на возможность повышения уровня (например, каждые 10 очков опыта)
                if (Experience >= 10)
                {
                    LevelUp();
                }
            }

            private void LevelUp()
            {
                Console.WriteLine($"{Name} достиг уровня! Выберите, что улучшить:");
                Console.WriteLine("1. Здоровье (+5)");
                Console.WriteLine("2. Сила (+1)");
                Console.WriteLine("3. Защита (+1)");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Health += 5;
                        break;
                    case "2":
                        Strength += 1;
                        break;
                    case "3":
                        Defense += 1;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        return;
                }

                // Сброс опыта после повышения уровня
                Experience -= 10;

                Console.WriteLine($"Новые характеристики: Здоровье: {Health}, Сила: {Strength}, Защита: {Defense}");
            }
        }

        class Enemy
        {
            public string Name { get; set; }
            public int Health { get; set; }
            public int Strength { get; set; }
            public int Defense { get; set; }

            private Random random = new Random();

            public Enemy(string name, int health, int strength, int defense)
            {
                Name = name;
                Health = health;
                Strength = strength;
                Defense = defense;
            }

            // Фабричные методы для создания разных типов противников
            public static Enemy CreateWeakEnemy(string name)
            {
                return new Enemy(name, 10, 2, 1);
            }

            public static Enemy CreateNormalEnemy(string name)
            {
                return new Enemy(name, 20, 4, 2);
            }

            public static Enemy CreateStrongEnemy(string name)
            {
                return new Enemy(name, 30, 6, 3);
            }

            public static Enemy CreateCustomEnemy(string name, int health, int strength, int defense)
            {
                return new Enemy(name, health, strength, defense);
            }

            public void Attack(Player player)
            {
                int damage = random.Next(1, Strength + 3) - player.Defense;
                if (damage > 0)
                {
                    player.Health -= damage;
                    Console.WriteLine($"{Name} атакует {player.Name} и наносит {damage} урона!");
                }
                else
                {
                    Console.WriteLine($"{Name} не может нанести урон {player.Name} из-за его защиты!");
                }
            }

            // Метод для улучшения характеристик врага
            public void Improve()
            {
                // Случайный выбор характеристики для улучшения
                Random rand = new Random();
                int improvementType = rand.Next(3); // Генерируем число от 0 до 2

                switch (improvementType)
                {
                    case 0:
                        Health += rand.Next(1, 4); // Увеличиваем здоровье на случайное значение от 1 до 3
                        Console.WriteLine($"{Name} улучшает здоровье! Новое здоровье: {Health}");
                        break;

                    case 1:
                        Strength += rand.Next(1, 2); // Увеличиваем силу на случайное значение от 1 до 1 (т.е. на единицу)
                        Console.WriteLine($"{Name} улучшает силу! Новая сила: {Strength}");
                        break;

                    case 2:
                        Defense += rand.Next(0, 2); // Увеличиваем защиту на случайное значение от 0 до 1 (т.е. может не увеличиться)
                        Console.WriteLine($"{Name} улучшает защиту! Новая защита: {Defense}");
                        break;

                    default:
                        break;
                }
            }
        }

        class PlayerStorage
        {
            private const string FileName = "players.json";

            private static string GetStoragePath()
            {
                // Папка Documents/MyGame
                string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string folder = Path.Combine(documents, "MyGame");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                return Path.Combine(folder, FileName);
            }

            public static void SavePlayers(List<Player> players)
            {
                string path = GetStoragePath();
                // Сериализуем в JSON (с отступами для читаемости)
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(players, options);
                File.WriteAllText(path, json);
                Console.WriteLine($"Сохранено {players.Count} игроков в {path}");
            }

            public static List<Player> LoadPlayers()
            {
                string path = GetStoragePath();
                if (!File.Exists(path))
                {
                    Console.WriteLine("Файл сохранения не найден, будет создан новый список игроков.");
                    return new List<Player>();
                }

                try
                {
                    string json = File.ReadAllText(path);
                    var players = JsonSerializer.Deserialize<List<Player>>(json);
                    Console.WriteLine($"Загружено {players.Count} игроков из {path}");
                    return players ?? new List<Player>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при загрузке игроков: {ex.Message}");
                    return new List<Player>();
                }
            }
        }

        class Menu
        {
            public BottomMenu[] menu { get; set; }
            public struct BottomMenu
            {
                public string Name { get; set; }
                public int Bottom { get; set; }
            }
            public Menu()
            {
                menu = new BottomMenu[4];

                BottomMenu Exit = new BottomMenu();
                Exit.Name = "exit";
                Exit.Bottom = 4;

                BottomMenu Save = new BottomMenu();
                Save.Name = "save";
                Save.Bottom = 3;

                BottomMenu NewGame = new BottomMenu();
                NewGame.Name = "New game";
                NewGame.Bottom = 2;

                BottomMenu СontinueBottom = new BottomMenu();
                СontinueBottom.Name = "Сontinue";
                СontinueBottom.Bottom = 1;

                menu[0] = СontinueBottom;
                menu[1] = NewGame;
                menu[2] = Save;
                menu[3] = Exit;

            }

            public static void WindowMenu(Menu menuObj)
            {
                Console.WriteLine("            Меню игры            ");
                //Console.WriteLine("Нажмите                  Действие");
                for (int i = 0; i < menuObj.menu.Length; i++)
                {
                    Console.WriteLine($"            {menuObj.menu[i].Bottom}. {menuObj.menu[i].Name}        ");
                }
            }
        }

        static void Main(string[] args)
        {
            List<Player> players = PlayerStorage.LoadPlayers(); // Загрузка сохраненных игроков
            Menu menu = new Menu();
            Player currentPlayer = null;
            bool gameLoop = true;


            while (gameLoop)
            {
                // Настройка меню в зависимости от наличия сохраненных игроков
                if (players.Count == 0)
                {
                    menu.menu = menu.menu.Where(m => m.Name != "Сontinue").ToArray();
                }

                Menu.WindowMenu(menu);
                string choice = Console.ReadLine()?.Trim();

                if (int.TryParse(choice, out int menuChoice))
                {
                    switch (menuChoice)
                    {
                        case 4: // Выход
                            gameLoop = false;
                            break;

                        case 2: // Новая игра
                            Console.WriteLine("Введите имя нового игрока:");
                            string newPlayerName = Console.ReadLine()?.Trim();
                            if (!string.IsNullOrWhiteSpace(newPlayerName))
                            {
                                Player.CreatePlayer(out currentPlayer, newPlayerName);
                                players.Add(currentPlayer);
                                StartGame(currentPlayer);
                            }
                            break;

                        case 3: // Сохранить игру
                            PlayerStorage.SavePlayers(players);
                            Console.WriteLine("Игра сохранена.");
                            break;

                        case 1: // Продолжить игру

                            Console.WriteLine("Список сохранённых игроков:");
                            for (int i = 0; i < players.Count; i++)
                                Console.WriteLine($"{i + 1}. {players[i].Name}");

                            Console.Write("Выберите номер игрока для продолжения или 0 — новый игрок: ");
                            string playerNomberStr = Console.ReadLine()?.Trim();
                            if (int.TryParse(playerNomberStr, out int playerNomber))
                            {
                                StartGame(players[playerNomber-1]);
                            }
                            break;

                        default:
                            Console.WriteLine("Неверный выбор.");
                            break;
                    }
                }
            }
        }

        private static void StartGame(Player player)
        {
            Enemy enemy = GenerateEnemy(player);
            Console.WriteLine($"\nПротивник появился: {enemy.Name}");
            Console.WriteLine($"Здоровье: {enemy.Health}, Атака: {enemy.Strength}, Защита: {enemy.Defense}");

            while (player.Health > 0 && enemy.Health > 0)
            {
                Console.WriteLine("\n1. Атаковать");
                Console.WriteLine("2. Защищаться");
                Console.WriteLine("3. Убежать");


                string action = Console.ReadLine()?.Trim();

                if (action == "3")
                {
                    Console.WriteLine("Вы сбежали с поля боя!");
                    break;
                }

                // Ход игрока
                if (action == "1")
                {
                    int damage = player.Strength - enemy.Defense;
                    enemy.Health -= Math.Max(1, damage);
                    Console.WriteLine($"Вы нанесли {Math.Max(1, damage)} урона!");
                }
                else if (action == "2")
                {
                    player.Defense *= 2;
                    Console.WriteLine("Вы приняли защитную стойку!");
                }

                if (enemy.Health <= 0)
                {
                    Console.WriteLine("Победа! Враг повержен!");
                    break;
                }

                // Ход противника
                int enemyDamage = enemy.Strength - player.Defense;
                player.Health -= Math.Max(1, enemyDamage);
                Console.WriteLine($"Противник нанес вам {Math.Max(1, enemyDamage)} урона!");

                if (player.Defense > 0) player.Defense /= 2; // Сброс усиленной защиты

                Console.WriteLine($"\nВаше здоровье: {player.Health}");
                Console.WriteLine($"Здоровье противника: {enemy.Health}");
            }
        }

        private static Enemy GenerateEnemy(Player player)
        {
            Random random = new Random();
            Enemy enemy = new Enemy(null, 0, 0, 0);
            enemy.Health = player.Health + random.Next(-10, 11);
            enemy.Strength = player.Strength + random.Next(-5, 6);
            enemy.Defense = player.Defense + random.Next(-3, 4);
            enemy.Name = "Противник #" + random.Next(1, 100);
            return enemy;
        }
    }
}