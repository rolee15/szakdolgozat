﻿using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Learning;

namespace KanjiKa.Core.Entities.Users;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }

    // storing the hashing algorithm and parameters would make it possible to phase out
    // the outdated passwords gradually, but it's not necessary for this project
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }

    public List<Proficiency> Proficiencies { get; set; } = new();
    public List<LessonCompletion> LessonCompletions { get; set; } = new();
}
