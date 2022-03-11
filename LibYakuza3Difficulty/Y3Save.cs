namespace LibYakuza3Difficulty
{
    public class Y3Save
    {
        public static string? GetPath() // Get the location where the Yakuza 3 savefile are located.
        {
            string? steamPath;
            if (OperatingSystem.IsWindows()) // Windows specific logic to find the location of Yakuza 3 save files.
            {
                steamPath = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", null) as string; // Get the location where Steam is installed from the Windows Registry.
            }
            else
            {
                steamPath = ".steam/steam/";
            }
            string userdataPath = Path.Join(steamPath, "/userdata/");
            string? path = Path.GetPathRoot(Environment.SystemDirectory) as string;

            if (Directory.Exists(Path.GetFullPath(userdataPath)))
            {
                try
                {
                    string[] subfolders = Directory.GetDirectories(userdataPath); // Get the subfolders in userdata.
                    if (subfolders.Length == 1) // Check if there is only 1 steamID folder in userdata
                    {
                        path = Path.GetFullPath(@subfolders[0] + "/1088710/remote/");
                        if (Directory.Exists(path))
                        {
                            return path;
                        }
                        return Path.GetFullPath(subfolders[0].ToString());
                    }
                    return Path.GetFullPath(userdataPath);
                }
                catch
                {
                    if (path == null)
                    {
                        return "";
                    }
                    return Path.GetFullPath(path);
                }
            }
            return path;
        }

        public static void Check(string filePath) // Check if Yakuza 3 save file is valid
        {
            if (filePath != null)
            {
                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                fs.Position = 0x00;
                int int1 = fs.ReadByte();
                if (int1 != 0x01)
                {
                    throw new Exception("Invalid Save File.");
                }
                return;
            }
            throw new ArgumentNullException();
        }

        public static string ReadDifficulty(string path) // difficulty values: 0=Easy 1=Normal 2=Hard 3=Legendary
        {
            var difficulty = ReadDifficultyInt(path);
            return difficulty switch
            {
                0 => "Easy",
                1 => "Normal",
                2 => "Hard",
                3 => "Legend",
                // error out if difficulty value is invalid
                _ => throw new ArgumentException("Invalid Difficulty: " + difficulty),
            };
        }
        public static int ReadDifficultyInt(string path) // difficulty values: 0=Easy 1=Normal 2=Hard 3=Legendary
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            // Read the difficulty value in the Yakuza 3 save file
            fs.Position = 0x14;
            int int1 = fs.ReadByte();
            int int2 = fs.ReadByte();

            // checks
            if (int1 != int2) throw new Exception("Not a Yakuza 3 save file.");
            if (int1 >= 4 || int1 <= -1) throw new Exception("Invalid difficulty in save file.");
            return int1;
        }

        public static void ChangeDifficulty(string path, string difficulty) // difficulty values: 0=Easy 1=Normal 2=Hard 3=Legendary
        {
            ChangeDifficulty(path, difficulty.ToLower() switch
            {
                "easy" => 0,
                "normal" => 1,
                "hard" => 2,
                "legend" => 3,
                // error out if difficulty value is invalid
                _ => throw new ArgumentException("Invalid Difficulty: " + difficulty),
            });
        }
        public static void ChangeDifficulty(string path, int difficulty) // difficulty values: 0=Easy 1=Normal 2=Hard 3=Legendary
        {
            //Throw exception if the difficulty value is invalid
            if (difficulty >= 4 || difficulty <= -1) throw new ArgumentException("Difficulty cannot be set to more then 3 or less than 0");

            using var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            // Write the difficulty value in the Yakuza 3 save file
            fs.Position = 0x14;
            fs.WriteByte(Convert.ToByte(difficulty));
            fs.Position = 0x15;
            fs.WriteByte(Convert.ToByte(difficulty));
        }
    }
}