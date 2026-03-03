using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Whatloll.Encoder
{
    /// <summary>
    /// Версия шифратора Whatloll
    /// Whatloll encoder version
    /// </summary>
    public enum WhatLollVersion
    {
        /// <summary>
        /// Первая версия (оригинальная таблица)
        /// First version (original table)
        /// </summary>
        V1 = 1,

        /// <summary>
        /// Вторая версия (расширенная таблица с английскими буквами и спецсимволами)
        /// Second version (extended table with English letters and special characters)
        /// </summary>
        V2 = 2
    }

    /// <summary>
    /// Класс для шифрования и дешифрования текста по таблицам замен Whatloll версий 1 и 2.
    /// Provides encryption and decryption methods using Whatloll substitution tables versions 1 and 2.
    /// </summary>
    public static class WhatLoll
    {
        #region V1 Tables

        // Таблица шифрования V1 (исходный символ -> зашифрованный)
        // V1 Encryption map (original character -> encrypted string)
        private static readonly Dictionary<char, string> EncryptMapV1 = new Dictionary<char, string>
        {
            {'а', "б"},
            {'б', "в"},
            {'в', "г"},
            {'г', "д"},
            {'д', "е"},
            {'е', "е1"},
            {'ё', "е2"},
            {'ж', "з"},
            {'з', "и"},
            {'и', "и1"},
            {'й', "и2"},
            {'к', "л"},
            {'л', "м"},
            {'м', "н"},
            {'н', "6"},
            {'о', "0"},
            {'п', "π"},
            {'р', "п"},
            {'с', "ß"},
            {'т', "у"},
            {'у', "ф"},
            {'ф', "х"},
            {'х', "ц"},
            {'ц', "ч"},
            {'ч', "ш"},
            {'ш', "щ"},
            {'щ', "_"},
            {'ь', "?"},
            {'ъ', "?"},
            {'ы', "?"},
            {'э', "ю"},
            {'ю', "я"},
            {'я', "_"}
        };

        // Таблица дешифрования V1 (зашифрованный -> исходный)
        // V1 Decryption map (encrypted -> original)
        private static readonly Dictionary<string, char> DecryptMapV1 = new Dictionary<string, char>();

        #endregion

        #region V2 Tables

        // Таблица шифрования V2 (исходный символ -> зашифрованный)
        // V2 Encryption map (original character -> encrypted string)
        private static readonly Dictionary<char, string> EncryptMapV2 = new Dictionary<char, string>
        {
            // Русские буквы (как в V1)
            {'а', "б"},
            {'б', "в"},
            {'в', "г"},
            {'г', "д"},
            {'д', "е"},
            {'е', "е1"},
            {'ё', "е2"},
            {'ж', "з"},
            {'з', "и"},
            {'и', "и1"},
            {'й', "и2"},
            {'к', "л"},
            {'л', "м"},
            {'м', "н"},
            {'н', "6"},
            {'о', "0"},
            {'п', "π"},
            {'р', "п"},
            {'с', "ß"},
            {'т', "у"},
            {'у', "ф"},
            {'ф', "х"},
            {'х', "ц"},
            {'ц', "ч"},
            {'ч', "ш"},
            {'ш', "щ"},
            {'щ', "_"},
            {'ь', "?"},
            {'ъ', "?"},
            {'ы', "?"},
            {'э', "ю"},
            {'ю', "я"},
            {'я', "_"},
            
            // Пробел и спецсимволы
            {' ', "_/"},
            
            // Английские буквы
            {'a', "А"},
            {'b', "йу"},
            {'c', "ц"},
            {'d', "дъ"},
            {'e', "и3"},
            {'f', "2ф"},
            {'g', "жъ"},
            {'h', "Н"}, // русская заглавная Н
            {'i', "8и"},
            {'j', "жъй"},
            {'k', "4а"},
            {'l', "m"},
            {'m', ":"},
            {'n', "h"},
            {'o', "0E"},
            {'p', "ПE"},
            {'q', "4у"},
            {'r', "0p"},
            {'s', "2"},
            {'t', "!"},
            {'u', "%E"},
            {'v', "(w-v)"},
            {'w', "(v+v)"},
            {'x', "1"},
            {'y', "2"},
            {'z', "3"}
        };

        // Таблица дешифрования V2 (зашифрованный -> исходный)
        // V2 Decryption map (encrypted -> original)
        private static readonly Dictionary<string, char> DecryptMapV2 = new Dictionary<string, char>();

        #endregion

        #region Common Tables

        // Таблица для специальных символов V2
        // V2 Special characters map
        private static readonly Dictionary<string, string> SpecialSymbolsMapV2 = new Dictionary<string, string>
        {
            {"[?]", "спец. символы"}
        };

        #endregion

        /// <summary>
        /// Статический конструктор, инициализирующий таблицы дешифрования.
        /// Static constructor initializing the decryption maps.
        /// </summary>
        static WhatLoll()
        {
            // Инициализация V1
            // Initialize V1
            foreach (var pair in EncryptMapV1)
            {
                if (!DecryptMapV1.ContainsKey(pair.Value))
                {
                    DecryptMapV1.Add(pair.Value, pair.Key);
                }
            }

            // Инициализация V2
            // Initialize V2
            foreach (var pair in EncryptMapV2)
            {
                if (!DecryptMapV2.ContainsKey(pair.Value))
                {
                    DecryptMapV2.Add(pair.Value, pair.Key);
                }
            }
        }

        #region V1 Methods

        /// <summary>
        /// Шифрует текст по версии V1.
        /// Encrypts text using V1.
        /// </summary>
        /// <param name="input">Входной текст / Input text</param>
        /// <returns>Зашифрованный текст / Encrypted text</returns>
        public static string EncryptV1(string input)
        {
            return Encrypt(input, WhatLollVersion.V1);
        }

        /// <summary>
        /// Дешифрует текст по версии V1.
        /// Decrypts text using V1.
        /// </summary>
        /// <param name="input">Зашифрованный текст / Encrypted text</param>
        /// <returns>Расшифрованный текст / Decrypted text</returns>
        public static string DecryptV1(string input)
        {
            return Decrypt(input, WhatLollVersion.V1, null);
        }

        /// <summary>
        /// Дешифрует текст по версии V1 с разрешением неоднозначностей.
        /// Decrypts text using V1 with ambiguity resolution.
        /// </summary>
        public static string DecryptV1(string input, Func<string, char> ambiguityResolver)
        {
            return Decrypt(input, WhatLollVersion.V1, ambiguityResolver);
        }

        #endregion

        #region V2 Methods

        /// <summary>
        /// Шифрует текст по версии V2.
        /// Encrypts text using V2.
        /// </summary>
        /// <param name="input">Входной текст / Input text</param>
        /// <returns>Зашифрованный текст / Encrypted text</returns>
        public static string EncryptV2(string input)
        {
            return Encrypt(input, WhatLollVersion.V2);
        }

        /// <summary>
        /// Дешифрует текст по версии V2.
        /// Decrypts text using V2.
        /// </summary>
        /// <param name="input">Зашифрованный текст / Encrypted text</param>
        /// <returns>Расшифрованный текст / Decrypted text</returns>
        public static string DecryptV2(string input)
        {
            return Decrypt(input, WhatLollVersion.V2, null);
        }

        /// <summary>
        /// Дешифрует текст по версии V2 с разрешением неоднозначностей.
        /// Decrypts text using V2 with ambiguity resolution.
        /// </summary>
        public static string DecryptV2(string input, Func<string, char> ambiguityResolver)
        {
            return Decrypt(input, WhatLollVersion.V2, ambiguityResolver);
        }

        #endregion

        #region Universal Methods

        /// <summary>
        /// Шифрует текст согласно указанной версии таблицы замен.
        /// Encrypts text according to the specified version of the substitution table.
        /// </summary>
        /// <param name="input">Входной текст / Input text</param>
        /// <param name="version">Версия шифратора / Encoder version</param>
        /// <returns>Зашифрованный текст / Encrypted text</returns>
        /// <exception cref="ArgumentException">Выбрасывается при неподдерживаемой версии / Thrown when version is not supported</exception>
        public static string Encrypt(string input, WhatLollVersion version = WhatLollVersion.V1)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var map = GetEncryptMap(version);
            var result = new StringBuilder();

            foreach (char c in input)
            {
                char lowerC = char.ToLowerInvariant(c);

                if (map.ContainsKey(lowerC))
                {
                    result.Append(map[lowerC]);
                }
                else if (map.ContainsKey(c)) // Проверяем оригинальный регистр / Check original case
                {
                    result.Append(map[c]);
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Дешифрует текст согласно указанной версии таблицы замен.
        /// Decrypts text according to the specified version of the substitution table.
        /// </summary>
        /// <param name="input">Зашифрованный текст / Encrypted text</param>
        /// <param name="version">Версия шифратора / Encoder version</param>
        /// <returns>Расшифрованный текст / Decrypted text</returns>
        public static string Decrypt(string input, WhatLollVersion version = WhatLollVersion.V1)
        {
            return Decrypt(input, version, null);
        }

        /// <summary>
        /// Дешифрует текст согласно указанной версии с разрешением неоднозначностей.
        /// Decrypts text according to the specified version with ambiguity resolution.
        /// </summary>
        /// <param name="input">Зашифрованный текст / Encrypted text</param>
        /// <param name="version">Версия шифратора / Encoder version</param>
        /// <param name="ambiguityResolver">Функция для разрешения неоднозначностей / Ambiguity resolution function</param>
        /// <returns>Расшифрованный текст / Decrypted text</returns>
        public static string Decrypt(string input, WhatLollVersion version, Func<string, char> ambiguityResolver)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var map = GetDecryptMap(version);

            if (ambiguityResolver == null)
                ambiguityResolver = (s) => 'ь'; // Default to 'ь'

            var result = new StringBuilder();
            int i = 0;

            while (i < input.Length)
            {
                bool found = false;

                // Проверяем длинные комбинации (для V2)
                // Check long combinations (for V2)
                if (version == WhatLollVersion.V2)
                {
                    // Проверяем комбинации до 5 символов (максимальная длина в V2)
                    // Check combinations up to 5 characters (max length in V2)
                    for (int len = 5; len >= 2; len--)
                    {
                        if (i + len <= input.Length)
                        {
                            string multiChars = input.Substring(i, len);
                            if (map.ContainsKey(multiChars))
                            {
                                result.Append(map[multiChars]);
                                i += len;
                                found = true;
                                break;
                            }
                        }
                    }
                }

                // Проверяем двухсимвольные комбинации (для V1)
                // Check two-character combinations (for V1)
                if (!found && i < input.Length - 1)
                {
                    string twoChars = input.Substring(i, 2);
                    if (map.ContainsKey(twoChars))
                    {
                        result.Append(map[twoChars]);
                        i += 2;
                        found = true;
                    }
                }

                // Проверяем односимвольные
                // Check single characters
                if (!found)
                {
                    string oneChar = input[i].ToString();

                    // Специальная обработка для "?"
                    // Special handling for "?"
                    if (oneChar == "?")
                    {
                        result.Append(ambiguityResolver(oneChar));
                    }
                    else if (map.ContainsKey(oneChar))
                    {
                        result.Append(map[oneChar]);
                    }
                    else
                    {
                        result.Append(input[i]);
                    }
                    i++;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Автоматически определяет версию шифратора по зашифрованному тексту и дешифрует его.
        /// Automatically detects encoder version from encrypted text and decrypts it.
        /// </summary>
        /// <param name="input">Зашифрованный текст / Encrypted text</param>
        /// <returns>Расшифрованный текст / Decrypted text</returns>
        public static string DecryptAuto(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Пробуем V2 сначала (он более специфичный)
            // Try V2 first (it's more specific)
            string resultV2 = DecryptV2(input);

            // Пробуем V1
            // Try V1
            string resultV1 = DecryptV1(input);

            // Если результаты отличаются, возвращаем V2 как более вероятный
            // If results differ, return V2 as more likely
            if (resultV2 != resultV1)
            {
                // Дополнительная эвристика: проверяем наличие специфичных для V2 паттернов
                // Additional heuristic: check for V2-specific patterns
                if (input.Contains("(w-v)") || input.Contains("(v+v)") ||
                    input.Contains("йу") || input.Contains("дъ") ||
                    input.Contains("жъ") || input.Contains("0E") ||
                    input.Contains("ПE") || input.Contains("%E"))
                {
                    return resultV2;
                }
            }

            return resultV1;
        }

        #endregion

        #region Helper Methods

        private static Dictionary<char, string> GetEncryptMap(WhatLollVersion version)
        {
            // Заменяем switch expression на обычный if-else для совместимости с C# 7.3
            // Replace switch expression with if-else for C# 7.3 compatibility
            if (version == WhatLollVersion.V1)
            {
                return EncryptMapV1;
            }
            else if (version == WhatLollVersion.V2)
            {
                return EncryptMapV2;
            }
            else
            {
                throw new ArgumentException($"Unsupported version: {version}");
            }
        }

        private static Dictionary<string, char> GetDecryptMap(WhatLollVersion version)
        {
            // Заменяем switch expression на обычный if-else для совместимости с C# 7.3
            // Replace switch expression with if-else for C# 7.3 compatibility
            if (version == WhatLollVersion.V1)
            {
                return DecryptMapV1;
            }
            else if (version == WhatLollVersion.V2)
            {
                return DecryptMapV2;
            }
            else
            {
                throw new ArgumentException($"Unsupported version: {version}");
            }
        }

        /// <summary>
        /// Возвращает таблицу шифрования для указанной версии.
        /// Returns the encryption table for the specified version.
        /// </summary>
        public static string GetEncryptionTable(WhatLollVersion version = WhatLollVersion.V1)
        {
            var map = GetEncryptMap(version);
            var table = new StringBuilder();

            table.AppendLine($"Таблица шифрования V{(int)version} (исходный -> зашифрованный):");
            table.AppendLine($"Encryption table V{(int)version} (original -> encrypted):");
            table.AppendLine("----------------------------------------");

            // Сортируем для удобства чтения
            // Sort for readability
            foreach (var pair in map.OrderBy(p => p.Key))
            {
                table.AppendLine($"'{pair.Key}' -> '{pair.Value}'");
            }

            if (version == WhatLollVersion.V2)
            {
                table.AppendLine();
                table.AppendLine("Специальные символы / Special characters:");
                foreach (var pair in SpecialSymbolsMapV2)
                {
                    table.AppendLine($"'{pair.Key}' -> {pair.Value}");
                }
            }

            return table.ToString();
        }

        /// <summary>
        /// Возвращает таблицу дешифрования для указанной версии.
        /// Returns the decryption table for the specified version.
        /// </summary>
        public static string GetDecryptionTable(WhatLollVersion version = WhatLollVersion.V1)
        {
            var map = GetDecryptMap(version);
            var table = new StringBuilder();

            table.AppendLine($"Таблица дешифрования V{(int)version} (зашифрованный -> исходный):");
            table.AppendLine($"Decryption table V{(int)version} (encrypted -> original):");
            table.AppendLine("----------------------------------------");

            foreach (var pair in map.OrderBy(p => p.Key.Length).ThenBy(p => p.Key))
            {
                table.AppendLine($"'{pair.Key}' -> '{pair.Value}'");
            }

            table.AppendLine("'?' -> 'ь' (по умолчанию) / '?' -> 'ь' (default)");
            table.AppendLine("'?' также может быть 'ъ' или 'ы' / '?' can also be 'ъ' or 'ы'");

            return table.ToString();
        }

        /// <summary>
        /// Проверяет, является ли символ поддерживаемым в указанной версии.
        /// Checks if a character is supported in the specified version.
        /// </summary>
        public static bool IsCharacterSupported(char character, WhatLollVersion version = WhatLollVersion.V1)
        {
            var map = GetEncryptMap(version);
            return map.ContainsKey(char.ToLowerInvariant(character)) || map.ContainsKey(character);
        }

        /// <summary>
        /// Возвращает количество поддерживаемых символов в указанной версии.
        /// Returns the number of supported characters in the specified version.
        /// </summary>
        public static int SupportedCharactersCount(WhatLollVersion version = WhatLollVersion.V1)
        {
            return GetEncryptMap(version).Count;
        }

        #endregion
    }
}