using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mk0.Tools.Password
{
    public class PasswordGenerator
    {
        private int minLength = 8;
        private int maxLength = 128;
        private int maxAttempts = 10000;

        private const string LOWERCASE_CHARACTERS = "abcdefghijklmnopqrstuvwxyz";
        private const string UPPERCASE_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string NUMERIC_CHARACTERS = "0123456789";
        private const string SPECIAL_CHARACTERS = @"!#$%&*@\";

        private int length;
        private bool lowercase;
        private bool uppercase;
        private bool numeric;
        private bool specialchars;

        public PasswordGenerator()
        {
            length = 8;
            lowercase = true;
            uppercase = true;
            numeric = true;
            specialchars = true;
        }

        public PasswordGenerator(int length)
        {
            this.length = length;
            lowercase = true;
            uppercase = true;
            numeric = true;
            specialchars = true;
        }

        public PasswordGenerator(bool lowercase = true, bool uppercase = true, bool numeric = true, bool specialchars = true)
        {
            length = 8;
            this.lowercase = lowercase;
            this.uppercase = uppercase;
            this.numeric = numeric;
            this.specialchars = specialchars;
        }

        public PasswordGenerator(int length, bool lowercase = true, bool uppercase = true, bool numeric = true, bool specialchars = true)
        {
            this.length = length;
            this.lowercase = lowercase;
            this.uppercase = uppercase;
            this.numeric = numeric;
            this.specialchars = specialchars;
        }

        public string Next()
        {
            string password;
            if (!LengthIsValid())

            {
                password = string.Format("Password length invalid. Must be between {0} and {1} characters long", minLength, maxLength);
            }
            else
            {
                int passwordAttempts = 0;
                do
                {
                    password = GenerateRandomPassword();
                    passwordAttempts++;
                }
                while (passwordAttempts < maxAttempts && !PasswordIsValid(password));

                password = PasswordIsValid(password) ? password : "Try again";
            }

            return password;
        }

        private string BuildCharacterSet()
        {
            StringBuilder characterSet = new StringBuilder();
            if (lowercase)
            {
                characterSet.Append(LOWERCASE_CHARACTERS);
            }

            if (uppercase)
            {
                characterSet.Append(UPPERCASE_CHARACTERS);
            }

            if (numeric)
            {
                characterSet.Append(NUMERIC_CHARACTERS);
            }

            if (specialchars)
            {
                characterSet.Append(SPECIAL_CHARACTERS);
            }

            return characterSet.ToString();
        }

        private string GenerateRandomPassword()
        {
            const int MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS = 2;
            char[] password = new char[length];

            char[] characters = BuildCharacterSet().ToCharArray();
            char[] shuffledChars = Shuffle(characters.Select(x => x)).ToArray();

            string shuffledCharacterSet = string.Join(null, shuffledChars);
            int characterSetLength = shuffledCharacterSet.Length;

            Random random = new Random();
            for (int characterPosition = 0; characterPosition < length; characterPosition++)
            {
                password[characterPosition] = shuffledCharacterSet[random.Next(characterSetLength - 1)];

                bool moreThanTwoIdenticalInARow =
                    characterPosition > MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS
                    && password[characterPosition] == password[characterPosition - 1]
                    && password[characterPosition - 1] == password[characterPosition - 2];

                if (moreThanTwoIdenticalInARow)
                {
                    characterPosition--;
                }
            }

            return string.Join(null, password);
        }

        private bool PasswordIsValid(string password)
        {
            const string REGEX_LOWERCASE = @"[a-z]";
            const string REGEX_UPPERCASE = @"[A-Z]";
            const string REGEX_NUMERIC = @"[\d]";
            const string REGEX_SPECIAL = @"([!#$%&*@\\])+";

            bool lowerCaseIsValid = !lowercase || (lowercase && Regex.IsMatch(password, REGEX_LOWERCASE));
            bool upperCaseIsValid = !uppercase || (uppercase && Regex.IsMatch(password, REGEX_UPPERCASE));
            bool numericIsValid = !numeric || (numeric && Regex.IsMatch(password, REGEX_NUMERIC));
            bool specialIsValid = !specialchars || (specialchars && Regex.IsMatch(password, REGEX_SPECIAL));

            return lowerCaseIsValid && upperCaseIsValid && numericIsValid && specialIsValid && LengthIsValid();
        }

        private bool LengthIsValid()
        {
            return length >= minLength && length <= maxLength;
        }

        private IEnumerable<T> Shuffle<T>(IEnumerable<T> items)
        {
            return from item in items orderby Guid.NewGuid() ascending select item;
        }
    }
}
