# Mk0.Tools.Password
(C) 2019 mk0.at

This includes the PasswordStrength Class which calculates the strength of a password.
Usage:
double score = PasswordStrength.Score("Te$tPassw0rD");

Also there is a password Generator.
Usage:
string password = new PasswordGenerator().Next();

You can use following one, some or all optional parameters:
PasswordGenerator(int length, bool lowercase, bool uppercase, bool numeric, bool specialchars);
