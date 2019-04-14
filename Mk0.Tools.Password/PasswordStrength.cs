using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mk0.Tools.Password
{
    public class PasswordStrength
    {
        private static int punkte = 0;
        private static bool hatNummer = false;
        private static bool hatLowercase = false;
        private static bool hatUppercase = false;
        private static bool hatSonderzeichen = false;

        public static double Score(string password)
        {
            if (string.IsNullOrEmpty(password.Trim()))
            {
                punkte = 0; //kein Passwort
            }
            else
            {
                if (password.Trim().Length < 6)
                {
                    punkte = 1; //zu kurz
                }
                else
                {
                    punkte = 2; //min Länge erreicht
                    if (password.Trim().Length >= 8) //1 pkt für je 2 stellen
                    {
                        punkte += ((password.Trim().Length - 8) / 2) + 1;
                    }
                    if (Regex.IsMatch(password.Trim(), @"[\d]", RegexOptions.ECMAScript)) //hat nummern
                    {
                        punkte += 5;
                        hatNummer = true;
                    }
                    if (Regex.IsMatch(password.Trim(), @"[a-z]", RegexOptions.ECMAScript)) //hat lowercase
                    {
                        punkte += 5;
                        hatLowercase = true;
                    }
                    if (Regex.IsMatch(password, @"[A-Z]", RegexOptions.ECMAScript)) //hat uppercase
                    {
                        punkte += 5;
                        hatUppercase = true;
                    }
                    if (Regex.IsMatch(password, @"[~`!@#$%\^\&\*\(\)\-_\+=\[\{\]\}\|\\;:'\""<\,>\.\?\/£]", RegexOptions.ECMAScript)) //hat sonderzeichen
                    {
                        punkte += 10;
                        hatSonderzeichen = true;
                    }
                    List<char> lstPass = password.Trim().ToList(); //länger als 2 zeichen und 3 wiederholende zeichen -2
                    if (lstPass.Count >= 3)
                    {
                        for (int i = 2; i < lstPass.Count; i++)
                        {
                            char charCurrent = lstPass[i];
                            if (charCurrent == lstPass[i - 1] && charCurrent == lstPass[i - 2] && punkte >= 4)
                            {
                                punkte -= 3;
                            }
                        }
                    }
                }
            }
            return (punkte + CalculateEntropy(password.Trim())) / 2;
        }



        private static double CalculateEntropy(string password)
        {
            double cardinality = 0;

            if (password.Length >= 6 && hatLowercase && hatNummer && hatSonderzeichen && hatUppercase)
            {
                // Password contains lowercase letters.
                if (password.Any(c => char.IsLower(c)))
                {
                    cardinality = 26;
                }

                // Password contains uppercase letters.
                if (password.Any(c => char.IsUpper(c)))
                {
                    cardinality += 26;
                }

                // Password contains numbers.
                if (password.Any(c => char.IsDigit(c)))
                {
                    cardinality += 10;
                }

                // Password contains symbols.
                if (password.IndexOfAny("\\|¬¦`!\"£$%^&*()_+-=[]{};:'@#~<>,./? ".ToCharArray()) >= 0)
                {
                    cardinality += 36;
                }

                cardinality = Math.Log(cardinality, 2) * password.Length;
            }
            return cardinality;
        }
    }
}
